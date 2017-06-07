using System.Collections.Generic;

namespace Cafe.Domain
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
    {
        private readonly IEventPublisher _eventPublisher;

        protected CommandHandlerBase(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public void Handle(TCommand command)
        {
            var events = OnCommandReceived(command);
            _eventPublisher.Publish(events);
        }

        protected abstract IEnumerable<Event> OnCommandReceived(TCommand command);
    }
}