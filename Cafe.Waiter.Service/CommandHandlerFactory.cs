using CQRSTutorial.Core;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEventStore _eventStore;
        private readonly ITabFactory _tabFactory;

        public CommandHandlerFactory(IEventStore eventStore, ITabFactory tabFactory)
        {
            _eventStore = eventStore;
            _tabFactory = tabFactory;
        }

        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command) where TCommand : ICommand
        {
            var tab = _tabFactory.Create(command.AggregateId);

            _eventStore.GetAllEventsFor(command.AggregateId);
            return (ICommandHandler<TCommand>)tab;
        }
    }
}
