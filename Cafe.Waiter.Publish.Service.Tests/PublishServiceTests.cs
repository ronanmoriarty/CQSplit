using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Publisher;
using CQRSTutorial.Tests.Common;
using log4net;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Publish.Service.Tests
{
    [TestFixture]
    public class PublishServiceTests
    {
        private readonly Guid _id = new Guid("6893FEEC-548F-4EB2-8306-6396C9BD2522");
        private readonly Guid _aggregateId = new Guid("E6CA5D4D-39EC-4E98-88D5-7444EAECF77E");
        private readonly Guid _commandId = new Guid("B72CF663-713C-4102-808C-955A9CF09E1B");
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private int _numberOfNotificationsReceived;
        private PublishService _publishService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PublishServiceTests));

        [SetUp]
        public void SetUp()
        {
            CleanUp();
            _numberOfNotificationsReceived = 0;
            var outboxToMessageQueuePublisher = Substitute.For<IOutboxToMessageQueuePublisher>();
            outboxToMessageQueuePublisher
                .When(x => x.PublishQueuedMessages())
                .Do(x =>
                    {
                        _numberOfNotificationsReceived++;
                        _manualResetEvent.Set();
                    }
            );
            _publishService = new PublishService(WriteModelConnectionStringProviderFactory.Instance, outboxToMessageQueuePublisher, new OutboxToMessageQueuePublisherConfiguration());
            _publishService.Start();
        }

        [Test]
        public void Notifications_received_when_EventsToPublish_table_changes()
        {
            var testEvent = CreateTestEvent();
            WhenEventQueuedForPublishing(testEvent);

            var unblockThreadSignalSent = _manualResetEvent.WaitOne(3000);
            _logger.Debug(unblockThreadSignalSent ? "New-events-published handler invoked" : "Timed out waiting for new-events-published handler to be invoked");
            Assert.That(_numberOfNotificationsReceived, Is.EqualTo(1));
        }

        [Test]
        public void Notifications_not_received_when_there_are_no_EventsToPublish_table_changes()
        {
            var unblockThreadSignalSent = _manualResetEvent.WaitOne(3000);
            _logger.Debug(unblockThreadSignalSent ? "New-events-published handler invoked" : "Timed out waiting for new-events-published handler to be invoked");
            if (_numberOfNotificationsReceived > 0)
            {
                OutputUnexpectedIdsFound();
            }

            Assert.That(_numberOfNotificationsReceived, Is.EqualTo(0));
        }

        private void OutputUnexpectedIdsFound()
        {
            var ids = new List<Guid>();
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteReader("SELECT Id FROM dbo.EventsToPublish_PublishServiceTests", reader => ids.Add(reader.GetGuid(reader.GetOrdinal("Id"))));
            foreach (var id in ids)
            {
                _logger.Debug($"Unexpected id found:{id}");
            }
        }

        private TestEvent CreateTestEvent()
        {
            return new TestEvent
            {
                Id = _id,
                AggregateId = _aggregateId,
                CommandId = _commandId,
                IntProperty = 123,
                StringProperty = "abc"
            };
        }

        private void WhenEventQueuedForPublishing(TestEvent testEvent)
        {
            using (var connection = new SqlConnection(WriteModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString()))
            {
                using (var command = new SqlCommand("INSERT INTO dbo.EventsToPublish_PublishServiceTests(Id, EventType, Data, Created) VALUES(@id, @eventType, @data, @created)", connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier) { Value = testEvent.Id });
                    command.Parameters.Add(new SqlParameter("@eventType", SqlDbType.NVarChar, 25) { Value = testEvent.GetType().Name });
                    command.Parameters.Add(new SqlParameter("@data", SqlDbType.NVarChar) { Value = JsonConvert.SerializeObject(testEvent) });
                    command.Parameters.Add(new SqlParameter("@created", SqlDbType.DateTime) { Value = DateTime.Now });

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            _publishService.Stop();
        }

        private void CleanUp()
        {
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish_PublishServiceTests WHERE Id = '{_id}'");
        }
    }
}
