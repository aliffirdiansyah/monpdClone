using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM;

namespace MonPDReborn.Controllers.AnalisisTren
{
    public class KontrolPrediksiController : Controller
    {
        private readonly ILogger<KontrolPrediksiController> _logger;
        private readonly string URLView;

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public KontrolPrediksiController(ILogger<KontrolPrediksiController> logger)
        {
            _logger = logger;
            URLView = "../AnalisisTren/KontrolPrediksi/";
        }

        public IActionResult Index()
        {
            try
            {
                var model = new MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM.Index();

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
                var model = new Show(); // Message: Hello World from Show!
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
