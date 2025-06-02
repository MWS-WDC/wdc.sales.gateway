using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Orders.Api.Entitys;

namespace Wdc.Sales.Orders.Api.Persistence
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }

}
