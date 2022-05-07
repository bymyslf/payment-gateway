using System.Collections.Concurrent;
using PaymentGateway.Core.Domain.Common;
using PaymentGateway.Core.UseCases.Common;

namespace PaymentGateway.Infrastructure.AggregateStore;

// Based on: https://github.com/gregoryyoung/m-r/blob/master/SimpleCQRS/EventStore.cs
public class InMemoryAggregateStore : IAggregateStore
{
    private readonly ConcurrentDictionary<Guid, List<EventDescriptor>> _current = new();

    public InMemoryAggregateStore()
    {
    }

    public Task Save(AggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        var aggregateId = aggregateRoot.Id;
        if (!_current.TryGetValue(aggregateId, out var eventDescriptors))
        {
            eventDescriptors = new List<EventDescriptor>();
            _current.GetOrAdd(aggregateId, eventDescriptors);
        }

        var events = aggregateRoot.GetUncommittedChanges();
        foreach (var @event in events)
            eventDescriptors.Add(new EventDescriptor(aggregateId, @event, @event.Version));

        aggregateRoot.MarkChangesAsCommitted();

        return Task.CompletedTask;
    }

    public Task<T> Load<T>(Guid aggregateId, CancellationToken cancellationToken) 
        where T : AggregateRoot
    {
        var aggregateEvents = GetEventsForAggregate(aggregateId);
        
        return Task.FromResult(CreateAggregate<T>(aggregateEvents));
    }

    private List<DomainEvent> GetEventsForAggregate(Guid aggregateId)
    {
        if (!_current.TryGetValue(aggregateId, out var eventDescriptors))
            throw new InvalidOperationException("Aggregate not found");

        return eventDescriptors.Select(desc => desc.EventData).ToList();
    }

    private T CreateAggregate<T>(IEnumerable<DomainEvent> events)
    {
        var constructor = typeof(T).GetConstructor(new[] { typeof(IEnumerable<DomainEvent>) });
        if (constructor == null)
            throw new InvalidCastException($"Type {typeof(T)} must have a constructor with the following signature: .ctor(IEnumerable<DomainEvent>)");

        return (T)constructor.Invoke(new object[] { events });
    }
    
    private readonly struct EventDescriptor
    {
        public EventDescriptor(Guid id, DomainEvent eventData, int version)
        {
            EventData = eventData;
            Version = version;
            Id = id;
        }

        public Guid Id { get; }

        public DomainEvent EventData { get; }

        public int Version { get; }
    }
}