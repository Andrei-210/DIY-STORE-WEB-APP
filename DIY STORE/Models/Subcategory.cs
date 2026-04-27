using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        // FK
        public int CategoryId { get; set; }

        // Navigatie - excluse din validare
        [ValidateNever]
        public Category Category { get; set; } = null!;
        [ValidateNever]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}