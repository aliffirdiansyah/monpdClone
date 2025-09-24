using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.DataOP.ProfileOPUPTBVM;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileOPUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileOPUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfileOPUPTBController(ILogger<ProfileOPUPTBController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
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

                string lastPart = nama.Split(' ').Last();

                if (!int.TryParse(lastPart, out int wilayah))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                // simpan nomor uptb ke ViewData
                ViewData["UptbNumber"] = lastPart;

                var model = new Models.DataOP.ProfileOPUPTBVM.Index();
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

        public object GetDetailPerWilayah(DataSourceLoadOptions load_options, int JenisPajak, string uptb, string kec, string kel)
        {
            var data = Models.DataOP.ProfileOPUPTBVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel);
            return DataSourceLoader.Load(data, load_options);
        }

        [HttpGet]
        public object GetDetailPerWilayahHotel(DataSourceLoadOptions load_options, int JenisPajak, string kec, string kel)
        {
            var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA)?.ToString();
            if (string.IsNullOrEmpty(nama))
                throw new ArgumentException("Session tidak ditemukan dalam sesi.");

            var uptb = nama.Split(' ').Last(); // ambil UPTB dari session

            var data = Models.DataOP.ProfileOPUPTBVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel, true);
            return DataSourceLoader.Load(data, load_options);
        }

        public async Task<IActionResult> RekapPerWilayahAsync(string kec, string kel)
        {
            try
            {

                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA)?.ToString();
                if (string.IsNullOrEmpty(nama))
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");

                var uptb = nama.Split(' ').Last(); // ambil UPTB dari session


                var model = new Models.DataOP.ProfileOPUPTBVM.RekapPerWilayah(uptb, kec, kel);
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
        public async Task<object> GetKec(DataSourceLoadOptions loadOptions)
        {   

            var context = DBClass.GetContext();

            var dataList = new List<kecamatanView>();

            var namaUptb = HttpContext.Session.GetString(Utility.SESSION_NAMA);

            if (!string.IsNullOrEmpty(namaUptb))
            {
                string uptb = namaUptb.Split(' ').Last();
                dataList = context.MWilayahs
                    .Where(x => x.Uptd == uptb && !string.IsNullOrEmpty(x.KdKecamatan))
                    .GroupBy(x => new { x.KdKecamatan, x.NmKecamatan })
                    .Select(g => new kecamatanView
                    {
                        Value = g.Key.KdKecamatan,
                        Text = g.Key.NmKecamatan
                    })
                    .ToList();

                dataList.Add(new kecamatanView { Value = "000", Text = "-- Semua Kecamatan --" });
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
        public IActionResult DetailWilayahOP(int JenisPajak, int kategori, string kec, string kel)
        {
            try
            {
                // uptb langsung dari session, bukan dari JS
                var namaUptb = HttpContext.Session.GetString(Utility.SESSION_NAMA);
                string uptb = namaUptb?.Split(' ').Last();

                Console.WriteLine($"[DEBUG] namaUptb: {namaUptb}, uptb: {uptb}, kec: {kec}, kel: {kel}");

                var jenisPajak = (EnumFactory.EPajak)JenisPajak;
                var filtered = Models.DataOP.ProfileOPUPTBVM.Method.GetDataRekapPerWilayahDetailOPList(jenisPajak, kategori, uptb, kec, kel);

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
