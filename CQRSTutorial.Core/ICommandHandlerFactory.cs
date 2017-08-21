namespace CQRSTutorial.Core
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand testCommand)
            where TCommand : ICommand;
    }
}