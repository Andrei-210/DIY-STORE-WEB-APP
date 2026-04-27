using DIY_STORE.Models;
using DIY_STORE.Services;
using DIY_STORE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIY_STORE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupportService _supportService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly IShopService _shopService;
        private readonly ApplicationDbContext _db;

        public AdminController(IProductService productService,
            ICategoryService categoryService,
            ISupportService supportService,
            ISubcategoryService subcategoryService,
            IShopService shopService,
            ApplicationDbContext db)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supportService = supportService;
            _subcategoryService = subcategoryService;
            _shopService = shopService;
            _db = db;
        }

        public IActionResult Index() => View();

        // ── PRODUCTS CRUD ──────────────────────────────────────────────────────
        public async Task<IActionResult> Products()
        {
            var vm = await _productService.GetProductsAsync(null, null, null, null, 1, 100);
            return View(vm.Products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Shops = await _shopService.GetAllShopsAsync();
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, List<string>? ImagePaths,
            List<int>? ShopIds, List<bool>? ShopInStocks)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Shops = await _shopService.GetAllShopsAsync();
                return View(product);
            }

            if (ImagePaths != null)
            {
                bool firstImg = true;
                foreach (var path in ImagePaths.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    product.Images.Add(new ProductImage { ImagePath = path.Trim(), IsMain = firstImg });
                    firstImg = false;
                }
            }

            await _productService.CreateProductAsync(product);

            // Save shop availabilities
            if (ShopIds != null)
            {
                for (int i = 0; i < ShopIds.Count; i++)
                {
                    _db.ProductShopAvailabilities.Add(new ProductShopAvailability
                    {
                        ProductId = product.Id,
                        ShopId = ShopIds[i],
                        InStock = ShopInStocks != null && i < ShopInStocks.Count && ShopInStocks[i]
                    });
                }
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = $"✅ Product \"{product.Name}\" created!";
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Shops = await _shopService.GetAllShopsAsync();
            ViewBag.ExistingAvailabilities = await _db.ProductShopAvailabilities
                .Where(a => a.ProductId == id).ToListAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product product, List<string>? ImagePaths,
            List<int>? ShopIds, List<bool>? ShopInStocks)
        {
            if (id != product.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Shops = await _shopService.GetAllShopsAsync();
                ViewBag.ExistingAvailabilities = await _db.ProductShopAvailabilities
                    .Where(a => a.ProductId == id).ToListAsync();
                return View(product);
            }

            if (ImagePaths != null)
            {
                product.Images.Clear();
                bool firstImg = true;
                foreach (var path in ImagePaths.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    product.Images.Add(new ProductImage { ImagePath = path.Trim(), ProductId = id, IsMain = firstImg });
                    firstImg = false;
                }
            }

            await _productService.UpdateProductAsync(product);

            // Update shop availabilities
            var existing = await _db.ProductShopAvailabilities.Where(a => a.ProductId == id).ToListAsync();
            _db.ProductShopAvailabilities.RemoveRange(existing);
            if (ShopIds != null)
            {
                for (int i = 0; i < ShopIds.Count; i++)
                {
                    _db.ProductShopAvailabilities.Add(new ProductShopAvailability
                    {
                        ProductId = id,
                        ShopId = ShopIds[i],
                        InStock = ShopInStocks != null && i < ShopInStocks.Count && ShopInStocks[i]
                    });
                }
            }
            await _db.SaveChangesAsync();

            TempData["Success"] = $"✅ Product \"{product.Name}\" updated!";
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["Success"] = "Product deleted!";
            return RedirectToAction(nameof(Products));
        }

        // ── CATEGORIES CRUD ────────────────────────────────────────────────────
        public async Task<IActionResult> Categories()
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            return View(cats);
        }

        public IActionResult CreateCategory() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            await _categoryService.CreateCategoryAsync(category);
            TempData["Success"] = $"Category \"{category.Name}\" created!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var cat = await _categoryService.GetCategoryByIdAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (id != category.Id) return NotFound();
            if (!ModelState.IsValid) return View(category);
            await _categoryService.UpdateCategoryAsync(category);
            TempData["Success"] = $"Category \"{category.Name}\" updated!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var cat = await _categoryService.GetCategoryByIdAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted!";
            return RedirectToAction(nameof(Categories));
        }

        // ── SUBCATEGORIES CRUD ─────────────────────────────────────────────────
        public async Task<IActionResult> CreateSubcategory()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubcategory(Subcategory subcategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(subcategory);
            }
            await _subcategoryService.CreateSubcategoryAsync(subcategory);
            TempData["Success"] = $"Subcategory \"{subcategory.Name}\" created!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditSubcategory(int id)
        {
            var sub = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (sub == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(sub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubcategory(int id, Subcategory subcategory)
        {
            if (id != subcategory.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(subcategory);
            }
            await _subcategoryService.UpdateSubcategoryAsync(subcategory);
            TempData["Success"] = $"Subcategory \"{subcategory.Name}\" updated!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteSubcategory(int id)
        {
            var sub = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (sub == null) return NotFound();
            return View(sub);
        }

        [HttpPost, ActionName("DeleteSubcategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubcategoryConfirmed(int id)
        {
            await _subcategoryService.DeleteSubcategoryAsync(id);
            TempData["Success"] = "Subcategory deleted!";
            return RedirectToAction(nameof(Categories));
        }

        // ── MESSAGES ───────────────────────────────────────────────────────────
        public async Task<IActionResult> Messages()
        {
            var messages = await _supportService.GetAllMessagesAsync();
            return View(messages);
        }

        public async Task<IActionResult> MessageDetails(int id)
        {
            var msg = await _supportService.GetMessageAsync(id);
            if (msg == null) return NotFound();
            await _supportService.MarkAsReadAsync(id);
            return View(msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _supportService.DeleteMessageAsync(id);
            TempData["Success"] = "Message deleted!";
            return RedirectToAction(nameof(Messages));
        }
    }
}
