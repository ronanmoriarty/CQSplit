using System;
using System.Configuration;
using CQRSTutorial.Publish;

namespace Cafe.Waiter.Command.Service
{
    public class OutboxToMessageQueuePublisherConfiguration : IOutboxToMessageQueuePublisherConfiguration
    {
        public int BatchSize => Convert.ToInt32(ConfigurationManager.AppSettings["BatchSize"]);
    }
}