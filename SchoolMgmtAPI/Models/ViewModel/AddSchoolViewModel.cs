using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.ViewModel
{
    public class AddSchoolViewModel
    {
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Pincode { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
