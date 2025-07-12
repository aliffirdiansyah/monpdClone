using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class ReklameSummaryController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ReklameSummaryController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ReklameSummaryController(ILogger<ReklameSummaryController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.ReklameSummaryVM.Index();
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
        public IActionResult Show(int tahun)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.Show(tahun);
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
        // Detail Reklame Permanen
        public IActionResult DetailPermanenJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailPermanenJT(tahun, bulan, skpdBlmJT);
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

        public IActionResult DetailPermanenBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailPermanenBP(tahun, bulan, skpdBlmPanjang);
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

        public IActionResult DetailPermanenKB(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailPermanenKB(tahun, bulan, skpdBlmPanjang);
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

        //Detail Reklame Terbatas

        public IActionResult DetailTerbatasJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailTerbatasJT(tahun, bulan, skpdBlmJT);
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

        public IActionResult DetailTerbatasBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailTerbatasBP(tahun, bulan, skpdBlmPanjang);
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

        public IActionResult DetailTerbatasKB(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailTerbatasKB(tahun, bulan, skpdBlmPanjang);
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

        // Detail Reklame Ketetapan Baru

        public IActionResult DetailIsidentilKB(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailIsidentilKB(tahun, bulan, skpdBlmPanjang);
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
