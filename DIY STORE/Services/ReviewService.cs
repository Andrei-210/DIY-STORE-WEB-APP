using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;
        public ReviewService(IReviewRepository repo) => _repo = repo;

        public Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        public Task AddReviewAsync(Review review) => _repo.AddAsync(review);
        public Task DeleteReviewAsync(int id) => _repo.DeleteAsync(id);
    }
}
