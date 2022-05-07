using Microsoft.Extensions.Logging;
using PaymentGateway.Core.UseCases.Common;

namespace PaymentGateway.Core.UseCases.GetPaymentDetails;

public class GetPaymentHandler : IQueryHandler<GetPayment, GetPaymentResult>
{
    private readonly IAggregateStore _aggregateStore;
    private readonly ILogger<GetPaymentHandler> _logger;
    
    public GetPaymentHandler(
        IAggregateStore aggregateStore,
        ILogger<GetPaymentHandler> logger)
    {
        _aggregateStore = aggregateStore;
        _logger = logger;
    }
    
    public async Task<GetPaymentResult> Handle(GetPayment query, CancellationToken cancellation)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object> { ["PaymentId"] =  query.PaymentId });
        
        _logger.LogInformation("Getting payment from store");
        
        var payment = await _aggregateStore.Load<Domain.Payment>(query.PaymentId, cancellation);
        if (payment is object)
        {
            return GetPaymentResult.FromDomain(payment);
        }
        
        _logger.LogInformation("Payment not found");
        
        return default;
    }
}