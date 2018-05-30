using CQRSTutorial.DAL.Serialized;
using Microsoft.EntityFrameworkCore;

namespace CQRSTutorial.DAL
{
    public class EventStoreDbContext : DbContext
    {
        private readonly string _connectionString;

        public EventStoreDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventToPublish> EventsToPublish { get; set; }
    }
}