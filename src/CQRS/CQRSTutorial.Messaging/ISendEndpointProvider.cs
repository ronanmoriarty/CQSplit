using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface ISendEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpoint(string queueName);
    }
}