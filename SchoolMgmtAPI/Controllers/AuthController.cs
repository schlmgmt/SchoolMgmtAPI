using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SchoolMgmtAPI.Models.ResponseModel;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authservice.Login(request);

            if (response == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("Email is required");
            }

            var response = await _authservice.ForgotPassword(Email);

            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst("id")?.Value);

            var response = await _authservice.ChangePassword(request, userId);

            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
