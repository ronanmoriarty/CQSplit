using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Queries;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Query.Service.Messaging
{
    public class OpenTabsQueryConsumer : IConsumer<IOpenTabsQuery>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabsQueryConsumer));
        public async Task Consume(ConsumeContext<IOpenTabsQuery> context)
        {
            var text = $"Received command: Type: {typeof(IOpenTabsQuery).Name}; Query Id: {context.Message.Id};";
            await Console.Out.WriteLineAsync(text);
            _logger.Debug(text);
        }
    }
}
