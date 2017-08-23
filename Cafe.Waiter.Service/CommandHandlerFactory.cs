using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using NHibernate.Util;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEventStore _eventStore;
        private readonly ITabFactory _tabFactory;
        private readonly IEventApplier _eventApplier;

        public CommandHandlerFactory(ITabFactory tabFactory, IEventStore eventStore, IEventApplier eventApplier)
        {
            _eventStore = eventStore;
            _tabFactory = tabFactory;
            _eventApplier = eventApplier;
        }

        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command) where TCommand : ICommand
        {
            var tab = _tabFactory.Create(command.AggregateId);
            var events = _eventStore.GetAllEventsFor(command.AggregateId);
            events.ForEach(@event => _eventApplier.ApplyEvent(@event, tab));
            return (ICommandHandler<TCommand>)tab;
        }
    }
}
