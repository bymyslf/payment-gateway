using PaymentGateway.Core.Domain.Common;
using PaymentGateway.Core.Domain.Events;

namespace PaymentGateway.Core.Domain;

public class Payment : AggregateRoot
{
    public Payment(IEnumerable<DomainEvent> history)
        => LoadsFromHistory(history);
    
    public Payment(int amount, string currency, PaymentCard card)
    {
        Guard.Against<ArgumentException>(amount < 0, "Payment amount cannot be negative");
        Guard.Against<ArgumentException>(!IsIso4217Currency(currency), "Payment currency is required");
        Guard.Against<ArgumentException>(card is null, "Payment card is required");

        Id = Guid.NewGuid();
        Amount = amount;
        Currency = currency;
        Card = card;
        Status = PaymentStatus.Pending;
        
        Apply(new PaymentRequested(Id, amount, currency, card));
    }

    public PaymentStatus Status { get; private set; }
    
    public int Amount { get; private set; }

    public string Currency { get; private set; }

    public PaymentCard Card { get; private set; }

    public DateTime ProcessedOn { get; private set; }

    public string PayoutId { get; private set; }
    
    public string DeclinedReason { get; private set; }

    public void Approve(string payoutId)
    {
        Guard.Against<ArgumentException>(string.IsNullOrWhiteSpace(payoutId), "Payout ID is required");
        Apply(new PaymentApproved(Id, payoutId));
    }

    public void Decline(string reason)
    {
        Guard.Against<ArgumentException>(string.IsNullOrWhiteSpace(reason), "Decline reason is required");
        Apply(new PaymentDeclined(Id, reason));
    }

    private void When(PaymentRequested @event)
    {
        Status = PaymentStatus.Pending;
        Id = @event.Id;
        Amount = @event.Amount;
        Currency = @event.Currency;
        Card = @event.Card;
    }

    private void When(PaymentApproved @event)
    {
        Status = PaymentStatus.Approved;
        PayoutId = @event.PayoutId;
        ProcessedOn = DateTime.Now;
    }

    private void When(PaymentDeclined @event)
    {
        Status = PaymentStatus.Declined;
        DeclinedReason = @event.Reason;
        ProcessedOn = DateTime.Now;
    }

    private void Apply(DomainEvent @event)
        => Apply(@event, true);

    protected override void Apply(DomainEvent @event, bool isNew)
    {
        if (isNew)
            @event.Version = Version + 1;

        dynamic dynamicThis = this;
        dynamicThis.When(@event as dynamic);
        Version = @event.Version;

        if (isNew)
            AddChange(@event);
    }
    
    private static bool IsIso4217Currency(string currency)
    {
        currency = (currency ?? string.Empty).Trim();

        if (currency.Length != 3)
            return false;

        return true;
    }
}
