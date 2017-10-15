using System;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Queries.DAL.Serialized
{
    public class TabDetails : IMapToTable
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}