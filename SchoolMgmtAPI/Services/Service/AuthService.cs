using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolMgmtAPI.Models;
using SchoolMgmtAPI.Models.ViewModel;
using SchoolMgmtAPI.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolMgmtAPI.Services.Service
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private AppDbContext _dbContext;

        public AuthService(IConfiguration config, AppDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
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
    }
}
