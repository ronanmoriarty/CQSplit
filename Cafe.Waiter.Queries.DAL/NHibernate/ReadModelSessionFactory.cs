using Cafe.Waiter.Queries.DAL.Mapping;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using FluentNHibernate.Automapping;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.NHibernate
{
    public class ReadModelSessionFactory
    {
        public static ISessionFactory Instance { get; } = CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            return new NHibernateConfiguration(ReadModelConnectionStringProviderFactory.Instance).CreateSessionFactory((autoMappingsContainer, cfg) =>
            {
                autoMappingsContainer.Add(
                    AutoMap.AssemblyOf<OpenTab>(cfg)
                        .UseOverridesFromAssemblyOf<OpenTabMapping>());
            });
        }
    }
}