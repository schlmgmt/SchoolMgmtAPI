using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Models;
using AutoMapper;
using SchoolMgmtAPI.Services.IService;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace SchoolMgmtAPI.Services.Service
{
    public class EmailService : IEmailService
    {
        private AppDbContext _dbContext;
        private IMapper _mapper;
        private SMTPSettings _smtpSettings;

        public EmailService(AppDbContext dbContext, IMapper mapper, IOptions<SMTPSettings> smtpSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<APIResponseModel<EmailTemplates>> AddEmailTemplate(AddEmailTemplateViewModel request)
        {
            var EmailTemplate = _mapper.Map<EmailTemplates>(request);

            EmailTemplate.CreatedAt = DateTime.UtcNow;
            EmailTemplate.UpdatedAt = DateTime.UtcNow;
            EmailTemplate.CreatedBy = 1;
            EmailTemplate.UpdatedBy = 1;

            await _dbContext.EmailTemplates.AddAsync(EmailTemplate);
            await _dbContext.SaveChangesAsync();

           var respone = new APIResponseModel<EmailTemplates>()
            {
                code = System.Net.HttpStatusCode.OK,
                message = "Email Template Added Successfully",
                data = EmailTemplate
            };

            return respone;
        }

        public async Task<bool> SendUserRegisterEmail(Users request, string OTP)
        {
            var mailtemplate = await _dbContext.EmailTemplates.FirstOrDefaultAsync(x => x.Name == "UserCredentials");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Support", _smtpSettings.From));
            email.To.Add(new MailboxAddress("", request.Email));
            email.Subject = mailtemplate?.Subject;

            var template = mailtemplate.Body;

            template = template.Replace("{{FirstName}}", request?.UserName)
                               .Replace("{{UserName}}", request.Email)
                               .Replace("{{Password}}", OTP);

            // HTML Body
            var builder = new BodyBuilder
            {
                HtmlBody = template
            };
            email.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }

            return true;
        }
    }
}
