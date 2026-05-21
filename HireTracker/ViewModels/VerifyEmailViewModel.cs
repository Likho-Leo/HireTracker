using System.ComponentModel.DataAnnotations;

namespace HireTracker.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email adddress is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
