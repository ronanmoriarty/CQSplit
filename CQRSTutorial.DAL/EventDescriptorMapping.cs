using FluentNHibernate.Mapping;

namespace CQRSTutorial.DAL
{
    public class EventDescriptorMapping : ClassMap<EventDescriptor>
    {
        public EventDescriptorMapping()
        {
            Table("Events");
            Id(x => x.Id);
            Map(x => x.EventType);
            Map(x => x.Data);
        }
    }
}