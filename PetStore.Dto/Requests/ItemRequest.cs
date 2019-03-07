using System.ComponentModel.DataAnnotations;

namespace PetStore.Dto.Requests
{
    public class ItemRequest
    {
        [Required]
        [MinLength(1)]
        public string ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
