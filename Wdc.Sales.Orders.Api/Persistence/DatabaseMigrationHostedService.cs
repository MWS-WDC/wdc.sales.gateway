using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Orders.Api.Models;

namespace Wdc.Sales.Orders.Api.Persistence;

public class DatabaseMigrationHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        OrdersDbContext db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

        await db.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
