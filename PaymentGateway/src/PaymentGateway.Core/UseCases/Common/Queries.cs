using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Core.UseCases.Common;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellation);
}

public interface IQueryDispatcher
{
    Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation);
}

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation)
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.Handle(query, cancellation);
    }
}
