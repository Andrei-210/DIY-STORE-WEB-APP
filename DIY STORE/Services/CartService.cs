using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
    public interface ICartService
    {
        CartViewModel GetCart(ISession session);
        void AddToCart(ISession session, int productId, string name, decimal price, string imageUrl, int quantity = 1);
        void RemoveFromCart(ISession session, int productId);
        void UpdateQuantity(ISession session, int productId, int quantity);
        void ClearCart(ISession session);
    }

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
        {
            var cart = GetCart(session);
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = quantity
                });
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
