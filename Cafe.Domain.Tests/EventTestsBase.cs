using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    public abstract class EventTestsBase<TCommandHandler, TCommand>
        where TCommandHandler : new()
        where TCommand : ICommand
    {
        private IEventPublisher _eventPublisher;
        private CommandDispatcher _commandDispatcher;
        private TCommandHandler _commandHandler;
        private EventApplier _eventApplier;
        private TCommand _command;
        private CommandHandlerProvider _commandHandlerProvider;

        [SetUp]
        public void SetUp()
        {
            var commandHandlerFactory = Substitute.For<ICommandHandlerFactory>();
            _commandHandlerProvider = new CommandHandlerProvider(commandHandlerFactory);
            _commandHandlerProvider.RegisterCommandHandler(new OpenTabCommandHandler());
            ConfigureCommandHandlerFactory(commandHandlerFactory);
            _eventPublisher = Substitute.For<IEventPublisher>();
            _commandDispatcher = new CommandDispatcher(_eventPublisher, _commandHandlerProvider);
            _eventApplier = new EventApplier(new TypeInspector());
            _commandHandler = GetAggregateToApplyEventsTo();
        }

        protected abstract void ConfigureCommandHandlerFactory(ICommandHandlerFactory commandHandlerFactory);

        protected abstract TCommandHandler GetAggregateToApplyEventsTo();

        protected void Given(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                _eventApplier.ApplyEvent(@event, _commandHandler);
            }
        }

        protected void When(TCommand command)
        {
            _command = command;
        }

        protected void Then(params object[] expectedEvents)
        {
            HandleCommands();
            foreach (var expectedEvent in expectedEvents)
            {
                _eventPublisher.Received(1).Publish(Arg.Is<IEnumerable<IEvent>>(events => AtLeastOneEventMatches(expectedEvent, events)));
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
            _commandDispatcher.Dispatch(_command);
        }
    }
}