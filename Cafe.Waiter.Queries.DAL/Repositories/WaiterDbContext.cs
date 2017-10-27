using System.Data.Entity;
using System.Data.SqlClient;
using Cafe.Waiter.Queries.DAL.Serialized;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class WaiterDbContext : DbContext
    {
        public WaiterDbContext(string connectionString)
            : base(new SqlConnection(connectionString), true)
        {
        }

        public DbSet<OpenTab> OpenTabs { get; set; }
        public DbSet<TabDetails> TabDetails { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}