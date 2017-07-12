using System.Data;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public static class NHibernateConfiguration
    {
        public static ISessionFactory CreateSessionFactory(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var msSqlConfiguration = MsSqlConfiguration
                .MsSql2012
                .IsolationLevel(isolationLevel)
                .ConnectionString(x => x.FromConnectionStringWithKey("CQRSTutorial"));

            var cfg = new CustomAutomappingConfiguration();
            return Fluently
                .Configure()
                .Database(msSqlConfiguration)
                .Mappings(m =>
                {
                    m.AutoMappings.Add(
                        AutoMap.AssemblyOf<EventToPublish>(cfg)
                            .UseOverridesFromAssemblyOf<EventToPublishMapping>());
                })
                .BuildSessionFactory();
        }
    }
}