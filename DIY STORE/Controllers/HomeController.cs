using Microsoft.AspNetCore.Mvc;
using DIY_STORE.Models;
using DIY_STORE.Services;
using DIY_STORE.Data;
using Microsoft.AspNetCore.Identity;

namespace DIY_STORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ICategoryService categoryService, IProductService productService,
            IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _productService = productService;
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allCategories = (await _categoryService.GetAllCategoriesAsync()).ToList();
            var rng = new Random();

            var popularCategories = allCategories
                .OrderBy(_ => rng.Next())
                .Take(3)
                .ToList();

            var allProducts = await _productService.GetProductsAsync(null, null, null, null, 1, 200);
            var featuredProducts = allProducts.Products
                .OrderBy(_ => rng.Next())
                .Take(4)
                .ToList();

            ViewBag.PopularCategories = popularCategories;
            ViewBag.FeaturedProducts = featuredProducts;

            // Favorite IDs for current user
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User)!;
                var favs = await _favoriteService.GetUserFavoritesAsync(userId);
                ViewBag.FavoriteIds = favs.Select(f => f.ProductId).ToList();
            }
            else
            {
                ViewBag.FavoriteIds = new List<int>();
            }

            return View();
        }

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Contact() => View(new ContactFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            TempData["ContactSuccess"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
