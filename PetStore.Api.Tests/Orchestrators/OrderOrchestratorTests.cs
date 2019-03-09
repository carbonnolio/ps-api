using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PetStore.Api.Orchestrators.Implementations;
using PetStore.Api.Services;
using PetStore.Dto.Models;

namespace PetStore.Api.Tests.Orchestrators
{
    [TestFixture]
    public class OrderOrchestratorTests
    {
        private IInventoryClient _inventoryClient;
        private ILiteDbService _liteDbService;
        
        [SetUp]
        public void Setup()
        {
            _inventoryClient = Substitute.For<IInventoryClient>();
            _liteDbService = Substitute.For<ILiteDbService>();
        }

        [Test]
        public async Task CanPostOrder()
        {
            var orderRequest = StaticObjects.OrderRequestItems2;

            _inventoryClient.GetInventory<InventoryItem>(Arg.Is<string>(x => x.Contains("8ed0e6f7")))
                .Returns(new InventoryItem
                {
                    Id = "8ed0e6f7",
                    Name = "leash",
                    Price = 9.99m
                });

            _inventoryClient.GetInventory<InventoryItem>(Arg.Is<string>(x => x.Contains("c0258525")))
                .Returns(new InventoryItem
                {
                    Id = "c0258525",
                    Name = "collar",
                    Price = 6.99m
                });

            _liteDbService.Insert(Arg.Any<OrderSummary>()).Returns(1);

            var orderOrchestrator = new OrderOrchestrator(_inventoryClient, _liteDbService);

            var result = await orderOrchestrator.PostOrder(orderRequest);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(43.95m, result.OrderTotal);
        }

        [TearDown]
        public void Teardown()
        {
            _inventoryClient = null;
            _liteDbService = null;
        }
    }
}
