using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.AspNetCore.Identity;

namespace DIY_STORE.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            // Try by email first, then by username
            var user = await _userManager.FindByEmailAsync(model.Email)
                    ?? await _userManager.FindByNameAsync(model.Email);

            if (user == null)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);
        }

        public async Task<(IdentityResult Result, ApplicationUser? User)> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return (result, user);
            }

            return (result, null);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string?> UploadProfilePictureAsync(string userId, IFormFile file, string webRootPath)
        {
            if (file == null || file.Length == 0) return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension)) return null;

            return await SaveProfilePictureAsync(userId, webRootPath, async (filePath) =>
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }, extension);
        }

        public async Task<string?> UploadCroppedProfilePictureAsync(string userId, string base64Data, string webRootPath)
        {
            if (string.IsNullOrEmpty(base64Data)) return null;

            string? mimeType = null;
            string? base64 = null;

            if (base64Data.StartsWith("data:"))
            {
                var semicolon = base64Data.IndexOf(';');
                if (semicolon > 5)
                    mimeType = base64Data[5..semicolon];
                var commaIdx = base64Data.IndexOf(',');
                if (commaIdx >= 0)
                    base64 = base64Data[(commaIdx + 1)..];
            }

            if (string.IsNullOrEmpty(base64)) return null;

            var extension = mimeType switch
            {
                "image/jpeg" => ".jpg",
                "image/png"  => ".png",
                "image/gif"  => ".gif",
                "image/webp" => ".webp",
                _            => ".jpg"
            };

            byte[] bytes;
            try { bytes = Convert.FromBase64String(base64); }
            catch { return null; }

            return await SaveProfilePictureAsync(userId, webRootPath, async (filePath) =>
            {
                await File.WriteAllBytesAsync(filePath, bytes);
            }, extension);
        }

        private async Task<string?> SaveProfilePictureAsync(
            string userId, string webRootPath,
            Func<string, Task> writeFile,
            string extension)
        {
            var uploadDir = Path.Combine(webRootPath, "images", "profile-pictures");
            Directory.CreateDirectory(uploadDir);

            var user = await _userManager.FindByIdAsync(userId);
            if (user?.ProfilePicturePath != null)
            {
                var oldPath = Path.Combine(webRootPath, "images", user.ProfilePicturePath.TrimStart('/'));
                if (File.Exists(oldPath)) File.Delete(oldPath);
            }

            var fileName = $"{userId}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDir, fileName);

            await writeFile(filePath);

            var relativePath = $"profile-pictures/{fileName}";

            if (user != null)
            {
                user.ProfilePicturePath = relativePath;
                await _userManager.UpdateAsync(user);
            }

            return relativePath;
        }
    }
}
