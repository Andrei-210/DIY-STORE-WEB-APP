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
        Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId, int subcategoryId);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryRepository _catRepo;

        public ProductService(IProductRepository repo, ICategoryRepository catRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
        }

        public async Task<ProductListViewModel> GetProductsAsync(string? categorySlug, string? subcategoryName,
            decimal? minPrice, decimal? maxPrice, int page, int perPage)
        {
            var products = await _repo.GetFilteredAsync(categorySlug, subcategoryName, minPrice, maxPrice, page, perPage);
            var total = await _repo.CountFilteredAsync(categorySlug, subcategoryName, minPrice, maxPrice);

            string title = "All Products";
            if (!string.IsNullOrEmpty(categorySlug))
            {
                var cat = await _catRepo.GetBySlugAsync(categorySlug);
                title = cat?.Name ?? categorySlug;
                if (!string.IsNullOrEmpty(subcategoryName))
                    title = $"{title} – {subcategoryName}";
            }

            return new ProductListViewModel
            {
                Products = products.ToList(),
                TotalCount = total,
                Page = page,
                PerPage = perPage,
                CategoryTitle = title,
                CurrentCategory = categorySlug,
                CurrentSubcategory = subcategoryName,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };
        }

        public async Task<ProductListViewModel> SearchProductsAsync(string query)
        {
            var products = await _repo.SearchAsync(query);
            var list = products.ToList();
            return new ProductListViewModel
            {
                Products = list,
                TotalCount = list.Count,
                Page = 1,
                PerPage = list.Count > 0 ? list.Count : 12,
                CategoryTitle = $"Search results for: {query}"
            };
        }

        public async Task<ProductListViewModel> GetOnSaleProductsAsync()
        {
            var products = await _repo.GetOnSaleAsync();
            var list = products.ToList();
            return new ProductListViewModel
            {
                Products = list,
                TotalCount = list.Count,
                Page = 1,
                PerPage = list.Count > 0 ? list.Count : 12,
                CategoryTitle = "Promotions",
                CurrentCategory = "promotions"
            };
        }

        public async Task<Product?> GetProductDetailAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId, int subcategoryId)
            => await _repo.GetSimilarAsync(productId, subcategoryId);

        public async Task CreateProductAsync(Product product)
            => await _repo.AddAsync(product);

        public async Task UpdateProductAsync(Product product)
            => await _repo.UpdateAsync(product);

        public async Task DeleteProductAsync(int id)
            => await _repo.DeleteAsync(id);
    }
}
