using System.Threading.Tasks;

namespace PetStore.Api.Services
{
    public interface IInventoryClient
    {
        Task<T> GetInventory<T>(string url);
    }
}
