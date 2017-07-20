using System;
using System.Linq;
using System.Reflection;

namespace CQRSTutorial.Core
{
    public class TypeInspector
    {
        public MethodInfo FindMethodTakingSingleArgument(object objectToInspect, string methodName, Type singleParameterType)
        {
            return objectToInspect
                .GetType()
                .GetMethods()
                .SingleOrDefault(methodInfo => methodInfo.Name == methodName
                                               && methodInfo.GetParameters().Length == 1
                                               && methodInfo.GetParameters().Single().ParameterType == singleParameterType);
        }
    }
}