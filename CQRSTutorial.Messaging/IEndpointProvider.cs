using System;
using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IEndpointProvider
    {
        Task<ISendEndpoint> GetSendEndpointFor(Type messageType);
    }
}