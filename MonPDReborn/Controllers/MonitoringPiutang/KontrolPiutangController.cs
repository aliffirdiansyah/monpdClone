using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.MonitoringPiutang
{
    public class KontrolPiutangController : BaseController
    {
        string URLView = string.Empty;
        private readonly ILogger<KontrolPiutangController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";

        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";

        ResponseBase response = new ResponseBase();

        public KontrolPiutangController(ILogger<KontrolPiutangController> logger)
        {
            URLView = string.Concat("../MonitoringPiutang/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.MonitoringPiutang.KontrolPiutangVM.Index();
                return View($"{URLView}{actionName}", model);
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

        public IActionResult DataPiutang()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.MonitoringPiutang.KontrolPiutangVM.DataPiutang();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[MONITORING_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult DataMutasi()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.MonitoringPiutang.KontrolPiutangVM.DataMutasi();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[MONITORING_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }

        public IActionResult DetailPiutang(int jenisPajak, int tahun)
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.MonitoringPiutang.KontrolPiutangVM.DetailPiutang((EnumFactory.EPajak)jenisPajak, tahun);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[MONITORING_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }

    }
}
