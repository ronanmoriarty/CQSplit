using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class OpenTabsRepository : NHibernateRepositoryBase<Serialized.OpenTab>, IOpenTabsRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OpenTabsRepository(ISessionFactory sessionFactory, ISqlConnectionFactory sqlConnectionFactory)
            : base(sessionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        protected override IEnumerable<Serialized.OpenTab> GetAll()
        {
            using (var sqlConnection = _sqlConnectionFactory.Create())
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand("SELECT Id, Data FROM dbo.OpenTabs", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        var id = sqlDataReader.GetGuid(sqlDataReader.GetOrdinal("Id"));
                        var data = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Data"));
                        yield return new Serialized.OpenTab
                        {
                            Id = id,
                            Data = data
                        };
                    }
                }
            }
        }

        public void Insert(OpenTab openTab)
        {
            using (var session = SessionFactory.OpenSession())
            {
                // TODO: all this UnitOfWork needs to move out of here - change tests to support this.
                using (var unitOfWork = new NHibernateUnitOfWork(session))
                {
                    unitOfWork.Start();
                    UnitOfWork = unitOfWork;
                    SaveOrUpdate(new Serialized.OpenTab
                    {
                        Id = openTab.Id,
                        Data = JsonConvert.SerializeObject(openTab)
                    });
                    unitOfWork.Commit();
                }
            }
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}