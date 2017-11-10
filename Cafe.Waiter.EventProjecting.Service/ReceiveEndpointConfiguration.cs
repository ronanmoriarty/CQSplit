using System.Configuration;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class ReceiveEndpointConfiguration : IReceiveEndpointConfiguration
    {
        public string QueueName { get; } = ConfigurationManager.AppSettings["QueueName"];
    }
}