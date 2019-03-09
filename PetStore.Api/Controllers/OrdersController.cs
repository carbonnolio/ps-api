using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Api.Orchestrators;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("Summary")]
        public List<OrderSummary> GetOrderSummaries([FromQuery] OrderSummaryRequest request)
        {
            return _orderOrchestrator.GetOrderSummary(request.CustomerId);
        }
    }
}