using DIY_STORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace DIY_STORE.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET /Categories?cat=power-tools
        public async Task<IActionResult> Index(string cat = "")
        {
            var category = await _categoryService.GetCategoryBySlugAsync(cat);

            if (category == null)
            {
                ViewBag.Title = "Category not found";
                ViewBag.Subcategories = new List<object>();
                ViewBag.Cat = cat;
                return View();
            }

            ViewBag.Title = category.Name;
            ViewBag.Subcategories = category.Subcategories;
            ViewBag.Cat = cat;
            return View();
        }
    }
}
