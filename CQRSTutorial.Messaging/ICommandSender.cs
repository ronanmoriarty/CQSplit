using System.Threading.Tasks;
using CQRSTutorial.Core;

namespace CQRSTutorial.Messaging
{
    public interface ICommandSender
    {
        Task Send<TCommand>(TCommand command, string queueName)
            where TCommand : class, ICommand;
    }
}