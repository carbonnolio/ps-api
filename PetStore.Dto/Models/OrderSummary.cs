using System.Collections.Generic;

namespace PetStore.Dto.Models
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal OrderTotal { get; set; }
        // Mention about other ways to get order total...
    }
}
