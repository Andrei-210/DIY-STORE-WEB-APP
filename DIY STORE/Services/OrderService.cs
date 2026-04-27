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

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
            => _orderRepo.GetByUserIdAsync(userId);

        public Task<Order?> GetOrderAsync(int id)
            => _orderRepo.GetByIdWithItemsAsync(id);

        public async Task<Order> PlaceOrderAsync(string userId, CheckoutViewModel checkout, CartViewModel cart)
        {
            var order = new Order
            {
                UserId = userId,
                Date = DateTime.Now,
                Status = "Pending",
                DeliveryAddress = checkout.Address,
                Phone = checkout.Phone,
                PaymentMethod = checkout.PaymentMethod,
                TotalAmount = cart.Total,
                OrderItems = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.Price
                }).ToList()
            };

            await _orderRepo.AddAsync(order);
            return order;
        }
    }
}
