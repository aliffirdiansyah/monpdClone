using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using static VendorCctvAPI.Models.TokenVM.ViewModel;

namespace VendorCctvAPI.Models
{
    public class TokenVM
    {
        public class ViewModel
        {
            public class LoginRequest
            {
                public string Username { get; set; } = "";
                public string Password { get; set; } = "";
            }

            public class JwtTokenResult
            {
                public string Username { get; set; } = string.Empty;
                public string Token { get; set; } = string.Empty;
                public DateTime IssuedAt { get; set; }
                public DateTime ExpiresAt { get; set; }
            }
        }

        public class Method
        {
            public static JwtTokenResult GenerateJwtToken(string username, IConfiguration config)
            {
                var jwtSettings = config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? ""));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var issuedAt = DateTime.Now;
                var expiresAt = issuedAt.AddHours(3);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    notBefore: issuedAt,
                    expires: expiresAt,
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new JwtTokenResult
                {
                    Username = username,
                    Token = tokenString,
                    IssuedAt = issuedAt,
                    ExpiresAt = expiresAt
                };
            }
        }
    }
}
