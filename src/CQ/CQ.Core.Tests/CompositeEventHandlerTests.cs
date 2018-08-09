using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace CQ.Core.Tests
{
    [TestFixture]
    public class CompositeEventHandlerTests
    {
        [Test]
        public void Calls_Handle_on_each_component()
        {
            var events = new List<IEvent>();
            var eventHandler1 = Substitute.For<IEventHandler>();
            var eventHandler2 = Substitute.For<IEventHandler>();
            IEnumerable<IEventHandler> componentEventHandlers = new IEventHandler[]{eventHandler1, eventHandler2};
            var compositeEventHandler = new CompositeEventHandler(componentEventHandlers);

            compositeEventHandler.Handle(events);

            eventHandler1.Received(1).Handle(Arg.Is(events));
            eventHandler2.Received(1).Handle(Arg.Is(events));
        }
    }
}