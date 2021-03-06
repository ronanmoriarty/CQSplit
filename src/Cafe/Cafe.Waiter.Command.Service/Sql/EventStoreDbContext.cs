﻿using CQSplit.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Waiter.Command.Service.Sql
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