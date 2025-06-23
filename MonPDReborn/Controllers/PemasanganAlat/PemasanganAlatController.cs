using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.Pencarian
{
    public class PemasanganAlatController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PemasanganAlatController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public PemasanganAlatController(ILogger<PemasanganAlatController> logger)
        {
            URLView = string.Concat("../Aktivitas/PemasanganAlat/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var model = new Models.PemasanganAlat.PemasanganAlat.Index();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new Models.PemasanganAlat.PemasanganAlat.Show(keyword);
                return PartialView("~/Views/Aktivitas/PemasanganAlat/_Show.cshtml", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Detail(string JenisPajak)
        {
            try
            {
                var model = new Models.PemasanganAlat.PemasanganAlat.Detail(JenisPajak);
                return PartialView("~/Views/Aktivitas/PemasanganAlat/_Detail.cshtml", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
