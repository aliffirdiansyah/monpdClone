using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.AktivitasOP;
using static MonPDReborn.Models.DataOP.ProfileOPVM;
using MonPDReborn.Models.MonitoringGlobal;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileOPController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public ProfileOPController(ILogger<ProfileOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
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
                var model = new Models.DataOP.ProfileOPVM.Index();
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
        public IActionResult ShowRekap(string keyword, int? tahun)
        {
            int finalTahun = tahun ?? DateTime.Now.Year;

            try
            {
                var model = new MonPDReborn.Models.DataOP.ProfileOPVM.ShowRekap(finalTahun);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult ShowSeries(string keyword)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.ShowSeries(keyword);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        #region REKAP DATA
        [HttpGet]
        public object GetRekapDetailData(DataSourceLoadOptions load_options, int JenisPajak, int tahun)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetRekapDetailData((EnumFactory.EPajak)JenisPajak, tahun);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult RekapMaster(int enumPajak, int kategori, string status, int tahun)
        {
            try
            {
                var jenisPajak = (EnumFactory.EPajak)enumPajak;
                var filtered = Models.DataOP.ProfileOPVM.Method.GetRekapMasterData(jenisPajak, kategori, status, tahun);

                //var filtered = allData
                //    .Where(x =>
                //        !string.IsNullOrEmpty(x.Kategori_Nama) &&
                //        !string.IsNullOrEmpty(x.Status) &&
                //        x.Kategori_Nama.Equals(kategori, StringComparison.OrdinalIgnoreCase) &&
                //        x.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                //    .ToList();

                return Json(filtered);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public IActionResult RekapDetailHotel(int enumPajak, int kategori, int tahun)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.RekapDetailHotel(enumPajak, kategori, tahun);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
        #region SERIES DATA
        [HttpGet]
        public object GetDetailJmlOPData(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetDetailJmlOPData((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult DetailOP(int enumPajak, int kategori, string status, int tahun)
        {
            try
            {
                var jenisPajak = (EnumFactory.EPajak)enumPajak;
                var filtered = Models.DataOP.ProfileOPVM.Method.GetDetailOPAllYears(jenisPajak, kategori, status, tahun);

                return Json(filtered);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public IActionResult SeriesMaster(int enumPajak, int kategori, string tahun)
        {
            try
            {
                var jenisPajak = (EnumFactory.EPajak)enumPajak;
                var filtered = Models.DataOP.ProfileOPVM.Method.GetSeriesMasterData(jenisPajak, kategori, tahun);



                return Json(filtered);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
        public IActionResult Detail(string nop, int pajak)
        {
            ViewData["Title"] = "Profile Objek Pajak";
            try
            {
                var model = new Models.DataOP.ProfileOPVM.Detail(nop, (EnumFactory.EPajak)pajak);
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
                response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return Json(response);
            }
        }
        public IActionResult JmlSeries()
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.RekapJml();
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        public IActionResult ShowTPK(int tahunKiri, int tahunKanan)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.ShowTPK(tahunKiri, tahunKanan);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        public IActionResult DetailOPSeries(EnumFactory.EPajak jenisPajak, int tahun)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.DetailOPSeries(jenisPajak, tahun);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        [HttpGet]
        public object GetDetailPerWilayah(DataSourceLoadOptions load_options, int JenisPajak, string uptb, string kec, string kel)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel);
            return DataSourceLoader.Load(data, load_options);
        }

        [HttpGet]
        public object GetDetailPerWilayahHotel(DataSourceLoadOptions load_options, int JenisPajak, string uptb, string kec, string kel)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel, true);
            return DataSourceLoader.Load(data, load_options);
        }

        public IActionResult RekapPerWilayah(string uptb, string kec, string kel)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.RekapPerWilayah(uptb, kec, kel);
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
                response.Message = "⚠️ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        [HttpGet]
        public async Task<object> GetUptb(DataSourceLoadOptions loadOptions)
        {
            var context = DBClass.GetContext();

            var dataList = context.MWilayahs
                .Where(x => !string.IsNullOrEmpty(x.Uptd))
                .GroupBy(x => x.Uptd)
                .Select(g => new uptbView
                {
                    Value = g.Key,
                    Text = "UPTB " + g.Key
                })
                .OrderBy(x => x.Text)
                .ToList();

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetKec(DataSourceLoadOptions loadOptions, string uptb)
        {
            var context = DBClass.GetContext();

            var dataList = new List<kecamatanView>();

            if (!string.IsNullOrEmpty(uptb))
            {
                dataList = context.MWilayahs
                    .Where(x => x.Uptd == uptb && !string.IsNullOrEmpty(x.KdKecamatan))
                    .GroupBy(x => new { x.KdKecamatan, x.NmKecamatan })
                    .Select(g => new kecamatanView
                    {
                        Value = g.Key.KdKecamatan,
                        Text = g.Key.NmKecamatan
                    })
                    .ToList();
            }

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetKel(DataSourceLoadOptions loadOptions, string kecamatan)
        {
            var context = DBClass.GetContext();

            var dataList = new List<kelurahanView>();

            if (!string.IsNullOrEmpty(kecamatan))
            {
                dataList = context.MWilayahs
                    .Where(x => x.KdKecamatan == kecamatan && !string.IsNullOrEmpty(x.KdKelurahan))
                    .GroupBy(x => new { x.KdKelurahan, x.NmKelurahan })
                    .Select(g => new kelurahanView
                    {
                        Value = g.Key.KdKelurahan,
                        Text = g.Key.NmKelurahan
                    })
                    .ToList();
            }

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

        public IActionResult DetailWilayahOP(int JenisPajak, int kategori, string uptb, string kec, string kel)
        {
            try
            {
                var jenisPajak = (EnumFactory.EPajak)JenisPajak;
                var filtered = Models.DataOP.ProfileOPVM.Method.GetDataRekapPerWilayahDetailOPList(jenisPajak, kategori, uptb, kec, kel);

                return Json(filtered);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
