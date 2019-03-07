using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Dto.Requests
{
    public class OrderRequest
    {
        [Required]
        [MinLength(1)]
        public string CustomerId { get; set; }
        [Required]
        [MinLength(1)]
        public List<ItemRequest> Items { get; set; }
    }
}
