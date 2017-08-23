using System;

namespace CQRSTutorial.Infrastructure
{
    public interface IConsumerFactory
    {
        object Create(Type typeToCreate);
    }
}