using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.MonitoringGlobal;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.MonitoringGlobal;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.MonitoringGlobalUPTB
{
    public class MonitoringHarianUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringHarianUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ResponseBase response = new ResponseBase();
        public MonitoringHarianUPTBController(ILogger<MonitoringHarianUPTBController> logger)
        {
            URLView = string.Concat("../MonitoringGlobalUPTB/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index(int? jenisPajak, int? tahun, int? bulan)
        {
            try
            {
                ViewData["Title"] = controllerName;
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                string lastPart = nama.Split(' ').Last();

                if (!int.TryParse(lastPart, out int wilayah))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                if (jenisPajak == null || bulan == null || tahun == null)
                {
                    var model = new Models.MonitoringGlobalUPTB.MonitoringHarianUPTBVM.Index();
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.MonitoringGlobalUPTB.MonitoringHarianUPTBVM.Index((EnumFactory.EPajak)jenisPajak, tahun.Value, bulan.Value);
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

        public IActionResult Show(int jenisPajak, int bulan, int tahun)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                var model = new Models.MonitoringGlobalUPTB.MonitoringHarianUPTBVM.Show(jenisPajak, wilayah, tahun, bulan);
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

        [HttpGet]
        public IActionResult GetMonitoringData(int jenisPajak, int tahun, int bulan, DataSourceLoadOptions loadOptions)
        {
            // Panggil method dari ViewModel dengan meneruskan parameter filter
            var filteredData = MonitoringHarianVM.Method.GetMonitoringHarianList((EnumFactory.EPajak)jenisPajak, tahun, bulan);

            // Proses data menggunakan DevExtreme loader dan kembalikan sebagai JSON
            var result = DataSourceLoader.Load(filteredData, loadOptions);
            return Json(result);
        }
    }
}
