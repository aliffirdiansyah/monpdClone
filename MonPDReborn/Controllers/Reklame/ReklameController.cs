using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Reklame
{
    public class ReklameController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ReklameController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public ReklameController(ILogger<ReklameController> logger)
        {
            URLView = string.Concat("../Reklame/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.Reklame.ReklameVM.Index();
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
                //// Parsing string ke DateTime (format: yyyy-MM-dd)
                //if (!DateTime.TryParse(tglAwal, out var tanggalAwal))
                //    throw new ArgumentException("Format Tanggal Awal tidak valid.");

                //if (!DateTime.TryParse(tglAkhir, out var tanggalAkhir))
                //    throw new ArgumentException("Format Tanggal Akhir tidak valid.");

                //var input = new Models.Reklame.ReklameVM.Index
                //{
                //    TglAwal = tanggalAwal.AddDays(1),
                //    TglAkhir = tanggalAkhir
                //};

                var model = new Models.Reklame.ReklameVM.ShowData();
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
        public IActionResult Detail(string kelasJalan, string namaJalan, string status, string jenisReklame, string tglAwal, string tglAkhir)
        {
            try
            {
                // Parsing string ke DateTime (format: yyyy-MM-dd)
                if (!DateTime.TryParse(tglAwal, out var tanggalAwal))
                    throw new ArgumentException("Format Tanggal Awal tidak valid.");

                if (!DateTime.TryParse(tglAkhir, out var tanggalAkhir))
                    throw new ArgumentException("Format Tanggal Akhir tidak valid.");

                if (string.IsNullOrWhiteSpace(kelasJalan) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(namaJalan))
                    return BadRequest("Invalid parameters.");

                var model = new Models.Reklame.ReklameVM.DetailReklame(kelasJalan, namaJalan, status, jenisReklame, tanggalAwal.AddDays(1), tanggalAkhir);
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
