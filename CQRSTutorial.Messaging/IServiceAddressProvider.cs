using System;

namespace CQRSTutorial.Messaging
{
    public interface IServiceAddressProvider
    {
        string GetServiceAddressFor<T>();
        string GetServiceAddressFor(Type consumerType, string stringToReplace, string stringToReplaceWith);
    }
}