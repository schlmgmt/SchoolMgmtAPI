using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Models;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IAuthService
    {
        Task<APIResponseModel<LoginResponse>> Login(LoginViewModel request);
    }
}
