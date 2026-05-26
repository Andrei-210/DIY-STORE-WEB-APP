using DIY_STORE.Models;
using DIY_STORE.Repositories;
using DIY_STORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DIY_STORE.Data;

namespace DIY_STORE.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;
        private readonly IReviewService _reviewService;
        private readonly ICategoryService _categoryService;
        private readonly IShopService _shopService;
        private readonly IProductShopAvailabilityRepository _shopAvailRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(IProductService productService,
            IFavoriteService favoriteService,
            IReviewService reviewService,
            ICategoryService categoryService,
            IShopService shopService,
            IProductShopAvailabilityRepository shopAvailRepo,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _favoriteService = favoriteService;
            _reviewService = reviewService;
            _categoryService = categoryService;
            _shopService = shopService;
            _shopAvailRepo = shopAvailRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? cat, string? sub, int perPage = 12, int page = 1,
                                               decimal minPrice = 0, decimal maxPrice = 0)
        {
            var model = await _productService.GetProductsAsync(
                string.IsNullOrEmpty(cat) ? null : cat,
                string.IsNullOrEmpty(sub) ? null : sub,
                minPrice > 0 ? minPrice : null,
                maxPrice > 0 ? maxPrice : null,
                page, perPage);
            await SetFavoriteIds();
            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var product = await _productService.GetProductBySlugAsync(slug);
            if (product == null) return NotFound();

            ViewBag.SimilarProducts = await _productService.GetSimilarProductsAsync(product.Id, product.SubcategoryId);

            // Shop availability
            var allShops = (await _shopService.GetAllShopsAsync()).ToList();
            var availabilities = await _shopAvailRepo.GetByProductIdAsync(product.Id);
            var availDict = availabilities.ToDictionary(a => a.ShopId, a => a.InStock);

            ViewBag.AllShops = allShops;
            ViewBag.AvailDict = availDict;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User)!;
                ViewBag.IsFavorite = await _favoriteService.IsFavoriteAsync(userId, product.Id);
            }
            else
            {
                ViewBag.IsFavorite = false;
            }
            return View(product);
        }

        public async Task<IActionResult> Search(string q = "")
        {
            var model = await _productService.SearchProductsAsync(q);
            await SetFavoriteIds();
            return View("Index", model);
        }

        public async Task<IActionResult> Promotions(string? filterCat = null)
        {
            var model = await _productService.GetOnSaleProductsAsync();
            ViewBag.IsPromotions = true;
            ViewBag.AllCategories = (await _categoryService.GetAllCategoriesAsync()).ToList();
            await SetFavoriteIds();
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var userId = _userManager.GetUserId(User)!;
            await _favoriteService.ToggleFavoriteAsync(userId, productId);
            bool isFav = await _favoriteService.IsFavoriteAsync(userId, productId);

            if (Request.Headers.Accept.ToString().Contains("application/json") ||
                Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, isFavorite = isFav });
            }
            // redirect back to product details by slug
            var product = await _productService.GetProductDetailAsync(productId);
            return RedirectToAction(nameof(Details), new { slug = product?.Slug ?? productId.ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                TempData["ReviewError"] = "Please fill in all required review fields correctly.";
                var p = await _productService.GetProductDetailAsync(review.ProductId);
                return RedirectToAction(nameof(Details), new { slug = p?.Slug ?? review.ProductId.ToString() });
            }
            review.UserId = _userManager.GetUserId(User)!;
            review.Date = DateTime.Now;
            await _reviewService.AddReviewAsync(review);
            TempData["ReviewSuccess"] = "✅ Review submitted successfully!";
            var product = await _productService.GetProductDetailAsync(review.ProductId);
            return RedirectToAction(nameof(Details), new { slug = product?.Slug ?? review.ProductId.ToString() });
        }

        private async Task SetFavoriteIds()
        {
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
        }
    }
}
