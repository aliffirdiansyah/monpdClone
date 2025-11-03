using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EFReklameSsw;
using MonPDLib.General;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers
{
    public class DashboardController : BaseController
    {
        string URLView = string.Empty;
        private readonly ILogger<DashboardController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";

        const string INPUTPENDATAAN_ERROR_MESSAGE = "INPUTPENDATAAN_ERROR_MESSAGE";
        public DashboardController(ILogger<DashboardController> logger)
        {
            URLView = string.Concat("../", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        private static ReklameSswContext _context = DBClass.GetReklameSswContext();
        public IActionResult Index()
        {

            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                string lastPart = nama.Split(' ').Last();

                if (!nama.Contains("BAPENDA"))
                {
                    if (!(nama.Contains("BPK") || nama.Contains("SEKDA")))
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = 403 });
                    }
                }
                var model = new Models.DashboardVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var model = new Models.DashboardVM.Index("Error, Pada Saat GetData");
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                ViewBag.ErrorMessage = "Terjadi kesalahan sistem. Silakan coba lagi.";
                var model = new Models.DashboardVM.Index("Terjadi kesalahan sistem. Silakan coba lagi.");
                return View($"{URLView}{actionName}", model);
            }
        }

        public IActionResult ShowCard()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.ShowCard();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult SeriesPajakDaerah()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.SeriesPajakDaerah();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        //public IActionResult DashboardLayanan()
        //{
        //    var response = new ResponseBase();
        //    try
        //    {
        //        var model = new Models.DashboardVM.DashboardLayanan();
        //        return PartialView($"{URLView}{actionName}", model);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
        //        return Json(response.ToErrorInfoMessage(ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
        //        return Json(response.ToInternalServerError());
        //    }
        //}
        [HttpGet]
        public object GetDetailLayanan(DataSourceLoadOptions load_options, int PajakId)
        {
            var data = Models.DashboardVM.Method.GetDataDashboard((EnumFactory.EPajak)PajakId);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult LayananDashboard()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.LayananDashboard();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult LayananDashboardHarian(DateTime tgl)
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.LayananHarian(tgl);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        /*public IActionResult JumlahObjekPajakTahunan()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajakTahunan();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult JumlahObjekPajakSeries()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajakSeries();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemasanganAlatRekamDetail()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemasanganAlatRekamDetail();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemasanganAlatRekamSeries()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemasanganAlatRekamSeries();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PemeriksaanPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PemeriksaanPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult PengedokanPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.PengedokanPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }
        public IActionResult DataKontrolPotensiOp()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.DataKontrolPotensiOp();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }

        public IActionResult JumlahObjekPajak()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.DashboardVM.JumlahObjekPajak();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return Json(response.ToInternalServerError());
            }
        }*/

        //public IActionResult DataPiutang()
        //{
        //    var response = new ResponseBase();
        //    try
        //    {
        //        var model = new Models.DashboardVM.DataPiutang();
        //        return PartialView($"{URLView}{actionName}", model);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
        //        return Json(response.ToErrorInfoMessage(ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
        //        return Json(response.ToInternalServerError());
        //    }
        //}
        //public IActionResult DataMutasi()
        //{
        //    var response = new ResponseBase();
        //    try
        //    {
        //        var model = new Models.DashboardVM.DataMutasi();
        //        return PartialView($"{URLView}{actionName}", model);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
        //        return Json(response.ToErrorInfoMessage(ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
        //        return Json(response.ToInternalServerError());
        //    }
        //}

        //public IActionResult DetailPiutang(int jenisPajak, int tahun)
        //{
        //    var response = new ResponseBase();
        //    try
        //    {
        //        var model = new Models.DashboardVM.DetailPiutang((EnumFactory.EPajak)jenisPajak, tahun);
        //        return PartialView($"{URLView}_{actionName}", model);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        TempData[INPUTPENDATAAN_ERROR_MESSAGE] = ex.Message;
        //        return Json(response.ToErrorInfoMessage(ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
        //        return Json(response.ToInternalServerError());
        //    }
        //}
    }
}