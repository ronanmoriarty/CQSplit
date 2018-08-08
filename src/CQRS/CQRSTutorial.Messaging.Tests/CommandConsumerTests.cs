using System.Threading.Tasks;
using CQRSTutorial.Core;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class CommandConsumerTests
    {
        private ICommandRouter _commandRouter;
        private TestCommandConsumer _testCommandConsumer;
        private ConsumeContext<TestCommand> _consumeContext;
        private TestCommand _testCommand;

        [SetUp]
        public void SetUp()
        {
            _commandRouter = Substitute.For<ICommandRouter>();
            _testCommandConsumer = new TestCommandConsumer(_commandRouter);
            _consumeContext = Substitute.For<ConsumeContext<TestCommand>>();
            _testCommand = new TestCommand();
            _consumeContext.Message.Returns(_testCommand);
        }

        [Test]
        public async Task Router_routes_incoming_commands()
        {
            await _testCommandConsumer.Consume(_consumeContext);

            _commandRouter.Received(1).Route(_testCommand);
        }

        private class TestCommandConsumer : CommandConsumer<TestCommand>
        {
            public TestCommandConsumer(ICommandRouter commandRouter)
                : base(commandRouter)
            {
            }
        }
    }
}