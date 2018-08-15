using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace CQSplit.Messaging.Tests.Common
{
    public class Consumer<TMessage>
        : IConsumer<TMessage> where TMessage : class
    {
        private readonly Semaphore _semaphore;
        public bool ReceivedMessage;
        public IList<TMessage> ReceivedMessages { get; }

        public Consumer(Semaphore semaphore)
        {
            _semaphore = semaphore;
            ReceivedMessages = new List<TMessage>();
        }

        public async Task Consume(ConsumeContext<TMessage> context)
        {
            ReceivedMessage = true;
            ReceivedMessages.Add(context.Message);
            AllowTestThreadToContinueToAssertions();
        }

        private void AllowTestThreadToContinueToAssertions()
        {
            _semaphore.Release();
        }
    }
}