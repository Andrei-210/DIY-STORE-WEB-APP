using System.ComponentModel.DataAnnotations;

namespace DIY_STORE.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Slug must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Slug can only contain lowercase letters, numbers and hyphens.")]
        public string Slug { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
