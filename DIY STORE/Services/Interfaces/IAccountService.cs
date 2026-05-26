using DIY_STORE.Data;
using DIY_STORE.Models;
using Microsoft.AspNetCore.Identity;

namespace DIY_STORE.Services
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<(IdentityResult Result, ApplicationUser? User)> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
        Task<string?> UploadProfilePictureAsync(string userId, IFormFile file, string webRootPath);
        Task<string?> UploadCroppedProfilePictureAsync(string userId, string base64Data, string webRootPath);
    }
}
