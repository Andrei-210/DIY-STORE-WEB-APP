using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
