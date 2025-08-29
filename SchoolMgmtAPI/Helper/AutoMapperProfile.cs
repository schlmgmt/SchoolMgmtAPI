using AutoMapper;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddSchoolViewModel, School>().ReverseMap();
            CreateMap<AddEmailTemplateViewModel, EmailTemplates>().ReverseMap();
            CreateMap<AddUserViewModel, Users>().ReverseMap();
        }
    }
}
