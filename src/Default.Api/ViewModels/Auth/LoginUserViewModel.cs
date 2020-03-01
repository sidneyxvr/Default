using System.ComponentModel.DataAnnotations;

namespace Default.Api.ViewModels.Auth
{
    public class LoginUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
