using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class CommandHandlerProviderTests
    {
        private CommandHandlerProvider _commandHandlerProvider;
        private ICommandHandlerFactory _commandHandlerFactory;

        [SetUp]
        public void SetUp()
        {
            _commandHandlerFactory = Substitute.For<ICommandHandlerFactory>();
            _commandHandlerProvider = new CommandHandlerProvider(_commandHandlerFactory);
        }

        [Test]
        public void Handler_created_if_no_handler_can_be_found()
        {
            var testCommand = new TestCommand();
            var commandHandler = Substitute.For<ICommandHandler<TestCommand>>();
            _commandHandlerFactory.CreateHandlerFor(testCommand).Returns(commandHandler);

            var actual = _commandHandlerProvider.GetCommandHandler(testCommand);

            Assert.That(actual, Is.EqualTo(commandHandler));
        }

        [ExcludeFromCodeCoverage]
        public class TestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }
    }
}
