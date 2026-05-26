using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IProductShopAvailabilityRepository
    {
        Task<IEnumerable<ProductShopAvailability>> GetByProductIdAsync(int productId);
        Task SetAvailabilitiesAsync(int productId, List<(int shopId, bool inStock)> availabilities);
    }
}
