using Cafe.Waiter.DAL;
using CQRSTutorial.DAL;
using NHibernate;

namespace Cafe.Waiter.Web
{
    public static class SessionFactory
    {
        static SessionFactory()
        {
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            Instance = new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory();
        }

        public static ISessionFactory Instance { get; set; }
    }
}