using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Payments.Api.Entities;

namespace Wdc.Sales.Payments.Api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Wallet> Wallets => Set<Wallet>();

    }
}
