using System.Threading.Tasks;
using CQRSTutorial.Core;

namespace CQRSTutorial.Messaging
{
    public interface ICommandSender
    {
        Task Send(ICommand command, string queueName);
    }
}