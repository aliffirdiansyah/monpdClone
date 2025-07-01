using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.MonitoringGlobal
{
    public class MonitoringHarianController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringHarianController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public MonitoringHarianController(ILogger<MonitoringHarianController> logger)
        {
            URLView = string.Concat("../MonitoringGlobal/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.MonitoringGlobal.MonitoringHarianVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public IActionResult Show(DateTime tglCutOff)
        //{
        //    //try
        //    //{
        //    //    var model = new Models.MonitoringGlobal.MonitoringHarianVM.Show(tglCutOff);
        //    //    return PartialView($"{URLView}_{actionName}", model);
        //    //}
        //    //catch (Exception)
        //    //{

        //    //    throw;
        //    //}
        //}
    }
}
