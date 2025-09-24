using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.Aktivitas;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.AktivitasOP
{
    public class PemasanganAlatUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PemasanganAlatUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public PemasanganAlatUPTBController(ILogger<PemasanganAlatUPTBController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
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

                if (!nama.Contains("UPTB"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.AktivitasOP.PemasanganAlatUPTBVM.Index();
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
        public IActionResult Show()
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                var model = new Models.AktivitasOP.PemasanganAlatUPTBVM.Show(wilayah);
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
        [HttpGet]
        public object GetDetailSeries(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
            if (string.IsNullOrEmpty(nama))
            {
                throw new ArgumentException("Session tidak ditemukan dalam sesi.");
            }
            int wilayah = int.Parse(nama.Split(' ').Last());

            var data = Models.AktivitasOP.PemasanganAlatUPTBVM.Method.GetDetailSeriesAlatRekam((EnumFactory.EPajak)JenisPajak, wilayah);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult Tahunan()
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                var model = new Models.AktivitasOP.PemasanganAlatUPTBVM.Tahunan(wilayah);
                return PartialView($"{URLView}_Show{actionName}", model);
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
        [HttpGet]
        public object GetDetailTahunan(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
            if (string.IsNullOrEmpty(nama))
            {
                throw new ArgumentException("Session tidak ditemukan dalam sesi.");
            }
            int wilayah = int.Parse(nama.Split(' ').Last());

            var data = Models.AktivitasOP.PemasanganAlatUPTBVM.Method.GetDetailTahunBerjalan((EnumFactory.EPajak)JenisPajak, wilayah);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult SubDetailOPModal(int jenisPajak, int kategori, int tahun)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                var model = new Models.AktivitasOP.PemasanganAlatUPTBVM.DetailOP(jenisPajak, kategori, tahun, wilayah);
                return PartialView("../AktivitasOP/PemasanganAlatUPTB/_SubDetailModal", model);
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

        public IActionResult SubDetailDataModal(int jenisPajak, int kategori, int status)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int wilayah = int.Parse(nama.Split(' ').Last());

                var model = new Models.AktivitasOP.PemasanganAlatUPTBVM.DetailTahun(jenisPajak, kategori, status, wilayah);
                return PartialView("../AktivitasOP/PemasanganAlatUPTB/_SubDetailDataModal", model);
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
