using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Tests.Common
{
    public abstract class EventTestsBase<TCommandHandler>
        where TCommandHandler : new()
    {
        private IEventReceiver _eventReceiver;
        private CommandDispatcher _commandDispatcher;
        private TCommandHandler _commandHandler;
        private EventApplier _eventApplier;
        private object[] _commands;

        [SetUp]
        public void SetUp()
        {
            _commandHandler = new TCommandHandler();
            _eventReceiver = Substitute.For<IEventReceiver>();
            _commandDispatcher = new CommandDispatcher(_eventReceiver, new object[] { _commandHandler });
            _eventApplier = new EventApplier(new object[] {_commandHandler});
        }

        // TODO: need to consider what tests are required involving a sequence of commands, or do we potentially say that each command will execute in isolation from all other commands, on an object whose current state is entirely dependent on previous events (rather than being dependent on previous events AND commands).
        protected void Given(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                _eventApplier.ApplyEvent(@event);
            }
        }

        protected void When(params object[] commands)
        {
            _commands = commands;
        }

        protected void Then(params object[] expectedEvents)
        {
            HandleCommands();
            foreach (var expectedEvent in expectedEvents)
            {
                _eventReceiver.Received(1).Receive(Arg.Is<IEnumerable<IEvent>>(events => AtLeastOneEventMatches(expectedEvent, events)));
            }
        }

        private bool AtLeastOneEventMatches(object expectedEvent, IEnumerable<IEvent> events)
        {
            var compareLogic = new CompareLogic();
            var matchingEvents = events.Where(@event => compareLogic.Compare(@event, expectedEvent).AreEqual);
            return matchingEvents.Any();
        }

        protected void ThenFailsWith<TException>()
        {
            Assert.That(
                HandleCommands,
                Throws.Exception.InstanceOf<TException>()
            );
        }

        private void HandleCommands()
        {
            _commandDispatcher.Dispatch(_commands);
        }
    }
}