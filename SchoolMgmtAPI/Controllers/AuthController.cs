using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
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


    }
}
