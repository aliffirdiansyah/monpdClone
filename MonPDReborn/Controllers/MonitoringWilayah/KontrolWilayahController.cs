using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using System.Globalization;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;


namespace MonPDReborn.Controllers.MonitoringWilayah
{
    public class KontrolWilayahController : BaseController
    {
        private readonly string URLView;

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";
        ResponseBase response = new ResponseBase();
        public KontrolWilayahController()
        {
            URLView = $"../MonitoringWilayah/{GetType().Name.Replace("Controller", "")}/";
        }

        public ActionResult Index(int? wilayah, int? jenisPajak)
        {
            try
            {
                ViewData["Title"] = controllerName;
                if (wilayah == null || jenisPajak == null)
                {
                    var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Index();

                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Index(wilayah.Value, jenisPajak.Value);
                    return View($"{URLView}{actionName}", model);
                }

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
        public IActionResult Show(int wilayah, int tahun, int bulan, int jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Show((EnumFactory.EUPTB)wilayah, tahun, bulan, (EnumFactory.EPajak)jenisPajak);
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

        public IActionResult Detail(int wilayah, int tahun, int bulan, int jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Detail((EnumFactory.EUPTB)wilayah, tahun, bulan, (EnumFactory.EPajak)jenisPajak);
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

        public IActionResult DetailModal(string tanggal, int wilayah, int jenisPajak)
        {
            try
            {
                if (!DateTime.TryParseExact(tanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTanggal))
                    throw new ArgumentException("Format tanggal tidak valid. Gunakan format yyyy-MM-dd");

                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.DetailModal(parsedTanggal, (EnumFactory.EUPTB)wilayah, (EnumFactory.EPajak)jenisPajak);
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