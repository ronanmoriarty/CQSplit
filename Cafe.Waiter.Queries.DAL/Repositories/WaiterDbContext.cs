using System.Data.Entity;
using System.Data.SqlClient;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class WaiterDbContext : DbContext
    {
        public WaiterDbContext()
            : base(new SqlConnection(ReadModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString()), true)
        {
        }

        public DbSet<Serialized.OpenTab> OpenTabs { get; set; }
    }
}