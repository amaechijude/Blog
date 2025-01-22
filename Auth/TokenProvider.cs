using System.Security.Claims;
using System.Text;
using Blog.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Auth
{
    public class TokenProvider
    {
        public string Create(User user)
        {
            DotNetEnv.Env.Load();
            string secretKey = $"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credetials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    ]
                ),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credetials,
                Issuer = $"{Environment.GetEnvironmentVariable("JWT_ISSUER")}",
                Audience = $"{Environment.GetEnvironmentVariable("JWT_AUDIENCE")}"
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}