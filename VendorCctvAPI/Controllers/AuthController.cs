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
        public async Task<IActionResult> Login([FromBody] AuthVM.ViewModel.LoginRequest request)
        {
            var response = new ResponseBase();

            try
            {
                var user = await AuthVM.Method.LoginAsync(request.Username, request.Password);
                if (user == null)
                {
                    return Unauthorized(response.ToErrorInfoMessage("Username atau password salah."));
                }

                var tokenResult = AuthVM.Method.GenerateJwtToken(user, _config);
                return Ok(response.ToSuccessInfoMessage("Login berhasil", tokenResult));
            }
            catch (ArgumentException ex)
            {
                return Ok(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return Ok(response.ToInternalServerError(ex.Message));
            }
        }
    }
}
