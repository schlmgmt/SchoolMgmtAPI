using System.Globalization;
using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IEmailService
    {
        Task<APIResponseModel<EmailTemplates>> AddEmailTemplate(AddEmailTemplateViewModel request);
        Task<bool> SendUserRegisterEmail(Users request, string OTP);
    }
}
