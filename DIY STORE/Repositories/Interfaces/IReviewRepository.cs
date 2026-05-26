using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<Review?> GetByIdAsync(int id);
        Task AddAsync(Review review);
        Task DeleteAsync(int id);
    }
}
