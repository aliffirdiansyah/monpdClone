using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers.KontrolPBB
{
    public class AnalitikViewController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<AnalitikViewController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public AnalitikViewController(ILogger<AnalitikViewController> logger)
        {
            URLView = string.Concat("../KontrolPBB/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Dashboard Analitik";
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.KontrolPBB.AnalitikViewVM.Index();
                return View($"{URLView}{actionName}", model);
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
        public IActionResult ShowWilayah(int wilayah, int kec)
        {
            try
            {
                var model = new Models.KontrolPBB.AnalitikViewVM.ShowWilayah(wilayah, kec);
                return PartialView($"{URLView}{actionName}", model);
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
        public IActionResult ShowSegmentasi(int wilayah, int kec)
        {
            try
            {
                var model = new Models.KontrolPBB.AnalitikViewVM.ShowSegmentasi(wilayah, kec);
                return PartialView($"{URLView}{actionName}", model);
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
