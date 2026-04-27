using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface IShopService
    {
        Task<IEnumerable<Shop>> GetAllShopsAsync();
        Task<Shop?> GetShopAsync(int id);
        Task CreateShopAsync(Shop shop);
        Task UpdateShopAsync(Shop shop);
        Task DeleteShopAsync(int id);
    }

    public class ShopService : IShopService
    {
        private readonly IShopRepository _repo;
        public ShopService(IShopRepository repo) => _repo = repo;

        public Task<IEnumerable<Shop>> GetAllShopsAsync() => _repo.GetAllAsync();
        public Task<Shop?> GetShopAsync(int id) => _repo.GetByIdAsync(id);
        public Task CreateShopAsync(Shop shop) => _repo.AddAsync(shop);
        public Task UpdateShopAsync(Shop shop) => _repo.UpdateAsync(shop);
        public Task DeleteShopAsync(int id) => _repo.DeleteAsync(id);
    }
}
