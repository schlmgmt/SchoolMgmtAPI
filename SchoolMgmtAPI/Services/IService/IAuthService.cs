using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Models.ResponseModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IAuthService
    {
        Task<APIResponseModel<LoginResponse>> Login(LoginViewModel request);
        Task<APIResponseModel<bool>> ForgotPassword(string Email);
        Task<APIResponseModel<bool>> ChangePassword(ChangePasswordViewModel request, int userId);
    }
}
