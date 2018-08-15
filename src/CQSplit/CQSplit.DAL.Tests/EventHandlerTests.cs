using System.Collections.Generic;
using CQSplit.Core;
using NSubstitute;
using NUnit.Framework;

namespace CQSplit.DAL.Tests
{
    [TestFixture]
    public class EventHandlerTests
    {
        private EventHandler _eventHandler;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IEventStore _eventStore;
        private IUnitOfWork _unitOfWork;
        private IEnumerable<IEvent> _events;
        private IEvent _event1;
        private IEvent _event2;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkFactory = Substitute.For<IUnitOfWorkFactory>();
            _eventStore = Substitute.For<IEventStore>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventHandler = new EventHandler(_unitOfWorkFactory, _eventStore);
            _event1 = Substitute.For<IEvent>();
            _event2 = Substitute.For<IEvent>();
            _events = new[] { _event1, _event2 };
            _unitOfWorkFactory.Create().Returns(_unitOfWork);

            _eventHandler.Handle(_events);
        }

        [Test]
        public void Enrolls_event_store_in_unit_of_work()
        {
            _unitOfWork.Received(1).Enrolling(Arg.Is(_eventStore));
        }
    }
}
