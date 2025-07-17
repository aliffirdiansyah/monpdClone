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
    public class SeriesPendapatanDaerahController : Controller
    {
        private readonly ILogger<SeriesPendapatanDaerahController> _logger;
        ResponseBase response = new ResponseBase();


        private string URLView => $"../AktivitasOP/{nameof(SeriesPendapatanDaerahController).Replace("Controller", "")}/";

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public SeriesPendapatanDaerahController(ILogger<SeriesPendapatanDaerahController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.SeriesPendapatanDaerahVM.Index();
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
                var model = new SeriesPendapatanDaerahVM.Show();
                return PartialView($"{URLView}_ShowPendapatanAsli", model);
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

        public IActionResult ShowPendapatanTransfer()
        {
            try
            {
                var model = new SeriesPendapatanDaerahVM.ShowPendapatanTransfer();
                return PartialView($"{URLView}_ShowPendapatanTransfer", model);
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

        public IActionResult ShowPenerimaanPembiayaan()
        {
            try
            {
                var model = new SeriesPendapatanDaerahVM.ShowPenerimaanPembiayaan();
                return PartialView($"{URLView}_ShowPenerimaanPembiayaan", model);
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
