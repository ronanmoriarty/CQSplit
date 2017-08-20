namespace CQRSTutorial.Core
{
    public interface IAggregateStore
    {
        ICommandHandler<TCommand> GetCommandHandler<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
