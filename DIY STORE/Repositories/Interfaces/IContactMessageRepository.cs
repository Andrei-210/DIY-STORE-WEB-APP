using DIY_STORE.Models;

namespace DIY_STORE.Repositories
{
    public interface IContactMessageRepository
    {
        Task<IEnumerable<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task AddAsync(ContactMessage message);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
