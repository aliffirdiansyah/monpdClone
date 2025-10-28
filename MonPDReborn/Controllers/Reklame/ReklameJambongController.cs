using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.AktivitasOP;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.Reklame
{
    public class ReklameJambongController : BaseController
    {
        private readonly ILogger<ReklameJambongController> _logger;
        ResponseBase response = new ResponseBase();

        private string URLView => $"../Reklame/{nameof(ReklameJambongController).Replace("Controller", "")}/";

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public ReklameJambongController(ILogger<ReklameJambongController> logger)
        {
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

                if (!nama.Contains("BAPENDA"))
                {
                    if (!(nama.Contains("BPK") || nama.Contains("SEKDA")))
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = 403 });
                    }
                }
                var model = new Models.Reklame.ReklameJambongVM.Index();
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
        public IActionResult Show()
        {
            try
            {
                var model = new Models.Reklame.ReklameJambongVM.Show();
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
        public IActionResult Detail(string jenis, int tahun, int bulan)
        {
            try
            {
                var model = new Models.Reklame.ReklameJambongVM.Detail(jenis, tahun, bulan);
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
    }
}
