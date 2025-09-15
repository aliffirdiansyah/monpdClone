using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDReborn.Lib.General;
using Microsoft.EntityFrameworkCore;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.Reklame.InputReklameVM;

namespace MonPDReborn.Controllers.Reklame
{
    public class InputReklameController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<InputReklameController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public InputReklameController(ILogger<InputReklameController> logger)
        {
            URLView = string.Concat("../Reklame/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.Reklame.InputReklameVM.Index();
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

        public IActionResult Show(string noFormulir)
        {
            try
            {
                var model = new Models.Reklame.InputReklameVM.Show(noFormulir);
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
        [HttpGet]
        public async Task<object> GetKdAktifitas(DataSourceLoadOptions loadOptions)
        {
            var context = DBClass.GetContext();
            var query = context.MPetugasReklames
                .Select(item => new KdAktifitasCbView
                {
                    Value = item.KdAktifitas,
                    Text = item.KdAktifitas ?? string.Empty
                });
            return await DataSourceLoader.LoadAsync(query, loadOptions);
        }
        [HttpGet]
        public async Task<IActionResult> GetNamaAktifitas(string kdAktifitas)
        {
            if (string.IsNullOrEmpty(kdAktifitas))
            {
                return BadRequest("Kode aktifitas tidak boleh kosong.");
            }

            var context = DBClass.GetContext();
            var namaAktifitas = await context.MPetugasReklames
                .Where(x => x.KdAktifitas == kdAktifitas)
                .Select(x => x.Nama) // pastikan nama kolom sesuai
                .FirstOrDefaultAsync();

            if (namaAktifitas == null)
            {
                return NotFound($"Nama Aktifitas untuk kode '{kdAktifitas}' tidak ditemukan.");
            }

            return Ok(new
            {
                KdAktifitas = kdAktifitas,
                NamaAktifitas = namaAktifitas
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetNikPetugas(string kdAktifitas)
        {
            if (string.IsNullOrEmpty(kdAktifitas))
            {
                return BadRequest("NIK Petugas tidak boleh kosong.");
            }

            var context = DBClass.GetContext();
            var nikPetugas = await context.MPetugasReklames
                .Where(x => x.KdAktifitas == kdAktifitas) // ganti filter dari KdAktifitas ke NIK
                .Select(x => x.Nik)
                .FirstOrDefaultAsync();

            if (nikPetugas == null)
            {
                return NotFound($"Nama Petugas untuk NIK '{kdAktifitas}' tidak ditemukan.");
            }

            return Ok(new
            {
                KdAktifitas = kdAktifitas,
                NikPetugas = nikPetugas
            });
        }

        [HttpGet]
        public async Task<object> GetNoFormulir(DataSourceLoadOptions loadOptions)
        {
            var context = DBClass.GetContext();
            var query = context.DbOpReklames
                .Select(item => new NoFormulirCbView
                {
                    Value = item.NoFormulir,
                    Text = item.NoFormulir ?? string.Empty
                });
            return await DataSourceLoader.LoadAsync(query, loadOptions);
        }
        [HttpGet]
        public async Task<IActionResult> GetAlamatReklame(string noFormulir)
        {
            if (string.IsNullOrEmpty(noFormulir))
            {
                return BadRequest("No Formuir tidak boleh kosong.");
            }

            var context = DBClass.GetContext();
            var alamatReklame = await context.DbOpReklames
                .Where(x => x.NoFormulir == noFormulir)
                .Select(x => x.Alamat) // pastikan nama kolom sesuai
                .FirstOrDefaultAsync();

            if (alamatReklame == null)
            {
                return NotFound($"No Formuir untuk '{noFormulir}' tidak ditemukan.");
            }

            return Ok(new
            {
                NoFormulir = noFormulir,
                AlamatReklame = alamatReklame
            });
        }
        [HttpPost]
        public IActionResult SimpanUpaya(Models.Reklame.InputReklameVM.Show input)
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
                var insert = new Models.Reklame.InputReklameVM.DetailUpaya.NewRow
                {
                    NoFormulir = input.Data.NewRowUpaya.NoFormulir,
                    IdUpaya = input.SelectedUpaya,
                    IdTindakan = input.SelectedTindakan,
                    NamaPetugas = input.Data.NewRowUpaya.NamaPetugas,
                    NIKPetugas = input.Data.NewRowUpaya.NIKPetugas,
                    KdKatifitas = input.Data.NewRowUpaya.KdKatifitas,
                    TglUpaya = input.Data.NewRowUpaya.TglUpaya,
                    Lampiran = input.Data.NewRowUpaya.Lampiran,
                };
                Models.Reklame.InputReklameVM.Method.SimpanUpaya(insert);

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
