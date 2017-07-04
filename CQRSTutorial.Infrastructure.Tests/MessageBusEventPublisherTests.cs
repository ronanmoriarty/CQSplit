using System;
using System.Threading;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace CQRSTutorial.Infrastructure.Tests
{
    [TestFixture]
    public class MessageBusEventPublisherTests
    {
        [Test, Explicit]
        public void Test()
        {
            var messageBusEventPublisher = new MessageBusEventPublisher();
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