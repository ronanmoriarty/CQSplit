using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Events;
using Castle.MicroKernel.Registration;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Messaging;
using CQRSTutorial.Messaging.Tests;
using MassTransit;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class EndToEndTest
    {
        private readonly Guid _aggregateId = new Guid("D0F855BF-14BD-45C6-B099-953E9BE05EA4");
        private readonly Guid _commandId = new Guid("AAE03F19-60C3-400A-B97E-90B791E129C1");
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(WriteModelConnectionStringProvider.Instance);
        private IBusControl _busControl;
        private const string EventConsumingApplicationQueueName = "commandService.EndToEndTest.EventConsumingApplication"; // typically Cafe.Waiter.EventProjecting.Service but could be anything.
        private const string LoopbackAddress = "loopback://localhost/";
        private static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private string _queueName;

        [SetUp]
        public void SetUp()
        {
            Container.Reset();
            Bootstrapper.Start();
            _queueName = ConfigurationManager.AppSettings["QueueName"];
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE AggregateId = '{_aggregateId.ToString()}'");
            CreateBus();
            OverrideIoCRegistrationToUseInMemoryBus();
            StartBus();
        }

        private void CreateBus()
        {
            var registerCommandConsumers = new InMemoryReceiveEndpointsConfigurator(Container.Instance.Resolve<IConsumerRegistrar>());
            var registerEventConsumers = new InMemoryReceiveEndpointsConfigurator(
                new ConsumerRegistrar(
                    new DefaultConstructorConsumerFactory(),
                    new CQRSTutorial.Messaging.Tests.ConsumerTypeProvider(typeof(TabOpenedTestConsumer)),
                    new CQRSTutorial.Messaging.Tests.ReceiveEndpointConfiguration(EventConsumingApplicationQueueName)
                )
            );
            _busControl = new InMemoryMessageBusFactory(
                registerCommandConsumers,
                registerEventConsumers
            ).Create();
        }

        private class TabOpenedTestConsumer : IConsumer<TabOpened>
        {
            public async Task Consume(ConsumeContext<TabOpened> context)
            {
                ReceivedTabCreatedEvent = context.Message; AllowTestThreadToContinueToAssertions();
            }

            private void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }

            public static TabOpened ReceivedTabCreatedEvent { get; set; }
        }

        private void OverrideIoCRegistrationToUseInMemoryBus()
        {
            Container.Instance.Register(Component
                .For<IBusControl>()
                .Instance(_busControl)
                .IsDefault());
        }

        private void StartBus()
        {
            _busControl.Start();
        }

        [Test]
        public async Task CreateTabCommand_causes_TabCreated_event_to_be_published()
        {
            var openTabCommand = CreateOpenTabCommand();

            await Send(openTabCommand);
            WaitUntilBusHasProcessedMessageOrTimedOut(ManualResetEvent);

            var receivedTabCreatedEvent = TabOpenedTestConsumer.ReceivedTabCreatedEvent;
            Assert.That(receivedTabCreatedEvent, Is.Not.Null);
            Assert.That(receivedTabCreatedEvent.CommandId, Is.EqualTo(_commandId));
            Assert.That(receivedTabCreatedEvent.AggregateId, Is.EqualTo(_aggregateId));
        }

        private OpenTabCommand CreateOpenTabCommand()
        {
            return new OpenTabCommand
            {
                Id = _commandId,
                AggregateId = _aggregateId
            };
        }

        private async Task Send(object command)
        {
            var sendEndpoint = await GetSendEndpoint();
            await sendEndpoint.Send(command);
        }

        private async Task<ISendEndpoint> GetSendEndpoint()
        {
            return await _busControl.GetSendEndpoint(new Uri($"{LoopbackAddress}{_queueName}"));
        }

        private void WaitUntilBusHasProcessedMessageOrTimedOut(ManualResetEvent manualResetEvent)
        {
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}
