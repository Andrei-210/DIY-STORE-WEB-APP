using DIY_STORE.Data;

namespace DIY_STORE.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;

        // FK spre AspNetUsers
        public string UserId { get; set; } = string.Empty;

        // Navigatie
        public ApplicationUser User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
