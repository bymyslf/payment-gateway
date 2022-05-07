namespace PaymentGateway.Core.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _changes = new();
    
    public int Version { get; internal set; }

    protected void AddChange(DomainEvent @event)
        =>  _changes.Add(@event);

    public IEnumerable<DomainEvent> GetUncommittedChanges()
        => _changes;
    
    public void MarkChangesAsCommitted()
        => _changes.Clear();

    public void LoadsFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var e in history)
            Apply(e, false);
            
    }

    protected abstract void Apply(DomainEvent @event, bool isNew);
}