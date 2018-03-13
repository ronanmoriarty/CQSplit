using Cafe.Waiter.Command.Service.Consumers;
using Cafe.Waiter.Commands;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests.Consumers
{
    [TestFixture]
    public class OpenTabConsumerTests : ConsumerTests<OpenTabConsumer, OpenTabCommand>
    {
    }
}
