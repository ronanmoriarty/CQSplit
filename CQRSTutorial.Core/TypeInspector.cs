using System;
using System.Linq;
using System.Reflection;

namespace CQRSTutorial.Core
{
    public class TypeInspector
    {
        public MethodInfo FindMethodTakingSingleArgument(Type typeToInspect, string methodName, Type singleParameterType)
        {
            return typeToInspect
                .GetMethods()
                .SingleOrDefault(methodInfo => methodInfo.Name == methodName
                                               && methodInfo.GetParameters().Length == 1
                                               && methodInfo.GetParameters().Single().ParameterType == singleParameterType);
        }
    }
}