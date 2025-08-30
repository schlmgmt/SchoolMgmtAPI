using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("AddNewEmail")]
        public async Task<IActionResult> AddEmailTemplate(AddEmailTemplateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var response = await _emailService.AddEmailTemplate(request);

            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
             return BadRequest(response);
        }
    }
}
