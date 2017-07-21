using System;

namespace CQRSTutorial.DAL
{
    public class ConnectionStringProviderFactory : IConnectionStringProviderFactory
    {
        public IConnectionStringProvider GetConnectionStringProvider()
        {
            var connectionString = Environment.GetEnvironmentVariable("CQRS_CONNECTIONSTRING_OVERRIDE", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(connectionString))
            {
                return new ConnectionStringOverride(connectionString);
            }

            return new AppConfigConnectionStringProvider("CQRSTutorial");
        }
    }
}