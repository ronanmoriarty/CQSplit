using System;
using System.Data;
using Cafe.Domain.Events;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Newtonsoft.Json;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class TimeoutExpiredTest
    {
        private ISession _writeSession;

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
                        AutoMap.AssemblyOf<EventDescriptor>(cfg)
                            .UseOverridesFromAssemblyOf<EventDescriptorMapping>());
                })
                .BuildSessionFactory();
        }

        [SetUp]
        public void SetUp()
        {
            _writeSession = CreateSessionFactory().OpenSession();
            _writeSession.BeginTransaction();
        }

        [Test]
        public void InsertAndReadTabOpened()
        {
            var objectToStoreInDbTemporarily = GetObjectToStoreInDbTemporarily();

            _writeSession.SaveOrUpdate(objectToStoreInDbTemporarily);
            _writeSession.Flush(); // the data needs to go to the database (even if only temporarily), rather than just staying in an NHibernate cache.

            var readSession = CreateSessionFactory(IsolationLevel.ReadUncommitted).OpenSession();
            var retrievedEventDescriptor = readSession.Get<EventDescriptor>(objectToStoreInDbTemporarily.Id); // this line throws the exception after 30 seconds.

            // If previous line worked, I could then assert inserted values match retrieved values at this point.
        }

        [TearDown]
        public void TearDown()
        {
            _writeSession.Transaction?.Rollback();
        }

        public class CustomAutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return type.Name == typeof(EventDescriptor).Name; // we're only mapping this class for now.
            }
        }

        private static EventDescriptor GetObjectToStoreInDbTemporarily()
        {
            var tabOpened = GetTabOpenedEvent();

            var eventDescriptor = new EventDescriptor
            {
                EventType = tabOpened.GetType(),
                Data = JsonConvert.SerializeObject(tabOpened)
            };
            return eventDescriptor;
        }

        private static TabOpened GetTabOpenedEvent()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                TableNumber = tableNumber,
                Waiter = waiter
            };
            return tabOpened;
        }
    }
}