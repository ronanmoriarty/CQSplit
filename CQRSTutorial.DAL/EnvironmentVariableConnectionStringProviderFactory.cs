﻿using System;

namespace CQRSTutorial.DAL
{
    public class EnvironmentVariableConnectionStringProviderFactory
    {
        public static IConnectionStringProvider Get(string environmentVariableKey)
        {
            return new ConnectionStringOverride(Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Machine));
        }
    }
}