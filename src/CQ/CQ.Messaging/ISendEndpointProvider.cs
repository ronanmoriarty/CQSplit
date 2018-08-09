using System.Threading.Tasks;
using MassTransit;

namespace CQ.Messaging
{
    public interface ISendEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpoint(string queueName);
    }
}