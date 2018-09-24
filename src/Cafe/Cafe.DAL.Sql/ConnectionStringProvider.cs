using System;
using System.Data.SqlClient;
using System.Threading;
using NLog;

namespace Cafe.DAL.Sql
{
    public static class ConnectionStringProvider
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Lazy<string> LazyConnectionString = new Lazy<string>(GetConnectionString);
        private static readonly object LockObj = new object();

        public static string ConnectionString => LazyConnectionString.Value;

        private static string GetConnectionString()
        {
            lock (LockObj)
            {
                var connectionString = ConfigurationRoot.Instance["connectionString"];
                var numberOfAttempts = 1;

                while (true)
                {
                    try
                    {
                        using (var sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                        }

                        return connectionString; // connection opened ok, so we can return it.
                    }
                    catch (SqlException)
                    {
                        if (numberOfAttempts < Maximum)
                        {
                            Logger.Debug($"[Attempt {numberOfAttempts} of {Maximum}]: Could not establish connection to database. Will try again in {DelayInMillisecondsBetweenSqlConnectionRetries} ms.");
                            Thread.Sleep(DelayInMillisecondsBetweenSqlConnectionRetries);
                        }
                        else
                        {
                            throw new Exception($"Could not establish connection to database after {numberOfAttempts} attempts.");
                        }
                    }
                    numberOfAttempts++;
                }
            }
        }

        private static int Maximum => Convert.ToInt32(ConfigurationRoot.Instance["maximumSqlConnectionRetries"]);
        private static int DelayInMillisecondsBetweenSqlConnectionRetries => Convert.ToInt32(ConfigurationRoot.Instance["delayInMillisecondsBetweenSqlConnectionRetries"]);
    }
}
