namespace CQSplit
{
    public interface ICommandRouter
    {
        void Route<TCommand>(TCommand command) where TCommand : ICommand;
    }
}