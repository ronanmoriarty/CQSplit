using System;
using System.Data.SqlClient;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SqlExecutor
    {
        public T ExecuteScalar<T>(string commandText)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionStringProviderFactory().GetConnectionStringProvider().GetConnectionString()))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    var executeScalar = command.ExecuteScalar();
                    return executeScalar != DBNull.Value ? (T)executeScalar : default(T);
                }
            }
        }

        public void ExecuteNonQuery(string commandText)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionStringProviderFactory().GetConnectionStringProvider().GetConnectionString()))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        private ConnectionStringProviderFactory GetConnectionStringProviderFactory()
        {
            return new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_CONNECTIONSTRING_OVERRIDE");
        }
    }
}