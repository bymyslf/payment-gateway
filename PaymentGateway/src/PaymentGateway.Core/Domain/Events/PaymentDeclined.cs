using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.Domain.Events;

public class PaymentDeclined : DomainEvent
{
    public PaymentDeclined(Guid id, string reason)
    {
        Id = id;
        Reason = reason;
    }

    public Guid Id { get; }
    
    public string Reason { get; }
}