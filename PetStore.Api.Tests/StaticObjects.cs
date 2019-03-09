using System.Collections.Generic;
using PetStore.Dto.Models;
using PetStore.Dto.Requests;

namespace PetStore.Api.Tests
{
    public class StaticObjects
    {
        public static OrderRequest OrderRequestItems2 =>
        new OrderRequest
        {
            CustomerId = "aaaaa",
            Items = new List<ItemRequest>
            {
                new ItemRequest
                {
                    ProductId = "8ed0e6f7",
                    Quantity = 3
                },
                new ItemRequest
                {
                    ProductId = "c0258525",
                    Quantity = 2
                }
            }
        };
    }
}
