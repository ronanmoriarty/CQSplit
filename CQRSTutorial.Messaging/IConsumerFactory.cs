using System;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerFactory
    {
        object Create(Type typeToCreate);
    }
}