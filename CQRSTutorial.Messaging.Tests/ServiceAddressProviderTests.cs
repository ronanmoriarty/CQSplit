using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class ServiceAddressProviderTests
    {
        private ServiceAddressProvider _serviceAddressProvider;

        [SetUp]
        public void SetUp()
        {
            _serviceAddressProvider = new ServiceAddressProvider();
        }

        [Test]
        public void Replaces_capitals_with_underscores()
        {
            var endPointAddress = _serviceAddressProvider.GetServiceAddressFor(typeof(TestClass));
            Assert.That(endPointAddress, Is.EqualTo("test_class"));
        }

        [Test]
        public void Ignores_leading_I_when_following_character_is_also_a_capital()
        {
            var endPointAddress = _serviceAddressProvider.GetServiceAddressFor(typeof(ITestClass));
            Assert.That(endPointAddress, Is.EqualTo("test_class"));
        }

        [Test]
        public void Keeps_leading_I_when_I_is_part_of_first_word()
        {
            var endPointAddress = _serviceAddressProvider.GetServiceAddressFor(typeof(IabcDef));
            Assert.That(endPointAddress, Is.EqualTo("iabc_def"));
        }

        public class TestClass
        {
        }

        public interface ITestClass
        {
        }

        public class IabcDef
        {
        }
    }
}
