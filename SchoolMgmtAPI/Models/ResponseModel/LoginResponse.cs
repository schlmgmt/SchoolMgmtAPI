using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI.Models.ResponseModel
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Users User {  get; set; }
    }
}
