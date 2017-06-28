using System;

namespace CQRSTutorial.DAL
{
    public class EventDescriptor
    {
        public virtual Guid Id { get; set; }
        public virtual Type EventType { get; set; }
        public virtual string Data { get; set; }
    }
}