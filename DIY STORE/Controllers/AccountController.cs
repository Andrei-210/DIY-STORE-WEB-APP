using DIY_STORE.Data;
using DIY_STORE.Models;
using DIY_STORE.Services;
using DIY_STORE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DIY_STORE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOrderService _orderService;
        private readonly IFavoriteService _favoriteService;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOrderService orderService,
            IFavoriteService favoriteService,
            IWebHostEnvironment env)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _orderService = orderService;
            _favoriteService = favoriteService;
            _env = env;
        }

        // GET /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // GET /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (result, user) = await _accountService.RegisterAsync(model);

            if (result.Succeeded && user != null)
            {
                if (model.ProfilePicture != null)
                    await _accountService.UploadProfilePictureAsync(user.Id, model.ProfilePicture, _env.WebRootPath);

                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // POST /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET /Account/Profile
        [Authorize]
        public async Task<IActionResult> Profile(string section = "account")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var orders = await _orderService.GetUserOrdersAsync(user.Id);
            var favorites = await _favoriteService.GetUserFavoritesAsync(user.Id);

            var vm = new ProfileViewModel
            {
                Email = user.Email ?? string.Empty,
                UserName = user.UserName,
                Address = user.Address,
                ProfilePicturePath = user.ProfilePicturePath,
                ActiveSection = section,
                Orders = orders.ToList(),
                Favorites = favorites.ToList()
            };

            return View(vm);
        }

        // GET /Account/AccessDenied
        public IActionResult AccessDenied() => View();

        // POST /Account/UpdateProfile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
            string? newUsername, string? newEmail, string? newAddress,
            string? currentPassword, string? newPassword, string? confirmPassword,
            IFormFile? profilePicture, string? croppedImageData)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var errors  = new List<string>();
            var success = new List<string>();

            // 1. Cropped image from canvas (takes priority over file input)
            if (!string.IsNullOrEmpty(croppedImageData))
            {
                var path = await _accountService.UploadCroppedProfilePictureAsync(user.Id, croppedImageData, _env.WebRootPath);
                if (path != null) success.Add("Profile picture updated.");
                else errors.Add("Could not save the cropped image.");
            }
            // 2. Plain file upload (fallback)
            else if (profilePicture != null && profilePicture.Length > 0)
            {
                var path = await _accountService.UploadProfilePictureAsync(user.Id, profilePicture, _env.WebRootPath);
                if (path != null) success.Add("Profile picture updated.");
                else errors.Add("Invalid image file. Allowed: jpg, jpeg, png, gif, webp.");
            }

            // 3. Username
            if (!string.IsNullOrWhiteSpace(newUsername) && newUsername != user.UserName)
            {
                var r = await _userManager.SetUserNameAsync(user, newUsername);
                if (r.Succeeded) success.Add("Username updated.");
                else errors.AddRange(r.Errors.Select(e => e.Description));
            }

            // 4. Email
            if (!string.IsNullOrWhiteSpace(newEmail) && newEmail != user.Email)
            {
                var r = await _userManager.SetEmailAsync(user, newEmail);
                if (r.Succeeded) success.Add("Email updated.");
                else errors.AddRange(r.Errors.Select(e => e.Description));
            }

            // 5. Address
            if (newAddress != null && newAddress != user.Address)
            {
                user.Address = newAddress;
                var r = await _userManager.UpdateAsync(user);
                if (r.Succeeded) success.Add("Address updated.");
                else errors.AddRange(r.Errors.Select(e => e.Description));
            }

            // 6. Password
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (string.IsNullOrWhiteSpace(currentPassword))
                    errors.Add("Current password is required to set a new password.");
                else if (newPassword != confirmPassword)
                    errors.Add("New passwords do not match.");
                else
                {
                    var r = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                    if (r.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        success.Add("Password updated.");
                    }
                    else errors.AddRange(r.Errors.Select(e => e.Description));
                }
            }

            if (success.Any()) await _signInManager.RefreshSignInAsync(user);

            TempData["ProfileSuccess"] = success.Any() ? string.Join(" ", success) : null;
            TempData["ProfileErrors"]  = errors.Any()  ? string.Join(" ", errors)  : null;

            return RedirectToAction("Profile", new { section = "account" });
        }
    }
}
