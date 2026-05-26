using DIY_STORE.Models;
using DIY_STORE.Repositories;

namespace DIY_STORE.Services
{
    public interface ISupportService
    {
        Task<IEnumerable<ContactMessage>> GetAllMessagesAsync();
        Task<ContactMessage?> GetMessageAsync(int id);
        Task SaveMessageAsync(ContactMessage message);
        Task MarkAsReadAsync(int id);
        Task DeleteMessageAsync(int id);
    }
}
