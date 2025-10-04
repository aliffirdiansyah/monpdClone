using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.Aktivitas;
using MonPDReborn.Lib.General;
using static MonPDReborn.Models.CCTVParkir.MonitoringCCTVVM;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.CCTVParkir
{
    public class MonitoringCCTVController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringCCTVController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public MonitoringCCTVController(ILogger<MonitoringCCTVController> logger)
        {
            URLView = string.Concat("../CCTVParkir/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Index();
                return PartialView($"{URLView}{actionName}", model);
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
        public IActionResult Show(int uptb)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Show(uptb);
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
        public IActionResult Detail(string nop, int vendorId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Detail(nop, vendorId);
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
        //ini sebagai kapasistas bulanan
        public IActionResult Kapasitas(string nop, int vendorId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Kapasitas(nop, vendorId);
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
        public IActionResult KapasitasHarian(string nop, int vendorId, int tahun, int bulan)
        {
            try
            {
                var data = Method.GetMonitoringCCTVHarian(nop, vendorId, tahun, bulan);
                return Json(data);
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = ex.Message;
                return Json(response);
            }
        }
        //public IActionResult KapasitasHarianDetail(string nop, int vendorId, DateTime tgl)
        //{
        //    try
        //    {
        //        var data = Method.GetMonitoringHarianDetail(nop, vendorId, tgl);
        //        return Json(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = ex.Message;
        //        return Json(response);
        //    }
        //}
        public IActionResult KapasitasHarianDetail(string nop, int vendorId, DateTime tgl)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.KapasitasHarianDetail(nop, vendorId, tgl);

                // pastikan URLView dan actionName sesuai convention di BaseController
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
        public IActionResult DataKapasitasParkir(string nop, DateTime tanggalAwal, DateTime tanggalAkhir)
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.DataKapasitasParkir(nop, tanggalAwal, tanggalAkhir);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException e)
            {
                return Json(response.ToErrorInfoMessage(e.Message));
            }
            catch (Exception ex)
            {
                return Json(response.ToInternalServerError);
            }
        }
        public IActionResult Log(string nop, int jenisKend, DateTime tanggalAwal, DateTime tanggalAkhir)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Log(nop, jenisKend, tanggalAwal, tanggalAkhir);
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
        public IActionResult LiveStreaming(string nop, int vendorId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreaming(nop, vendorId);
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult LiveStreamingVideo(string nop, int cctvId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreamingVideo(nop, cctvId);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult LiveStreamingCounting(string nop)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreamingCounting(nop);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
