using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;


namespace MonPDReborn.Controllers.MonitoringWilayah
{
    public class KontrolWilayahController : Controller
    {
        private readonly string URLView;

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public KontrolWilayahController()
        {
            URLView = $"../MonitoringWilayah/{GetType().Name.Replace("Controller", "")}/";
        }

        public ActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;

                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Index();

                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Show(int wilayah, int tahun, int bulan, int jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Show((EnumFactory.EUPTB)wilayah, tahun, bulan, (EnumFactory.EPajak)jenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(int wilayah, int tahun, int bulan, int jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Detail((EnumFactory.EUPTB)wilayah, tahun, bulan, (EnumFactory.EPajak)jenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}