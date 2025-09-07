using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost("AddNewUser")]
        public async Task<IActionResult> CreateUser([FromBody] AddUserViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var response = await _userService.AddUser(request);

            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

        [Authorize]
        [HttpPost("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            if (response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

    }
}
