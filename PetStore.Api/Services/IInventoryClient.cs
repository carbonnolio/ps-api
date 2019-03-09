using System;
using System.Threading.Tasks;

namespace PetStore.Api.Services
{
    public interface IInventoryClient
    {
        Task<T> GetInventory<T>(string route, Action<int> errorHandler = null);
    }
}
