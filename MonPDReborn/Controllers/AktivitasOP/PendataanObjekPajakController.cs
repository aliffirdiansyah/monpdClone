using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.AktivitasOP;
using System;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class PendataanObjekPajakController : Controller
    {
        private readonly ILogger<PendataanObjekPajakController> _logger;
        ResponseBase response = new ResponseBase();


        private string URLView => $"../AktivitasOP/{nameof(PendataanObjekPajakController).Replace("Controller", "")}/";

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public PendataanObjekPajakController(ILogger<PendataanObjekPajakController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.PendataanObjekPajakVM.Index();
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

        public IActionResult Show()
        {
            try
            {
                var model = new PendataanObjekPajakVM.Show();
                return PartialView($"{URLView}_Show", model);
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

        // ✅ Versi baru: menerima jenisPajak dan tahun
        public IActionResult Detail(int jenisPajak)
        {
            try
            {
                var model = new PendataanObjekPajakVM.Detail((EnumFactory.EPajak)jenisPajak);
                return PartialView($"{URLView}_Detail", model);
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

        public IActionResult SubDetail(int jenisPajak, string nop)
        {
            try
            {
                var model = new PendataanObjekPajakVM.SubDetail((EnumFactory.EPajak)jenisPajak, nop);
                return PartialView($"{URLView}_SubDetail", model);
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
