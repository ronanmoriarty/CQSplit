using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public interface IEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpointFor<T>();
    }
}