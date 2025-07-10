using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfilePembayaranOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfilePembayaranOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfilePembayaranOPController(ILogger<ProfilePembayaranOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index(string? keyword)
        {
            try
            {
                ViewData["Title"] = controllerName;
                if (string.IsNullOrEmpty(keyword))
                {
                    var model = new Models.DataOP.ProfilePembayaranOPVM.Index(string.Empty);
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.DataOP.ProfilePembayaranOPVM.Index(keyword);
                    return View($"{URLView}{actionName}", model);
                }


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
        public IActionResult Show(EnumFactory.EPajak jenisPajak, string keyword)
        {
            try
            {
                // Pastikan Anda memberi 2 argumen
                var model = new Models.DataOP.ProfilePembayaranOPVM.Show(jenisPajak, keyword);

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

        public IActionResult Detail(EnumFactory.EPajak jenisPajak, string nop, int tahunKiri, int tahunKanan)
        {

            try
            {
                Console.WriteLine($"NOP={nop}, JenisPajak={jenisPajak}, TahunKiri={tahunKiri}, TahunKanan={tahunKanan}");

                if (string.IsNullOrWhiteSpace(nop))
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "⚠ NOP tidak boleh kosong.";
                    return Json(response);
                }

                // Buat model Detail dengan constructor 4 argumen
                var model = new Models.DataOP.ProfilePembayaranOPVM.Detail(jenisPajak, nop, tahunKiri, tahunKanan);

                // Kalau DataRealisasi null, beri feedback
                if (model.DataRealisasiKiri == null && model.DataRealisasiKanan == null)
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "⚠ Data tidak ditemukan untuk NOP dan tahun yang dimaksud.";
                    return Json(response);
                }

                // Berhasil, kembalikan PartialView
                return PartialView("~/Views/DataOP/ProfilePembayaranOP/_Detail.cshtml", model);
            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException?.Message ?? e.Message;
                return Json(response);
            }
            catch (Exception)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }




    }
}
