using CQRSTutorial.Core;

namespace Cafe.Waiter.Web
{
    public interface IMessageBus
    {
        void Send(ICommand command);
    }
}