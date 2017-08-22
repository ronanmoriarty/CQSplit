using Cafe.Domain;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand testCommand) where TCommand : ICommand
        {
            return (ICommandHandler<TCommand>)new Tab
            {
                Id = testCommand.AggregateId
            };
        }
    }
}
