using System;
using Cafe.Waiter.Contracts.Queries;

namespace Cafe.Waiter.Web.Controllers
{
    public class OpenTabsQuery : IOpenTabsQuery
    {
        public Guid Id { get; set; }
    }
}