using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using MonPDReborn.Models.AktivitasOP;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MonPDLib.General;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class PemasanganAlatController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PemasanganAlatController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public PemasanganAlatController(ILogger<PemasanganAlatController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.PemasanganAlatVM.Index();
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
        public IActionResult Show()
        {
            try
            {
                var model = new Models.AktivitasOP.PemasanganAlatVM.Show();
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
        public object GetDetailSeries(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.AktivitasOP.PemasanganAlatVM.Method.GetDetailSeriesAlatRekam((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult Tahunan()
        {
            try
            {
                var model = new Models.AktivitasOP.PemasanganAlatVM.Tahunan();
                return PartialView($"{URLView}_Show{actionName}", model);
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
        public object GetDetailTahunan(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.AktivitasOP.PemasanganAlatVM.Method.GetDetailTahunanPemasanganAlatList((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult SubDetailOPModal(int jenisPajak, int kategori, int tahun, string status)
        {
            try
            {
                var model = new Models.AktivitasOP.PemasanganAlatVM.DetailOP(jenisPajak, kategori, tahun, status);
                return PartialView("../AktivitasOP/PemasanganAlat/_SubDetailModal", model);
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
