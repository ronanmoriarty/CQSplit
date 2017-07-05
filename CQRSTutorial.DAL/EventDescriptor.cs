namespace CQRSTutorial.DAL
{
    public class EventDescriptor
    {
        public virtual int Id { get; set; }
        public virtual string EventType { get; set; }
        public virtual string Data { get; set; }
        public virtual string PublishTo { get; set; }
    }
}