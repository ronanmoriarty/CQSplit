using System.Collections.Generic;
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
    public class TabRepositoryTests
    {
        private const string FoodDescription = "Chicken Madras";
        private const int FoodMenuNumber = 234;
        private const decimal FoodPrice = 10.5m;
        private const string DrinkDescription = "Coca Cola";
        private const int DrinkMenuNumber = 345;
        private const decimal DrinkPrice = 2.5m;
        private TabRepository _tabRepository;
        private const int TabId = -1;
        private TabInspector _tabInspector;
        private EventStore _repository;
        private ISession _writeSession;

        [SetUp]
        public void SetUp()
        {
            var sqlExecutor = new SqlExecutor();
            sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE AggregateId = {TabId}");
            _tabRepository = new TabRepository(new EventStore(SessionFactory.ReadInstance, new EventMapper(typeof(TabOpened).Assembly)), new EventApplier(new TypeInspector()));
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
                Id = -1,
                AggregateId = TabId,
                TableNumber = tableNumber,
                Waiter = waiter
            };

            Insert(tabOpened);
        }

        private void AssumingFoodOrdered()
        {
            var foodOrdered = new FoodOrdered
            {
                Id = -2,
                AggregateId = TabId,
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
                Id = -3,
                AggregateId = TabId,
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
            _tabInspector = new TabInspector(_tabRepository.Get(TabId));
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
            using (var transaction = _writeSession.BeginTransaction())
            {
                _repository.Add(@event);
                transaction.Commit();
            }
        }

        private EventStore CreateRepository()
        {
            _writeSession = SessionFactory.WriteInstance.OpenSession();
            var eventStore = new EventStore(
                SessionFactory.ReadInstance,
                new EventMapper(typeof(TabOpened).Assembly))
            {
                UnitOfWork = new NHibernateUnitOfWork(_writeSession)
            };
            return eventStore;
        }
    }
}
