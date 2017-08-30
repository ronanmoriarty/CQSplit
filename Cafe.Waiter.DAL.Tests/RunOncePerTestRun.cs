using Cafe.Waiter.DAL.Mapping;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests.Common;
using FluentNHibernate.Automapping;
using log4net.Config;
using NUnit.Framework;

namespace Cafe.Waiter.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XmlConfigurator.Configure();
            var connectionStringProviderFactory = ReadModelConnectionStringProviderFactory.Instance;
            var nHibernateConfiguration = new NHibernateConfiguration(connectionStringProviderFactory);
            SessionFactory.Instance = nHibernateConfiguration.CreateSessionFactory(
                (autoMappingsContainer, cfg) =>
                {
                    autoMappingsContainer.Add(
                        AutoMap.AssemblyOf<OpenTab>(cfg)
                            .UseOverridesFromAssemblyOf<OpenTabMapping>());
                }
            );
        }
    }
}
