using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface IShopService
    {
        Task<IEnumerable<Shop>> GetAllShopsAsync();
        Task<Shop?> GetShopAsync(int id);
        Task<Shop?> GetShopBySlugAsync(string slug);
        Task CreateShopAsync(Shop shop);
        Task UpdateShopAsync(Shop shop);
        Task DeleteShopAsync(int id);
    }
}
