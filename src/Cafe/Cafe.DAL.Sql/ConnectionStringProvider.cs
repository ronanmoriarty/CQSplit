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
                const int maximum = 24;
                const int delayInMilliseconds = 5000;

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
                        if (numberOfAttempts < maximum)
                        {
                            Logger.Debug($"[Attempt {numberOfAttempts} of {maximum}]: Could not establish connection to database. Will try again in {delayInMilliseconds} ms.");
                            Thread.Sleep(delayInMilliseconds);
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
    }
}
