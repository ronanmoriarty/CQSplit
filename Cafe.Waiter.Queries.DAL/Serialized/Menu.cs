using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Waiter.Queries.DAL.Serialized
{
    [Table("Menu")]
    public class Menu
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}