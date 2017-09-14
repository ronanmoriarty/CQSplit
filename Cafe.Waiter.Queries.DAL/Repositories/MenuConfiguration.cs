using System;
using System.Configuration;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class MenuConfiguration : IMenuConfiguration
    {
        public Guid Id => new Guid(ConfigurationManager.AppSettings["MenuId"]);
    }
}