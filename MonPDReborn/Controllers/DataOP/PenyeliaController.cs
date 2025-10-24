using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EFPenyelia;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.DataOP.PenyeliaVM.ViewModels;

namespace MonPDReborn.Controllers.DataOP
{
    public class PenyeliaController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PenyeliaController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PenyeliaController(ILogger<PenyeliaController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        private static PenyeliaContext _context = DBClass.GetPenyeliaContext();
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Rapot Pegawai";
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.DataOP.PenyeliaVM.Index();
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
        public IActionResult Show(int tahun, int bulan, string bidang)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PenyeliaVM.Show(tahun, bulan, bidang);
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
        public object GetDetail(DataSourceLoadOptions load_options, int tahun, int bulan, string nip)
        {
            var data = Models.DataOP.PenyeliaVM.Methods.GetDetailPenyelia( tahun, bulan, nip);
            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult DetailPajak(int tahun, int bulan, string nip, int pajakId)
        {
            try
            {
                var model = new MonPDReborn.Models.DataOP.PenyeliaVM.DetailPajak(tahun, bulan, nip, pajakId);
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
        public async Task<object> GetBidang(DataSourceLoadOptions loadOptions)
        {
            var context = _context;

            // EF Core query langsung, tanpa ToListAsync
            var query = context.MBidangs
                .Select(item => new BidangView
                {
                    Value = item.Nama,
                    Text = item.Nama
                });

            return await DevExtreme.AspNet.Data.DataSourceLoader.LoadAsync(query, loadOptions);
        }
    }
}
