using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.DataOP;
using MonPDReborn.Lib.General;
using System.Globalization;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.KontrolPembayaran
{
    public class PembayaranKontrolController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PembayaranKontrolController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PembayaranKontrolController(ILogger<PembayaranKontrolController> logger)
        {
            URLView = string.Concat("../KontrolPembayaran/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Dashboard Profil Objek Pajak";
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                if (!nama.Contains("BKP"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.KontrolPembayaran.PembayaranKontrolVM.Index();
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
        public IActionResult Show(string tanggalAwal, string tanggalAkhir, string rekening)
        {
            try
            {
                var tglAwalDate = DateTime.ParseExact(tanggalAwal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var tglAkhirDate = DateTime.ParseExact(tanggalAkhir, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var enumRekening = (EnumFactory.EBankRekening)Convert.ToInt32(rekening);

                var model = new Models.KontrolPembayaran.PembayaranKontrolVM.Show(enumRekening, tglAwalDate, tglAkhirDate);
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

        public IActionResult Detail(int enumPajak)
        {
            try
            {
                var model = new Models.KontrolPembayaran.PembayaranKontrolVM.Detail((EnumFactory.EPajak)enumPajak);
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
