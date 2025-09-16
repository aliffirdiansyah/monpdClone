using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.MonitoringGlobal;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.MonitoringGlobal;
using MonPDReborn.Models.MonitoringGlobalUPTB;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.MonitoringGlobalUPTB
{
    public class MonitoringBulananUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringBulananUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public MonitoringBulananUPTBController(ILogger<MonitoringBulananUPTBController> logger)
        {
            URLView = string.Concat("../MonitoringGlobalUPTB/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index(int? jenisPajak)
        {
            try
            {
                ViewData["Title"] = controllerName;
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("UPTB"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                
                string lastPart = nama.Split(' ').Last();

                if (!int.TryParse(lastPart, out int wilayah))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                if (jenisPajak != null)
                {
                    var model = new Models.MonitoringGlobalUPTB.MonitoringBulananUPTBVM.Index((EnumFactory.EPajak)jenisPajak);
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.MonitoringGlobalUPTB.MonitoringBulananUPTBVM.Index();
                    return View($"{URLView}{actionName}", model);
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
        public IActionResult Show(int jenisPajak, int tahun)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                // Teruskan ID jenisPajak ke View melalui ViewData
                ViewData["JenisPajakId"] = jenisPajak;
                ViewData["Tahun"] = tahun; // Kita juga teruskan tahun utama

                var model = new Models.MonitoringGlobalUPTB.MonitoringBulananUPTBVM.Show((EnumFactory.EPajak)jenisPajak, wilayah, tahun);
                return PartialView($"{URLView}_{actionName}", model);
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


        //test
        // Tambahkan dua action ini di dalam MonitoringBulananController.cs

        [HttpGet]
        public IActionResult GetRealisasiTahunan(int jenisPajak, int tahun, DataSourceLoadOptions loadOptions)
        {
            var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
            if (string.IsNullOrEmpty(nama))
            {
                throw new ArgumentException("Session tidak ditemukan dalam sesi.");
            }
            int wilayah = int.Parse(nama.Split(' ').Last());

            // Panggil method yang sudah ada untuk mengambil data realisasi
            var data = MonitoringBulananUPTBVM.Method.GetDataBulananPajak((EnumFactory.EPajak)jenisPajak, wilayah, tahun);
            var result = DataSourceLoader.Load(data, loadOptions);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetAkumulasiTahunan(int jenisPajak, int tahun, DataSourceLoadOptions loadOptions)
        {
            var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
            if (string.IsNullOrEmpty(nama))
            {
                throw new ArgumentException("Session tidak ditemukan dalam sesi.");
            }
            int wilayah = int.Parse(nama.Split(' ').Last());

            // Panggil method yang sudah ada untuk mengambil data akumulasi
            var data = MonitoringBulananUPTBVM.Method.GetBulananPajakAkumulasi((EnumFactory.EPajak)jenisPajak, wilayah, tahun);
            var result = DataSourceLoader.Load(data, loadOptions);
            return Json(result);
        }
    }
}
