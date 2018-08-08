using System.Threading.Tasks;
using CQ.Core;

namespace CQ.Messaging
{
    public interface ICommandSender
    {
        Task Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}