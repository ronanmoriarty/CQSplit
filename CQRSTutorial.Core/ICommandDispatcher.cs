namespace CQRSTutorial.Core
{
    public interface ICommandDispatcher
    {
        void Dispatch(params ICommand[] commands);
    }
}