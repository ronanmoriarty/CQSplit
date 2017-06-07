using System;

namespace Cafe.Domain
{
    public interface IEvent
    {
        Guid Id { get; set; }
    }
}