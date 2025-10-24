using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using System.Text;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers
{
    public class ApiController : Controller
    {
        private readonly string _connectionString;

        ResponseBase response = new ResponseBase();
        public ApiController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MONPEDE") ?? "";
        }

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
        [HttpGet]
        public IActionResult GetDataTSAPI(string nop, int tahun, string masa, string username, string password)
        {
            try
            {
                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    //int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
                    //DateTime tglCutOff = new DateTime(tahun, 1, 1);
                    //DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
                    var json = Models.ApiVM.Method.GetTSData(_connectionString, nop, tahun, masa);
                    return Json(json);
                }
                else
                {
                    return Unauthorized();
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
        [HttpGet]
        public IActionResult GetDataTBAPI(string nop, int tahun, string masa, string username, string password)
        {
            try
            {

                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    //int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
                    //DateTime tglCutOff = new DateTime(tahun, 1, 1);
                    //DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
                    var json = Models.ApiVM.Method.GetTBData(_connectionString, nop, tahun, masa);
                    return Json(json);
                }
                else
                {
                    return Unauthorized();
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
        [HttpGet]
        public IActionResult GetDataSBAPI(string nop, int tahun, string masa, string username, string password)
        {
            try
            {
                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    //int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
                    //DateTime tglCutOff = new DateTime(tahun, 1, 1);
                    //DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
                    var json = Models.ApiVM.Method.GetSBData(_connectionString, nop, tahun, masa);
                    return Json(json);
                }
                else
                {
                    return Unauthorized();
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
