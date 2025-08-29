using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.DbModel
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set;  }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
