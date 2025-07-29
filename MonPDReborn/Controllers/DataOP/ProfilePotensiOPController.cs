using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.DataOP;
using static MonPDReborn.Lib.General.ResponseBase;


namespace MonPDReborn.Controllers.DataOP
{
    public class ProfilePotensiOPController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfilePotensiOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfilePotensiOPController(ILogger<ProfilePotensiOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfilePotensiOPVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ShowRekap(string jenisPajak)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowRekap(jenisPajak);
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
        public object GetDetailPotensi(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.DataOP.ProfilePotensiOPVM.Method.GetDetailPotensiList((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }
        [HttpGet]
        public async Task<object> GetDataPotensi(DataSourceLoadOptions load_options, int JenisPajak, int kategori)
        {
            await Task.Delay(1000);

            var data = Models.DataOP.ProfilePotensiOPVM.Method.GetDataPotensiList(
                (EnumFactory.EPajak)JenisPajak, kategori
            );

            return DataSourceLoader.Load(data, load_options);
        }
        //public IActionResult ShowDetail(int jenisPajak)
        //{
        //    try
        //    {
        //        var model = new Models.DataOP.ProfilePotensiOPVM.ShowDetail
        //        {
        //           /* JenisPajak = jenisPajak,*/
        //            DataDetailPotensi = Models.DataOP.ProfilePotensiOPVM.Method.GetDetailPotensiList((EnumFactory.EPajak)jenisPajak)
        //        };
        //        return PartialView($"{URLView}_{actionName}", model);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
        //        return Json(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = "⚠ Server Error: Internal Server Error";
        //        return Json(response);
        //    }
        //}
        public IActionResult ShowData(int jenisPajak, int kategori)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowData((EnumFactory.EPajak)jenisPajak, kategori);

                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                // optional: log ex.Message
                throw;
            }
        }

        // Di dalam file ProfilePotensiOPController.cs

        public IActionResult Detail(string nop, string jenisPajak, string? kategoriHiburan = null)
        {
            try
            {
                switch (jenisPajak)
                {
                    case "PBJT atas Jasa Perhotelan":
                        var hotelModel = new ProfilePotensiOPVM.DetailHotel(nop);
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailHotel.cshtml", hotelModel);

                    case "PBJT atas Jasa Parkir":
                        var parkirModel = new ProfilePotensiOPVM.DetailParkir(nop);
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailParkir.cshtml", parkirModel);

                    case "PBJT atas Makanan dan/atau Minuman":
                        // Anda mungkin perlu membuat ViewModel untuk restoran juga nantinya
                        var restoranModel = new ProfilePotensiOPVM.Detail(); // Placeholder
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailRestoran.cshtml", restoranModel);

                    case "PBJT atas Jasa Kesenian dan Hiburan":
                        // DIUBAH: Panggil method ShowDetailHiburan untuk menangani semua logika hiburan
                        var hiburanModel = new ProfilePotensiOPVM.Detail(); // Kirim model dasar
                        return ShowDetailHiburan(kategoriHiburan, hiburanModel, nop); // Tambahkan nop jika perlu

                    default:
                        var defaultModel = new ProfilePotensiOPVM.Detail();
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailDefault.cshtml", defaultModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error di Action Detail");
                return Content("Terjadi kesalahan di server.");
            }
        }

        // Ubah juga method ShowDetailHiburan untuk lebih fleksibel
        private IActionResult ShowDetailHiburan(string? kategori, object detailModel, string nop)
        {
            if (string.IsNullOrEmpty(kategori))
            {
                // Tampilkan halaman hiburan umum jika tidak ada kategori
                return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);
            }

            switch (kategori.ToLower())
            {
                case "bioskop":
                    var bioskopModel = new ProfilePotensiOPVM.DetailBioskop(nop);
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailBioskop.cshtml", bioskopModel);

                case "massage":
                    // var massageModel = new ProfilePotensiOPVM.DetailMassage(nop);
                    // return View("~/Views/DataOP/ProfilePotensiOP/DetailMassage.cshtml", massageModel);
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailMassage.cshtml", detailModel);


                case "gym":
                    // var gymModel = new ProfilePotensiOPVM.DetailGym(nop);
                    // return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", gymModel);
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", detailModel);

                default:
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);
            }
        }
    }
}
