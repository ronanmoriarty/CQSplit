using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SqlExecutor
    {
        private readonly ConnectionStringProviderFactory _connectionStringProviderFactory;
        private readonly ILog _logger = LogManager.GetLogger(typeof(SqlExecutor));

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
                    _logger.Debug(commandText);
                    var executeScalar = command.ExecuteScalar();
                    return executeScalar != DBNull.Value ? (T)executeScalar : default(T);
                }
            }
        }

        public void ExecuteNonQuery(string commandText)
        {
            _logger.Debug(commandText);
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

        public void ExecuteReader(string commandText, Action<IDataReader> readerAction)
        {
            _logger.Debug(commandText);
            using (var sqlConnection = new SqlConnection(_connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString()))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            readerAction(reader);
                        }
                    }
                }
            }
        }
    }
}