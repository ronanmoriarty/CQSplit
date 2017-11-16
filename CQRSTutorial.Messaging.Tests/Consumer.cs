using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging.Tests
{
    public class Consumer<TMessage>
        : IConsumer<TMessage> where TMessage : class
    {
        private readonly ManualResetEvent _manualResetEvent;
        public bool ReceivedMessage;
        public IList<TMessage> ReceivedMessages { get; }

        public Consumer(ManualResetEvent manualResetEvent)
        {
            _manualResetEvent = manualResetEvent;
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
            _manualResetEvent.Set();
        }
    }
}