using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpoint();
    }
}