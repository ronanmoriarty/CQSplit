namespace CQRSTutorial.DAL
{
    public class Event : IMapToTable
    {
        public virtual int Id { get; set; }
        public virtual int AggregateId { get; set; }
        public virtual string EventType { get; set; }
        public virtual string Data { get; set; }
    }
}