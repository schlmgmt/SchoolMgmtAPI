using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleViewModel Request)
        {
            if (string.IsNullOrEmpty(Request.RoleName))
            {
                return BadRequest("Role name is required.");
            }
            var response = await _roleService.CreateRole(Request.RoleName);
            
            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
