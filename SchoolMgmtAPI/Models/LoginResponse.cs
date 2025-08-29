using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public Users user {  get; set; }
    }
}
