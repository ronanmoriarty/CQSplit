using CQRSTutorial.DAL;
using NHibernate;

namespace Cafe.Waiter.Web.Controllers
{
    public static class SessionFactory
    {
        static SessionFactory()
        {
            Instance = new NHibernateConfiguration(new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_CONNECTIONSTRING_OVERRIDE")).CreateSessionFactory();
        }

        public static ISessionFactory Instance { get; set; }
    }
}