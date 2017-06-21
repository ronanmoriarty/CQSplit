using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    public abstract class EventTestsBase<TCommandHandler>
        where TCommandHandler : new()
    {
        private IEventPublisher _eventPublisher;
        private CommandDispatcher _commandDispatcher;
        private TCommandHandler _commandHandler;
        private EventApplier _eventApplier;

        [SetUp]
        public void SetUp()
        {
            _commandHandler = new TCommandHandler();
            _eventPublisher = Substitute.For<IEventPublisher>();
            SetUpEventPublisher();
            _commandDispatcher = new CommandDispatcher(_eventPublisher, new object[] { _commandHandler });
            _eventApplier = new EventApplier(_commandHandler);
        }

        private void SetUpEventPublisher()
        {
            _eventPublisher.When(e => e.Publish(Arg.Any<IEnumerable<IEvent>>())).Do(publishInvocation =>
            {
                //TODO: apply events automatically for now, but long-term we'll want to make this a more explicit step to get more control over exactly when the events get applied.
                //TODO: events are currently applied regardless of TabId - fine for now in these simpler tests that only have one Tab, but that will likely become a problem quite soon.
                foreach (IEnumerable<IEvent> eventsFromOnePublishInvocation in publishInvocation.Args())
                {
                    foreach (var @event in eventsFromOnePublishInvocation)
                    {
                        var eventType = @event.GetType();
                        _eventApplier.ApplyEvent(eventType, @event);
                    }
                }
            });
        }

        protected void WhenCommandReceived<TCommand>(TCommand command)
        {
            _commandDispatcher.Dispatch(command);
        }

        protected void AssertEventPublished<TEvent>(Func<TEvent, bool> matchCriteria)
        {
            _eventPublisher.Received(1).Publish(Arg.Is<IEnumerable<IEvent>>(events => AtLeastOneEventMatchesCriteria(matchCriteria, events)));
        }

        private bool AtLeastOneEventMatchesCriteria<TEvent>(Func<TEvent, bool> matchCriteria, IEnumerable<IEvent> events)
        {
            var allEventsOfMatchingType = events.Where(evt => evt is TEvent).Cast<TEvent>();
            var listOfAllEventsOfMatchingType = allEventsOfMatchingType as IList<TEvent> ?? allEventsOfMatchingType.ToList();
            Assert.IsTrue(listOfAllEventsOfMatchingType.Count > 0, $"No events of type {typeof(TEvent).FullName} received.");
            return listOfAllEventsOfMatchingType.Any(matchCriteria);
        }
    }
}