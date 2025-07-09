using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;

namespace MonPDReborn.Controllers.DataOP
{
    public class PelaporanOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PelaporanOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public PelaporanOPController(ILogger<PelaporanOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.PelaporanOPVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Show(int jenisPajak)
        {
            try
            {
                var model = new Models.DataOP.PelaporanOPVM.Show((EnumFactory.EPajak)jenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Detail(int jenisPajak, string nop)
        {
            try
            {
                var model = new Models.DataOP.PelaporanOPVM.Detail((EnumFactory.EPajak)jenisPajak, nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
