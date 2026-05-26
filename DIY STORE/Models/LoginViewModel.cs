using System.ComponentModel.DataAnnotations;

namespace DIY_STORE.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or username is required.")]
        public string Email { get; set; } = string.Empty;  // field name kept as Email for compatibility

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
