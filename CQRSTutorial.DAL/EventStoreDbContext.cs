﻿using System.Data.Entity;
using System.Data.SqlClient;
using CQRSTutorial.DAL.Serialized;

namespace CQRSTutorial.DAL
{
    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(string connectionString)
            : base(new SqlConnection(connectionString), true)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventToPublish> EventsToPublish { get; set; }
    }
}