using System.Threading.Tasks;

namespace CQSplit.Messaging
{
    public interface ICommandSender
    {
        Task Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}