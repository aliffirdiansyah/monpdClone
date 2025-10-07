using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorCctvAPI.Models;

namespace VendorCctvAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] TokenVM.ViewModel.LoginRequest request)
        {
            var response = new ResponseBase();

            if (request.Username == "admin" && request.Password == "1234")
            {
                var tokenResult = TokenVM.Method.GenerateJwtToken(request.Username, _config);
                return Ok(response.ToSuccessInfoMessage("Login berhasil", tokenResult));
            }

            return Unauthorized(response.ToErrorInfoMessage("Username atau password salah."));
        }
    }
}
