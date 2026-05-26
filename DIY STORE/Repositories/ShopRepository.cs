using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;
        public ShopRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Shop>> GetAllAsync()
            => await _context.Shops.ToListAsync();

        public async Task<Shop?> GetByIdAsync(int id)
            => await _context.Shops.FindAsync(id);

        public async Task<Shop?> GetBySlugAsync(string slug)
            => await _context.Shops.FirstOrDefaultAsync(s => s.Slug == slug);

        public async Task AddAsync(Shop shop)
        {
            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shop shop)
        {
            _context.Shops.Update(shop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null) { _context.Shops.Remove(shop); await _context.SaveChangesAsync(); }
        }
    }
}
