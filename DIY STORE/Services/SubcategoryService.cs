using DIY_STORE.Models;
using DIY_STORE.Repositories;
using System.Text.RegularExpressions;

namespace DIY_STORE.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _repo;
        public SubcategoryService(ISubcategoryRepository repo) => _repo = repo;

        private static string GenerateSlug(string name)
        {
            var slug = name.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            return slug.Trim('-');
        }

        public Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync() => _repo.GetAllAsync();
        public Task<Subcategory?> GetSubcategoryByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Subcategory?> GetSubcategoryBySlugAsync(string slug) => _repo.GetBySlugAsync(slug);

        public async Task CreateSubcategoryAsync(Subcategory subcategory)
        {
            if (string.IsNullOrWhiteSpace(subcategory.Slug))
                subcategory.Slug = GenerateSlug(subcategory.Name);
            await _repo.AddAsync(subcategory);
        }

        public async Task UpdateSubcategoryAsync(Subcategory subcategory)
        {
            if (string.IsNullOrWhiteSpace(subcategory.Slug))
                subcategory.Slug = GenerateSlug(subcategory.Name);
            await _repo.UpdateAsync(subcategory);
        }

        public Task DeleteSubcategoryAsync(int id) => _repo.DeleteAsync(id);
    }
}
