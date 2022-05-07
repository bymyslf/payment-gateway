using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.UnitTests.UseCases;

public class AcquiringBankSpy : IAcquiringBank
{
    public ConcurrentDictionary<Guid, Payment> Requests { get; set; } = new();

    public Task<PayoutResult> ProcessPayout(Payment payment, CancellationToken cancellationToken)
    {
        Requests.TryAdd(payment.Id, payment);
        return Task.FromResult(new PayoutResult { PayoutId = Guid.NewGuid().ToString(), Succeeded = true });
    }
}