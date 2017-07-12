using System;
using System.Linq;
using FluentNHibernate.Automapping;

namespace CQRSTutorial.DAL
{
    public class CustomAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IMapToTable));
        }
    }
}