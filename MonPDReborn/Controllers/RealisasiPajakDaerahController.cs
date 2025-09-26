using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers
{
    public class RealisasiPajakDaerahController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<RealisasiPajakDaerahController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";

        const string INPUTPENDATAAN_ERROR_MESSAGE = "INPUTPENDATAAN_ERROR_MESSAGE";
        public RealisasiPajakDaerahController(ILogger<RealisasiPajakDaerahController> logger)
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

                if (!nama.Contains("BAPENDA"))
                {
                    if (nama.Contains("BPK"))
                    {

                    }
                    else
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = 403 });
                    }
                }
                // ini diganti
                var model = new Models.RealisasiPajakDaerahVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                // ini diganti
                var model = new Models.RealisasiPajakDaerahVM.Index("Error, Pada Saat GetData");
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                ViewBag.ErrorMessage = "Terjadi kesalahan sistem. Silakan coba lagi.";
                // ini diganti
                var model = new Models.RealisasiPajakDaerahVM.Index("Terjadi kesalahan sistem. Silakan coba lagi.");
                return View($"{URLView}{actionName}", model);
            }
        }
    }
}
