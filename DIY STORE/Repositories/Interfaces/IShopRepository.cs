using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop?> GetByIdAsync(int id);
        Task<Shop?> GetBySlugAsync(string slug);
        Task AddAsync(Shop shop);
        Task UpdateAsync(Shop shop);
        Task DeleteAsync(int id);
    }
}
