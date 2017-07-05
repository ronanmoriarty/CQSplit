using CQRSTutorial.Core;

namespace Cafe.Domain.Events
{
    public interface ITabEvent : IEvent
    {
        int TabId { get; set; }
    }
}