using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SessionFactory.Instance = GetSessionFactory();
        }

        private ISessionFactory GetSessionFactory()
        {
            var msSqlConfiguration = MsSqlConfiguration.MsSql2012.ConnectionString(x => x.FromConnectionStringWithKey("CQRSTutorial"));
            return Fluently
                .Configure()
                .Database(msSqlConfiguration)
                .Mappings(c => c.FluentMappings.AddFromAssemblyOf<EventDescriptor>())
                .BuildSessionFactory();
        }
    }
}
