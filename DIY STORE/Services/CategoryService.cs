using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public Task<IEnumerable<Category>> GetAllCategoriesAsync() => _repo.GetAllAsync();
        public Task<Category?> GetCategoryBySlugAsync(string slug) => _repo.GetBySlugAsync(slug);
        public Task<Category?> GetCategoryByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task CreateCategoryAsync(Category category) => _repo.AddAsync(category);
        public Task UpdateCategoryAsync(Category category) => _repo.UpdateAsync(category);
        public Task DeleteCategoryAsync(int id) => _repo.DeleteAsync(id);
    }
}
