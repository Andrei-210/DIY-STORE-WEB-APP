using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
    public interface ICartService
    {
        CartViewModel GetCart(ISession session);
        void AddToCart(ISession session, int productId, string name, decimal price, string imageUrl, int quantity = 1);
        void AddToCart(ISession session, int productId, string productSlug, string name, decimal price, string imageUrl, int quantity = 1);
        void AddToCart(ISession session, int productId, string productSlug, string name, decimal price, string imageUrl, int quantity, int stock);
        void RemoveFromCart(ISession session, int productId);
        void UpdateQuantity(ISession session, int productId, int quantity);
        void ClearCart(ISession session);
    }
}