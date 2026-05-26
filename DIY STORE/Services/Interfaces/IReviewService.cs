using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task AddReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
    }
}
