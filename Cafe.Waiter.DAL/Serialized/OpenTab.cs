using System;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.DAL.Serialized
{
    public class OpenTab : IMapToTable
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}