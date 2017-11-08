using System;
using System.Collections.Generic;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerTypeProvider
    {
        List<Type> GetConsumerTypes();
    }
}