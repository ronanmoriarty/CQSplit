using System;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Web
{
    public class MenuConfiguration : IMenuConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public MenuConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public Guid Id => new Guid(_configurationRoot["MenuId"]);
    }
}