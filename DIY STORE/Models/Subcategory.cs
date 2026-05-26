using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subcategory name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a parent category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; } = null!;
        [ValidateNever]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
