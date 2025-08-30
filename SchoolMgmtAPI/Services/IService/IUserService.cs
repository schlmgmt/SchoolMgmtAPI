using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ResponseModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IUserService
    {
        Task<APIResponseModel<Users>> AddUser(AddUserViewModel request);
    }
}
