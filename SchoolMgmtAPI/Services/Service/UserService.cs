using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Cryptography;
using SchoolMgmtAPI.Models.ResponseModel;

namespace SchoolMgmtAPI.Services.Service
{
    public class UserService : IUserService
    {
        private AppDbContext _dbContext;
        private IEmailService _emailService;
        private IMapper _mapper;

        public UserService(AppDbContext dbContext, IMapper mapper, IEmailService emailService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<APIResponseModel<List<Users>>> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return new APIResponseModel<List<Users>>()
            {
                data = users,
                code = System.Net.HttpStatusCode.OK,
                message = "Users fetched successfully"
            };
        }

        public async Task<APIResponseModel<Users>> AddUser(AddUserViewModel request)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if(existingUser != null)
            {
                var response = new APIResponseModel<Users>()
                {
                    code = System.Net.HttpStatusCode.Forbidden,
                    message = $"User with this {request.Email} already exists",
                };

                return response;
            }
            var NewUser = _mapper.Map<Users>(request);
            var OTP = GenerateRandomPassword();
            var HashedPassword = BCrypt.Net.BCrypt.HashPassword(OTP);

            NewUser.Password = HashedPassword;
            NewUser.CreatedAt = DateTime.UtcNow;
            NewUser.UpdatedAt = DateTime.UtcNow;
            NewUser.CreatedBy = 1;
            NewUser.UpdatedBy = 1;

            var isEmailSent = await _emailService.SendUserRegisterEmail(NewUser, OTP);

            if (isEmailSent)
            {
                await _dbContext.Users.AddAsync(NewUser);
                await _dbContext.SaveChangesAsync();

                return new APIResponseModel<Users>()
                {
                    code = System.Net.HttpStatusCode.OK,
                    message = "User Created Successfully",
                    data = NewUser
                };
            }

            return new APIResponseModel<Users>()
            {
                code = System.Net.HttpStatusCode.InternalServerError,
                message = "Cannot Create User",
            };   
        }

        public string GenerateRandomPassword()
        {
            string AllowedChars ="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            int length = 8;
            var randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var passwordChars = randomBytes
                .Select(b => AllowedChars[b % AllowedChars.Length])
                .ToArray();

            return new string(passwordChars);
        }
    }
}
