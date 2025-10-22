using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Controllers.ReklamePublic;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Reklame
{
    public class KalkulatorReklameController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<KalkulatorReklameController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        private IConfiguration configuration;
        public KalkulatorReklameController(ILogger<KalkulatorReklameController> logger, IConfiguration configuration)
        {
            URLView = string.Concat("../Reklame/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.Reklame.KalkulatorReklameVM.Index();

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
        public IActionResult Show(MonPDLib.Lib.KalkulatorReklamePermanen.ReklameInput input)
        {
            try
            {
                var model = new Models.Reklame.KalkulatorReklameVM.Show(input);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException e)
            {
                response.Status = ResponseBase.StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Status = ResponseBase.StatusEnum.Error;
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult ShowKontrak(decimal NilaiKontrak)
        {
            try
            {
                var model = new Models.Reklame.KalkulatorReklameVM.ShowKontrak(NilaiKontrak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException e)
            {
                response.Status = ResponseBase.StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Status = ResponseBase.StatusEnum.Error;
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
    }
}
