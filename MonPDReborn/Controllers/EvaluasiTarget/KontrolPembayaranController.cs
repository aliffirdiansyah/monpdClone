using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.EvaluasiTarget
{
    public class KontrolPembayaranController : Controller
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

        //public IActionResult ShowHotel(string keyword = "", int tahun = 2025)
        //{
        //    try
        //    {
        //        var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowHotel
        //        {
        //            Tahun = tahun,
        //            DataHotelList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataHotelList(keyword, tahun)
        //        };
        //        return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowHotel.cshtml", model);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
        //        return Json(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = "⚠ Server Error: Internal Server Error";
        //        return Json(response);
        //    }
        //}

        //public IActionResult ShowRestoran(string keyword = "", int tahun = 2025)
        //{
        //    try
        //    {
        //        var model = new Models.EvaluasiTarget.KontrolPembayaranVM.ShowRestoran
        //        {
        //            Tahun = tahun,
        //            DataRestoranList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataRestoranList(keyword, tahun)
        //        };
        //        return PartialView("~/Views/EvaluasiTarget/KontrolPembayaran/_ShowRestoran.cshtml", model);
        //    }
        //    catch (ArgumentException e)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
        //        return Json(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = StatusEnum.Error;
        //        response.Message = "⚠ Server Error: Internal Server Error";
        //        return Json(response);
        //    }
        //}

        public IActionResult Show(string jenisPajak = "Hotel", int tahun = 2025)
        {
            try
            {
                Console.WriteLine($"[Show] JenisPajak: {jenisPajak}, Tahun: {tahun}");

                var model = new Models.EvaluasiTarget.KontrolPembayaranVM.Show
                {
                    Tahun = tahun,
                    DataKontrolPembayaranList = Models.EvaluasiTarget.KontrolPembayaranVM.Method.GetDataKontolPembayaranList(jenisPajak, tahun)
                };
                Console.WriteLine($"Jumlah data: {model.DataKontrolPembayaranList.Count}");

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

    }
}
