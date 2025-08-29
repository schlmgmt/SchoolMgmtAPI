using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.DbModel
{
    public class Users
    {
        [Key]
        public int UserId {  get; set; }
        public string UserName { get; set; }
        public int SchoolId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsPasswordUpdated { get; set; }
    }
}
