using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.AktivitasWP
{
    public class PeneguranController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PeneguranController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PeneguranController(ILogger<PeneguranController> logger)
        {
            URLView = string.Concat("../AktivitasWP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasWP.PeneguranVM.Index();
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
        public IActionResult Show(DateTime tglCutOff)
        {
            try
            {
                var model = new Models.AktivitasWP.PeneguranVM.Show();
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
