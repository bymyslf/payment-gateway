using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.UseCases.Common;

public interface IAggregateStore
{
    Task Save(AggregateRoot aggregateRoot, CancellationToken cancellationToken = default);

    Task<T> Load<T>(Guid aggregateId, CancellationToken cancellationToken = default)
        where T : AggregateRoot;
}