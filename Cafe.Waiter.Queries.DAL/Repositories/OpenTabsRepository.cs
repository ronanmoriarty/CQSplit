using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class OpenTabsRepository : IOpenTabsRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OpenTabsRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        private IEnumerable<Serialized.OpenTab> GetAll()
        {
            return new WaiterDbContext().OpenTabs.ToList();
        }

        public Serialized.OpenTab Get(Guid id)
        {
            using (var sqlConnection = _sqlConnectionFactory.Create())
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand("SELECT Data FROM dbo.OpenTabs WHERE Id = @id", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        var data = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Data"));
                        return new Serialized.OpenTab
                        {
                            Id = id,
                            Data = data
                        };
                    }
                }
            }

            return null;
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                using (var sqlConnection = _sqlConnectionFactory.Create())
                {
                    sqlConnection.Open();

                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        using (var sqlCommand = sqlConnection.CreateCommand())
                        {
                            sqlCommand.Transaction = transaction;
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.CommandText = "INSERT INTO dbo.OpenTabs(Id, Data) VALUES (@id, @data)";
                            sqlCommand.Parameters.AddWithValue("@id", openTab.Id);
                            sqlCommand.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(openTab));
                            sqlCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
            }
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}