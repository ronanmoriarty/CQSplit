using System.Threading.Tasks;
using CQSplit.Core;

namespace CQSplit.Messaging
{
    public interface ICommandSender
    {
        Task Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}