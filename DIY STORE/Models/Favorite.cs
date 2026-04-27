using DIY_STORE.Data;

namespace DIY_STORE.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // FK
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }

        // Navigatie
        public ApplicationUser User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
