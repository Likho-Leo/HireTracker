using System.ComponentModel.DataAnnotations;

namespace HireTracker.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
