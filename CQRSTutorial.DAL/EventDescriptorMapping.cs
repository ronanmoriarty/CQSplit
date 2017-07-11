using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CQRSTutorial.DAL
{
    public class EventDescriptorMapping : IAutoMappingOverride<EventDescriptor>
    {
        public void Override(AutoMapping<EventDescriptor> mapping)
        {
            mapping.Table("EventsToPublish");
        }
    }
}