using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Core.Domain.Common;
using PaymentGateway.Core.UseCases.Common;

namespace PaymentGateway.UnitTests.UseCases;

public class AggregateStoreStub: IAggregateStore
{
    public Task Save(AggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<T> Load<T>(Guid aggregateId, CancellationToken cancellationToken = default) where T : AggregateRoot
        => Task.FromResult<T>(default);
}