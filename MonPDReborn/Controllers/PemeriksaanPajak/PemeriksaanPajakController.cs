using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.Pemeriksaan
{
    public class PemeriksaanPajakController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PemeriksaanPajakController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public PemeriksaanPajakController(ILogger<PemeriksaanPajakController> logger)
        {
            URLView = string.Concat("../Aktivitas/PemeriksaanPajak/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new Models.Pemeriksaan.PemeriksaanPajak.Index();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new Models.Pemeriksaan.PemeriksaanPajak.Show(keyword);
                return PartialView("~/Views/Aktivitas/PemeriksaanPajak/_Show.cshtml", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(string keyword)
        {
            try
            {
                var model = new Models.Pemeriksaan.PemeriksaanPajak.Detail(keyword);
                return PartialView("~/Views/Aktivitas/PemeriksaanPajak/_Detail.cshtml", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
