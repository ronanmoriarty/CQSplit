using System;
using System.Collections.Generic;

namespace CQ.Messaging
{
    public interface IConsumerTypeProvider
    {
        List<Type> GetConsumerTypes();
    }
}