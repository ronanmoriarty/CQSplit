using System.Threading.Tasks;
using MassTransit;

namespace CQSplit.Messaging
{
    public interface ISendEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpoint(string queueName);
    }
}