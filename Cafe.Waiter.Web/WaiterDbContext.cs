using Cafe.Waiter.Queries.DAL.Serialized;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Waiter.Web
{
    public class WaiterDbContext : DbContext
    {
        private readonly string _connectionString;

        public WaiterDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<OpenTab> OpenTabs { get; set; }
        public DbSet<TabDetails> TabDetails { get; set; }
        public DbSet<Menu> Menus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}