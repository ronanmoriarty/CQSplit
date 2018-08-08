using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Command.Service.Consumers;
using CQ.Messaging;

namespace Cafe.Waiter.Command.Service
{
    public class ConsumerTypeProvider : IConsumerTypeProvider
    {
        public List<Type> GetConsumerTypes()
        {
            return typeof(OpenTabConsumer)
                .Assembly
                .GetTypes()
                .Where(type => type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(CommandConsumer<>))
                .ToList();
        }
    }
}