using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wdc.Sales.Payments.Api.Entities;
using Wdc.Sales.Payments.Api.Events;
using Wdc.Sales.Payments.Api.Events.Data;
using Wdc.Sales.Payments.Api.Models;
using Wdc.Sales.Payments.Api.Persistence;
using Wdc.Sales.Payments.Api.Persistence.ServiceBus;

namespace Wdc.Sales.Payments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController(
        AppDbContext context,
        ServiceBusToProductPublisher serviceBusPublisher
    ) : ControllerBase
    {
        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWalletAsync(
            [FromBody] CreateWalletInput input,
            CancellationToken cancellationToken = default
        )
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            Guid userId = Guid.Parse(userIdClaim.Value);

            Wallet wallet = Wallet.Create(userId, input.Value, " ");

            await context.Wallets.AddAsync(wallet, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Ok("Create Wallet successfully");
        }

        [HttpPost("reduce-balance")]
        public async Task<IActionResult> ReduceBalanceWalletAsync(
            [FromBody] ReduceBalanceWalletInput input,
            CancellationToken cancellationToken = default
        )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            Wallet? wallet = await context.Wallets.Where(x => x.OwnerId == userId).FirstOrDefaultAsync(cancellationToken);
            if (wallet is null)
            {
                wallet = Wallet.Create(userId, 10000000, input.LocationName);
                await context.Wallets.AddAsync(wallet, cancellationToken);
            }
            else
            {
                wallet.ReduceBalance(input.ReduceBalanceWallets.Sum(x => x.Value * x.Quantity), input.LocationName);
            }
            await context.SaveChangesAsync(cancellationToken);
            ReduceBalanceWallet? reduceBalanceWallet = input.ReduceBalanceWallets.FirstOrDefault();
            await serviceBusPublisher.PublishEventAsync(new QuantityReduced
            {
                AggregateId = reduceBalanceWallet?.ProductId ?? "apple",
                Sequence = wallet is null ? 0 : wallet.Sequence,
                Data = new QuantityReducedData { Quantity = reduceBalanceWallet?.Quantity ?? 0 },
                DateTime = DateTime.UtcNow,
                UserId = userId.ToString(),
                Version = 1
            });

            return Ok($"Reduce balance Wallet successfully ** new balance: {wallet?.Balance} ");
        }
    }
}