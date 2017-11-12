using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.DAL;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Publish.Tests
{
    [TestFixture]
    public class OutboxToMessageBusPublisherTests
    {
        private IEventToPublishRepository _eventToPublishRepository;
        private TestEvent _event;
        private IEventToPublishSerializer _eventToPublishSerializer;
        private IBusControl _busControl;
        private OutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private EventToPublish _eventToPublish;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUnitOfWork _unitOfWork;
        private bool _invokingActionInTransaction;
        private bool _eventToPublishDeletedInTransaction;

        [SetUp]
        public void SetUp()
        {
            _eventToPublishSerializer = Substitute.For<IEventToPublishSerializer>();
            _event = new TestEvent();
            _eventToPublishRepository = Substitute.For<IEventToPublishRepository>();
            _eventToPublish = new EventToPublish();
            _eventToPublishRepository
                .GetEventsAwaitingPublishing()
                .Returns(new List<EventToPublish> { _eventToPublish });
            _eventToPublishRepository.When(x => x.Delete(Arg.Is(_eventToPublish))).Do(callInfo =>
            {
                if (_invokingActionInTransaction)
                {
                    _eventToPublishDeletedInTransaction = true;
                }
            });
            _eventToPublishSerializer.Deserialize(_eventToPublish).Returns(_event);
            _busControl = Substitute.For<IBusControl>();
            _unitOfWorkFactory = Substitute.For<IUnitOfWorkFactory>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.Enrolling(Arg.Is(_eventToPublishRepository)).Returns(_unitOfWork);
            _unitOfWork.When(x => x.ExecuteInTransaction(Arg.Any<Action>())).Do(callInfo =>
            {
                _invokingActionInTransaction = true;
                var actionToInvokeInTransaction = (Action) callInfo.Args().First();
                actionToInvokeInTransaction.Invoke();
                _invokingActionInTransaction = false;
            });
            _unitOfWorkFactory.Create().Returns(_unitOfWork);
            _outboxToMessageBusPublisher = new OutboxToMessageBusPublisher(
                _eventToPublishRepository,
                _busControl,
                _eventToPublishSerializer,
                _unitOfWorkFactory);

            _outboxToMessageBusPublisher.PublishQueuedMessages();
        }

        [Test]
        public void Publishes_event_to_message_bus()
        {
            _busControl.Received(1).Publish(_event);
        }

        [Test]
        public void Removes_published_event_from_eventsToPublish_list()
        {
            Assert.That(_eventToPublishDeletedInTransaction);
        }
    }
}
