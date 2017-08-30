using System;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.DAL
{
    public class OpenTab : IMapToTable
    {
        public virtual Guid Id { get; set; }
    }
}