using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using System.Text;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers
{
    public class ApiController : Controller
    {
        ResponseBase response = new ResponseBase();
        [HttpGet]
        public IActionResult GetRealisasiPajak(string nop)
        {
            try
            {
                // Manual Basic Auth check
                if (!Request.Headers.ContainsKey("Authorization"))
                    return Unauthorized("Missing Authorization Header");

                var authHeader = Request.Headers["Authorization"].ToString();
                if (!authHeader.StartsWith("Basic")) return Unauthorized("Invalid Authorization Header");

                var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

                var parts = decodedCredentials.Split(':');
                if (parts.Length != 2) return Unauthorized("Invalid Credential Format");

                var username = parts[0];
                var password = parts[1];

                // Ganti username & password sesuai permintaan
                if (username != "bapemkesra" || password != "SurabayaHebat@2025!")
                    return Unauthorized("Username/Password salah");

                var getData = Models.ApiVM.Method.GetRealisasiPajak(nop);
                if (string.IsNullOrEmpty(getData.NPWPD) || string.IsNullOrEmpty(getData.NPWPD))
                {
                    return Json(null);
                }
                else
                {
                    return Json(getData);
                }

            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
    }
}
