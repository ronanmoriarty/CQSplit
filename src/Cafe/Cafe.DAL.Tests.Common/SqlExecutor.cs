using System;
using System.Data;
using System.Data.SqlClient;
using Cafe.DAL.Sql;
using NLog;

namespace Cafe.DAL.Tests.Common
{
    public class SqlExecutor
    {
        private readonly string _connectionString;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SqlExecutor()
        {
            _connectionString = ConnectionStringProvider.ConnectionString;
        }

        public T ExecuteScalar<T>(string commandText)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
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

        public void ExecuteReader(string commandText, Action<IDataReader> readerAction)
        {
            _logger.Debug(commandText);
            using (var sqlConnection = new SqlConnection(_connectionString))
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