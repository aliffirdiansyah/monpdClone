using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;
using VendorCctvAPI.Models;

namespace VendorCctvAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TelkomController : Controller
    {
        [HttpPost("send-cctv-status")]
        public async Task<IActionResult> SendCctvStatus([FromBody] List<TelkomVM.ViewModel.TelkomEvent> events)
        {
            var response = new ResponseBase();

            try
            {
                if (events == null || events.Count == 0)
                {
                    return BadRequest(response.ToErrorInfoMessage("Tidak ada data event yang diterima."));
                }

                int vendorId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Name) ?? "0");
                if (vendorId == (int)EnumFactory.EVendorParkirCCTV.Telkom)
                {
                    await TelkomVM.Method.SendTelkomLogAsync(events);
                }
                else
                {
                    throw new ArgumentException("Anda tidak berhak mengakses ini");
                }

                return Ok(response.ToSuccessInfoMessage("Data Berhasil Disimpan"));
            }
            catch (ArgumentException ex)
            {
                return Ok(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return Ok(response.ToInternalServerError());
            }
        }
    }
}
