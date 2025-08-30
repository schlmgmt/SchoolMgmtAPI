using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI.Models.ResponseModel
{
    public class LoginResponse
    {
        public string token { get; set; }
        public Users user {  get; set; }
    }
}
