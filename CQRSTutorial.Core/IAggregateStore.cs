namespace CQRSTutorial.Core
{
    public interface IAggregateStore
    {
        ICommandHandler GetCommandHandler(ICommand command);
    }
}
