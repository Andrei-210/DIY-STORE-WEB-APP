using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class ProductShopAvailability
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public bool InStock { get; set; } = true;

        [ValidateNever]
        public Product Product { get; set; } = null!;
        [ValidateNever]
        public Shop Shop { get; set; } = null!;
    }
}
