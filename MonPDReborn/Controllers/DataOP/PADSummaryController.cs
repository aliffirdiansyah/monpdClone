using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EFPenyelia;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class PADSummaryController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PADSummaryController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PADSummaryController(ILogger<PADSummaryController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        private static PenyeliaContext _context = DBClass.GetPenyeliaContext();
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "PAD Summary";
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.DataOP.PADSummaryVM.Index();
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
        public IActionResult Show(int tahun, int bulan)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.Show(tahun, bulan);
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
        public object GetKategori(DataSourceLoadOptions load_options, int tahun, int bulan, EnumFactory.EPajak pajakId)
        {
            var data = Models.DataOP.PADSummaryVM.Methods.GetDataKategori(tahun, bulan, pajakId);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult DetailOPbuka(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailOPbuka(tahun, bulan, pajakId, kategoriId);
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
        public IActionResult DetailUpaya(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId, int upaya)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailUpaya(tahun, bulan, pajakId, kategoriId, upaya);
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
        public IActionResult DetailTotalOPKategori(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailTotalOPKategori(tahun, bulan, pajakId, kategoriId);
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
        public IActionResult DetailOPAll(int tahun, int bulan, EnumFactory.EPajak pajakId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailOPAll(tahun, bulan, pajakId);
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
        public IActionResult DetailUpayaAll(int tahun, int bulan, EnumFactory.EPajak pajakId, int upaya)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailUpayaAll(tahun, bulan, pajakId, upaya);
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
        public IActionResult DetailBayarAll(int tahun, int bulan, EnumFactory.EPajak pajakId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailBayarAll(tahun, bulan, pajakId);
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
        public IActionResult DetailBlmBayar(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailBlmBayar(tahun, bulan, pajakId, kategoriId);
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
        public IActionResult DetailAllBlmBayar(int tahun, int bulan, EnumFactory.EPajak pajakId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PADSummaryVM.DetailAllBlmBayar(tahun, bulan, pajakId);
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
        public IActionResult LihatFile(string nip, string nop, long idAktifitas, int seq, string? tipe)
        {
            try
            {
                var konteks = _context;

                var file = konteks.TAktifitasPegawaiFiles
                    .FirstOrDefault(f =>
                        f.Nip == nip &&
                        f.Nop == nop &&
                        f.IdAktifitas == idAktifitas &&
                        f.Seq == seq);

                if (file == null)
                    return NotFound("Data tidak ditemukan.");

                byte[]? fileBytes = null;
                string contentType = "application/octet-stream";
                string fileExt = "";

                // 🧠 1️⃣ Pilih sumber file sesuai tipe
                if (string.Equals(tipe, "foto", StringComparison.OrdinalIgnoreCase))
                {
                    fileBytes = file.FotoFile;
                }
                else if (string.Equals(tipe, "dokumen", StringComparison.OrdinalIgnoreCase))
                {
                    fileBytes = file.FileData;
                }

                if (fileBytes == null || fileBytes.Length == 0)
                    return NotFound($"File {tipe} tidak ditemukan di database.");

                // 🧠 2️⃣ Deteksi jenis file berdasarkan header bytes
                if (fileBytes.Length > 4 && fileBytes[0] == 0x25 && fileBytes[1] == 0x50) // %PDF
                {
                    contentType = "application/pdf";
                    fileExt = ".pdf";
                }
                else if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8) // JPEG
                {
                    contentType = "image/jpeg";
                    fileExt = ".jpg";
                }
                else if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47) // PNG
                {
                    contentType = "image/png";
                    fileExt = ".png";
                }
                else if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 && fileBytes[2] == 0x46) // GIF
                {
                    contentType = "image/gif";
                    fileExt = ".gif";
                }
                else
                {
                    contentType = "application/octet-stream";
                    fileExt = ".bin";
                }

                // 🧠 3️⃣ Return langsung agar browser preview (tanpa "attachment")
                Response.Headers.Remove("Content-Disposition");
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"preview{fileExt}\"");

                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error di LihatFile()");
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}
