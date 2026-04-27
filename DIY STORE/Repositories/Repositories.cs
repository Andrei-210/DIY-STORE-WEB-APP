using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    // ─── ShopRepository ────────────────────────────────────────────────────────
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;
        public ShopRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Shop>> GetAllAsync()
            => await _context.Shops.ToListAsync();

        public async Task<Shop?> GetByIdAsync(int id)
            => await _context.Shops.FindAsync(id);

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

    // ─── CategoryRepository ────────────────────────────────────────────────────
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.Include(c => c.Subcategories).ToListAsync();

        public async Task<Category?> GetBySlugAsync(string slug)
            => await _context.Categories.Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.Slug == slug);

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat != null) { _context.Categories.Remove(cat); await _context.SaveChangesAsync(); }
        }
    }

    // ─── OrderRepository ───────────────────────────────────────────────────────
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
            => await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

        public async Task<Order?> GetByIdAsync(int id)
            => await _context.Orders.FindAsync(id);

        public async Task<Order?> GetByIdWithItemsAsync(int id)
            => await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }

    // ─── ContactMessageRepository ──────────────────────────────────────────────
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public ContactMessageRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
            => await _context.ContactMessages.OrderByDescending(m => m.Date).ToListAsync();

        public async Task<ContactMessage?> GetByIdAsync(int id)
            => await _context.ContactMessages.FindAsync(id);

        public async Task AddAsync(ContactMessage message)
        {
            await _context.ContactMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null) { msg.IsRead = true; await _context.SaveChangesAsync(); }
        }

        public async Task DeleteAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null) { _context.ContactMessages.Remove(msg); await _context.SaveChangesAsync(); }
        }
    }

    // ─── ReviewRepository ──────────────────────────────────────────────────────
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
            => await _context.Reviews.Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

        public async Task<Review?> GetByIdAsync(int id)
            => await _context.Reviews.FindAsync(id);

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null) { _context.Reviews.Remove(review); await _context.SaveChangesAsync(); }
        }
    }

    // ─── FavoriteRepository ────────────────────────────────────────────────────
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

    // ─── SubcategoryRepository ─────────────────────────────────────────────────
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public SubcategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
            => await _context.Subcategories.Include(s => s.Category).ToListAsync();

        public async Task<Subcategory?> GetByIdAsync(int id)
            => await _context.Subcategories.Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(Subcategory subcategory)
        {
            await _context.Subcategories.AddAsync(subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subcategory subcategory)
        {
            _context.Subcategories.Update(subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sub = await _context.Subcategories.FindAsync(id);
            if (sub != null) { _context.Subcategories.Remove(sub); await _context.SaveChangesAsync(); }
        }
    }
}
