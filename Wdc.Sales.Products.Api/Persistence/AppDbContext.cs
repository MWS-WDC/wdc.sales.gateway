using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Products.Api.Entities;

namespace Wdc.Sales.Products.Api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

    }
}
