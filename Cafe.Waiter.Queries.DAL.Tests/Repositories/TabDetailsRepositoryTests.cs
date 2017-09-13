using System;
using System.Collections.Generic;
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
        private TabDetailsRepository _tabDetailsRepository;
        private TabDetails _tabDetails;

        [SetUp]
        public void SetUp()
        {
            _tabDetails = GetTabDetails();
            var tabDetailsJson = JsonConvert.SerializeObject(_tabDetails);
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.TabDetails WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.TabDetails(Id,Data) VALUES ('{_id}','{tabDetailsJson}')");
            _tabDetailsRepository = new TabDetailsRepository(ReadModelSessionFactory.Instance);
        }

        [Test]
        public void Can_retrieve_tab_details()
        {
            var tabDetails = _tabDetailsRepository.GetTabDetails(_id);

            Assert.That(tabDetails.TabId, Is.EqualTo(_tabDetails.TabId));
            Assert.That(tabDetails.Waiter, Is.EqualTo(_tabDetails.Waiter));
            Assert.That(tabDetails.TableNumber, Is.EqualTo(_tabDetails.TableNumber));
            Assert.That(tabDetails.Status, Is.EqualTo(_tabDetails.Status));
            CollectionAssert.AreEquivalent(tabDetails.Items, tabDetails.Items);
        }

        private TabDetails GetTabDetails()
        {
            return new TabDetails
            {
                TabId = _id,
                Waiter = "Louise",
                TableNumber = 654,
                Status = TabStatus.OrderPlaced,
                Items = new List<TabItem>
                {
                    new TabItem
                    {
                        MenuNumber = 123,
                        Notes = "No pickle"
                    },
                    new TabItem
                    {
                        MenuNumber = 123,
                        Notes = "Extra pickle. No mayonnaise."
                    },
                    new TabItem
                    {
                        MenuNumber = 234
                    },
                }
            };
        }
    }
}