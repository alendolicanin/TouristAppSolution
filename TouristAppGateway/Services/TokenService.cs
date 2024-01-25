using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TouristAppGateway.Models;

namespace TouristAppGateway.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token:Secret").Value));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id)
            };

            var token = new JwtSecurityToken(
              expires: DateTime.UtcNow.AddHours(1),
              claims: authClaims,
              signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
              );

            var toReturn = new JwtSecurityTokenHandler().WriteToken(token);

            return toReturn;
        }
    }
}
