using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.ViewModel
{
    public class AddEmailTemplateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
