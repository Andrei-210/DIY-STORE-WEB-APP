namespace DIY_STORE.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        // FK
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigatie
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
