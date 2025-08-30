using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolMgmtAPI.Models.DbModel;
using SchoolMgmtAPI.Models.ResponseModel;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace SchoolMgmtAPI.Services.Service
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private AppDbContext _dbContext;
        private IEmailService _emailService;

        public AuthService(IConfiguration config, AppDbContext dbContext, IEmailService emailService)
        {
            _config = config;
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public string GenerateToken(string userId, string email, string role)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<APIResponseModel<LoginResponse>>Login(LoginViewModel request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
            {
                return new APIResponseModel<LoginResponse>()
                {
                    code = System.Net.HttpStatusCode.NotFound,
                    message = $"No user found with {request.Email}"
                };
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new APIResponseModel<LoginResponse>()
                {
                    code = System.Net.HttpStatusCode.Forbidden,
                    message = "Invalid Password"
                };
            }

            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == user.RoleId);
            var token = GenerateToken(user.UserId.ToString(), request.Email, role.RoleName);


            var loginres = new LoginResponse()
            {
                token = token,
                user = user,
            };

            return new APIResponseModel<LoginResponse>()
            {
                code = System.Net.HttpStatusCode.OK,
                message = "Logged in successfully",
                data = loginres
            };
        }

        public async Task<APIResponseModel<bool>> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return new APIResponseModel<bool>()
                {
                    code = System.Net.HttpStatusCode.BadRequest,
                    message = "Email is required",
                    data = false
                };
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if(user == null)
            {
                return new APIResponseModel<bool>()
                {
                    code = System.Net.HttpStatusCode.NotFound,
                    message = $"No User exist with {Email}",
                    data = false
                };
            }

            var NewPassword = GenerateRandomPassword();

            user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            user.IsPasswordUpdated = false;

            var IsEmailSent = await _emailService.ForgetPasswordMail(user.Email, user.UserName, NewPassword);
            if (IsEmailSent)
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new APIResponseModel<bool>()
                {
                    code = System.Net.HttpStatusCode.OK,
                    message = "Forgot password mail sent successfully",
                    data = true
                };
            }

            return new APIResponseModel<bool>()
            {
                code = System.Net.HttpStatusCode.BadRequest,
                message = "Can not reset password!! Please try again",
                data = true
            };
        }

        public async Task<APIResponseModel<bool>> ChangePassword(ChangePasswordViewModel request, int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if(user == null)
            {
                return new APIResponseModel<bool>()
                {
                    code = System.Net.HttpStatusCode.NotFound,
                    message = "Invalid User",
                    data = false
                };
            }

            var currentPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.CurrentPassword);

            if (user.Password != currentPasswordHash)
            {
                return new APIResponseModel<bool>()
                {
                    code = System.Net.HttpStatusCode.BadRequest,
                    message = "Current password is incorrect",
                    data = false
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return new APIResponseModel<bool>()
            {
                code = System.Net.HttpStatusCode.OK,
                message = "Password changed successfully",
                data = true
            };
        }

        public string GenerateRandomPassword()
        {
            string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
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
