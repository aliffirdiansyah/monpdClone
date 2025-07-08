using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Show(string wilayah, int tahun, int bulan, string jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Show(wilayah, tahun, bulan, jenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(string wilayah, int tahun, int bulan, string jenisPajak)
        {
            try
            {
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Detail(wilayah, tahun, bulan, jenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}