using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.DAL.Tests.Inspectors;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace Cafe.Waiter.DAL.Tests
{
    [TestFixture]
    public class TabRepositoryTests
    {
        private const string FoodDescription = "Chicken Madras";
        private const int FoodMenuNumber = 234;
        private const decimal FoodPrice = 10.5m;
        private const string DrinkDescription = "Coca Cola";
        private const int DrinkMenuNumber = 345;
        private const decimal DrinkPrice = 2.5m;
        private TabRepository _tabRepository;
        private readonly Guid _tabId = new Guid("E6BC4329-FE80-4EF7-9F4C-EBF2F10DC37A");
        private TabInspector _tabInspector;
        private EventStore _repository;
        private ISession _session;

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE AggregateId = '{_tabId}'");
            _tabRepository = new TabRepository(new EventStore(SessionFactory.Instance, new EventMapper(typeof(TabOpened).Assembly)), new EventApplier(new TypeInspector()));
            _repository = CreateRepository();
        }

        [Test]
        public void Can_restore_tab_from_multiple_events()
        {
            AssumingTabOpened();

            AssumingFoodOrdered();
            AssumingDrinkOrdered();

            WhenTabRetrievedFromRepository();

            AssertTabReflectsAllSavedEvents();
        }

        private void AssumingTabOpened()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                Id = new Guid("7F85BE15-94E4-4536-80B8-DF301ACA47F2"),
                CommandId = new Guid("593243D2-9C57-46F2-99F0-1B2D3E4916A6"),
                AggregateId = _tabId,
                TableNumber = tableNumber,
                Waiter = waiter
            };

            Insert(tabOpened);
        }

        private void AssumingFoodOrdered()
        {
            var foodOrdered = new FoodOrdered
            {
                Id = new Guid("E259269A-F46E-4524-92A1-60DAC14339A6"),
                CommandId = new Guid("310CBA59-0F0E-480F-83AE-89B8E24065D1"),
                AggregateId = _tabId,
                Items = new List<OrderedItem>
                {
                    new OrderedItem
                    {
                        Description = FoodDescription,
                        IsDrink = false,
                        MenuNumber = FoodMenuNumber,
                        Price = FoodPrice
                    }
                }
            };

            Insert(foodOrdered);
        }

        private void AssumingDrinkOrdered()
        {
            var foodOrdered = new DrinksOrdered
            {
                Id = new Guid("ACA9FBEF-F005-4F7F-99B8-9ED9B7855264"),
                CommandId = new Guid("665AC089-7C2B-4180-992D-F3C928521452"),
                AggregateId = _tabId,
                Items = new List<OrderedItem>
                {
                    new OrderedItem
                    {
                        Description = DrinkDescription,
                        IsDrink = true,
                        MenuNumber = DrinkMenuNumber,
                        Price = DrinkPrice
                    }
                }
            };

            Insert(foodOrdered);
        }

        private void WhenTabRetrievedFromRepository()
        {
            _tabInspector = new TabInspector(_tabRepository.Get(_tabId));
        }

        private void AssertTabReflectsAllSavedEvents()
        {
            Assert.That(_tabInspector.FoodAwaitingServing.Single(x =>
                    x.Description == FoodDescription
                    && x.IsDrink == false
                    && x.MenuNumber == FoodMenuNumber
                    && x.Price == FoodPrice)
                , Is.Not.Null);
            Assert.That(_tabInspector.DrinksAwaitingServing.Single(x =>
                    x.Description == DrinkDescription
                    && x.IsDrink
                    && x.MenuNumber == DrinkMenuNumber
                    && x.Price == DrinkPrice)
                , Is.Not.Null);
        }

        private void Insert(IEvent @event)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _repository.Add(@event);
                transaction.Commit();
            }
        }

        private EventStore CreateRepository()
        {
            _session = SessionFactory.Instance.OpenSession();
            var eventStore = new EventStore(
                SessionFactory.Instance,
                new EventMapper(typeof(TabOpened).Assembly))
            {
                UnitOfWork = new NHibernateUnitOfWork(_session)
            };
            return eventStore;
        }
    }
}
