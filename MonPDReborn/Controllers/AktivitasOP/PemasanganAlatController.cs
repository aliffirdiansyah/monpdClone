using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Models.AktivitasOP;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class PemasanganAlatController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<PemasanganAlatController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public PemasanganAlatController(ILogger<PemasanganAlatController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.AktivitasOP.PemasanganAlatVM.Index();
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
                var model = new Models.AktivitasOP.PemasanganAlatVM.Show(keyword);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Detail(string JenisPajak)
        {
            try
            {
                var model = new Models.AktivitasOP.PemasanganAlatVM.Detail(JenisPajak);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult SubDetail(string jenisPajak)
        {
            ViewBag.JenisPajak = jenisPajak;

            var subData = PemasanganAlatVM.Method.GetSubDetailData()
                .Where(x => x.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return PartialView("../AktivitasOP/PemasanganAlat/_SubDetail", subData);
        }

        public IActionResult SubDetailModal(string kategori)
        {
            try
            {
                var data = PemasanganAlatVM.Method.GetSubDetailModalData()
                    .Where(x => x.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return PartialView("../AktivitasOP/PemasanganAlat/_SubDetailModal", data);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }




    }
}
