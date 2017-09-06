using Cafe.Domain.Events;
using CQRSTutorial.DAL;
using FluentNHibernate.Automapping;
using NHibernate;

namespace Cafe.Waiter.Service
{
    public class WriteModelSessionFactory
    {
        public static ISessionFactory Instance { get; } = CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            return new NHibernateConfiguration(WriteModelConnectionStringProviderFactory.Instance).CreateSessionFactory((autoMappingsContainer, cfg) =>
            {
                autoMappingsContainer.Add(
                    AutoMap.AssemblyOf<TabOpened>(cfg)
                        .UseOverridesFromAssemblyOf<EventMapping>());
            });
        }
    }
}