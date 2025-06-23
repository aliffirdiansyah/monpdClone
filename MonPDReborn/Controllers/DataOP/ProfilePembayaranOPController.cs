using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfilePembayaranOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfilePembayaranOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ProfilePembayaranOPController(ILogger<ProfilePembayaranOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var model = new Models.DataOP.ProfilePembayaranOPVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
