using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(Users user);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
