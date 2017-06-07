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
                        _commandHandlerMappings.Add(commandType, commandHandler);
                    }
                }
            }
        }

        public ICommandHandler<TCommand> GetHandlerFor<TCommand>()
        {
            var commandHandler = _commandHandlerMappings[typeof(TCommand)] as ICommandHandler<TCommand>;
            if (commandHandler == null)
            {
               throw new Exception($"Could not find handler for {typeof(TCommand).FullName} command.");
            }

            return commandHandler;
        }
    }
}