using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using static MonPDLib.General.EnumFactory;
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
        //[HttpGet]
        //public IActionResult Index(string username, string password, string tanggal = "")
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        DailyReportVM mod = new DailyReportVM(tglRealisasi, tglCutOff, true);

        //        return Json(mod.lstDailyReport);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
        //[HttpGet]
        //public IActionResult Realisasi(string username, string password)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        //int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        //DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        //DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        RealisasiNew.GetSubRinci mod = new GetSubRinci();

        //        return Json(mod.SubRinciList);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetDataUPTBAPI(string tanggal, string username, string password)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        var json = new Models.AksiSembilanAPI.GetDataUPTBAPI(tglRealisasi, tglCutOff);

        //        return Json(json);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
        //[HttpGet]
        //public IActionResult GetDataUPTBAPITES(string tanggal, string username, string password)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        var json = new Models.AksiSembilanVM.WilayahDailyAPI(tglRealisasi, tglCutOff);

        //        return Json(json);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
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
        //[HttpGet]
        //public ActionResult GetDataPajakRealisasi(int masa, int tahun, int uptb, int pajak, string username, string password)
        //{
        //    if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
        //    {
        //        //int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        //DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        //DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        var data = new Models.TSVM.Realisasi(masa, tahun, uptb, (EPajak)pajak);
        //        string json = JsonConvert.SerializeObject(data);
        //        return Content(json, "application/json");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
        //[HttpGet]
        //public ActionResult GetRealisasiAllPajak(string tanggal, string username, string password, bool? withDenda = false)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        var json = new Models.AksiSembilanAPI.GetDataRealisasiPajakAPI(tglCutOff, tglRealisasi, withDenda);

        //        return Json(json);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
        //[HttpGet]
        //public ActionResult GetRealisasiAllPajakTesting(string tanggal, string username, string password, bool? withDenda = false)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        int tahun = Convert.ToInt32(Convert.ToDateTime(tanggal).Year);
        //        DateTime tglCutOff = new DateTime(tahun, 1, 1);
        //        DateTime tglRealisasi = DateTime.ParseExact(tanggal, "yyyy-MM-dd", new CultureInfo("id-ID"));
        //        var json = new Models.AksiSembilanAPI.GetDataRealisasiPajakAPITesting(tglCutOff, tglRealisasi, withDenda);

        //        return Json(json);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //}
        //[HttpGet]
        //public ActionResult GetRealisasiUPTB2024(string username, string password)
        //{
        //    if (username == "BimoUW!API22023" && password == "!Esidatra2023!")
        //    {
        //        var data = new Models.AksiSembilanAPI.GetDataUPTBSetahun();
        //        return Json(data);
        //    }
        //    else
        //    {
        //        return Json(new { });
        //    }
        //}
        //public ActionResult GetPotensiParkir(string username, string password, string wilayah)
        //{
        //    if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
        //    {
        //        var data = new Models.KartuDataVM.GetPotensiParkir(wilayah);
        //        return Json(data);
        //    }
        //    else
        //    {
        //        return Json(new { });
        //    }
        //}
        //public ActionResult GetPotensiRestoran(string username, string password, string wilayah)
        //{
        //    if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
        //    {
        //        var data = new Models.KartuDataVM.GetPotensiRestoran(wilayah);
        //        return Json(data);
        //    }
        //    else
        //    {
        //        return Json(new { });
        //    }
        //}
        //public ActionResult GetPotensiHiburanBioskop(string username, string password, string wilayah)
        //{
        //    if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
        //    {
        //        var data = new Models.KartuDataVM.GetPotensiHiburanBioskop(wilayah);
        //        return Json(data);
        //    }
        //    else
        //    {
        //        return Json(new { });
        //    }
        //}
        //public ActionResult GetPotensiHiburanNonBioskop(string username, string password, string wilayah)
        //{
        //    if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
        //    {
        //        var data = new Models.KartuDataVM.GetPotensiHiburanNonBioskop(wilayah);
        //        return Json(data);
        //    }
        //    else
        //    {
        //        return Json(new { });
        //    }
        //}
        [HttpGet]
        public IActionResult GetKartuData(string username, string password, string nop, int tahun, int tahun2, int jenis)
        {
            try
            {
                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    var data = Models.ApiVM.Method.GetKartuDataData(_connectionString, nop, tahun, tahun2, jenis);
                    return Json(data);
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
        public ActionResult GetPenagihanData(string username, string password, int tahun, int tahun2, int jenis)
        {
            try
            {
                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    var data = Models.ApiVM.Method.GetPenagihanData(_connectionString, tahun, tahun2, jenis);
                    string json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
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
        public ActionResult GetPenagihanDataByNOP(string username, string password, int tahun, int tahun2, int jenis, string nop)
        {
            try
            {
                if (username == "SBYTAX!API22024" && password == "!SBYTAX2024!")
                {
                    var data = Models.ApiVM.Method.GetPenagihanData(_connectionString, tahun, tahun2, jenis, nop);
                    string json = JsonConvert.SerializeObject(data);
                    return Content(json, "application/json");
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
