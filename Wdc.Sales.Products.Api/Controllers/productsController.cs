using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Products.Api.Entities;
using Wdc.Sales.Products.Api.Models;
using Wdc.Sales.Products.Api.Persistence;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(AppDbContext context) : ControllerBase
{
    [HttpPost("Add")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductInputModel input, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(Product.Add(input.Id, input.Quantity, input.Price), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Ok("Add Product successfully.");
    }

    [AllowAnonymous]
    [HttpGet("get-products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken = default)
    {
        IEnumerable<Product> products = await context.Products.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);
        return Ok(products);
    }
}
