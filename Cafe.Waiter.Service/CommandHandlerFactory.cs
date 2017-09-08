using Cafe.Waiter.Domain;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using NHibernate.Util;

namespace Cafe.Waiter.Service
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventApplier _eventApplier;

        public CommandHandlerFactory(IEventRepository eventRepository, IEventApplier eventApplier)
        {
            _eventRepository = eventRepository;
            _eventApplier = eventApplier;
        }

        public ICommandHandler<TCommand> CreateHandlerFor<TCommand>(TCommand command) where TCommand : ICommand
        {
            var tab = new Tab
            {
                Id = command.AggregateId
            };
            var events = _eventRepository.GetAllEventsFor(command.AggregateId);
            events.ForEach(@event => _eventApplier.ApplyEvent(@event, tab));
            return (ICommandHandler<TCommand>)tab;
        }
    }
}
