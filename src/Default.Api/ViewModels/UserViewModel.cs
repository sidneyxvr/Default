using Default.Api.Resources;
using System.ComponentModel.DataAnnotations;

namespace Default.Api.ViewModels
{
    public class UserViewModel : ViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(MessageValidation))]
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
