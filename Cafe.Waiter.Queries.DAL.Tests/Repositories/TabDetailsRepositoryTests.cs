using System;
using System.Collections.Generic;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web;
using CQRSTutorial.DAL.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cafe.Waiter.Queries.DAL.Tests.Repositories
{
    [TestFixture]
    public class TabDetailsRepositoryTests
    {
        private readonly Guid _id = new Guid("82EBC82F-72EE-42D8-9565-49B0E1844C86");
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProvider.Instance);
        private TabDetailsRepository _tabDetailsRepository;
        private TabDetails _tabDetails;

        [SetUp]
        public void SetUp()
        {
            _tabDetails = GetTabDetails();
            var tabDetailsJson = JsonConvert.SerializeObject(_tabDetails);
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.TabDetails WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.TabDetails(Id,Data) VALUES ('{_id}','{tabDetailsJson}')");
            _tabDetailsRepository = new TabDetailsRepository(new WaiterDbContextAdapter(ReadModelConnectionStringProvider.Instance));
        }

        [Test]
        public void Can_retrieve_tab_details()
        {
            var retrievedTabDetails = _tabDetailsRepository.GetTabDetails(_id);

            Assert.That(retrievedTabDetails.Id, Is.EqualTo(_tabDetails.Id));
            Assert.That(retrievedTabDetails.Waiter, Is.EqualTo(_tabDetails.Waiter));
            Assert.That(retrievedTabDetails.TableNumber, Is.EqualTo(_tabDetails.TableNumber));
            Assert.That(retrievedTabDetails.Status, Is.EqualTo(_tabDetails.Status));
            CollectionAssert.AreEquivalent(retrievedTabDetails.Items, retrievedTabDetails.Items);
        }

        private TabDetails GetTabDetails()
        {
            return new TabDetails
            {
                Id = _id,
                Waiter = "Louise",
                TableNumber = 654,
                Status = TabStatus.OrderPlaced,
                Items = new List<TabItem>
                {
                    new TabItem
                    {
                        MenuNumber = 234,
                        IsDrink = false,
                        Name = "Bacon & Cheese Hamburger",
                        Notes = "No pickle"
                    },
                    new TabItem
                    {
                        MenuNumber = 234,
                        IsDrink = false,
                        Name = "Bacon & Cheese Hamburger",
                        Notes = "Extra pickle. No mayonnaise."
                    },
                    new TabItem
                    {
                        MenuNumber = 123,
                        IsDrink = true,
                        Name = "Coca Cola",
                    },
                }
            };
        }
    }
}