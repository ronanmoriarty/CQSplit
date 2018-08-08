using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class TypeInspectorTests
    {
        private TypeInspector _typeInspector;

        [SetUp]
        public void SetUp()
        {
            _typeInspector = new TypeInspector();
        }

        [Test]
        public void Can_get_methods_taking_single_argument_of_a_particular_type()
        {
            var methodInfoTakingSingleStringParameter = _typeInspector.FindMethodTakingSingleArgument(typeof(TestType), "TakesSingleStringParameter", typeof(string));
            var methodInfoTakingSingleIntParameter = _typeInspector.FindMethodTakingSingleArgument(typeof(TestType), "TakesSingleIntParameter", typeof(int));

            Assert.That(methodInfoTakingSingleStringParameter.GetParameters().Single().ParameterType, Is.EqualTo(typeof(string)));
            Assert.That(methodInfoTakingSingleIntParameter.GetParameters().Single().ParameterType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Does_not_return_methods_with_more_than_one_parameter()
        {
            var methodInfoTakingSingleStringArgument = _typeInspector.FindMethodTakingSingleArgument(typeof(TestType), "TakesTwoStringParameters", typeof(string));

            Assert.That(methodInfoTakingSingleStringArgument, Is.Null);
        }

        [Test]
        public void Does_not_return_methods_with_mismatching_parameter_type()
        {
            var nonMatchingParameterType = typeof(int);

            var methodInfoTakingSingleStringArgument = _typeInspector.FindMethodTakingSingleArgument(typeof(TestType), "TakesSingleStringParameter", nonMatchingParameterType);

            Assert.That(methodInfoTakingSingleStringArgument, Is.Null);
        }

        [ExcludeFromCodeCoverage]
        private class TestType
        {
            public void TakesSingleStringParameter(string a)
            {
            }

            public void TakesTwoStringParameters(string a, string b)
            {
            }

            public void TakesSingleIntParameter(int a)
            {
            }
        }
    }
}
