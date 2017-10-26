using System;
using System.ComponentModel.DataAnnotations.Schema;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Queries.DAL.Serialized
{
    [Table("Menu")]
    public class Menu : IMapToTable
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}