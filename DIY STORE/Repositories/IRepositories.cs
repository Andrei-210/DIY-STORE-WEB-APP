using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop?> GetByIdAsync(int id);
        Task AddAsync(Shop shop);
        Task UpdateAsync(Shop shop);
        Task DeleteAsync(int id);
    }

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetBySlugAsync(string slug);
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }

    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }

    public interface IContactMessageRepository
    {
        Task<IEnumerable<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task AddAsync(ContactMessage message);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }

    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<Review?> GetByIdAsync(int id);
        Task AddAsync(Review review);
        Task DeleteAsync(int id);
    }

    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);
        Task<Favorite?> GetAsync(string userId, int productId);
        Task AddAsync(Favorite favorite);
        Task DeleteAsync(string userId, int productId);
    }

    public interface ISubcategoryRepository
    {
        Task<IEnumerable<Subcategory>> GetAllAsync();
        Task<Subcategory?> GetByIdAsync(int id);
        Task AddAsync(Subcategory subcategory);
        Task UpdateAsync(Subcategory subcategory);
        Task DeleteAsync(int id);
    }

    public interface IProductShopAvailabilityRepository
    {
        Task<IEnumerable<DIY_STORE.Models.ProductShopAvailability>> GetByProductIdAsync(int productId);
        Task SetAvailabilitiesAsync(int productId, List<(int shopId, bool inStock)> availabilities);
    }
}