using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IRoleService
    {
        Task<APIResponseModel<Roles>> CreateRole(string roleName);
    }
}
