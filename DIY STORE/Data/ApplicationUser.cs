using Microsoft.AspNetCore.Identity;

namespace DIY_STORE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Relative path to profile picture stored under wwwroot/images/
        /// e.g. "profile-pictures/userId_guid.jpg"
        /// </summary>
        public string? ProfilePicturePath { get; set; }
    }
}
