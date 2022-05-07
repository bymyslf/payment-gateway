using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.Domain.Events;

public class PaymentRequested : DomainEvent
{
    public PaymentRequested(Guid id, int amount, string currency, PaymentCard card)
    {
        Id = id;
        Version = 1;
        Amount = amount;
        Currency = currency;
        Card = card;
    }

    public Guid Id { get; }
    
    public int Amount { get; }
    
    public string Currency { get; }
    
    public PaymentCard Card { get;  }
}