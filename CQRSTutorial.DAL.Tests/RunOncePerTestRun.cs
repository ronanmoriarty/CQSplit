using System;
using FluentNHibernate.Automapping;
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
            SessionFactory.Instance = NHibernateConfiguration.CreateSessionFactory();
        }
    }

    public static class NHibernateConfiguration
    {
        public static ISessionFactory CreateSessionFactory()
        {
            var msSqlConfiguration = MsSqlConfiguration.MsSql2012.ConnectionString(x => x.FromConnectionStringWithKey("CQRSTutorial"));
            var cfg = new CustomAutomappingConfiguration();
            return Fluently
                .Configure()
                .Database(msSqlConfiguration)
                .Mappings(m =>
                {
                    m.AutoMappings.Add(
                        AutoMap.AssemblyOf<EventDescriptor>(cfg)
                            .UseOverridesFromAssemblyOf<EventDescriptorMapping>());
                })
                .BuildSessionFactory();
        }
    }

    public class CustomAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Name == "EventDescriptor"; // TODO: we'll cater for entities here too as tests demand it.
        }
    }
}
