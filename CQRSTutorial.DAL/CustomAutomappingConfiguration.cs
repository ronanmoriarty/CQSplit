using System;
using FluentNHibernate.Automapping;

namespace CQRSTutorial.DAL
{
    public class CustomAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Name == "EventDescriptor"; // TODO: we'll cater for entities here too as tests demand it.
        }
    }
}