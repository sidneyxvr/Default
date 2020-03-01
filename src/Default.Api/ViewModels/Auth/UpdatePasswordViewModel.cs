using System.ComponentModel.DataAnnotations;

namespace Default.Api.ViewModels.Auth
{
    public class UpdatePasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
