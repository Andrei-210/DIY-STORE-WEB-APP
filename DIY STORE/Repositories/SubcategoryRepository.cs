using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public SubcategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
            => await _context.Subcategories.Include(s => s.Category).ToListAsync();

        public async Task<Subcategory?> GetByIdAsync(int id)
            => await _context.Subcategories.Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Subcategory?> GetBySlugAsync(string slug)
            => await _context.Subcategories.Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Slug == slug);

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
