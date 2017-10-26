using System.Data.Entity;
using System.Data.SqlClient;
using Cafe.Waiter.Queries.DAL.Serialized;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class WaiterDbContext : DbContext
    {
        public WaiterDbContext()
            : base(new SqlConnection(ReadModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString()), true)
        {
        }

        public DbSet<OpenTab> OpenTabs { get; set; }
        public DbSet<TabDetails> TabDetails { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}