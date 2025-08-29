using System.Data;
using System.Net;
using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Services.Service
{
    public class RoleService : IRoleService
    {
        private AppDbContext _DbContext;

        public RoleService(AppDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<APIResponseModel<Roles>> CreateRole(string roleName)
        {
            Roles role = new Roles();
            role.RoleName = roleName;
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;
            role.CreatedBy = 1;
            role.UpdatedBy = 1;

            _DbContext.Roles.Add(role);
            await _DbContext.SaveChangesAsync();

            var response = new APIResponseModel<Roles>()
            {
                code = HttpStatusCode.OK,
                message = "Role Created Successfully",
                data = role
            };
            return response;
        }
    }
}
