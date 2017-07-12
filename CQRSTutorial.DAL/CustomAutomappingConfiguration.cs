using System;
using FluentNHibernate.Automapping;

namespace CQRSTutorial.DAL
{
    public class CustomAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Name == typeof(EventToPublish).Name;
        }
    }
}