using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.Domain.Events;

public class PaymentApproved : DomainEvent
{
    public PaymentApproved(Guid id, string payoutId)
    {
        Id = id;
        PayoutId = payoutId;
    }

    public Guid Id { get; }
    
    public string PayoutId { get; }
}