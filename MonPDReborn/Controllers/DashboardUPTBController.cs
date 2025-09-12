using Microsoft.AspNetCore.Mvc;
using MonPDLib.EF;
using MonPDLib.General;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers
{
    public class DashboardUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<DashboardUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";

        const string INPUTPENDATAAN_ERROR_MESSAGE = "INPUTPENDATAAN_ERROR_MESSAGE";
        public DashboardUPTBController(ILogger<DashboardUPTBController> logger)
        {
            URLView = string.Concat("../", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {

            try
            {
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

                var model = new Models.DashboardUPTBVM.Index(wilayah);
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var model = new Models.DashboardUPTBVM.Index("Error, Pada Saat GetData");
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                ViewBag.ErrorMessage = "Terjadi kesalahan sistem. Silakan coba lagi.";
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
    }
}