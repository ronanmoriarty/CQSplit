using System;
using System.Linq;
using CQRSTutorial.DAL.Tests.Common;
using NUnit.Framework;

namespace Cafe.Waiter.DAL.Tests
{
    [TestFixture]
    public class OpenTabsProviderTests
    {
        private readonly Guid _id = new Guid("82EBC82F-72EE-42D8-9565-49B0E1844C86");
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProviderFactory.Instance);

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.OpenTabs WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.OpenTabs(Id) VALUES ('{_id}')");
        }

        [Test]
        public void Can_retrieve_open_tabs()
        {
            var openTabs = new OpenTabsProvider(SessionFactory.Instance).GetOpenTabs();

            var tab = openTabs.Single(openTab => openTab.Id == _id);
            Assert.That(tab, Is.Not.Null);
        }
    }
}
