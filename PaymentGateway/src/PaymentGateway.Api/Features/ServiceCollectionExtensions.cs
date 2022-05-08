using PaymentGateway.Core.UseCases.Common;
using PaymentGateway.Core.UseCases.GetPaymentDetails;
using PaymentGateway.Core.UseCases.PaymentRequest;
using PaymentGateway.Infrastructure.AcquiringBank;
using PaymentGateway.Infrastructure.AggregateStore;

namespace PaymentGateway.Api.Features;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentsDomain(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.AddScoped<ICommandHandler<PaymentRequest, PaymentRequestResult>, PaymentRequestHandler>();
        services.AddScoped<IQueryHandler<GetPayment, GetPaymentResult>, GetPaymentHandler>();
        
        return services;
    }
    
    public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IAcquiringBank, AcquiringBankClient>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ACQUIRING_BANK_ENDPOINT"));
            }
        );
        services.AddSingleton<IAggregateStore, InMemoryAggregateStore>();
        
        return services;
    }
}