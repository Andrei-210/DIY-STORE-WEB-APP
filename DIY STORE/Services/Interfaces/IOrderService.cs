using DIY_STORE.Models;
using DIY_STORE.Repositories;
using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderAsync(int id);
        Task<Order> PlaceOrderAsync(string userId, CheckoutViewModel checkout, CartViewModel cart);
    }
}
