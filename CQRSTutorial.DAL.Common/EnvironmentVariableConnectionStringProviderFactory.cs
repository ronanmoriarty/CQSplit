using System;

namespace CQRSTutorial.DAL.Common
{
    public class EnvironmentVariableConnectionStringProviderFactory
    {
        public static IConnectionStringProvider Get(string environmentVariableKey)
        {
            return new ConnectionStringOverride(Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Machine));
        }
    }
}