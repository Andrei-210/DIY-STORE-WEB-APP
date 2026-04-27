using DIY_STORE.Models;
using DIY_STORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIY_STORE.Controllers
{
    public class ShopsController : Controller
    {
        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        // READ - Lista toate magazinele (public)
        public async Task<IActionResult> Index()
        {
            var shops = await _shopService.GetAllShopsAsync();
            return View(shops);
        }

        // READ - Detalii un magazin (public)
        public async Task<IActionResult> Details(int id)
        {
            var shop = await _shopService.GetShopAsync(id);
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
        public async Task<IActionResult> Create(Shop shop)
        {
            if (!ModelState.IsValid)
                return View(shop);

            await _shopService.CreateShopAsync(shop);
            TempData["Success"] = $"Shop in {shop.City} added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // EDIT - Formular (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var shop = await _shopService.GetShopAsync(id);
            if (shop == null)
                return NotFound();
            return View(shop);
        }

        // EDIT - Salvare (admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Shop shop)
        {
            if (id != shop.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(shop);

            await _shopService.UpdateShopAsync(shop);
            TempData["Success"] = $"Shop in {shop.City} updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // DELETE - Confirmare (admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var shop = await _shopService.GetShopAsync(id);
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
