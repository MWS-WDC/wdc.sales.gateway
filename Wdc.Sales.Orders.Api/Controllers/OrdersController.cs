using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wdc.Sales.Orders.Api.Entitys;
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderInputModel request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            Order order = new()
            {
                UserId = userId,
                ShippingAddress = request.ShippingAddress,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
            };

            context.Orders.Add(order);

            await context.SaveChangesAsync();

            return Ok(new { order.Id });
        }
    }
}