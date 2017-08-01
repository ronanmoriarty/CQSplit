using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        public void ExecuteReader(string commandText, Action<IDataReader> readerAction)
        {
            Console.WriteLine(commandText);
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