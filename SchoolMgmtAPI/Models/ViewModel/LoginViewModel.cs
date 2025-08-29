using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
