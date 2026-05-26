using DIY_STORE.Models;
using DIY_STORE.Repositories;
using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
    public interface IProductService
    {
        Task<ProductListViewModel> GetProductsAsync(string? categorySlug, string? subcategoryName,
            decimal? minPrice, decimal? maxPrice, int page, int perPage);
        Task<ProductListViewModel> SearchProductsAsync(string query);
        Task<ProductListViewModel> GetOnSaleProductsAsync();
        Task<Product?> GetProductDetailAsync(int id);
    Task<Product?> GetProductBySlugAsync(string slug);
        Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId, int subcategoryId);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
