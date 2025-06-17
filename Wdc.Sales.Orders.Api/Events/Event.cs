using MediatR;

namespace Wdc.Sales.Orders.Api.Events;

public abstract class Event : IRequest<bool>
{
    public required string AggregateId { get; init; }
    public required int Sequence { get; init; }
    public required DateTime DateTime { get; init; }
    public required int Version { get; init; }
    public required string UserId { get; init; }
    public TEvent As<TEvent>() where TEvent : Event => (TEvent)this;
}

public abstract class Event<T> : Event
{
    public required T Data { get; init; }
}

