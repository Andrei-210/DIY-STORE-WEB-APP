using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class ProductShopAvailabilityRepository : IProductShopAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductShopAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductShopAvailability>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductShopAvailabilities
                .Where(a => a.ProductId == productId)
                .ToListAsync();
        }

        public async Task SetAvailabilitiesAsync(int productId, List<(int shopId, bool inStock)> availabilities)
        {
            var existing = await _context.ProductShopAvailabilities
                .Where(a => a.ProductId == productId)
                .ToListAsync();
            _context.ProductShopAvailabilities.RemoveRange(existing);

            foreach (var (shopId, inStock) in availabilities)
            {
                _context.ProductShopAvailabilities.Add(new ProductShopAvailability
                {
                    ProductId = productId,
                    ShopId = shopId,
                    InStock = inStock
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
