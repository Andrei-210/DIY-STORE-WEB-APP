using DIY_STORE.Models;
using DIY_STORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIY_STORE.Controllers
{
    public class ShopsController : Controller
    {
        private readonly IShopService _shopService;
        private readonly IWebHostEnvironment _env;

        public ShopsController(IShopService shopService, IWebHostEnvironment env)
        {
            _shopService = shopService;
            _env = env;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file, string subfolder)
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

        // READ - Lista toate magazinele (public)
        public async Task<IActionResult> Index()
        {
            var shops = await _shopService.GetAllShopsAsync();
            return View(shops);
        }

        // READ - Detalii un magazin (public) - by slug
        public async Task<IActionResult> Details(string slug)
        {
            var shop = await _shopService.GetShopBySlugAsync(slug);
            if (shop == null)
                return NotFound();
            return View(shop);
        }

        // CREATE - Formular (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - Salvare (admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Shop shop, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View(shop);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveImageAsync(ImageFile, "shops");
                if (path != null) shop.Image = path;
            }

            await _shopService.CreateShopAsync(shop);
            TempData["Success"] = $"Shop in {shop.City} added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // EDIT - Formular (admin only) - by slug
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string slug)
        {
            var shop = await _shopService.GetShopBySlugAsync(slug);
            if (shop == null)
                return NotFound();
            return View(shop);
        }

        // EDIT - Salvare (admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string slug, Shop shop, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View(shop);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var path = await SaveImageAsync(ImageFile, "shops");
                if (path != null) shop.Image = path;
            }

            await _shopService.UpdateShopAsync(shop);
            TempData["Success"] = $"Shop in {shop.City} updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // DELETE - Confirmare (admin only) - by slug
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string slug)
        {
            var shop = await _shopService.GetShopBySlugAsync(slug);
            if (shop == null)
                return NotFound();
            return View(shop);
        }

        // DELETE - Stergere efectiva (admin only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shopService.DeleteShopAsync(id);
            TempData["Success"] = "Shop deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
