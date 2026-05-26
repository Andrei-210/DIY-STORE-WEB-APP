using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
        Task<Subcategory?> GetSubcategoryByIdAsync(int id);
        Task<Subcategory?> GetSubcategoryBySlugAsync(string slug);
        Task CreateSubcategoryAsync(Subcategory subcategory);
        Task UpdateSubcategoryAsync(Subcategory subcategory);
        Task DeleteSubcategoryAsync(int id);
    }
}
