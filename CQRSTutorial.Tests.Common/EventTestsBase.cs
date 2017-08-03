using System;
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
        private ICommand[] _commands;
        private IAggregateStore _aggregateStore;

        [SetUp]
        public void SetUp()
        {
            _aggregateStore = GetAggregateStore();
            _eventReceiver = Substitute.For<IEventReceiver>();
            _commandDispatcher = new CommandDispatcher(_eventReceiver, _aggregateStore, new TypeInspector());
            _eventApplier = new EventApplier(new TypeInspector());
            _commandHandler = GetSystemUnderTest();
        }

        protected abstract IAggregateStore GetAggregateStore();

        protected abstract TCommandHandler GetSystemUnderTest();

        protected void Given(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                _eventApplier.ApplyEvent(@event, _commandHandler);
            }
        }

        protected void When(params ICommand[] commands)
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
            var compareEverythingExceptIdProperty = CompareEverythingExceptIdProperty();
            var matchingEvents = events.Where(@event =>
                compareEverythingExceptIdProperty.Compare(@event, expectedEvent).AreEqual
                && EnsureIdHasNonEmptyValue(@event)
            );

            return matchingEvents.Any();
        }

        private static bool EnsureIdHasNonEmptyValue(IEvent @event)
        {
            return @event.Id != Guid.Empty;
        }

        private static CompareLogic CompareEverythingExceptIdProperty()
        {
            var compareLogic = new CompareLogic
            {
                Config =
                {
                    MembersToIgnore = new List<string> {"Id"}
                }
            };
            return compareLogic;
        }

        private void HandleCommands()
        {
            _commandDispatcher.Dispatch(_commands);
        }
    }
}