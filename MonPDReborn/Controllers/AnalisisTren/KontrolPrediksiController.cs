using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonPDReborn.Models.AktivitasOP;
using MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM;

namespace MonPDReborn.Controllers.AnalisisTren
{
    public class KontrolPrediksiController : Controller
    {
        private readonly string URLView;
        private readonly ILogger<KontrolPrediksiController> _logger;

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";

        public KontrolPrediksiController(ILogger<KontrolPrediksiController> logger)
        {
            URLView = string.Concat("../AnalisisTren/", nameof(KontrolPrediksiController).Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new 
                {
                    Keyword = HttpContext.Session.GetString(TD_KEY) ?? string.Empty
                };
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Index()");
                return Content("SERVER ERROR: " + ex.Message);
            }
        }

        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new Models.AnalisisTren.KontrolPrediksiVM.Show(keyword)
                {
                    Data = new List<Models.AnalisisTren.KontrolPrediksiVM.DataRealisasiPrediksi>
                    {
                        new() { JenisPajak = "Hotel", Target = 1000000000, Realisasi = 800000000, Prediksi = 950000000, Potensi = 1200000000, Tren = Models.AnalisisTren.KontrolPrediksiVM.TrendStatus.Naik, Aksi = "<button class='btn btn-sm btn-primary'>Detail</button>" },
                        new() { JenisPajak = "Restoran", Target = 1200000000, Realisasi = 700000000, Prediksi = 850000000, Potensi = 1300000000, Tren = Models.AnalisisTren.KontrolPrediksiVM.TrendStatus.Turun, Aksi = "<button class='btn btn-sm btn-primary'>Detail</button>" }
                    }
                };

                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Show()");
                return Content("SERVER ERROR: " + ex.Message);
            }
        }
    }
}
