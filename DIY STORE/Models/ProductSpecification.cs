namespace DIY_STORE.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public string SpecKey { get; set; } = string.Empty;
        public string SpecValue { get; set; } = string.Empty;

        // FK
        public int ProductId { get; set; }

        // Navigatie
        public Product Product { get; set; } = null!;
    }
}
