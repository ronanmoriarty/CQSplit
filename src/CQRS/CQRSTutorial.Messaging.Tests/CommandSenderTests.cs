using System.Threading.Tasks;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class CommandSenderTests
    {
        private CommandSender _commandSender;
        private const string QueueName = "some queue";

        [Test]
        public async Task SendsCommandToSendEndpoint()
        {
            var sendEndPoint = Substitute.For<ISendEndpoint>();
            var sendEndpointProvider = Substitute.For<ISendEndpointProvider>();
            sendEndpointProvider.GetSendEndpoint(Arg.Is(QueueName)).Returns(Task.FromResult(sendEndPoint));
            var commandSendConfiguration = Substitute.For<ICommandSendConfiguration>();
            commandSendConfiguration.QueueName.Returns(QueueName);
            _commandSender = new CommandSender(sendEndpointProvider, commandSendConfiguration);
            var testCommand = new TestCommand();

            await _commandSender.Send(testCommand);

            await sendEndPoint.Received(1).Send(testCommand);
        }
    }
}