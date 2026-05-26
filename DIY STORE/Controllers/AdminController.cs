using DIY_STORE.Models;
using DIY_STORE.Repositories;
using DIY_STORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IProductShopAvailabilityRepository _shopAvailRepo;
        private readonly IWebHostEnvironment _env;

        public AdminController(
            IProductService productService,
            ICategoryService categoryService,
            ISupportService supportService,
            ISubcategoryService subcategoryService,
            IShopService shopService,
            IProductShopAvailabilityRepository shopAvailRepo,
            IWebHostEnvironment env)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supportService = supportService;
            _subcategoryService = subcategoryService;
            _shopService = shopService;
            _shopAvailRepo = shopAvailRepo;
            _env = env;
        }

        private async Task<string?> SaveUploadedFileAsync(IFormFile? file, string subfolder)
        {
            if (file == null || file.Length == 0) return null;
            var dir = Path.Combine(_env.WebRootPath, "images", subfolder);
            Directory.CreateDirectory(dir);
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(dir, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"{subfolder}/{fileName}";
        }

        public IActionResult Index() => View();

        // ── PRODUCTS ──────────────────────────────────────────────────────────
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
        public async Task<IActionResult> CreateProduct(Product product,
            List<IFormFile>? ImageFiles,
            IFormFile? ManufacturerLogoFile,
            List<int>? ShopIds, List<bool>? ShopInStocks)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Shops = await _shopService.GetAllShopsAsync();
                return View(product);
            }

            if (ManufacturerLogoFile != null && ManufacturerLogoFile.Length > 0)
            {
                var logoPath = await SaveUploadedFileAsync(ManufacturerLogoFile, "manufacturers");
                if (logoPath != null) product.ManufacturerLogo = logoPath;
            }

            if (ImageFiles != null)
            {
                bool firstImg = true;
                foreach (var file in ImageFiles.Where(f => f != null && f.Length > 0))
                {
                    var path = await SaveUploadedFileAsync(file, "products");
                    if (path != null)
                    {
                        product.Images.Add(new ProductImage { ImagePath = path, IsMain = firstImg });
                        firstImg = false;
                    }
                }
            }

            await _productService.CreateProductAsync(product);

            if (ShopIds != null && ShopIds.Any())
            {
                var availabilities = ShopIds.Select((shopId, i) =>
                    (shopId, ShopInStocks != null && i < ShopInStocks.Count && ShopInStocks[i])
                ).ToList();
                await _shopAvailRepo.SetAvailabilitiesAsync(product.Id, availabilities);
            }

            TempData["Success"] = $"✅ Product \"{product.Name}\" created!";
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> EditProduct(string slug)
        {
            var product = await _productService.GetProductBySlugAsync(slug);
            if (product == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Shops = await _shopService.GetAllShopsAsync();
            ViewBag.ExistingAvailabilities = await _shopAvailRepo.GetByProductIdAsync(product.Id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(string slug, Product product,
            List<IFormFile>? ImageFiles,
            IFormFile? ManufacturerLogoFile,
            List<int>? ShopIds, List<bool>? ShopInStocks)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Shops = await _shopService.GetAllShopsAsync();
                ViewBag.ExistingAvailabilities = await _shopAvailRepo.GetByProductIdAsync(product.Id);
                return View(product);
            }

            if (ManufacturerLogoFile != null && ManufacturerLogoFile.Length > 0)
            {
                var logoPath = await SaveUploadedFileAsync(ManufacturerLogoFile, "manufacturers");
                if (logoPath != null) product.ManufacturerLogo = logoPath;
            }

            if (ImageFiles != null && ImageFiles.Any(f => f != null && f.Length > 0))
            {
                product.Images.Clear();
                bool firstImg = true;
                foreach (var file in ImageFiles.Where(f => f != null && f.Length > 0))
                {
                    var path = await SaveUploadedFileAsync(file, "products");
                    if (path != null)
                    {
                        product.Images.Add(new ProductImage { ImagePath = path, ProductId = product.Id, IsMain = firstImg });
                        firstImg = false;
                    }
                }
            }

            await _productService.UpdateProductAsync(product);

            var availabilities = (ShopIds ?? new List<int>())
                .Select((shopId, i) =>
                    (shopId, ShopInStocks != null && i < ShopInStocks.Count && ShopInStocks[i]))
                .ToList();
            await _shopAvailRepo.SetAvailabilitiesAsync(product.Id, availabilities);

            TempData["Success"] = $"✅ Product \"{product.Name}\" updated!";
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteProduct(string slug)
        {
            var product = await _productService.GetProductBySlugAsync(slug);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(string slug)
        {
            var product = await _productService.GetProductBySlugAsync(slug);
            if (product != null)
                await _productService.DeleteProductAsync(product.Id);
            TempData["Success"] = "Product deleted!";
            return RedirectToAction(nameof(Products));
        }

        // ── CATEGORIES ────────────────────────────────────────────────────────
        public async Task<IActionResult> Categories()
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            return View(cats);
        }

        public IActionResult CreateCategory() => View(new Category());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(category);
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveUploadedFileAsync(ImageFile, "categories");
                if (path != null) category.Image = path;
            }
            await _categoryService.CreateCategoryAsync(category);
            TempData["Success"] = $"Category \"{category.Name}\" created!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditCategory(string slug)
        {
            var cat = await _categoryService.GetCategoryBySlugAsync(slug);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(string slug, Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(category);
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveUploadedFileAsync(ImageFile, "categories");
                if (path != null) category.Image = path;
            }
            await _categoryService.UpdateCategoryAsync(category);
            TempData["Success"] = $"Category \"{category.Name}\" updated!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteCategory(string slug)
        {
            var cat = await _categoryService.GetCategoryBySlugAsync(slug);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(string slug)
        {
            var cat = await _categoryService.GetCategoryBySlugAsync(slug);
            if (cat != null) await _categoryService.DeleteCategoryAsync(cat.Id);
            TempData["Success"] = "Category deleted!";
            return RedirectToAction(nameof(Categories));
        }

        // ── SUBCATEGORIES ─────────────────────────────────────────────────────
        public async Task<IActionResult> CreateSubcategory()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubcategory(Subcategory subcategory, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(subcategory);
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveUploadedFileAsync(ImageFile, "subcategories");
                if (path != null) subcategory.Image = path;
            }
            await _subcategoryService.CreateSubcategoryAsync(subcategory);
            TempData["Success"] = $"Subcategory \"{subcategory.Name}\" created!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditSubcategory(string slug)
        {
            var sub = await _subcategoryService.GetSubcategoryBySlugAsync(slug);
            if (sub == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(sub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubcategory(string slug, Subcategory subcategory, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(subcategory);
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveUploadedFileAsync(ImageFile, "subcategories");
                if (path != null) subcategory.Image = path;
            }
            await _subcategoryService.UpdateSubcategoryAsync(subcategory);
            TempData["Success"] = $"Subcategory \"{subcategory.Name}\" updated!";
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteSubcategory(string slug)
        {
            var sub = await _subcategoryService.GetSubcategoryBySlugAsync(slug);
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

        // ── MESSAGES ──────────────────────────────────────────────────────────
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
