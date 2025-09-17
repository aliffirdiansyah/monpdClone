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
        public IActionResult GetKdAktifitas(string kode)
        {
            var context = DBClass.GetContext();

            var result = context.MPetugasReklames
                .Where(x => x.KdAktifitas.ToUpper().Trim() == kode.ToUpper().Trim())
                .Select(x => new {
                    NamaPetugas = x.Nama,
                    NikPetugas = x.Nik
                })
                .FirstOrDefault();

            if (result == null)
                return Json(new { success = false, message = "Data tidak ditemukan" });

            return Json(new { success = true, data = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetNamaAktifitas(string kdAktifitas)
        {
            if (string.IsNullOrEmpty(kdAktifitas))
                return BadRequest("Kode aktifitas tidak boleh kosong.");

            var context = DBClass.GetContext();
            var petugas = await context.MPetugasReklames
                .Where(x => x.KdAktifitas.ToUpper().Trim() == kdAktifitas.ToUpper().Trim())
                .Select(x => new {
                    KdAktifitas = x.KdAktifitas,
                    NamaAktifitas = x.Nama,
                    NikPetugas = x.Nik
                })
                .FirstOrDefaultAsync();

            if (petugas == null)
                return NotFound($"Data petugas untuk kode '{kdAktifitas}' tidak ditemukan.");

            return Ok(petugas);
        }

        [HttpGet]
        public IActionResult GetNoFormulir(string filter)
        {
            var context = DBClass.GetContext();
            var data = context.DbOpReklames
                .Where(x => x.NoFormulir.ToUpper().Trim() == filter.ToUpper().Trim())
                .Select(x => new
                {
                    NoFormulir = x.NoFormulir,
                    AlamatReklame = x.Alamat // pastikan ini sesuai dengan field di DbOpReklames
                })
                .ToList();

            return Json(data);
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
                .Where(x => x.NoFormulir.ToUpper().Trim() == noFormulir.ToUpper().Trim())
                .Select(x => x.Alamatreklame) // pastikan nama kolom sesuai
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
                    NoFormulir = input.SelectedNoFormulir,
                    IdUpaya = input.SelectedUpaya,
                    IdTindakan = input.SelectedTindakan,
                    NamaPetugas = input.Data.NewRowUpaya.NamaPetugas,
                    NIKPetugas = input.Data.NewRowUpaya.NIKPetugas,
                    KdKatifitas = input.SelectedKdAktifitas,
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
                Console.WriteLine(ex);
                return Json(response);
            }
            return Json(response);
        }
    }
}
