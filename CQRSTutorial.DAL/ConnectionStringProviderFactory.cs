namespace CQRSTutorial.DAL
{
    public class ConnectionStringProviderFactory : IConnectionStringProviderFactory
    {
        public IConnectionStringProvider GetConnectionStringProvider()
        {
            return new AppConfigConnectionStringProvider("CQRSTutorial");
        }
    }
}