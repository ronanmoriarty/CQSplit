using System.Collections.Generic;
using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventDescriptorRepository : RepositoryBase<EventDescriptor>
    {
        private readonly ISessionFactory _readSessionFactory;
        private readonly IsolationLevel _isolationLevel;

        public EventDescriptorRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
            : base(readSessionFactory, isolationLevel)
        {
            _readSessionFactory = readSessionFactory;
            _isolationLevel = isolationLevel;
        }

        public IList<EventDescriptor> GetEventsAwaitingPublishing()
        {
            using (var session = _readSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(_isolationLevel))
                {
                    return session.QueryOver<EventDescriptor>().List();
                }
            }
        }
    }
}