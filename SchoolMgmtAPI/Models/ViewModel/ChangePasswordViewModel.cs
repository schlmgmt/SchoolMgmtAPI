using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }
 
        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
