using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wdc.Sales.Orders.Api.DTOs;
using Wdc.Sales.Orders.Api.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;

    public OrdersController(OrdersDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token.");

        var userId = Guid.Parse(userIdClaim.Value);

        var order = new Order
        {
            UserId = userId,
            ShippingAddress = request.ShippingAddress,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { order.Id });
    }
}
