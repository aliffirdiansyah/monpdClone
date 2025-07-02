using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.AktivitasOP;
using MonPDReborn.Models.DataOP;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileOPController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public ProfileOPController(ILogger<ProfileOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfileOPVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ShowRekap(string keyword, int? tahun)
        {
            int finalTahun = tahun ?? DateTime.Now.Year;

            try
            {
                var model = new MonPDReborn.Models.DataOP.ProfileOPVM.ShowRekap(keyword, finalTahun);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                return Content("⚠️ Server Error: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }
        }




        public IActionResult ShowSeries(string keyword)
        {
            try
            {
                var model = new Models.DataOP.ProfileOPVM.ShowSeries(keyword);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

        [HttpGet]
        public object GetRekapDetailData(DataSourceLoadOptions load_options, int JenisPajak, int tahun)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetRekapDetailData((EnumFactory.EPajak)JenisPajak, tahun);
            return DataSourceLoader.Load(data, load_options);
        }

        [HttpGet]
        public object GetSeriesDetailData(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.DataOP.ProfileOPVM.Method.GetSeriesDetailData((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }

        public IActionResult Detail(string nop, int pajak)
        {
            ViewData["Title"] = "Profile Objek Pajak";
            try
            {
                var model = new Models.DataOP.ProfileOPVM.Detail(nop, (EnumFactory.EPajak)pajak);
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
                response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return Json(response);
            }
        }
    }
}
