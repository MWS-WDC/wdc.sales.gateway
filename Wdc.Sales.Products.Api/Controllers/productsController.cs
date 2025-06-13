using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Products.Api.Entities;
using Wdc.Sales.Products.Api.Models;
using Wdc.Sales.Products.Api.Persistence;

namespace Wdc.Sales.Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController(AppDbContext context) : ControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddProductAsync([FromBody] AddProductInputModel input, CancellationToken cancellationToken = default)
        {
            Product? product = await context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == input.ProductId);

            if (product is not null)
            {
                return Problem(detail: "already exists", statusCode: 409);
            }

            await context.Products.AddAsync(Product.Add(input.ProductId, input.Quantity, input.Price), cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Ok("Add Product successfully.");
        }

        [AllowAnonymous]
        [HttpGet("get-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(CancellationToken cancellationToken = default)
        {
            List<Product> products = await context.Products.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

            if (products.Count() < 0)
            {
                products.Add(Product.Add(id: "apple", price: 100, quantity: 10));

                products.Add(Product.Add(id: "banana", price: 100, quantity: 10));

                products.Add(Product.Add(id: "strawberry", price: 100, quantity: 10));
            }

            await context.Products.AddRangeAsync(products);

            await context.SaveChangesAsync(cancellationToken);

            return Ok(products);
        }
    }
}