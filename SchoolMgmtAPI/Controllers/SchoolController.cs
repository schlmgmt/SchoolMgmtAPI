using Microsoft.AspNetCore.Mvc;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;

namespace SchoolMgmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpPost("AddNewSchool")]
        public async Task<IActionResult> CreateSchool(AddSchoolViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _schoolService.AddSchool(request);

            if(response.code == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
