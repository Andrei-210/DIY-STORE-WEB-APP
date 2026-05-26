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
}
