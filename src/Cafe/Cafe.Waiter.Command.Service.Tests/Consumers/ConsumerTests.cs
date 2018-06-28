using System;
using CQRSTutorial.Core;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests.Consumers
{
    public abstract class ConsumerTests<TConsumer, TCommand>
        where TConsumer : IConsumer<TCommand>
        where TCommand : class, ICommand, new()
    {
        private ICommandRouter _commandRouter;
        private TConsumer _consumer;
        private ConsumeContext<TCommand> _consumeContext;
        private TCommand _command;

        [SetUp]
        public void SetUp()
        {
            _commandRouter = Substitute.For<ICommandRouter>();
            _consumer = (TConsumer)Activator.CreateInstance(typeof(TConsumer),_commandRouter);
            _consumeContext = Substitute.For<ConsumeContext<TCommand>>();
            _command = new TCommand();
            _consumeContext.Message.Returns(_command);
        }

        [Test]
        public void Command_dispatcher_dispatches_received_commands()
        {
            _consumer.Consume(_consumeContext);

            _commandRouter.Received(1).Route(Arg.Is(_command));
        }
    }
}