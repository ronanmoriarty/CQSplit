using NHibernate;

namespace CQRSTutorial.DAL.Tests
{
    public class SessionFactory
    {
        public static ISessionFactory Instance { get; set; }
    }
}