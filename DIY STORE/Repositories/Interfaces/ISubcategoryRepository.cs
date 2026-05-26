using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface ISubcategoryRepository
    {
        Task<IEnumerable<Subcategory>> GetAllAsync();
        Task<Subcategory?> GetByIdAsync(int id);
        Task<Subcategory?> GetBySlugAsync(string slug);
        Task AddAsync(Subcategory subcategory);
        Task UpdateAsync(Subcategory subcategory);
        Task DeleteAsync(int id);
    }
}
