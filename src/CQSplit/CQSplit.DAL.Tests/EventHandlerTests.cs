using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace CQSplit.DAL.Tests
{
    [TestFixture]
    public class EventHandlerTests
    {
        private EventHandler _eventHandler;
        private IEventStore _eventStore;
        private IUnitOfWork _unitOfWork;
        private IEnumerable<IEvent> _events;
        private IEvent _event1;
        private IEvent _event2;
        private List<IEvent> _eventsAddedWithinTransaction;
        private bool _executingInTransaction;

        [SetUp]
        public void SetUp()
        {
            _eventStore = Substitute.For<IEventStore>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventStore.UnitOfWork.Returns(_unitOfWork);
            _eventHandler = new EventHandler(_eventStore);
            _event1 = Substitute.For<IEvent>();
            _event2 = Substitute.For<IEvent>();
            _events = new[] { _event1, _event2 };
            _eventsAddedWithinTransaction = new List<IEvent>();
            _executingInTransaction = false;
        }

        [Test]
        public void Adds_events_to_event_store_within_transaction()
        {
            RecordEventsAddedToEventStoreWithinUnitOfWork();

            _eventHandler.Handle(_events);

            Assert.That(_eventsAddedWithinTransaction.Contains(_event1));
            Assert.That(_eventsAddedWithinTransaction.Contains(_event2));
        }

        private void RecordEventsAddedToEventStoreWithinUnitOfWork()
        {
            _eventStore.When(x => x.Add(Arg.Any<IEvent>())).Do(callInfo =>
            {
                if (_executingInTransaction)
                {
                    Console.WriteLine("Executing IEventStore.Add() in transaction...");
                    _eventsAddedWithinTransaction.Add(callInfo.Arg<IEvent>());
                }
                else
                {
                    Console.WriteLine("Executing IEventStore.Add(), but not within transaction...");
                }
            });

            _unitOfWork.When(x => x.ExecuteInTransaction(Arg.Any<Action>())).Do(callInfo =>
            {
                Console.WriteLine("Executing in transaction...");
                _executingInTransaction = true;
                InvokeAction(callInfo);
                _executingInTransaction = false;
            });
        }

        private static void InvokeAction(CallInfo callInfo)
        {
            callInfo.Arg<Action>()();
        }
    }
}
