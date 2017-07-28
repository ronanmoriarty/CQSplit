using System;
using System.Data.SqlClient;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SqlExecutor
    {
        private readonly ConnectionStringProviderFactory _connectionStringProviderFactory;

        public SqlExecutor(ConnectionStringProviderFactory connectionStringProviderFactory)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
        }

        public T ExecuteScalar<T>(string commandText)
        {
            using (var sqlConnection = new SqlConnection(_connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString()))
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
            Console.WriteLine(commandText);
            using (var sqlConnection = new SqlConnection(_connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString()))
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