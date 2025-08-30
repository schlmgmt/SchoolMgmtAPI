using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ResponseModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IRoleService
    {
        Task<APIResponseModel<Roles>> CreateRole(string roleName);
    }
}
