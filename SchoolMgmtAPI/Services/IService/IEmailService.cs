using System.Globalization;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ResponseModel;
using SchoolMgmtAPI.Models.ViewModel;

namespace SchoolMgmtAPI.Services.IService
{
    public interface IEmailService
    {
        Task<APIResponseModel<EmailTemplates>> AddEmailTemplate(AddEmailTemplateViewModel request);
        Task<bool> SendUserRegisterEmail(Users request, string OTP);
        Task<bool> ForgetPasswordMail(string Email, string Username, string Password);
    }
}
