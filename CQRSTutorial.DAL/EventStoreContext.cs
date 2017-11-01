using Microsoft.EntityFrameworkCore;

namespace CQRSTutorial.DAL
{
    public class EventStoreContext : DbContext
    {
        private readonly string _connectionString;

        public EventStoreContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventToPublish> EventsToPublish { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}