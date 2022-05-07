using Microsoft.Extensions.Logging;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.Common;

namespace PaymentGateway.Core.UseCases.PaymentRequest;

public class PaymentRequestHandler : ICommandHandler<PaymentRequest, PaymentRequestResult>
{
    private readonly IAggregateStore _aggregateStore;
    private readonly IAcquiringBank _acquiringBank;
    private readonly ILogger<PaymentRequestHandler> _logger;
    
    public PaymentRequestHandler(
        IAggregateStore aggregateStore, 
        IAcquiringBank acquiringBank, 
        ILogger<PaymentRequestHandler> logger)
    {
        _aggregateStore = aggregateStore;
        _acquiringBank = acquiringBank;
        _logger = logger;
    }
    
    public async Task<PaymentRequestResult> Handle(PaymentRequest paymentRequest, CancellationToken cancellation)
    {
        _logger.LogInformation("Process payment request started");

        var paymentCardNumber = new PaymentCardNumber(paymentRequest.Card.Number);
        var paymentCard = new PaymentCard(
            paymentCardNumber,
            paymentRequest.Card.Type.ToEnum<PaymentCardType>(),
            paymentRequest.Card.HolderName,
            paymentRequest.Card.ExpireMonth,
            paymentRequest.Card.ExpireYear,
            paymentRequest.Card.Cvv);
        
        var payment = new Payment(paymentRequest.Amount, paymentRequest.Currency, paymentCard);

        _logger.LogInformation("Storing pending payment request");
        await _aggregateStore.Save(payment, cancellation);

        _logger.LogInformation("Send payment to acquiring bank");
        var result = await _acquiringBank.ProcessPayout(payment, cancellation);

        if (result.Succeeded)
        {
            _logger.LogDebug("Payment approved");
            payment.Approve(result.PayoutId);
        }
        else
        {
            _logger.LogDebug("Payment declined");
            payment.Decline(result.Message);
        }

        _logger.LogInformation("Storing processed payment request");
        await _aggregateStore.Save(payment, cancellation);

        _logger.LogInformation("Process payment request finished");
        
        return new PaymentRequestResult(payment.Id, payment.Status);
    }
}