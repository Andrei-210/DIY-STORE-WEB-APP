using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategorySlugAsync(string slug)
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Where(p => p.Subcategory.Category.Slug == slug)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetBySubcategoryAsync(string categorySlug, string subcategoryName)
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Where(p => p.Subcategory.Category.Slug == categorySlug
                         && p.Subcategory.Name == subcategoryName)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetOnSaleAsync()
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Where(p => p.IsOnSale)
                .ToListAsync();

        public async Task<IEnumerable<Product>> SearchAsync(string query)
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetFilteredAsync(string? categorySlug, string? subcategoryName,
            decimal? minPrice, decimal? maxPrice, int page, int perPage)
        {
            var query = BuildFilterQuery(categorySlug, subcategoryName, minPrice, maxPrice);
            return await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();
        }

        public async Task<int> CountFilteredAsync(string? categorySlug, string? subcategoryName,
            decimal? minPrice, decimal? maxPrice)
        {
            var query = BuildFilterQuery(categorySlug, subcategoryName, minPrice, maxPrice);
            return await query.CountAsync();
        }

        private IQueryable<Product> BuildFilterQuery(string? categorySlug, string? subcategoryName,
            decimal? minPrice, decimal? maxPrice)
        {
            var q = _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categorySlug))
                q = q.Where(p => p.Subcategory.Category.Slug == categorySlug);

            if (!string.IsNullOrEmpty(subcategoryName))
                q = q.Where(p => p.Subcategory.Name == subcategoryName);

            if (minPrice.HasValue)
                q = q.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue && maxPrice.Value > 0)
                q = q.Where(p => p.Price <= maxPrice.Value);

            return q;
        }

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product?> GetBySlugAsync(string slug)
            => await _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Reviews).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Slug == slug);

        public async Task<IEnumerable<Product>> GetSimilarAsync(int productId, int subcategoryId, int count = 4)
            => await _context.Products
                .Include(p => p.Images)
                .Where(p => p.SubcategoryId == subcategoryId && p.Id != productId)
                .Take(count)
                .ToListAsync();

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            // Sterge imaginile vechi si le inlocuieste cu cele noi
            var existingImages = await _context.ProductImages
                .Where(pi => pi.ProductId == product.Id)
                .ToListAsync();
            _context.ProductImages.RemoveRange(existingImages);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates ONLY the Stock column — does NOT touch images or other fields.
        /// Use this instead of UpdateAsync when only deducting stock.
        /// </summary>
        public async Task UpdateStockAsync(int productId, int newStock)
        {
            await _context.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, newStock));
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
