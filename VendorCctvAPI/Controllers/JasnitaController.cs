using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using VendorCctvAPI.Models;

namespace VendorCctvAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JasnitaController : ControllerBase
    {
        [HttpPost("send-cctv-status")]
        public IActionResult SendCctvStatus([FromBody] List<JasnitaVM.ViewModel.JasnitaEvent> events)
        {
            var response = new ResponseBase();

            try
            {
                if (events == null || events.Count == 0)
                    return BadRequest(response.ToErrorInfoMessage("Tidak ada data event yang diterima."));

                JasnitaVM.Method.SendJasnitaLog(events);

                return Ok(response.ToSuccessInfoMessage("Data Berhasil Disimpan"));
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
