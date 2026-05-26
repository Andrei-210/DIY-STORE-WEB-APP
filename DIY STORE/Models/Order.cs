using System.ComponentModel.DataAnnotations;
using DIY_STORE.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Delivery address is required.")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 300 characters.")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[\d\s\+\-\(\)]{7,20}$", ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
