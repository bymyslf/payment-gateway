using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.UseCases.Common;

public interface IAggregateStore
{
    Task Save(AggregateRoot aggregateRoot);
}