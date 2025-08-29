using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IUserService
    {
        Task<APIResponseModel<Users>> AddUser(AddUserViewModel request);
    }
}
