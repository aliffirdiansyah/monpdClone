using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class PemeriksaanPajakController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PemeriksaanPajakController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public PemeriksaanPajakController(ILogger<PemeriksaanPajakController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.PemeriksaanPajakVM.Index()
                {
                    TotalOpDiperiksa = 37,
                    RataRataKurangBayar = 10500000m,
                    TotalKurangBayar = 251100000m
                };
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
                var model = new Models.AktivitasOP.PemeriksaanPajakVM.Show(keyword);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Detail(string nop)
        {
            try
            {
                var model = new Models.AktivitasOP.PemeriksaanPajakVM.Detail(nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
