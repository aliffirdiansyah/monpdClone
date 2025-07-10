using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new PendataanObjekPajakVM.Show(keyword);
                return PartialView($"{URLView}_Show", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data Show");
                return BadRequest("Terjadi kesalahan saat memuat data.");
            }
        }

        // ✅ Versi baru: menerima jenisPajak dan tahun
        public IActionResult Detail(string jenisPajak, int tahun)
        {
            try
            {
                var model = new PendataanObjekPajakVM.Detail(jenisPajak, tahun);
                return PartialView($"{URLView}_Detail", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data Detail");
                return BadRequest("Terjadi kesalahan saat memuat data detail.");
            }
        }
    }
}
