using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.ApiTests.Common;

public class AcquiringBankFake : IAcquiringBank
{
    public Task<PayoutResult> ProcessPayout(Payment payment, CancellationToken cancellationToken)
    {
        if (payment.Card.Number.Equals("4275765574319271"))
            return Task.FromResult(new PayoutResult {Succeeded = false, Message = "Lack funds"});
        
        return Task.FromResult(new PayoutResult {Succeeded = true, PayoutId = "b4e21265-ba74-40df-91c4-6b367d0b63af"});
    }
}