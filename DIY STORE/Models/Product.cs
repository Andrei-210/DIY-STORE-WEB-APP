using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public bool IsOnSale { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public string ManufacturerLogo { get; set; } = string.Empty;

        // FK
        public int SubcategoryId { get; set; }

        // Navigatie - excluse din validare
        [ValidateNever]
        public Subcategory Subcategory { get; set; } = null!;
        [ValidateNever]
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        [ValidateNever]
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        [ValidateNever]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        [ValidateNever]
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}