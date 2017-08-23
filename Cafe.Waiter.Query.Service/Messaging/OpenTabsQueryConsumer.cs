using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Queries;
using Cafe.Waiter.Contracts.QueryResponses;
using Cafe.Waiter.Query.Service.QueryResponses;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Query.Service.Messaging
{
    public class OpenTabsQueryConsumer : IConsumer<IOpenTabsQuery>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabsQueryConsumer));
        public async Task Consume(ConsumeContext<IOpenTabsQuery> context)
        {
            var text = $"Received query: Type: {typeof(IOpenTabsQuery).Name}; Query Id: {context.Message.Id};";
            await Console.Out.WriteLineAsync(text);
            _logger.Debug(text);

            // TODO: get tests around this - just spiking it for now
            var openTabsQueryResponse = GetOpenTabsQueryResponse(context.Message);
            await context.RespondAsync<IOpenTabsQueryResponse>(openTabsQueryResponse);
        }

        private static OpenTabsQueryResponse GetOpenTabsQueryResponse(IOpenTabsQuery openTabsQuery)
        {
            return new OpenTabsQueryResponse
            {
                QueryId = openTabsQuery.Id,
                Tabs = GetOpenTabs()
            };
        }

        private static List<ITab> GetOpenTabs()
        {
            return new List<ITab>
            {
                new Tab
                {
                    TableNumber = 123,
                    Waiter = "John"
                },
                new Tab
                {
                    TableNumber = 234,
                    Waiter = "Mary"
                }
            };
        }
    }
}
