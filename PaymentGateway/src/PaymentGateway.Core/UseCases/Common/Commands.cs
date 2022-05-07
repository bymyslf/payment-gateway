using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Core.UseCases.Common;

public interface ICommandHandler<in TCommand, TCommandResult>
{
    Task<TCommandResult> Handle(TCommand query, CancellationToken cancellation);
}

public interface ICommandDispatcher
{
    Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation);
}

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        return handler.Handle(command, cancellation);
    }
}