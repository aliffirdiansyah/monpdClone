using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using System.Drawing;
using static MonPDReborn.Lib.General.ResponseBase;

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
        public IActionResult Show(int tahun)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.Show(tahun);
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
        public IActionResult DetailSummary(int tahun, int bulan, int jenis, int kategori)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.GetDetailSummary(tahun, bulan, jenis, kategori);
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
        public IActionResult DetailUpaya(string noFormulir, int tahun, int bulan)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.GetDetailUpaya(noFormulir, tahun, bulan);
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
        //[HttpGet]
        //public async Task<object> GetTindakan(DataSourceLoadOptions loadOptions, int idUpaya)
        //{
        //    List<dynamic> TindakanList = new();
        //    var context = DBClass.GetContext();

        //    var upaya = await context.MTindakanReklames.Where(x => x.IdUpaya.Value == idUpaya).ToList();

        //    foreach (var item in opList.Where(x => x.Pajak != EnumFactory.EPajak.AirTanah))
        //    {

        //        NopList.Add(new NOPView()
        //        {
        //            ObjekPajak = item,
        //            NamaPajak = item.Pajak.GetDescription(),
        //            InfoNOP = item.GetFormattedNOP() + "[" + item.Nama + "]"
        //        });
        //    }
        //    return DataSourceLoader.Load(NopList, loadOptions);
        //}
        //Simpan Upaya
        [HttpPost]
        public IActionResult SimpanUpaya(Models.AktivitasOP.ReklameSummaryVM.GetDetailUpaya input)
        {
            try
            {
                var model = new Models.AktivitasOP.ReklameSummaryVM.DetailUpaya.NewRow
                {
                    NoFormulir = input.Data.NoFormulir,
                    IdUpaya = input.SelectedUpaya,
                    IdTindakan = input.SelectedTindakan,
                    NamaPetugas = input.Data.NewRowUpaya.NamaPetugas,
                    TglUpaya = input.Data.NewRowUpaya.TglUpaya,
                };
                Models.AktivitasOP.ReklameSummaryVM.Method.SimpanUpaya(model);

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
