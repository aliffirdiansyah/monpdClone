using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfilePotensiOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfilePotensiOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ProfilePotensiOPController(ILogger<ProfilePotensiOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfilePotensiOPVM.Index();
                return View($"{URLView}{actionName}", model);
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
                var model = new Models.DataOP.ProfilePotensiOPVM.Show(keyword);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Detail(string nop)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.Detail(nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
