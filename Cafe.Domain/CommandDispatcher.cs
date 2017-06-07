namespace Cafe.Domain
{
    public class CommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly CommandHandlerDictionary _commandHandlerDictionary;

        public CommandDispatcher(IEventPublisher eventPublisher, CommandHandlerDictionary commandHandlerDictionary)
        {
            _eventPublisher = eventPublisher;
            _commandHandlerDictionary = commandHandlerDictionary;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            var handler = _commandHandlerDictionary.GetHandlerFor<TCommand>();
            var events = handler.Handle(command);
            _eventPublisher.Publish(events);
        }
    }
}