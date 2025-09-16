using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using System.Drawing;
using System.Globalization;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.AktivitasOP.ReklameSummaryVM;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class ReklameSummaryController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ReklameSummaryController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ReklameSummaryController(ILogger<ReklameSummaryController> logger)
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

                if (!nama.Contains("BAPENDA") || !nama.Contains("MAGANG PENAGIHAN"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.AktivitasOP.ReklameSummaryVM.Index();
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
        public IActionResult Show(int tahun, int lokasi)
        {
            try
            {
                if (tahun == 0)
                    tahun = DateTime.Now.Year;

                var model = new Models.AktivitasOP.ReklameSummaryVM.Show(tahun, lokasi)
                {
                    TahunNow = tahun,
                    TahunMin1 = tahun - 1
                };

                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = ex.InnerException?.Message ?? ex.Message;
                return Json(response);
            }
            catch (Exception)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }


        public IActionResult ShowTerbatas(int tahun, int lokasi)
        {
            try
            {
                if (tahun == 0)
                    tahun = DateTime.Now.Year;

                var model = new Models.AktivitasOP.ReklameSummaryVM.ShowTerbatas(tahun, lokasi)
                {
                    TahunNow = tahun,
                    TahunMin1 = tahun - 1
                };
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

        public IActionResult ShowIsidentil(int tahun , int lokasi)
        {
            try
            {
                if (tahun == 0)
                    tahun = DateTime.Now.Year;

                var model = new Models.AktivitasOP.ReklameSummaryVM.ShowIsidentil(tahun, lokasi)
                {
                    TahunNow = tahun,
                    TahunMin1 = tahun - 1
                };
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

        // Detail Reklame Permanen
        public IActionResult DetailSummary(int tahun, int bulan, int jenis, int kategori, int lokasi)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.GetDetailSummary(tahun, bulan, jenis, kategori , lokasi);
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

        public IActionResult DetailBongkar(int tahun, int bulan, int jenis, int kategori, int lokasi)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.BongkarDetail(tahun, bulan, jenis, kategori, lokasi);
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

        public IActionResult DetailSilang(int tahun, int bulan, int jenis, int kategori, int lokasi)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.SilangDetail(tahun, bulan, jenis, kategori, lokasi);
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
        // Detail Upaya
        public IActionResult DetailUpaya(string noFormulir, int tahun, int bulan , int lokasi)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.GetDetailUpaya(noFormulir, tahun, bulan, lokasi);
                return PartialView("../AktivitasOP/ReklameSummary/_DetailUpaya", model);
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
        public async Task<object> GetUpaya(DataSourceLoadOptions loadOptions)
        {
            var context = DBClass.GetContext();

            // EF Core query langsung, tanpa ToListAsync
            var query = context.MUpayaReklames
                .Select(item => new UpayaCbView
                {
                    Value = (int)item.Id,
                    Text = item.Upaya ?? string.Empty
                });

            return await DevExtreme.AspNet.Data.DataSourceLoader.LoadAsync(query, loadOptions);
        }
        [HttpGet]
        public async Task<object> GetTindakan(DataSourceLoadOptions loadOptions, int idUpaya)
        {
            var context = DBClass.GetContext();

            var query = context.MTindakanReklames
                .Where(x => x.IdUpaya == idUpaya)
                .Select(item => new TindakanCbView
                {
                    Value = (int)item.Id,
                    Text = item.Tindakan ?? string.Empty
                });

            return await DataSourceLoader.LoadAsync(query, loadOptions);
        }
        //Simpan Upaya
        [HttpPost]
        public IActionResult SimpanUpaya(Models.AktivitasOP.ReklameSummaryVM.GetDetailUpaya input)
        {
            try
            {
                if (input.Lampiran == null && input.Lampiran.Length <= 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }
                if (input.Lampiran != null && input.Lampiran.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        input.Lampiran.CopyTo(ms);
                        input.Data.NewRowUpaya.Lampiran = ms.ToArray();
                    }
                }
                var insert = new Models.AktivitasOP.ReklameSummaryVM.DetailUpaya.NewRow
                {
                    NoFormulir = input.Data.NewRowUpaya.NoFormulir,
                    IdUpaya = input.SelectedUpaya,
                    IdTindakan = input.SelectedTindakan,
                    NamaPetugas = input.Data.NewRowUpaya.NamaPetugas,
                    TglUpaya = input.Data.NewRowUpaya.TglUpaya,
                    Lampiran = input.Data.NewRowUpaya.Lampiran,
                };
                Models.AktivitasOP.ReklameSummaryVM.Method.SimpanUpaya(insert);

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
