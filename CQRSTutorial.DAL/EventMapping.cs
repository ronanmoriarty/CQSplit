using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CQRSTutorial.DAL
{
    public class EventMapping : IAutoMappingOverride<Event>
    {
        public void Override(AutoMapping<Event> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping.Table("Events");
        }
    }
}