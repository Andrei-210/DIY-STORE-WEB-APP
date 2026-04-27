using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategorySlugAsync(string slug);
        Task<IEnumerable<Product>> GetBySubcategoryAsync(string categorySlug, string subcategoryName);
        Task<IEnumerable<Product>> GetOnSaleAsync();
        Task<IEnumerable<Product>> SearchAsync(string query);
        Task<IEnumerable<Product>> GetFilteredAsync(string? categorySlug, string? subcategoryName, decimal? minPrice, decimal? maxPrice, int page, int perPage);
        Task<int> CountFilteredAsync(string? categorySlug, string? subcategoryName, decimal? minPrice, decimal? maxPrice);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetSimilarAsync(int productId, int subcategoryId, int count = 4);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
