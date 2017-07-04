using System.Data;
using Cafe.Domain.Events;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class OutboxEventPublisherTests
    {
        private OutboxEventPublisher _outboxEventPublisher;
        private int _tableNumber = 123;
        private string _waiter = "John";
        private TabOpened _tabOpened;
        private SqlExecutor _sqlExecutor;

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor = new SqlExecutor();
            _outboxEventPublisher = new OutboxEventPublisher(SessionFactory.WriteInstance, new EventRepository(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted));
        }

        [Test]
        public void Published_events_get_saved_to_database()
        {
            try
            {
                _tabOpened = new TabOpened
                {
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                };

                _outboxEventPublisher.Publish(new[]
                {
                _tabOpened
            });

                AssertThatEventSaved();
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEvent();
            }
        }
        
        private void AssertThatEventSaved()
        {
            var numberOfEventsInserted = _sqlExecutor.ExecuteScalar($"SELECT COUNT(*) FROM dbo.Events WHERE Id = '{_tabOpened.Id}'");
            Assert.That(numberOfEventsInserted, Is.EqualTo(1));
        }

        private void DeleteNewlyInsertedTabOpenedEvent()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE Id = '{_tabOpened.Id}'");
        }
    }
}
