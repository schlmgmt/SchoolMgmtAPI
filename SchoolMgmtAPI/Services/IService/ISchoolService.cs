using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface ISchoolService
    {
        Task<APIResponseModel<School>> AddSchool(AddSchoolViewModel request);
    }
}
