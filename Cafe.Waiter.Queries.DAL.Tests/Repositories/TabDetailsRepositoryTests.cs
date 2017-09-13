using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.NHibernate;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.DAL.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cafe.Waiter.Queries.DAL.Tests.Repositories
{
    [TestFixture]
    public class TabDetailsRepositoryTests
    {
        private readonly Guid _id = new Guid("82EBC82F-72EE-42D8-9565-49B0E1844C86");
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProviderFactory.Instance);
        private readonly string _waiter = "Louise";
        private readonly int _tableNumber = 654;
        private readonly TabStatus _tabStatus = TabStatus.OrderPlaced;
        private TabDetailsRepository _tabDetailsRepository;

        [SetUp]
        public void SetUp()
        {
            var tabDetailsJson = GetTabDetailsJson();
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.TabDetails WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.TabDetails(Id,Data) VALUES ('{_id}','{tabDetailsJson}')");
            _tabDetailsRepository = new TabDetailsRepository(ReadModelSessionFactory.Instance);
        }

        [Test]
        public void Can_retrieve_tab_details()
        {
            var tabDetails = _tabDetailsRepository.GetTabDetails(_id);

            Assert.That(tabDetails.TabId, Is.EqualTo(_id));
            //Assert.That(tabDetails.Waiter, Is.EqualTo(_waiter));
            //Assert.That(tabDetails.TableNumber, Is.EqualTo(_tableNumber));
            //Assert.That(tabDetails.Status, Is.EqualTo(_tabStatus));
        }

        private string GetTabDetailsJson()
        {
            var openTab = new TabDetails
            {
                TabId = _id,
                //Waiter = _waiter,
                //TableNumber = _tableNumber,
                //Status = TabStatus.OrderPlaced
            };

            return JsonConvert.SerializeObject(openTab);
        }
    }
}