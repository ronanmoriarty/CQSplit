namespace CQRSTutorial.Infrastructure
{
    public interface IServiceAddressProvider
    {
        string GetServiceAddressFor<T>();
    }
}