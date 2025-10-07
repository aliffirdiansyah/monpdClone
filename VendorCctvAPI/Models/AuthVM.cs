using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using static VendorCctvAPI.Models.AuthVM.ViewModel;

namespace VendorCctvAPI.Models
{
    public class AuthVM
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
                public string Name { get; set; } = string.Empty;
                public string Token { get; set; } = string.Empty;
                public DateTime IssuedAt { get; set; }
                public DateTime ExpiresAt { get; set; }
            }
        }

        public class Method
        {
            public static async Task<UserApiVendorCctv?> LoginAsync(string username, string password)
            {
                await using var context = DBClass.GetContext();
                username = username.Trim().ToUpper();

                var user = await context.UserApiVendorCctvs.FirstOrDefaultAsync(x => x.Username.Trim().ToUpper() == username && x.Pass == password);

                return user;
            }

            public static JwtTokenResult GenerateJwtToken(UserApiVendorCctv user, IConfiguration config)
            {
                var jwtSettings = config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? ""));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var issuedAt = DateTime.Now;
                var expiresAt = issuedAt.AddHours(3);

                int vendorId = 0;
                if (user.Name?.ToUpper() == "JASNITA")
                {
                    vendorId = (int)EnumFactory.EVendorParkirCCTV.Jasnita;
                }
                else if (user.Name?.ToUpper() == "TELKOM")
                {
                    vendorId = (int)EnumFactory.EVendorParkirCCTV.Telkom;
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, vendorId.ToString()),
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
                    Name = user.Name ?? "",
                    Token = tokenString,
                    IssuedAt = issuedAt,
                    ExpiresAt = expiresAt
                };
            }
        }
    }
}
