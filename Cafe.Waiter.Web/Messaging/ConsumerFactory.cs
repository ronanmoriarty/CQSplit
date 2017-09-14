using System;
using Cafe.Waiter.Web.DependencyInjection;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Web.Messaging
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}