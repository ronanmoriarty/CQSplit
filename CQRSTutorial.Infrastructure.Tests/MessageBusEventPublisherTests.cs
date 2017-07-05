using System;
using System.Threading;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace CQRSTutorial.Infrastructure.Tests
{
    [TestFixture]
    public class MessageBusEventPublisherTests
    {
        [Test, Explicit("Not a real automated test - just a quick way to verify if RabbitMQ is up and running on this environment.")]
        public void Can_publish_to_message_queue()
        {
            var messageBusEventPublisher = new MessageBusEventPublisher(new EnvironmentVariableMessageBusConfiguration());
            messageBusEventPublisher.Publish(new [] {new TestEvent()});
            Thread.Sleep(1000);
            messageBusEventPublisher.Stop();
        }

        public class TestEvent : IEvent
        {
            public Guid Id { get; set; }
        }
    }
}