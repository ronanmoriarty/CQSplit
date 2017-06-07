using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    public abstract class EventTestsBase<TCommandHandler, TCommand>
        where TCommandHandler : ICommandHandler<TCommand>, new()
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly CommandDispatcher _commandDispatcher;

        protected EventTestsBase()
        {
            var commandHandler = new TCommandHandler();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _commandDispatcher = new CommandDispatcher(_eventPublisher, new CommandHandlerDictionary(new object[] {commandHandler}));
        }

        protected void WhenCommandReceived(TCommand command)
        {
            _commandDispatcher.Dispatch(command);
        }

        protected void AssertEventPublished<TEvent>(Func<TEvent, bool> matchCriteria)
        {
            _eventPublisher.Received(1).Publish(Arg.Is<IEnumerable<Event>>(events => AtLeastOneEventMatchesCriteria(matchCriteria, events)));
        }

        private bool AtLeastOneEventMatchesCriteria<TEvent>(Func<TEvent, bool> matchCriteria, IEnumerable<Event> events)
        {
            var allEventsOfMatchingType = events.Where(evt => evt is TEvent).Cast<TEvent>();
            var listOfAllEventsOfMatchingType = allEventsOfMatchingType as IList<TEvent> ?? allEventsOfMatchingType.ToList();
            Assert.IsTrue(listOfAllEventsOfMatchingType.Count > 0, $"No events of type {typeof(TEvent).FullName} received.");
            return listOfAllEventsOfMatchingType.Any(matchCriteria);
        }
    }
}