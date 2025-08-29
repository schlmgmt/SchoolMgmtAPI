namespace SchoolMgmtAPI.Models.ViewModel
{
    public class AddUserViewModel
    {
        public string UserName { get; set; }
        public int SchoolId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
