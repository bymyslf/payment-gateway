using PaymentGateway.Core.Domain;

namespace PaymentGateway.Core.UseCases.PaymentRequest;

public interface IAcquiringBank
{
    Task<PayoutResult> ProcessPayout(Payment payment, CancellationToken cancellationToken);
}

public class PayoutResult
{
    public string PayoutId { get; set; }
    
    public bool Succeeded { get; set; }

    public string Message { get; set; }
}