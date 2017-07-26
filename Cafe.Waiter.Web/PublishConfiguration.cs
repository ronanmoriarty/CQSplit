using System;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Web
{
    public class PublishConfiguration : IPublishConfiguration
    {
        public string GetPublishLocationFor(Type typeToPublish)
        {
            return "whatevr"; // TODO: might be getting rid of this concept shortly - need to investigate topics / queues / message routing a bit more with MassTransit.
        }
    }
}