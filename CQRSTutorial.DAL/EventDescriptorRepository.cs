using System.Collections.Generic;
using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventDescriptorRepository
    {
        private readonly ISessionFactory _readSessionFactory;
        private readonly IsolationLevel _isolationLevel;

        public EventDescriptorRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
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

        public EventDescriptor Get(int id)
        {
            using (var session = _readSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(_isolationLevel))
                {
                    return session.Get<EventDescriptor>(id);
                }
            }
        }
    }
}