using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
        Task<Subcategory?> GetSubcategoryByIdAsync(int id);
        Task CreateSubcategoryAsync(Subcategory subcategory);
        Task UpdateSubcategoryAsync(Subcategory subcategory);
        Task DeleteSubcategoryAsync(int id);
    }

    public class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _repo;
        public SubcategoryService(ISubcategoryRepository repo) => _repo = repo;

        public Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync() => _repo.GetAllAsync();
        public Task<Subcategory?> GetSubcategoryByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task CreateSubcategoryAsync(Subcategory subcategory) => _repo.AddAsync(subcategory);
        public Task UpdateSubcategoryAsync(Subcategory subcategory) => _repo.UpdateAsync(subcategory);
        public Task DeleteSubcategoryAsync(int id) => _repo.DeleteAsync(id);
    }
}
