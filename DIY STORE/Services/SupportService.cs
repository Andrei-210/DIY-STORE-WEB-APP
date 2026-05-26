using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public class SupportService : ISupportService
    {
        private readonly IContactMessageRepository _repo;
        public SupportService(IContactMessageRepository repo) => _repo = repo;

        public Task<IEnumerable<ContactMessage>> GetAllMessagesAsync() => _repo.GetAllAsync();
        public Task<ContactMessage?> GetMessageAsync(int id) => _repo.GetByIdAsync(id);
        public Task SaveMessageAsync(ContactMessage message) => _repo.AddAsync(message);
        public Task MarkAsReadAsync(int id) => _repo.MarkAsReadAsync(id);
        public Task DeleteMessageAsync(int id) => _repo.DeleteAsync(id);
    }
}
