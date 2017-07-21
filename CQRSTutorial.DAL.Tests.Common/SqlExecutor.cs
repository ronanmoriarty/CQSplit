﻿using System;
using System.Data.SqlClient;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SqlExecutor
    {
        public T ExecuteScalar<T>(string commandText)
        {
            using (var sqlConnection = new SqlConnection(new ConnectionStringProviderFactory().GetConnectionStringProvider().GetConnectionString()))
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
            using (var sqlConnection = new SqlConnection(new ConnectionStringProviderFactory().GetConnectionStringProvider().GetConnectionString()))
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