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
    public class WalletController(AppDbContext context, ServiceBusPublisher serviceBusPublisher) : ControllerBase
    {
        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWalletAsync([FromBody] decimal value, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            await context.Wallets.AddAsync(Wallet.Create(userId, value), cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Ok("Create Wallet successfully.");
        }

        [HttpPost("reduce-balance")]
        public async Task<IActionResult> ReduceBalanceWalletAsync([FromBody] ReduceBalanceWalletInput input, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            Wallet? wallet = await context.Wallets.Where(x => x.OwnerId == userId).FirstOrDefaultAsync(cancellationToken);

            wallet?.ReduceBalance(input.Value * input.Quantity);

            await context.SaveChangesAsync(cancellationToken);

            await serviceBusPublisher.PublishEventAsync(new QuantityReduced
            {
                AggregateId = input.ProductId,
                Sequence = wallet is null ? 0 : wallet.Sequence,
                Data = new QuantityReducedData { Quantity = input.Quantity },
                DateTime = DateTime.UtcNow,
                UserId = userId.ToString(),
                Version = 1
            });

            return Ok($"Reduce balance Wallet successfully ** new balance: {wallet?.Balance} ");
        }
    }
}