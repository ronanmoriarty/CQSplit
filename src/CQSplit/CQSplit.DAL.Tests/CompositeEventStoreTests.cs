using System.Collections.Generic;
using CQSplit.Core;
using NSubstitute;
using NUnit.Framework;

namespace CQSplit.DAL.Tests
{
    [TestFixture]
    public class CompositeEventStoreTests
    {
        private IEnumerable<IEventStore> _eventStores;
        private CompositeEventStore _compositeEventStore;
        private IEvent _event;
        private IEventStore _eventStore1;
        private IEventStore _eventStore2;

        [SetUp]
        public void SetUp()
        {
            _eventStore1 = Substitute.For<IEventStore>();
            _eventStore2 = Substitute.For<IEventStore>();
            _eventStores = new[] { _eventStore1, _eventStore2 };
            _compositeEventStore = new CompositeEventStore(_eventStores);
            _event = new TestEvent();
        }

        [Test]
        public void Add_delegates_to_all_underlying_event_stores()
        {
            _compositeEventStore.Add(_event);

            _eventStore1.Received(1).Add(Arg.Is(_event));
            _eventStore2.Received(1).Add(Arg.Is(_event));
        }
    }
}
