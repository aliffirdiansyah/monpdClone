using ClosedXML.Excel;
using DevExpress.Charts.Native;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.EvaluasiTarget;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM;

namespace MonPDReborn.Controllers.EvaluasiTarget
{
    public class KontrolPembayaranController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<KontrolPembayaranController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public KontrolPembayaranController(ILogger<KontrolPembayaranController> logger)
        {
            URLView = string.Concat("../EvaluasiTarget/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName; var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA") || !nama.Contains("MAGANG PENDATAAN"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.EvaluasiTarget.KontrolPembayaranVM.Index();
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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        public IActionResult Show(int tahun, string jenisPajak, int uptb)
        {
            var response = new ResponseBase();

            try
            {
                EPajak enumPajak;

                switch ((jenisPajak ?? "").ToLower())
                {
                    case "hotel":
                        enumPajak = EPajak.JasaPerhotelan;
                        break;
                    case "restoran":
                        enumPajak = EPajak.MakananMinuman;
                        break;
                    case "ppj":
                        enumPajak = EPajak.TenagaListrik;
                        break;
                    case "parkir":
                        enumPajak = EPajak.JasaParkir;
                        break;
                    case "hiburan":
                        enumPajak = EPajak.JasaKesenianHiburan;
                        break;
                    case "airtanah":
                        enumPajak = EPajak.AirTanah;
                        break;
                    case "pbb":
                        enumPajak = EPajak.PBB;
                        break;
                    case "reklame":
                        enumPajak = EPajak.Reklame;
                        break;
                    case "bphtb":
                        enumPajak = EPajak.BPHTB;
                        break;
                    case "opsenpkb":
                        enumPajak = EPajak.OpsenPkb;
                        break;
                    case "opsenbbnkb":
                        enumPajak = EPajak.OpsenBbnkb;
                        break;
                    case "semua":
                        enumPajak = EPajak.Semua;
                        break;
                    default:
                        return BadRequest("Jenis pajak tidak dikenal: " + jenisPajak);
                }

                // Konversi uptb (int) ke enum EUPTB
                if (!Enum.IsDefined(typeof(EUPTB), uptb))
                {
                    return BadRequest("UPTB tidak valid: " + uptb);
                }
                var enumUptb = (EUPTB)uptb;

                // Inisialisasi ViewModel sesuai constructor baru
                var model = new KontrolPembayaranVM.Show(tahun, enumPajak, enumUptb);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_Show.cshtml", model);
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


        public IActionResult ShowUpaya(int tahun, string jenisPajak)
        {
            var response = new ResponseBase(); // Pastikan ResponseResult ini adalah class response standar kamu

            try
            {
                EPajak enumPajak;

                switch ((jenisPajak ?? "").ToLower())
                {
                    case "hotel":
                        enumPajak = EPajak.JasaPerhotelan;
                        break;
                    case "restoran":
                        enumPajak = EPajak.MakananMinuman;
                        break;
                    case "ppj":
                        enumPajak = EPajak.TenagaListrik;
                        break;
                    case "parkir":
                        enumPajak = EPajak.JasaParkir;
                        break;
                    case "hiburan":
                        enumPajak = EPajak.JasaKesenianHiburan;
                        break;
                    case "airtanah":
                        enumPajak = EPajak.AirTanah;
                        break;
                    case "pbb":
                        enumPajak = EPajak.PBB;
                        break;
                    case "reklame":
                        enumPajak = EPajak.Reklame;
                        break;
                    case "bphtb":
                        enumPajak = EPajak.BPHTB;
                        break;
                    case "opsenpkb":
                        enumPajak = EPajak.OpsenPkb;
                        break;
                    case "opsenbbnkb":
                        enumPajak = EPajak.OpsenBbnkb;
                        break;
                    case "semua":
                        enumPajak = EPajak.Semua;
                        break;
                    default:
                        return BadRequest("Jenis pajak tidak dikenal: " + jenisPajak);
                }

                // Inisialisasi ViewModel
                var model = new KontrolPembayaranVM.ShowUpaya(tahun, enumPajak);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowUpaya.cshtml", model);
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
        public IActionResult ShowPotensi(int tahun, string jenisPajak, int uptb)
        {
            var response = new ResponseBase(); // Pastikan ResponseResult ini adalah class response standar kamu

            try
            {
                EPajak enumPajak;

                switch ((jenisPajak ?? "").ToLower())
                {
                    case "hotel":
                        enumPajak = EPajak.JasaPerhotelan;
                        break;
                    case "restoran":
                        enumPajak = EPajak.MakananMinuman;
                        break;
                    case "ppj":
                        enumPajak = EPajak.TenagaListrik;
                        break;
                    case "parkir":
                        enumPajak = EPajak.JasaParkir;
                        break;
                    case "hiburan":
                        enumPajak = EPajak.JasaKesenianHiburan;
                        break;
                    case "airtanah":
                        enumPajak = EPajak.AirTanah;
                        break;
                    case "pbb":
                        enumPajak = EPajak.PBB;
                        break;
                    case "reklame":
                        enumPajak = EPajak.Reklame;
                        break;
                    case "bphtb":
                        enumPajak = EPajak.BPHTB;
                        break;
                    case "opsenpkb":
                        enumPajak = EPajak.OpsenPkb;
                        break;
                    case "opsenbbnkb":
                        enumPajak = EPajak.OpsenBbnkb;
                        break;
                    case "semua":
                        enumPajak = EPajak.Semua;
                        break;
                    default:
                        return BadRequest("Jenis pajak tidak dikenal: " + jenisPajak);
                }

                // Konversi uptb (int) ke enum EUPTB
                if (!Enum.IsDefined(typeof(EUPTB), uptb))
                {
                    return BadRequest("UPTB tidak valid: " + uptb);
                }
                var enumUptb = (EUPTB)uptb;

                // Inisialisasi ViewModel
                var model = new KontrolPembayaranVM.ShowPotensi(tahun, enumPajak, enumUptb);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowPotensi.cshtml", model);
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

        public IActionResult DetailPembayaran(
            int jenisPajak,
            int kategoriId,
            int bulan,
            int tahun,
            int status,
            int uptb, // tambahkan uptb di parameter
            bool isTotal = false,
            bool isHotelNonBintang = false)
        {
            var response = new ResponseBase();

            try
            {
                // Validasi dan konversi uptb ke enum
                if (!Enum.IsDefined(typeof(EnumFactory.EUPTB), uptb))
                {
                    return BadRequest("UPTB tidak valid: " + uptb);
                }
                var enumUptb = (EnumFactory.EUPTB)uptb;

                DetailPembayaran model;

                if (isTotal)
                {
                    // Untuk summary/total - tidak perlu kategoriId, tapi perlu info hotel type
                    model = new DetailPembayaran(
                        (EnumFactory.EPajak)jenisPajak,
                        tahun,
                        bulan,
                        status,
                        enumUptb,
                        true,
                        isHotelNonBintang
                    );
                }
                else
                {
                    // Untuk detail kategori
                    model = new DetailPembayaran(
                        (EnumFactory.EPajak)jenisPajak,
                        kategoriId,
                        tahun,
                        bulan,
                        status,
                        enumUptb
                    );
                }

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowDetailPajak.cshtml", model);
            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }



        //buat endpoint untuk mengaplikasikan dari method DetailPotensiPajak (jenis pajak, kategori, tahun , bulan )
        public IActionResult DetailPotensiPajak(
    int jenisPajak,
    int kategoriId,
    int bulan,
    int tahun,
    int uptb, // tambahkan uptb di parameter
    bool isTotal = false,
    bool isHotelNonBintang = false)
        {
            var response = new ResponseBase();

            try
            {
                // Validasi dan konversi uptb ke enum
                if (!Enum.IsDefined(typeof(EnumFactory.EUPTB), uptb))
                {
                    return BadRequest("UPTB tidak valid: " + uptb);
                }
                var enumUptb = (EnumFactory.EUPTB)uptb;

                DetailPotensiPajak model;

                if (isTotal)
                {
                    // Untuk summary/total - tidak perlu kategoriId, tapi perlu info hotel type
                    model = new DetailPotensiPajak(
                        (EnumFactory.EPajak)jenisPajak,
                        tahun,
                        bulan,
                        enumUptb,
                        true,
                        isHotelNonBintang
                    );
                }
                else
                {
                    // Untuk detail kategori
                    model = new DetailPotensiPajak(
                        (EnumFactory.EPajak)jenisPajak,
                        kategoriId,
                        tahun,
                        bulan,
                        enumUptb
                    );
                }

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowDetailPotensiPajak.cshtml", model);
            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult UpayaDetail(int jenisPajak, int kategoriId, int bulan, int tahun, int status)
        {
            try
            {
                //var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowDetailUpaya
                //{
                //    Tahun = tahun,
                //    JenisPajak = jenisPajak,
                //    Kategori = kategori,
                //    Status = status,
                //    DataDetailList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataDetailPajakList(jenisPajak, kategori, tahun, status)
                //};
                var model = new UpayaDetail((EnumFactory.EPajak)jenisPajak, kategoriId, tahun, bulan, status);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowDetailUpaya.cshtml", model);
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
