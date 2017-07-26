using CQRSTutorial.DAL;
using NHibernate;

namespace Cafe.Waiter.Web
{
    public static class SessionFactory
    {
        static SessionFactory()
        {
            var connectionStringProviderFactory = new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_CONNECTIONSTRING_OVERRIDE");
            Instance = new NHibernateConfiguration(connectionStringProviderFactory).CreateSessionFactory();
        }

        public static ISessionFactory Instance { get; set; }
    }
}