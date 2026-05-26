using DIY_STORE.Models;
using DIY_STORE.Repositories;
using DIY_STORE.ViewModels;

namespace DIY_STORE.Services
{
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
            // ── Validate and deduct stock ─────────────────────────────────────
            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product #{item.ProductId} no longer exists.");

                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for \"{product.Name}\". " +
                        $"Available: {product.Stock}, requested: {item.Quantity}.");

                // Use UpdateStockAsync — only touches the Stock column, images are preserved
                int newStock = product.Stock - item.Quantity;
                await _productRepo.UpdateStockAsync(product.Id, newStock);
            }

            // ── Create order ──────────────────────────────────────────────────
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
