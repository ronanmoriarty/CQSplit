using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class NHibernateConfiguration
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;

        public NHibernateConfiguration(IConnectionStringProviderFactory connectionStringProviderFactory)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
        }

        public ISessionFactory CreateSessionFactory(Action<AutoMappingsContainer, IAutomappingConfiguration> configureAutoMapping = null)
        {
            var msSqlConfiguration = MsSqlConfiguration
                .MsSql2012
                .ConnectionString(x => x.Is(_connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString()));

            var cfg = new CustomAutomappingConfiguration();
            return Fluently
                .Configure()
                .Database(msSqlConfiguration)
                .Mappings(m =>
                {
                    m.AutoMappings.Add(
                        AutoMap.AssemblyOf<EventToPublish>(cfg)
                            .UseOverridesFromAssemblyOf<EventToPublishMapping>());
                    configureAutoMapping?.Invoke(m.AutoMappings, cfg);
                })
                .BuildSessionFactory();
        }
    }
}