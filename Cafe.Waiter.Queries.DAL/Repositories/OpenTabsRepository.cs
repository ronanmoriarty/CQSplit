using System.Collections.Generic;
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