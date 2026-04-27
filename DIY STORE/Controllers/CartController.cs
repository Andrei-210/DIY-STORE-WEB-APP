using DIY_STORE.Services;
using DIY_STORE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DIY_STORE.Data;

namespace DIY_STORE.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService, IProductService productService,
            IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var product = await _productService.GetProductDetailAsync(productId);
            if (product == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.Headers.Accept.ToString().Contains("application/json"))
                    return Json(new { success = false, message = "Product not found." });
                return NotFound();
            }

            var imageUrl = product.Images.FirstOrDefault()?.ImagePath ?? "images/placeholder.webp";
            _cartService.AddToCart(HttpContext.Session, product.Id, product.Name, product.Price, imageUrl, quantity);

            var cart = _cartService.GetCart(HttpContext.Session);
            int cartCount = cart.Items.Sum(i => i.Quantity);

            if (Request.Headers.Accept.ToString().Contains("application/json") ||
                Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = $"\"{product.Name}\" adăugat în coș!", count = cartCount });
            }

            TempData["CartMessage"] = $"\"{product.Name}\" a fost adăugat în coș!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Count()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            return Json(new { count = cart.Items.Sum(i => i.Quantity) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(HttpContext.Session, productId);
            TempData["CartMessage"] = "Produs eliminat din coș.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(HttpContext.Session, productId, quantity);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Items.Any())
                return RedirectToAction("Index");
            var vm = new CheckoutViewModel { Cart = cart };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (!cart.Items.Any())
                return RedirectToAction("Index");

            model.Cart = cart;
            if (!ModelState.IsValid)
                return View("Checkout", model);

            var userId = _userManager.GetUserId(User)!;
            var order = await _orderService.PlaceOrderAsync(userId, model, cart);
            _cartService.ClearCart(HttpContext.Session);

            // Redirect to confirmation page with order details
            TempData["OrderId"] = order.Id;
            TempData["FullName"] = model.FullName;
            TempData["PaymentMethod"] = model.PaymentMethod;
            TempData["Total"] = cart.Total.ToString("0.00");

            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            ViewBag.OrderId = TempData["OrderId"];
            ViewBag.FullName = TempData["FullName"];
            ViewBag.PaymentMethod = TempData["PaymentMethod"];
            ViewBag.Total = TempData["Total"];
            return View();
        }
    }
}
