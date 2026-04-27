namespace DIY_STORE.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public bool IsMain { get; set; }

        // FK
        public int ProductId { get; set; }

        // Navigatie
        public Product Product { get; set; } = null!;
    }
}
