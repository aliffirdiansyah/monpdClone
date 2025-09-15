using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MonPDReborn.Lib.General.ResponseBase;
using MonPDLib.General;

namespace MonPDReborn.Controllers.DataOP
{
    public class UploadDataController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<UploadDataController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public UploadDataController(ILogger<UploadDataController> logger)
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

                if (!nama.Contains("UPLOAD"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }

                var model = new Models.DataOP.UploadDataVM.Index
                {
                    TahunList = Enumerable.Range((DateTime.Now.Year + 6) - 10, 20)
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
        public IActionResult UploadPotensiHotel(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.SimpanLampiranExcelHotel(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPotensiParkir(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.SimpanLampiranExcelParkir(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPotensiResto(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.SimpanLampiranExcelResto(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPotensiHiburan(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.SimpanLampiranExcelHiburan(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPemeriksaanExcel(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.UploadPemeriksaanPajak(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPengedokanResto(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.UploadPendataanResto(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
        [HttpPost]
        public IActionResult UploadPengedokanParkir(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.UploadPendataanParkir(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }

        [HttpPost]
        public IActionResult UploadKategoriOp(IFormFile file, int tahun)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }

                // Panggil method static untuk proses penyimpanan data
                MonPDReborn.Models.DataOP.UploadDataVM.Method.UploadPendataanParkir(file, tahun);

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
                response.Message = ex.Message;
                //response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
            return Json(response);
        }
    }
}
