using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Tests.Common;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
{
    [TestFixture]
    public class PublishServiceTests
    {
        private readonly Guid _id = new Guid("6893FEEC-548F-4EB2-8306-6396C9BD2522");
        private readonly Guid _aggregateId = new Guid("E6CA5D4D-39EC-4E98-88D5-7444EAECF77E");
        private readonly Guid _commandId = new Guid("B72CF663-713C-4102-808C-955A9CF09E1B");
        private EventToPublishRepository _eventToPublishRepository;
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private int _numberOfNotificationsReceived;
        private PublishService _publishService;
        private OutboxToMessageQueuePublisherConfiguration _outboxToMessageQueuePublisherConfiguration;
        private string _connectionString;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _outboxToMessageQueuePublisherConfiguration = new OutboxToMessageQueuePublisherConfiguration();
            _connectionString = WriteModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString();
            SqlDependency.Start(_connectionString, _outboxToMessageQueuePublisherConfiguration.QueueName);
        }

        [SetUp]
        public void SetUp()
        {
            CleanUp();
            _eventToPublishRepository = new EventToPublishRepository(SessionFactory.Instance, new EventToPublishMapper(typeof(TestEvent).Assembly));
            _numberOfNotificationsReceived = 0;
            _publishService = new PublishService(
                WriteModelConnectionStringProviderFactory.Instance,
                () =>
                {
                    _numberOfNotificationsReceived++;
                    _manualResetEvent.Set();
                });
            _publishService.Start();
        }

        [Test]
        public void Notifications_received_when_EventsToPublish_table_changes()
        {
            var testEvent = CreateTestEvent();
            WhenEventQueuedForPublishing(testEvent);

            var unblockThreadSignalSent = _manualResetEvent.WaitOne(3000);
            Console.WriteLine(unblockThreadSignalSent ? "New-events-published handler invoked" : "Timed out waiting for new-events-published handler to be invoked");
            Assert.That(_numberOfNotificationsReceived, Is.EqualTo(1));
        }

        [Test]
        public void Notifications_not_received_when_there_are_no_EventsToPublish_table_changes()
        {
            var unblockThreadSignalSent = _manualResetEvent.WaitOne(3000);
            Console.WriteLine(unblockThreadSignalSent ? "New-events-published handler invoked" : "Timed out waiting for new-events-published handler to be invoked");
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
            sqlExecutor.ExecuteReader("SELECT Id FROM dbo.EventsToPublish", reader => ids.Add(reader.GetGuid(reader.GetOrdinal("Id"))));
            foreach (var id in ids)
            {
                Console.WriteLine($"Unexpected id found:{id}");
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
            using (var session = SessionFactory.Instance.OpenSession())
            {
                using (var unitOfWork = new NHibernateUnitOfWork(session))
                {
                    unitOfWork.Start();
                    _eventToPublishRepository.UnitOfWork = unitOfWork;
                    _eventToPublishRepository.Add(testEvent);
                    unitOfWork.Commit();
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            _publishService.Stop();
            CleanUp(); // would rather leave these rows here for investigation if tests ever fail, but these records are causing problems for other integration tests.
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            SqlDependency.Stop(_connectionString, _outboxToMessageQueuePublisherConfiguration.QueueName);
        }

        private void CleanUp()
        {
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id = '{_id}'");
        }
    }
}
