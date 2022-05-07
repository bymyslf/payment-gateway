using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.UseCases.Common;

public interface IAggregateStore
{
    Task Save(AggregateRoot aggregateRoot);

    Task<T> Load<T>(Guid aggregateId)
        where T : AggregateRoot;
}