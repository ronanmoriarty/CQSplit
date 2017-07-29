using System;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Threading;
using CQRSTutorial.DAL.Tests;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
{
    [TestFixture, Ignore("Test works now, but can't have more than one call to SqlDependency.Start() in the one AppDomain.")]
    public class StackOverflowQuestion
    {
        private string _queueName = "EventsToPublishChangeMessages";
        private bool _notificationReceived;
        private string _connectionString;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _connectionString = GetConnectionString();
            var started = SqlDependency.Start(_connectionString, _queueName); // exception below seems to suggest that I haven't started the SqlDependency. Am I doing something wrong on this line?
            Console.WriteLine($"Started:{started}"); // returns true
        }

        [Test]
        public void WhyDoesExceptionIndcateSqlDependencyStartHasNotBeenCalledPriorToCommandExecuteReader()
        {
            Console.WriteLine($"canRequestNotifications: {CanRequestNotifications()}"); // returns true
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT Id, EventType, [Data], Created FROM dbo.EventsToPublish", connection);
            var sqlDependency = new SqlDependency(command, "SERVICE=EventsToPublishChangeNotifications", int.MaxValue);
            sqlDependency.OnChange += OnChange;
            connection.Open();

            // The following line causes the exception:
            // System.InvalidOperationException : When using SqlDependency without providing an options value, SqlDependency.Start() must be called prior to execution of a command added to the SqlDependency instance.
            // But you can see that I *did* call SqlDependency.Start() above.
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Process(reader);
                }
            }

            TryToTriggerANotification(_connectionString);
            Thread.Sleep(5000); // ie. wait a few seconds to ensure notification-handling background thread has had a chance to complete.

            Assert.That(_notificationReceived, Is.True);
        }

        private void TryToTriggerANotification(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(
                    "INSERT INTO dbo.EventsToPublish(Id, EventType, [Data], Created) VALUES(newid(), 'StackOverflowQuestionTest', '', GETDATE())",
                    connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void OnChange(object sender, SqlNotificationEventArgs e)
        {
            _notificationReceived = true;
        }

        private bool CanRequestNotifications()
        {
            try
            {
                var sqlClientPermission = new SqlClientPermission(PermissionState.Unrestricted);
                sqlClientPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetConnectionString()
        {
            return WriteModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString();
        }

        private void Process(SqlDataReader reader)
        {
            var id = reader.GetGuid(reader.GetOrdinal("Id"));
            var eventType = reader.GetString(reader.GetOrdinal("EventType"));
            var data = reader.GetString(reader.GetOrdinal("Data"));
            var created = reader.GetDateTime(reader.GetOrdinal("Created"));
        }
    }
}