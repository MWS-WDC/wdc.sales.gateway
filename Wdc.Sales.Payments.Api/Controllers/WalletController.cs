using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wdc.Sales.Payments.Api.Entities;
using Wdc.Sales.Payments.Api.Persistence;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController(AppDbContext context) : ControllerBase
{
    [HttpPost("create-wallet")]
    public async Task<IActionResult> CreateWallet([FromBody] decimal value, CancellationToken cancellationToken = default)
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
    public async Task<IActionResult> ReduceBalanceWallet([FromBody] decimal value, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

        if (userIdClaim == null)
            return Unauthorized("User ID not found in token.");

        var userId = Guid.Parse(userIdClaim.Value);

        Wallet? wallet = await context.Wallets.Where(x => x.OwnerId == userId).FirstOrDefaultAsync(cancellationToken);

        wallet?.ReduceBalance(value);

        await context.SaveChangesAsync(cancellationToken);

        return Ok($"Reduce balance Wallet successfully ** new balance: {wallet?.Balance} ");
    }
}
