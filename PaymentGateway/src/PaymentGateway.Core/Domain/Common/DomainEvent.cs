namespace PaymentGateway.Core.Domain.Common;

public abstract class DomainEvent
{
    public int Version { get; set; }
    
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    
}