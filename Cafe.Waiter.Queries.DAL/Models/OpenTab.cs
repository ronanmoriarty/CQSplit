using System;

namespace Cafe.Waiter.Queries.DAL.Models
{
    public class OpenTab
    {
        public virtual Guid Id { get; set; }
        public virtual string Waiter { get; set; }
        public virtual int TableNumber { get; set; }
        public virtual TabStatus Status { get; set; }
    }
}