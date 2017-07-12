using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CQRSTutorial.DAL
{
    public class EventToPublishMapping : IAutoMappingOverride<EventToPublish>
    {
        public void Override(AutoMapping<EventToPublish> mapping)
        {
            mapping.Table("EventsToPublish");
        }
    }
}