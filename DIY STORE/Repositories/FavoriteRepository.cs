using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;
        public FavoriteRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId)
            => await _context.Favorites
                .Include(f => f.Product).ThenInclude(p => p.Images)
                .Where(f => f.UserId == userId)
                .ToListAsync();

        public async Task<Favorite?> GetAsync(string userId, int productId)
            => await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId, int productId)
        {
            var fav = await GetAsync(userId, productId);
            if (fav != null) { _context.Favorites.Remove(fav); await _context.SaveChangesAsync(); }
        }
    }
}
