using System.ComponentModel.DataAnnotations;

namespace SchoolMgmtAPI.Models.DbModel
{
    public class EmailTemplates
    {
        [Key]
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
