using Cafe.Domain;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEventStore _eventStore;

        public CommandHandlerFactory(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command) where TCommand : ICommand
        {
            var tab = new Tab
            {
                Id = command.AggregateId
            };

            _eventStore.GetAllEventsFor(command.AggregateId);
            return (ICommandHandler<TCommand>)tab;
        }
    }
}
