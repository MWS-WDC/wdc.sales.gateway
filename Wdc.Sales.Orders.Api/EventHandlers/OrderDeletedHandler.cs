using MediatR;
using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Orders.Api.Events;
using Wdc.Sales.Orders.Api.Persistence;

namespace Wdc.Sales.Orders.Api.EventHandlers;

public class OrderDeletedHandler(OrdersDbContext context) : IRequestHandler<OrderDeleted, bool>
{
    public async Task<bool> Handle(OrderDeleted @event, CancellationToken cancellationToken)
    {

        var order = await context.Orders
        .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == @event.AggregateId);

        if (order == null)
            return true;

        context.Orders.Remove(order);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
