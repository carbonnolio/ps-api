using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public async Task<int> SubmitOrder([FromBody] OrderRequest request)
        {
            return 1;
        }

        [HttpGet("Summary")]
        public List<OrderSummary> GetOrderSummaries([FromQuery] OrderSummaryRequest request)
        {
            return null;
        }
    }
}