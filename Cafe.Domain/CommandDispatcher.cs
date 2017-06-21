using System;
using System.Collections.Generic;

namespace Cafe.Domain
{
    public class CommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private Dictionary<Type, object> _commandHandlerMappings;

        public CommandDispatcher(IEventPublisher eventPublisher, object[] commandHandlers)
        {
            _eventPublisher = eventPublisher;
            MapCommandTypesToCommandHandlerInstance(commandHandlers);
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            var handler = GetHandlerFor<TCommand>();
            var events = handler.Handle(command);
            _eventPublisher.Publish(events);
        }

        private void MapCommandTypesToCommandHandlerInstance(object[] commandHandlers)
        {
            _commandHandlerMappings = new Dictionary<Type, object>();
            foreach (var commandHandler in commandHandlers)
            {
                foreach (var interfaceType in commandHandler.GetType().GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                    {
                        var commandType = interfaceType.GenericTypeArguments[0];
                        if (_commandHandlerMappings.ContainsKey(commandType))
                        {
                            throw new ArgumentException($"More than one handler found for {commandType.FullName}");
                        }

                        _commandHandlerMappings.Add(commandType, commandHandler);
                    }
                }
            }
        }

        private ICommandHandler<TCommand> GetHandlerFor<TCommand>()
        {
            return _commandHandlerMappings[typeof(TCommand)] as ICommandHandler<TCommand>;
        }
    }
}