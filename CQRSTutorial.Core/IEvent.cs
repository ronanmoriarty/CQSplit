using System;

namespace CQRSTutorial.Core
{
    public interface IEvent
    {
        Guid Id { get; set; }
    }
}