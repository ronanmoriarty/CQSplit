using System;
using System.Threading;
using CQRSTutorial.Core;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NUnit.Framework;

namespace CQRSTutorial.Infrastructure.Tests
{
    [TestFixture]
    public class MessageBusEventPublisherTests
    {
        [Test, Explicit("Not a real automated test - just a quick way to verify if RabbitMQ is up and running on this environment.")]
        public void Can_publish_to_message_queue()
        {
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(), ConfigureTestReceiver));
            messageBusEventPublisher.Receive(new [] {new TestEvent()});
            Thread.Sleep(1000);
            messageBusEventPublisher.Stop();
        }

        public class TestEvent : IEvent
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
            public Guid CommandId { get; set; }
        }

        private void ConfigureTestReceiver(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            sbc.ReceiveEndpoint(host, "test_queue", ep =>
            {
                ep.Handler<IEvent>(context =>
                {
                    var messages = string.Join("\n", context.Message);
                    return Console.Out.WriteLineAsync($"Received: {messages}");
                });
            });
        }
    }
}