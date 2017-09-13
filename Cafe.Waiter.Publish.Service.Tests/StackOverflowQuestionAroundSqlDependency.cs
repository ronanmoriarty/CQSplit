using System;
using System.Data.SqlClient;
using System.Threading;
using NUnit.Framework;

namespace Cafe.Waiter.Publish.Service.Tests
{
    [TestFixture]
    public class StackOverflowQuestionAroundSqlDependency
    {
        private readonly string _connectionString = WriteModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString();
        private readonly string _id = "FCB78962-7CC6-4094-B97E-78145E8C5BC2";
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        [Test]
        public void OnChange_event_gets_triggered_when_table_being_watched_changes()
        {
            ExecuteNonQuery("DELETE FROM dbo.SOTest");
            var _queue = "SOTestChangeMessages";
            SqlDependency.Start(_connectionString, _queue);

            try
            {
                var sqlConnection = new SqlConnection(_connectionString);
                var command = new SqlCommand("SELECT Id FROM dbo.SOTest", sqlConnection);
                var sqlDependency = new SqlDependency(command, "SERVICE=SOTestChangeNotifications", int.MaxValue);
                var changeNotificationReceived = false;
                sqlDependency.OnChange += (sender, e) =>
                {
                    Console.WriteLine($"OnChange fired. Info: {e.Info}");
                    if (e.Info == SqlNotificationInfo.Insert)
                    {
                        changeNotificationReceived = true;
                        _manualResetEvent.Set();
                    }
                };
                sqlConnection.Open();
                command.ExecuteReader();

                // this should trigger the OnChange event above.
                ExecuteNonQuery($"INSERT INTO dbo.SOTest(Id) VALUES('{_id}')");

                // Wait until change event handling thread completes - timeout after 3 seconds at latest.
                _manualResetEvent.WaitOne(3000);

                // But this assertion still fails
                Assert.That(changeNotificationReceived, Is.True);
            }
            finally
            {
                SqlDependency.Stop(_connectionString);
            }
        }

        public void ExecuteNonQuery(string commandText)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
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