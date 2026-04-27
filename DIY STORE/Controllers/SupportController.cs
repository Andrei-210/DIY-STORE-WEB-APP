using DIY_STORE.Models;
using DIY_STORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIY_STORE.Controllers
{
    public class SupportController : Controller
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        // GET /Support
        public IActionResult Index()
        {
            return View();
        }

        // POST /Support/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactFormModel model)
        {
            if (!ModelState.IsValid)
                return View("Index");

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                Date = DateTime.Now
            };

            await _supportService.SaveMessageAsync(message);
            TempData["SupportSuccess"] = $"Thank you, {model.Name}! Your message has been sent.";
            return RedirectToAction("Index");
        }
    }
}
