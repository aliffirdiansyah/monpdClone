using DevExpress.DataAccess.Native.Web;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.MonitoringGlobal;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.MonitoringGlobal
{
    public class MonitoringBulananController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringBulananController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public MonitoringBulananController(ILogger<MonitoringBulananController> logger)
        {
            URLView = string.Concat("../MonitoringGlobal/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index(int? jenisPajak)
        {
            try
            {
                ViewData["Title"] = controllerName;
                if (jenisPajak != null)
                {
                    var model = new Models.MonitoringGlobal.MonitoringBulananVM.Index((EnumFactory.EPajak)jenisPajak);
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.MonitoringGlobal.MonitoringBulananVM.Index();
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
                // Teruskan ID jenisPajak ke View melalui ViewData
                ViewData["JenisPajakId"] = jenisPajak;
                ViewData["Tahun"] = tahun; // Kita juga teruskan tahun utama

                var model = new Models.MonitoringGlobal.MonitoringBulananVM.Show((EnumFactory.EPajak)jenisPajak, tahun);
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
            // Panggil method yang sudah ada untuk mengambil data realisasi
            var data = MonitoringBulananVM.Method.GetBulananPajak((EnumFactory.EPajak)jenisPajak, tahun);
            var result = DataSourceLoader.Load(data, loadOptions);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetAkumulasiTahunan(int jenisPajak, int tahun, DataSourceLoadOptions loadOptions)
        {
            // Panggil method yang sudah ada untuk mengambil data akumulasi
            var data = MonitoringBulananVM.Method.GetBulananPajakAkumulasi((EnumFactory.EPajak)jenisPajak, tahun);
            var result = DataSourceLoader.Load(data, loadOptions);
            return Json(result);
        }
    }
}
