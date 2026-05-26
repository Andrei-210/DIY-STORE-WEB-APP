using DIY_STORE.Models;
using DIY_STORE.Repositories;
using System.Text.RegularExpressions;

namespace DIY_STORE.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _repo;
        public ShopService(IShopRepository repo) => _repo = repo;

        private static string GenerateSlug(string city, string address)
        {
            var raw = $"{city}-{address}".ToLowerInvariant();
            raw = Regex.Replace(raw, @"[^a-z0-9\s-]", "");
            raw = Regex.Replace(raw, @"\s+", "-");
            raw = Regex.Replace(raw, @"-+", "-");
            return raw.Trim('-');
        }

        public Task<IEnumerable<Shop>> GetAllShopsAsync() => _repo.GetAllAsync();
        public Task<Shop?> GetShopAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Shop?> GetShopBySlugAsync(string slug) => _repo.GetBySlugAsync(slug);

        public async Task CreateShopAsync(Shop shop)
        {
            if (string.IsNullOrWhiteSpace(shop.Slug))
                shop.Slug = GenerateSlug(shop.City, shop.Address);
            await _repo.AddAsync(shop);
        }

        public async Task UpdateShopAsync(Shop shop)
        {
            if (string.IsNullOrWhiteSpace(shop.Slug))
                shop.Slug = GenerateSlug(shop.City, shop.Address);
            await _repo.UpdateAsync(shop);
        }

        public Task DeleteShopAsync(int id) => _repo.DeleteAsync(id);
    }
}
