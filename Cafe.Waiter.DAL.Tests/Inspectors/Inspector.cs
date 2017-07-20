using System.Reflection;

namespace Cafe.Waiter.DAL.Tests.Inspectors
{
    public abstract class Inspector<TTypeToInspect>
    {
        private TTypeToInspect _objectToInspect;

        public Inspector(TTypeToInspect objectToInspect)
        {
            _objectToInspect = objectToInspect;
        }

        protected T GetPrivateInstanceField<T>(string fieldName)
        {
            var fieldInfo = typeof(TTypeToInspect).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T) fieldInfo.GetValue(_objectToInspect);
        }
    }
}