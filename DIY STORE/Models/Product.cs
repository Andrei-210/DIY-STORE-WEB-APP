using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99 RON.")]
        public decimal Price { get; set; }

        [Range(0, 99999.99, ErrorMessage = "Old price must be between 0 and 99999.99 RON.")]
        public decimal? OldPrice { get; set; }

        public bool IsOnSale { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, 100000, ErrorMessage = "Stock must be between 0 and 100000.")]
        public int Stock { get; set; }

        [StringLength(100, ErrorMessage = "Manufacturer name cannot exceed 100 characters.")]
        public string ManufacturerName { get; set; } = string.Empty;

        public string ManufacturerLogo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a subcategory.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid subcategory.")]
        public int SubcategoryId { get; set; }

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
