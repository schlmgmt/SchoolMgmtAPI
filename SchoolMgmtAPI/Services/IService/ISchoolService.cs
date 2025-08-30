using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ResponseModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface ISchoolService
    {
        Task<APIResponseModel<School>> AddSchool(AddSchoolViewModel request);
    }
}
