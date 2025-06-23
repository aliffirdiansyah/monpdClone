using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileSpasialOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileSpasialOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ProfileSpasialOPController(ILogger<ProfileSpasialOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var model = new Models.DataOP.ProfileSpasialOPVM.Index();
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
                var model = new Models.DataOP.ProfileSpasialOPVM.Show(keyword);
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
                var model = new Models.DataOP.ProfileSpasialOPVM.Detail(nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
