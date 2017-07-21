namespace CQRSTutorial.DAL
{
    public interface IConnectionStringProviderFactory
    {
        IConnectionStringProvider GetConnectionStringProvider();
    }
}