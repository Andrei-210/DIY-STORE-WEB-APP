using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
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
}
