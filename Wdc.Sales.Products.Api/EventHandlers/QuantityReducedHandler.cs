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
        List<Product> products = await context.Products.ToListAsync(cancellationToken);

        Product? product = products.SingleOrDefault(x => x.Id == @event.AggregateId);

        if (product is null || product.Sequence + 1 > @event.Sequence) return true;

        if (product.Sequence + 1 < @event.Sequence)
            return false;

        product.UpdateQuantity(@event.Data.Quantity);

        products.ForEach(p => p.UpdateSequence(@event.Sequence));

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
