using Cafe.Waiter.Command.Service.Consumers;
using Cafe.Waiter.Commands;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.IntegrationTests.Consumers
{
    [TestFixture]
    public class MarkFoodServedConsumerTests : ConsumerTests<MarkFoodServedConsumer, MarkFoodServedCommand>
    {
    }
}