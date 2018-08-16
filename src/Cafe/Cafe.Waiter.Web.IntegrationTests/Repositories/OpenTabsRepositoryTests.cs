using System;
using System.Linq;
using Cafe.DAL.Tests.Common;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Web.IntegrationTests.Controllers;
using Cafe.Waiter.Web.Repositories;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cafe.Waiter.Web.IntegrationTests.Repositories
{
    [TestFixture, Category(TestConstants.Integration)]
    public class OpenTabsRepositoryTests
    {
        private readonly Guid _id = new Guid("82EBC82F-72EE-42D8-9565-49B0E1844C86");
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProvider.Instance);
        private readonly string _waiter = "Louise";
        private readonly int _tableNumber = 654;
        private readonly TabStatus _tabStatus = TabStatus.OrderPlaced;
        private OpenTabsRepository _openTabsRepository;

        [SetUp]
        public void SetUp()
        {
            var openTabJson = GetOpenTabJson();
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.OpenTabs WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.OpenTabs(Id,Data) VALUES ('{_id}','{openTabJson}')");
            _openTabsRepository = new OpenTabsRepository(ReadModelConnectionStringProvider.Instance);
        }

        [Test]
        public void Can_retrieve_open_tabs()
        {
            var openTabs = _openTabsRepository.GetOpenTabs();

            var tab = openTabs.Single(openTab => openTab.Id == _id);
            Assert.That(tab, Is.Not.Null);
            Assert.That(tab.Waiter, Is.EqualTo(_waiter));
            Assert.That(tab.TableNumber, Is.EqualTo(_tableNumber));
            Assert.That(tab.Status, Is.EqualTo(_tabStatus));
        }

        private string GetOpenTabJson()
        {
            var openTab = new OpenTab
            {
                Id = _id,
                Waiter = _waiter,
                TableNumber = _tableNumber,
                Status = TabStatus.OrderPlaced
            };

            return JsonConvert.SerializeObject(openTab);
        }
    }
}
