using System;

namespace CQRSTutorial.Infrastructure
{
    public interface IServiceAddressProvider
    {
        string GetServiceAddressFor<T>();
        string GetServiceAddressFor(Type consumerType, string stringToReplace, string stringToReplaceWith);
    }
}