using System.Collections.Generic;
using System.Linq;

namespace PetStore.Dto.Models
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal OrderTotal { get; set; }
        // Another way to get OrderTotal could be doing something like this:

        // public decimal OrderTotal => Items?.Sum(x => x.Price * x.Quantity) ?? 0;

        // However with this approach, calculation code will need to be executed on every POST/GET request 
        // which could potencially lead to extra unnecessary load on the server.

    }
}
