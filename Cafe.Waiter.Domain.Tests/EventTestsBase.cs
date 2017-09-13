using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Domain;
using CQRSTutorial.Core;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    public abstract class EventTestsBase<TCommandHandler, TCommand>
        where TCommandHandler : Aggregate, new()
        where TCommand : ICommand
    {
        private IEventHandler _eventHandler;
        private CommandDispatcher _commandDispatcher;
        private EventApplier _eventApplier;
        private TCommand _command;
        private CommandHandlerProvider _commandHandlerProvider;
        protected readonly Guid AggregateId = new Guid("17BEED1C-2084-4ADA-938A-4F850212EB5D");
        protected TCommandHandler CommandHandler;
        protected readonly Guid CommandId = new Guid("0E72CCC8-2C6C-4F02-B524-2FB958347564");
        protected TCommandHandler CommandHandler2;
        private readonly Guid _aggregateId2 = new Guid("88CEC1FD-A666-4A51-ABD4-3AA49AE35001");
        private OpenTabCommandHandler _openTabCommandHandler;

        [SetUp]
        public void SetUp()
        {
            var commandHandlerFactory = Substitute.For<ICommandHandlerFactory>();
            _commandHandlerProvider = new CommandHandlerProvider(commandHandlerFactory);
            _openTabCommandHandler = new OpenTabCommandHandler();
            _commandHandlerProvider.RegisterCommandHandler(_openTabCommandHandler);
            CommandHandler = new TCommandHandler
            {
                Id = AggregateId
            };
            CommandHandler2 = new TCommandHandler
            {
                Id = _aggregateId2
            };

            if (!CanUsePreregisteredCommandHandlersToHandleCommand())
            {
                ConfigureCommandHandlerFactory(commandHandlerFactory);
            }

            _eventHandler = Substitute.For<IEventHandler>();
            _commandDispatcher = new CommandDispatcher(_eventHandler, _commandHandlerProvider);
            _eventApplier = new EventApplier(new TypeInspector());
        }

        protected virtual bool CanUsePreregisteredCommandHandlersToHandleCommand()
        {
            return false;
        }

        protected void Given(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                _eventApplier.ApplyEvent(@event, CommandHandler);
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
                _eventHandler.Received(1).Handle(Arg.Is<IEnumerable<IEvent>>(events => AtLeastOneEventMatches(expectedEvent, events)));
            }
        }

        private void ConfigureCommandHandlerFactory(ICommandHandlerFactory commandHandlerFactory)
        {
            commandHandlerFactory.CreateHandlerFor(Arg.Is<TCommand>(command => command.AggregateId == AggregateId)).Returns((ICommandHandler<TCommand>) CommandHandler);
            commandHandlerFactory.CreateHandlerFor(Arg.Is<TCommand>(command => command.AggregateId == _aggregateId2)).Returns((ICommandHandler<TCommand>) CommandHandler2);
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