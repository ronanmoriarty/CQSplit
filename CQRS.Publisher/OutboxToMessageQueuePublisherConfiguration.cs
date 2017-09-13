using System;
using System.Configuration;

namespace CQRSTutorial.Publisher
{
    public class OutboxToMessageQueuePublisherConfiguration : IOutboxToMessageQueuePublisherConfiguration
    {
        public int BatchSize => Convert.ToInt32(ConfigurationManager.AppSettings["BatchSize"]);
        public string QueueName => ConfigurationManager.AppSettings["QueueName"];
        public string QueryToWatch => ConfigurationManager.AppSettings["QueryToWatch"];
        public string ServiceName => ConfigurationManager.AppSettings["ServiceName"];
    }
}