using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.DataOP.ProfileOPUPTBVM;
using static MonPDReborn.Models.DataOP.ProfileOPVM;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileOPUPTBController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileOPUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfileOPUPTBController(ILogger<ProfileOPUPTBController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Profil Objek Pajak UPTB";
                var model = new Models.DataOP.ProfileOPUPTBVM.Index();
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

        public object GetDetailPerWilayah(DataSourceLoadOptions load_options, int JenisPajak, string uptb, string kec, string kel)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel);
            return DataSourceLoader.Load(data, load_options);
        }

        [HttpGet]
        public object GetDetailPerWilayahHotel(DataSourceLoadOptions load_options, int JenisPajak, string uptb, string kec, string kel)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetDataRekapPerWilayahDetailList((EnumFactory.EPajak)JenisPajak, uptb, kec, kel, true);
            return DataSourceLoader.Load(data, load_options);
        }

        public IActionResult RekapPerWilayah(string uptb, string kec, string kel)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.RekapPerWilayah(uptb, kec, kel);
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
        public async Task<object> GetUptb(DataSourceLoadOptions loadOptions)
        {
            var context = DBClass.GetContext();

            var dataList = context.MWilayahs
                .Where(x => !string.IsNullOrEmpty(x.Uptd))
                .GroupBy(x => x.Uptd)
                .Select(g => new uptbView
                {
                    Value = g.Key,
                    Text = "UPTB " + g.Key
                })
                .OrderBy(x => x.Text)
                .ToList();

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetKec(DataSourceLoadOptions loadOptions, string uptb)
        {
            var context = DBClass.GetContext();

            var dataList = new List<kecamatanView>();

            if (!string.IsNullOrEmpty(uptb))
            {
                dataList = context.MWilayahs
                    .Where(x => x.Uptd == uptb && !string.IsNullOrEmpty(x.KdKecamatan))
                    .GroupBy(x => new { x.KdKecamatan, x.NmKecamatan })
                    .Select(g => new kecamatanView
                    {
                        Value = g.Key.KdKecamatan,
                        Text = g.Key.NmKecamatan
                    })
                    .ToList();
            }

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetKel(DataSourceLoadOptions loadOptions, string kecamatan)
        {
            var context = DBClass.GetContext();

            var dataList = new List<kelurahanView>();

            if (!string.IsNullOrEmpty(kecamatan))
            {
                dataList = context.MWilayahs
                    .Where(x => x.KdKecamatan == kecamatan && !string.IsNullOrEmpty(x.KdKelurahan))
                    .GroupBy(x => new { x.KdKelurahan, x.NmKelurahan })
                    .Select(g => new kelurahanView
                    {
                        Value = g.Key.KdKelurahan,
                        Text = g.Key.NmKelurahan
                    })
                    .ToList();
            }

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }
    }
}
