using System;
using System.Collections.Generic;
using System.Linq;

namespace CQSplit.Messaging.Tests.Common
{
    public class ConsumerTypeProvider : IConsumerTypeProvider
    {
        private readonly Type[] _consumerTypes;

        public ConsumerTypeProvider(params Type[] consumerTypes)
        {
            _consumerTypes = consumerTypes;
        }

        public List<Type> GetConsumerTypes()
        {
            return _consumerTypes.ToList();
        }
    }
}