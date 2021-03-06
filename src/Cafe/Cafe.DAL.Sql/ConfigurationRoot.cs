﻿using Microsoft.Extensions.Configuration;

namespace Cafe.DAL.Sql
{
    public static class ConfigurationRoot
    {
        public static IConfigurationRoot Instance => GetConfigurationRoot();

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
