using System;
using System.Data;
using System.Data.SqlClient;
using CQRSTutorial.DAL.Common;
using NLog;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SqlExecutor
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SqlExecutor(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public T ExecuteScalar<T>(string commandText)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
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
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
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
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
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

        private string GetConnectionString()
        {
            return _connectionStringProvider?.GetConnectionString();
        }
    }
}