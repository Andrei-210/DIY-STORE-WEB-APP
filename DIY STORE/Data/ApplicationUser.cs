using Microsoft.AspNetCore.Identity;

namespace DIY_STORE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; } = string.Empty;
    }
}
