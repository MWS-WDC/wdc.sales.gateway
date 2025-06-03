using MediatR;
using Microsoft.EntityFrameworkCore;
using Wdc.Sales.Products.Api.Entities;
using Wdc.Sales.Products.Api.Events;
using Wdc.Sales.Products.Api.Persistence;

namespace Wdc.Sales.Products.Api.EventHandlers;

public class QuantityReducedHandler(AppDbContext context) : IRequestHandler<QuantityReduced, bool>
{
    public async Task<bool> Handle(QuantityReduced @event, CancellationToken cancellationToken)
    {
        Product? product = await context.Products.FirstOrDefaultAsync(x => x.Id == @event.AggregateId, cancellationToken);

        if (product is null || product.Sequence + 1 < @event.Sequence)
            return false;

        if (product.Sequence + 1 > @event.Sequence) return true;

        product.UpdateQuantity(@event.Data.Quantity, @event.Sequence);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
