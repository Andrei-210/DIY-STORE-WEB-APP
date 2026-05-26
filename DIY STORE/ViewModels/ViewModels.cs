using System.ComponentModel.DataAnnotations;

namespace DIY_STORE.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductSlug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; }   // current stock — stored in session for cart validation
        public decimal Subtotal => Price * Quantity;
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }

    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = "cash";

        public CartViewModel Cart { get; set; } = new();
    }

    public class ProductListViewModel
    {
        public List<DIY_STORE.Models.Product> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public string? CurrentCategory { get; set; }
        public string? CurrentSubcategory { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PerPage);
        public int Start => TotalCount == 0 ? 0 : (Page - 1) * PerPage + 1;
        public int End => Math.Min(Page * PerPage, TotalCount);
    }

    public class ProfileViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? ProfilePicturePath { get; set; }
        public string ActiveSection { get; set; } = "account";
        public List<DIY_STORE.Models.Order> Orders { get; set; } = new();
        public List<DIY_STORE.Models.Favorite> Favorites { get; set; } = new();
    }
}
