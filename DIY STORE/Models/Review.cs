using DIY_STORE.Data;

namespace DIY_STORE.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsVerifiedPurchase { get; set; }

        // FK
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Navigatie
        public Product Product { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
