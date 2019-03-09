using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PetStore.Api.Exceptions;
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
                    await _inventoryClient.GetInventory<InventoryItem>($"{ProductUrl}/{item.ProductId}", errorCode => 
                    {
                        switch (errorCode)
                        {
                            case 404:
                                throw new CustomHttpException(HttpStatusCode.NotFound,
                                    $"ItemId: {item.ProductId} is not available.");

                            default:
                                throw new CustomHttpException(HttpStatusCode.InternalServerError,
                                    $"Inventory API responded with status code {errorCode}");
                        }
                    });

                orderItems.Add(new OrderItem
                {
                    Name = inventoryItem.Name,
                    Price = inventoryItem.Price,
                    ProductId = inventoryItem.Id,
                    Quantity = item.Quantity
                });
            }

            // If there are too many items in the request, looping over each item and await could potencially take a lot of time.
            // Doing sometning like: 

            //var inventoryItemTasks = order.Items.Select(x => _inventoryClient.GetInventory<InventoryItem>($"{ProductUrl}/{x.ProductId}"));
            //var results = await Task.WhenAll(inventoryItemTasks);
            //var resultsList = results.ToList();

            // May save a lot of time and yield better response time, however it will also put more load on the external API. 

            var orderSummary = new OrderSummary
            {
                CustomerId = order.CustomerId,
                Items = orderItems,
                OrderTotal = orderItems.Sum(x => x.Price * x.Quantity)
            };

            orderSummary.Id = _liteDbService.Insert(orderSummary);

            return orderSummary;
        }

        public List<OrderSummary> GetOrderSummary(string customerId)
        {
            var result = _liteDbService.Get<OrderSummary>(col => 
                col.Find(x => x.CustomerId.Equals(customerId, StringComparison.InvariantCultureIgnoreCase)));

            if (result == null || !result.Any())
                throw new CustomHttpException(HttpStatusCode.NotFound, $"No order summaries found for the customerId: {customerId}");

            return result;
        }

        public OrderSummary GetOrderSummary(int id)
        {
            var result = _liteDbService.Get<OrderSummary>(col =>
                col.FindOne(x => x.Id == id));

            if(result == null)
                throw new CustomHttpException(HttpStatusCode.NotFound, $"No order summaries found for the id: {id}");

            return result;
        }
    }
}
