using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Waiter.DAL.Tests.Inspectors;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace Cafe.Waiter.DAL.Tests
{
    [TestFixture]
    public class TabRepositoryTests : InsertAndReadTest<EventStore, Event>
    {
        private const string FoodDescription = "Chicken Madras";
        private const int FoodMenuNumber = 234;
        private const decimal FoodPrice = 10.5m;
        private const string DrinkDescription = "Coca Cola";
        private const int DrinkMenuNumber = 345;
        private const decimal DrinkPrice = 2.5m;
        private readonly TabRepository _tabRepository;
        private readonly SqlExecutor _sqlExecutor;
        private readonly int _tabId;
        private TabInspector _tabInspector;

        public TabRepositoryTests()
        {
            _sqlExecutor = new SqlExecutor();
            _tabRepository = new TabRepository(new EventStore(SessionFactory.ReadInstance, IsolationLevel.ReadUncommitted, new EventMapper(typeof(TabOpened).Assembly)), new EventApplier(new TypeInspector()));
            _tabId = GetTabId();
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
                Id = GetNewId(),
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
                Id = GetNewId(),
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
                Id = GetNewId(),
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

        private int GetTabId()
        {
            var maxValue = _sqlExecutor.ExecuteScalar<int?>("SELECT MIN(Id) FROM dbo.Events");
            return maxValue - 1 ?? -1;
        }

        private int GetNewId()
        {
            var maxValue = _sqlExecutor.ExecuteScalar<int?>("SELECT MAX(Id) FROM dbo.EventsToPublish");
            return maxValue + 1 ?? 1;
        }

        private void Insert(IEvent @event)
        {
            Repository.Add(@event);
            WriteSession.Flush();
        }

        protected override EventStore CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventStore(SessionFactory.ReadInstance, IsolationLevel.ReadUncommitted, new EventMapper(typeof(TabOpened).Assembly));
        }
    }
}
