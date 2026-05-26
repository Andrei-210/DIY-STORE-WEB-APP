using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
    public class CartService : ICartService
    {
        private const string CartKey = "Cart";

        public CartViewModel GetCart(ISession session)
        {
            var json = session.GetString(CartKey);
            if (string.IsNullOrEmpty(json))
                return new CartViewModel();
            return System.Text.Json.JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
        }

        public void AddToCart(ISession session, int productId, string name, decimal price, string imageUrl, int quantity = 1)
            => AddToCart(session, productId, string.Empty, name, price, imageUrl, quantity, 0);

        public void AddToCart(ISession session, int productId, string productSlug, string name, decimal price, string imageUrl, int quantity = 1)
            => AddToCart(session, productId, productSlug, name, price, imageUrl, quantity, 0);

        public void AddToCart(ISession session, int productId, string productSlug, string name, decimal price, string imageUrl, int quantity, int stock)
        {
            var cart = GetCart(session);
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                existing.Stock = stock; // refresh stock
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    ProductSlug = productSlug,
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = quantity,
                    Stock = stock
                });
            }
            SaveCart(session, cart);
        }

        public void RemoveFromCart(ISession session, int productId)
        {
            var cart = GetCart(session);
            cart.Items.RemoveAll(i => i.ProductId == productId);
            SaveCart(session, cart);
        }

        public void UpdateQuantity(ISession session, int productId, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Items.Remove(item);
                else
                    item.Quantity = quantity;
            }
            SaveCart(session, cart);
        }

        public void ClearCart(ISession session)
            => session.Remove(CartKey);

        private static void SaveCart(ISession session, CartViewModel cart)
            => session.SetString(CartKey, System.Text.Json.JsonSerializer.Serialize(cart));
    }
}
