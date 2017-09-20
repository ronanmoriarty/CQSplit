using System;

namespace CQRSTutorial.Messaging
{
    public interface IServiceAddressProvider
    {
        string GetServiceAddressFor(Type messageType);
        string GetServiceAddressFor(Type consumerType, string stringToReplace, string stringToReplaceWith);
    }
}