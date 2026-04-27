namespace DIY_STORE.Models
{
    public class CategoryData
    {
        public string Title { get; set; } = string.Empty;
        public List<SubcategoryItem> Subcategories { get; set; } = [];
    }

    public class SubcategoryItem
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public SubcategoryItem(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
}
