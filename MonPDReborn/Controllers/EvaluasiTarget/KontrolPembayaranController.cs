using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
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
                ViewData["Title"] = controllerName;
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

        public IActionResult Show(string jenisPajak = "Hotel", int tahun = 2025)
        {
            try
            {

                var model = new Models.EvaluasiTarget.KontrolPembayaranVM.Show
                {
                    Tahun = tahun,
                    DataKontrolPembayaranList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataKontolPembayaranList(jenisPajak, tahun)
                };

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

        public IActionResult ShowUpaya(string jenisPajak = "Hotel", int tahun = 2025)
        {
            try
            {

                var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowUpaya
                {
                    Tahun = tahun,
                    DataUpayaPajakList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataUpayaPajakList(jenisPajak, tahun)
                };

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
        public IActionResult ShowPotensi(string jenisPajak = "Hotel", int tahun = 2025)
        {
            try
            {

                var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowPotensi
                {
                    Tahun = tahun,
                    DataPotensiList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataPotensiList(jenisPajak, tahun)
                };

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

        public IActionResult ShowDetailPajak(int jenisPajak, string kategori, int tahun = 2025, string status = "")
        {
            try
            {
                //var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowDetailPajak
                //{
                //    Tahun = tahun,
                //    JenisPajak = jenisPajak,
                //    Kategori = kategori,
                //    Status = status,
                //    DataDetailList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataDetailPajakList(jenisPajak, kategori, tahun, status)
                //};
                var model = new ShowDetailPajak((EnumFactory.EPajak)jenisPajak, kategori, tahun, status);

                return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowDetailPajak.cshtml", model);
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

        public IActionResult ShowDetailUpaya(string jenisPajak, string kategori, int tahun = 2025, string status = "")
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
                var model = new ShowDetailUpaya(jenisPajak, kategori, tahun, status);

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
