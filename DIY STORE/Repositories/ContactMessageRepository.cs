using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public ContactMessageRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
            => await _context.ContactMessages.OrderByDescending(m => m.Date).ToListAsync();

        public async Task<ContactMessage?> GetByIdAsync(int id)
            => await _context.ContactMessages.FindAsync(id);

        public async Task AddAsync(ContactMessage message)
        {
            await _context.ContactMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null) { msg.IsRead = true; await _context.SaveChangesAsync(); }
        }

        public async Task DeleteAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null) { _context.ContactMessages.Remove(msg); await _context.SaveChangesAsync(); }
        }
    }
}
