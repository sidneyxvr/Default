using System.ComponentModel.DataAnnotations;

namespace Default.Api.ViewModels.Auth
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display]
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Display]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
