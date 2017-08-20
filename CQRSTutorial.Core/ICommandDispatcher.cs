namespace CQRSTutorial.Core
{
    public interface ICommandDispatcher
    {
        void Dispatch(ICommand command);
    }
}