using System;
using System.Collections.Generic;

namespace CQSplit.Messaging
{
    public interface IConsumerTypeProvider
    {
        List<Type> GetConsumerTypes();
    }
}