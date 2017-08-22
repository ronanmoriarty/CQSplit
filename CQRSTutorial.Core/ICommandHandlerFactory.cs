namespace CQRSTutorial.Core
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}