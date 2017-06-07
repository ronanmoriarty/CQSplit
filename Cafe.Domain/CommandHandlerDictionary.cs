using System;
using System.Collections.Generic;

namespace Cafe.Domain
{
    public class CommandHandlerDictionary
    {
        private readonly Dictionary<Type, object> _commandHandlerMappings;

        public CommandHandlerDictionary(object[] commandHandlers)
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

        public ICommandHandler<TCommand> GetHandlerFor<TCommand>()
        {
            return _commandHandlerMappings[typeof(TCommand)] as ICommandHandler<TCommand>;
        }
    }
}