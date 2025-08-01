using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.AktivitasOP
{
    public class PemeriksaanPajakUploadController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PemeriksaanPajakUploadController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public PemeriksaanPajakUploadController(ILogger<PemeriksaanPajakUploadController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.PemeriksaanPajakUploadVM.Index
                {
                    TahunList = Enumerable.Range(DateTime.Now.Year - 5, 6)
                        .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                        .ToList(),
                    Tahun = DateTime.Now.Year
                };
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult UploadExcel(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.AktivitasOP.PemeriksaanPajakUploadVM.Method.UploadPemeriksaanPajak(file, tahun);

                response.Status = StatusEnum.Success;
                response.Message = "Data Berhasil Disimpan";
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
            return Json(response);
        }
    }
}
