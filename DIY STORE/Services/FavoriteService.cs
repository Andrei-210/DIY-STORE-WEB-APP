using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId);
        Task ToggleFavoriteAsync(string userId, int productId);
        Task<bool> IsFavoriteAsync(string userId, int productId);
    }

    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _repo;
        public FavoriteService(IFavoriteRepository repo) => _repo = repo;

        public Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId)
            => _repo.GetByUserIdAsync(userId);

        public async Task ToggleFavoriteAsync(string userId, int productId)
        {
            var existing = await _repo.GetAsync(userId, productId);
            if (existing != null)
                await _repo.DeleteAsync(userId, productId);
            else
                await _repo.AddAsync(new Favorite { UserId = userId, ProductId = productId });
        }

        public async Task<bool> IsFavoriteAsync(string userId, int productId)
            => await _repo.GetAsync(userId, productId) != null;
    }
}
