using System;
using System.Configuration;

namespace CQRSTutorial.Publisher
{
    public class OutboxToMessageQueuePublisherConfiguration : IOutboxToMessageQueuePublisherConfiguration
    {
        public int BatchSize => Convert.ToInt32(ConfigurationManager.AppSettings["BatchSize"]);
    }
}