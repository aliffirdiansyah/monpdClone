using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.KontrolPBB
{
    public class StrategicViewController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<StrategicViewController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public StrategicViewController(ILogger<StrategicViewController> logger)
        {
            URLView = string.Concat("../KontrolPBB/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Dashboard Strategi";
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.KontrolPBB.StrategicViewVM.Index();
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult ShowCapaian()
        {
            try
            {
                var model = new Models.KontrolPBB.StrategicViewVM.ShowCapaian();
                return PartialView($"{URLView}_{actionName}", model);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult ShowKategori()
        {
            try
            {
                var model = new Models.KontrolPBB.StrategicViewVM.ShowKategori();
                return PartialView($"{URLView}_{actionName}", model);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult ShowTunggakan()
        {
            try
            {
                var model = new Models.KontrolPBB.StrategicViewVM.ShowTunggakan();
                return PartialView($"{URLView}_{actionName}", model);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        
    }
}
