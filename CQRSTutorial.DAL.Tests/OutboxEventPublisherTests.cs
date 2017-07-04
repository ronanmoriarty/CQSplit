using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        [SetUp]
        public void SetUp()
        {
            _outboxEventPublisher = new OutboxEventPublisher(SessionFactory.WriteInstance, new EventRepository(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted));
        }

        [Test]
        public void Published_events_get_saved_to_database()
        {
            _tabOpened = new TabOpened
            {
                TableNumber = _tableNumber,
                Waiter = _waiter
            };

            _outboxEventPublisher.Publish(new []
            {
                _tabOpened
            });

            AssertThatEventSaved();
        }

        private void AssertThatEventSaved()
        {
            var numberOfEventsInserted = ExecuteScalar($"SELECT COUNT(*) FROM dbo.Events WHERE Id = '{_tabOpened.Id}'");
            Assert.That(numberOfEventsInserted, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            ExecuteNonQuery($"DELETE FROM dbo.Events WHERE Id = '{_tabOpened.Id}'");
        }

        private int ExecuteScalar(string commandText)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    return (int) command.ExecuteScalar();
                }
            }
        }

        private void ExecuteNonQuery(string commandText)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
