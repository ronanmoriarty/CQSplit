using System;

namespace CQSplit.Messaging
{
    public interface IConsumerFactory
    {
        object Create(Type typeToCreate);
    }
}