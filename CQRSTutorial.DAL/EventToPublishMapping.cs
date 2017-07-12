using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CQRSTutorial.DAL
{
    public class EventToPublishMapping : IAutoMappingOverride<EventToPublish>
    {
        public void Override(AutoMapping<EventToPublish> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping.Table("EventsToPublish");
        }
    }
}