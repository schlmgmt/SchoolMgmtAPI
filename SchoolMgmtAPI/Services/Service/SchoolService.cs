using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Services.IService;
using AutoMapper;
using System.Net;

namespace SchoolMgmtAPI.Services.Service
{
    public class SchoolService : ISchoolService
    {
        private AppDbContext _dbContext;
        private IMapper _mapper;

        public SchoolService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponseModel<School>> AddSchool(AddSchoolViewModel request)
        {
            School schoolData = _mapper.Map<School>(request);
            schoolData.CreatedAt = DateTime.UtcNow;
            schoolData.CreatedBy = 1;
            schoolData.UpdatedBy = 1;
            schoolData.UpdatedAt = DateTime.UtcNow;

            await _dbContext.School.AddAsync(schoolData);
            await _dbContext.SaveChangesAsync();

            return new APIResponseModel<School>()
            {
                code = HttpStatusCode.OK,
                message = "School Added Successfully",
                data = schoolData
            };
        }
    }
}
