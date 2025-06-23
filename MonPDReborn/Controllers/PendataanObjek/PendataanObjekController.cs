using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.PendataanObjek
{
    public class PendataanObjekController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PendataanObjekController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public PendataanObjekController(ILogger<PendataanObjekController> logger)
        {
            URLView = string.Concat("../Aktivitas/PendataanObjek/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new Models.PendataanObjek.PendataanObjek.Index();
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
                var model = new Models.PendataanObjek.PendataanObjek.Show(keyword);
                return PartialView("~/Views/Aktivitas/PendataanObjek/_Show.cshtml", model);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(string keyword)
        {
            try
            {
                var model = new Models.PendataanObjek.PendataanObjek.Detail(keyword);
                return PartialView("~/Views/Aktivitas/PendataanObjek/_Detail.cshtml", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
