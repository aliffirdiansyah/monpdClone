using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Controllers.ReklamePublic;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Setting
{
    public class CekWSController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<CekWSController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        private IConfiguration configuration;
        public CekWSController(ILogger<CekWSController> logger, IConfiguration configuration)
        {
            URLView = string.Concat("../Setting/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.ReklamePublic.ReklamePublicVM.Index();

                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
    }
}
