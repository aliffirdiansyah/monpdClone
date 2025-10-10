using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.KontrolPBB
{
    public class PencarianPBBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PencarianPBBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PencarianPBBController(ILogger<PencarianPBBController> logger)
        {
            URLView = string.Concat("../KontrolPBB/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index(string? keyword)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                if (nama.Contains("BPK"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                ViewData["Title"] = controllerName;
                if (string.IsNullOrEmpty(keyword))
                {
                    var model = new Models.KontrolPBB.PencarianPBBVM.Index();
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.KontrolPBB.PencarianPBBVM.Index(keyword);
                    return View($"{URLView}{actionName}", model);
                }

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
        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new Models.KontrolPBB.PencarianPBBVM.Show(keyword);
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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult Detail(string nop)
        {
            try
            {
                var model = new Models.KontrolPBB.PencarianPBBVM.Detail(nop);
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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
    }
}
