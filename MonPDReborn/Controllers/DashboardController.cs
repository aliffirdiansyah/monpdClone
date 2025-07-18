using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers
{
    public class DashboardController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<DashboardController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";

        const string INPUTPENDATAAN_ERROR_MESSAGE = "INPUTPENDATAAN_ERROR_MESSAGE";
        public DashboardController(ILogger<DashboardController> logger)
        {
            URLView = string.Concat("../", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return RedirectToAction("Index", "Dashboard", new { em = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return RedirectToAction("Index", "Dashboard", new { em = response.InternalServerErrorMessage });
            }
        }
        public IActionResult SeriesPajakDaerah()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.SeriesPajakDaerah();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult JumlahObjekPajakTahunan()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajakTahunan();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult JumlahObjekPajakSeries()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajakSeries();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemasanganAlatRekamDetail()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemasanganAlatRekamDetail();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemasanganAlatRekamSeries()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemasanganAlatRekamSeries();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemeriksaanPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemeriksaanPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PengedokanPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PengedokanPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult DataKontrolPotensiOp()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.DataKontrolPotensiOp();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult DataPiutang()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.DataPiutang();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
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
                var model = new Models.DashboardVM.DataMutasi();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }

        public IActionResult JumlahObjekPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
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
                var model = new Models.DashboardVM.DetailPiutang((EnumFactory.EPajak)jenisPajak, tahun);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
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