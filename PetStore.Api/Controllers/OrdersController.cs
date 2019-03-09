using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Api.Orchestrators;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderOrchestrator _orderOrchestrator;

        public OrdersController(IOrderOrchestrator orderOrchestrator)
        {
            _orderOrchestrator = orderOrchestrator;
        }

        [HttpPost]
        public async Task<OrderSummary> SubmitOrder([FromBody] OrderRequest request)
        {
            return await _orderOrchestrator.PostOrder(request);
        }

        [HttpGet("Summary/CustomerId/{customerId}")]
        public List<OrderSummary> GetOrderSummaries([Required][MinLength(1)] string customerId)
        {
            return _orderOrchestrator.GetOrderSummary(customerId);
        }

        [HttpGet("Summary/{id}")]
        public OrderSummary GetOrderSummaries([Required][Range(1, int.MaxValue)] int id)
        {
            return _orderOrchestrator.GetOrderSummary(id);
        }
    }
}