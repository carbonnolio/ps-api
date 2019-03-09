using System.Collections.Generic;
using System.Threading.Tasks;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Orchestrators
{
    public interface IOrderOrchestrator
    {
        Task<OrderSummary> PostOrder(OrderRequest order);
        List<OrderSummary> GetOrderSummary(string customerId);
        OrderSummary GetOrderSummary(int id);
    }
}
