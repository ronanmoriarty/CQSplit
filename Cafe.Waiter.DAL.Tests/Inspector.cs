using System.Reflection;
using Cafe.Domain;

namespace Cafe.Waiter.DAL.Tests
{
    public abstract class Inspector
    {
        protected T GetPrivateInstanceField<TTypeToInspect, T>(string fieldName, Tab tab)
        {
            var fieldInfo = typeof(TTypeToInspect).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T) fieldInfo.GetValue(tab);
        }
    }
}