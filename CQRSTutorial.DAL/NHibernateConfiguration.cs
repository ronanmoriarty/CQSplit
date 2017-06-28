using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace CQRSTutorial.DAL
{
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
}