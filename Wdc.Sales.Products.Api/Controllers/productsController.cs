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
            Product? product = await context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == input.Id);
            if (product is not null)
            {
                return Problem(detail: "already exists", statusCode: 409);
            }

            await context.Products.AddAsync(Product.Add(input.Id, input.Quantity, input.Price), cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Ok("Add Product successfully.");
        }

        [AllowAnonymous]
        [HttpGet("get-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Product> products = await context.Products.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);
            return Ok(products);
        }
    }
}