using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.EvaluasiTarget;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranUPTBVM;


namespace MonPDReborn.Controllers.EvaluasiTarget
{
    public class KontrolPembayaranUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<KontrolPembayaranUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public KontrolPembayaranUPTBController(ILogger<KontrolPembayaranUPTBController> logger)
        {
            URLView = string.Concat("../EvaluasiTarget/", GetType().Name.Replace("Controller", ""), "/");
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
                var model = new Models.EvaluasiTarget.KontrolPembayaranUPTBVM.Index();
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

        public IActionResult Show(int tahun, string jenisPajak)
        {
            var response = new ResponseBase();

            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int uptb = int.Parse(nama.Split(' ').Last());

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
                var model = new KontrolPembayaranUPTBVM.Show(tahun, enumPajak, enumUptb);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaranUPTB/_Show.cshtml", model);
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

        public IActionResult ShowPotensi(int tahun, string jenisPajak)
        {
            var response = new ResponseBase(); // Pastikan ResponseResult ini adalah class response standar kamu

            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int uptb = int.Parse(nama.Split(' ').Last());

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
                var model = new KontrolPembayaranUPTBVM.ShowPotensi(tahun, enumPajak, enumUptb);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaranUPTB/_ShowPotensi.cshtml", model);
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
            bool isTotal = false,
            bool isHotelNonBintang = false)
        {
            var response = new ResponseBase();

            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int uptb = int.Parse(nama.Split(' ').Last());

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

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaranUPTB/_ShowDetailPajak.cshtml", model);
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
    bool isTotal = false,
    bool isHotelNonBintang = false)
        {
            var response = new ResponseBase();

            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }
                int uptb = int.Parse(nama.Split(' ').Last());

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

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaranUPTB/_ShowDetailPotensiPajak.cshtml", model);
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

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaranUPTB/_ShowDetailUpaya.cshtml", model);
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
