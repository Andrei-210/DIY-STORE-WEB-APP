namespace DIY_STORE.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        // Navigatie
        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
