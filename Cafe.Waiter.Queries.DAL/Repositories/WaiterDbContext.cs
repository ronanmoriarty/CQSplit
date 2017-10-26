using System.Data.Entity;
using System.Data.SqlClient;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class WaiterDbContext : DbContext
    {
        public WaiterDbContext()
            : base(new SqlConnection(ReadModel.GetEntityFrameworkConnectionString()), true)
        {
        }

        public DbSet<Serialized.OpenTab> OpenTabs { get; set; }
    }
}