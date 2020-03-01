using System.ComponentModel.DataAnnotations;

namespace Default.Api.ViewModels.Auth
{
    public class RegisterUserViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 4)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
