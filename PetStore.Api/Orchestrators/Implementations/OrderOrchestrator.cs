using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Api.Services;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Orchestrators.Implementations
{
    public class OrderOrchestrator : IOrderOrchestrator
    {
        private readonly IInventoryClient _inventoryClient;
        private readonly ILiteDbService _liteDbService;

        private const string ProductUrl = "default/product";

        public OrderOrchestrator(IInventoryClient inventoryClient, ILiteDbService liteDbService)
        {
            _inventoryClient = inventoryClient;
            _liteDbService = liteDbService;
        }

        public async Task<OrderSummary> PostOrder(OrderRequest order)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in order.Items)
            {
                var inventoryItem =
                    await _inventoryClient.GetInventory<InventoryItem>($"{ProductUrl}/{item.ProductId}");

                orderItems.Add(new OrderItem
                {
                    Name = inventoryItem.Name,
                    Price = inventoryItem.Price,
                    ProductId = inventoryItem.Id,
                    Quantity = item.Quantity
                });
            }

            var orderSummary = new OrderSummary
            {
                CustomerId = order.CustomerId,
                Items = orderItems,
                OrderTotal = orderItems.Select(x => x.Price * x.Quantity).Sum()
            };

            orderSummary.Id = _liteDbService.Insert(orderSummary);

            return orderSummary;
        }

        public List<OrderSummary> GetOrderSummary(string customerId)
        {
            var result = _liteDbService.Get<OrderSummary>(col => 
                col.Find(x => x.CustomerId.Equals(customerId, StringComparison.InvariantCultureIgnoreCase)));

            return result;
        }
    }
}
