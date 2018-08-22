namespace CQSplit
{
    public interface ICommandHandlerProvider
    {
        ICommandHandler<TCommand> GetCommandHandler<TCommand>(TCommand command) where TCommand : ICommand;
        void RegisterCommandHandler(ICommandHandler commandHandler);
    }
}
