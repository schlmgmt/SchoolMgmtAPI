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

namespace SchoolMgmtAPI.Services.Service
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private AppDbContext _dbContext;
        private IEmailService _emailService;
        private ITokenService _tokenService;

        public AuthService(IConfiguration config, AppDbContext dbContext, IEmailService emailService, ITokenService tokenService)
        {
            _config = config;
            _dbContext = dbContext;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<APIResponseModel<LoginResponse>>Login(LoginViewModel request, string IpAddress)
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

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshEntity = new RefreshToken
            {
                TokenHash = _tokenService.HashToken(refreshToken),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpiryDays"])),
                Created = DateTime.UtcNow,
                CreatedByIp = IpAddress,
                UserId = user.UserId
            };

            await _dbContext.RefreshToken.AddAsync(refreshEntity);
            await _dbContext.SaveChangesAsync();

            var loginres = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = user,
            };

            return new APIResponseModel<LoginResponse>()
            {
                code = System.Net.HttpStatusCode.OK,
                message = "Logged in successfully",
                data = loginres
            };
        }

        public async Task<APIResponseModel<LoginResponse>> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var hash = _tokenService.HashToken(refreshToken);
            var entity = await _dbContext.RefreshToken.FirstOrDefaultAsync(x => x.TokenHash == hash);

            if (entity == null || !entity.IsActive)
                return null;

            // revoke old token
            entity.Revoked = DateTime.UtcNow;
            entity.RevokedByIp = ipAddress;

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == entity.UserId);

            // create new
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newEntity = new RefreshToken
            {
                TokenHash = _tokenService.HashToken(newRefreshToken),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserId = entity.UserId
            };
            entity.ReplacedByTokenHash = newEntity.TokenHash;

            await _dbContext.RefreshToken.AddAsync(newEntity);
            await _dbContext.SaveChangesAsync();

            var response =  new LoginResponse()
            {
                AccessToken = await _tokenService.GenerateAccessToken(user),
                RefreshToken = newRefreshToken,
                User = user,
            };

            return new APIResponseModel<LoginResponse>()
            {
                data = response,
                code = System.Net.HttpStatusCode.OK,
                message = "Token refresh successfully"
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
        {
            var entity = await _dbContext.RefreshToken
                .SingleOrDefaultAsync(t => t.TokenHash == _tokenService.HashToken(refreshToken));

            if (entity == null || !entity.IsActive)
                return false;

            entity.Revoked = DateTime.UtcNow;
            entity.RevokedByIp = ipAddress;

            await _dbContext.SaveChangesAsync();
            return true;
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
