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
        public async Task<ActionResult<CreateOrderOutPutModel>> CreateOrderAsync([FromBody] CreateOrderInputModel input)
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            Order order = new()
            {
                UserId = userId,
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

            return Ok(new CreateOrderOutPutModel { Id = order.Id });
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderAsync(string orderId)
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

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

        [HttpGet("all")]
        public async Task<ActionResult<GetAllOrdersOutput>> GetAllOrdersAsync()
        {
            Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            var orders = await context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            GetAllOrdersOutput result = new GetAllOrdersOutput
            {
                getAllOrderOutPutModels = orders.Select(o => new GetAllOrderOutPutModel
                {
                    OrderId = o.Id,
                    CreatedAt = o.CreatedAt,
                    Status = nameof(o.Status),
                    orderItemOutPutModels = o.Items.Select(x => new OrderItemOutPutModel { ProductId = x.ProductId, Quantity = x.Quantity })
                })
            };

            return Ok(result);
        }
    }
}