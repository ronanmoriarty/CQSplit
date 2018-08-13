using System;
using System.Collections.Generic;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using CQSplit.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class ConsumerTypeProvider : IConsumerTypeProvider
    {
        public List<Type> GetConsumerTypes()
        {
            var consumerTypes = new List<Type>();
            consumerTypes.AddRange(new[]
            {
                typeof(TabOpenedEventConsumer)
            });

            return consumerTypes;
        }
    }
}