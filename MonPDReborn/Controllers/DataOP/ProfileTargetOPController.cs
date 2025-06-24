using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileTargetOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileTargetOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ProfileTargetOPController(ILogger<ProfileTargetOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfileTargetOPVM.Index();
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
                var model = new Models.DataOP.ProfileTargetOPVM.Show(keyword);
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
                var model = new Models.DataOP.ProfileTargetOPVM.Detail(nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
