using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Cafe.Waiter.DAL.Mapping
{
    public class OpenTabMapping : IAutoMappingOverride<Serialized.OpenTab>
    {
        public void Override(AutoMapping<Serialized.OpenTab> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping.Table("OpenTabs");
        }
    }
}