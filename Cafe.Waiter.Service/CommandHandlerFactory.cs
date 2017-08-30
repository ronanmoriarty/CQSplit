using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using NHibernate.Util;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITabFactory _tabFactory;
        private readonly IEventApplier _eventApplier;

        public CommandHandlerFactory(ITabFactory tabFactory, IEventRepository eventRepository, IEventApplier eventApplier)
        {
            _eventRepository = eventRepository;
            _tabFactory = tabFactory;
            _eventApplier = eventApplier;
        }

        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command) where TCommand : ICommand
        {
            var tab = _tabFactory.Create(command.AggregateId);
            var events = _eventRepository.GetAllEventsFor(command.AggregateId);
            events.ForEach(@event => _eventApplier.ApplyEvent(@event, tab));
            return (ICommandHandler<TCommand>)tab;
        }
    }
}
