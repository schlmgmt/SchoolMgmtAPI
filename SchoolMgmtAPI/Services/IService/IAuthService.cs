using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Models.ResponseModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IAuthService
    {
        Task<APIResponseModel<LoginResponse>> Login(LoginViewModel request, string IpAddress);
        Task<APIResponseModel<LoginResponse>> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress);
        Task<APIResponseModel<bool>> ForgotPassword(string Email);
        Task<APIResponseModel<bool>> ChangePassword(ChangePasswordViewModel request, int userId);
    }
}
