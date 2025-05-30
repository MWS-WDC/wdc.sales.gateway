using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Users.Api.Models;

namespace Wdc.Sales.Users.Api.Persistence;

public class DatabaseMigrationHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        UsersDbContext db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        await db.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
