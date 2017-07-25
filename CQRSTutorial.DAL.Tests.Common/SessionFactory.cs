using NHibernate;

namespace CQRSTutorial.DAL.Tests.Common
{
    public class SessionFactory
    {
        public static ISessionFactory Instance { get; set; }
    }
}