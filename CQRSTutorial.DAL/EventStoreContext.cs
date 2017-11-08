using System.Data.Entity;
using System.Data.SqlClient;

namespace CQRSTutorial.DAL
{
    public class EventStoreContext : DbContext
    {
        public EventStoreContext(string connectionString)
            : base(new SqlConnection(connectionString), true)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventToPublish> EventsToPublish { get; set; }
    }
}