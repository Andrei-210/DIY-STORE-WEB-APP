using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);
        Task<Favorite?> GetAsync(string userId, int productId);
        Task AddAsync(Favorite favorite);
        Task DeleteAsync(string userId, int productId);
    }
}
