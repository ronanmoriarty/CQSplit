using System.Threading.Tasks;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class EventConsumerTests
    {
        private IProjector<TestEvent> _projector;
        private TestEventConsumer _testCommandConsumer;
        private ConsumeContext<TestEvent> _consumeContext;
        private TestEvent _testEvent;

        [SetUp]
        public void SetUp()
        {
            _projector = Substitute.For<IProjector<TestEvent>>();
            _testCommandConsumer = new TestEventConsumer(_projector);
            _consumeContext = Substitute.For<ConsumeContext<TestEvent>>();
            _testEvent = new TestEvent();
            _consumeContext.Message.Returns(_testEvent);
        }

        [Test]
        public async Task Projector_projects_incoming_events()
        {
            await _testCommandConsumer.Consume(_consumeContext);

            _projector.Received(1).Project(_testEvent);
        }

        private class TestEventConsumer : EventConsumer<TestEvent>
        {
            public TestEventConsumer(IProjector<TestEvent> projector)
                : base(projector)
            {
            }
        }
    }
}