using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.DbModel
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }
        public int SchoolId { get; set; }
        public string SectionName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
