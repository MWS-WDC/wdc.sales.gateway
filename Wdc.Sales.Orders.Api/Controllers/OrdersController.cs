using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wdc.Sales.Orders.Api.Entitys;
using Wdc.Sales.Orders.Api.Enums;
using Wdc.Sales.Orders.Api.Models;
using Wdc.Sales.Orders.Api.Persistence;

namespace Wdc.Sales.Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(OrdersDbContext context) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderInputModel input)
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            Guid userId = Guid.Parse(userIdClaim.Value);

            Order order = new()
            {
                UserId = userId,
                ShippingAddress = input.ShippingAddress,
                Items = input.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            await context.Orders.AddAsync(order);

            await context.SaveChangesAsync();

            return Ok(new { order.Id });
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderAsync(string orderId)
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            Guid userId = Guid.Parse(userIdClaim.Value);

            var order = await context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return NotFound("Order not found.");

            if (order.Status != OrderStatus.Pending)
                return BadRequest("Only pending orders can be cancelled.");

            order.Status = OrderStatus.Cancelled;
            await context.SaveChangesAsync();

            return Ok(new { message = "Order cancelled." });
        }

    }
}