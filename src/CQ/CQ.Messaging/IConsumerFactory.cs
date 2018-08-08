using System;

namespace CQ.Messaging
{
    public interface IConsumerFactory
    {
        object Create(Type typeToCreate);
    }
}