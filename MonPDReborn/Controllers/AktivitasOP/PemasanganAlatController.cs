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
                var model = new Models.AktivitasOP.PemasanganAlatVM.Index()
                {
                    TotalTerpasang = 171,
                    TotalBelumTerpasang = 25,
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

        public IActionResult SubDetailModal(string kategori, string status)
        {
            try
            {
                var allData = PemasanganAlatVM.Method.GetSubDetailModalData();

                // Filter utama berdasarkan kategori
                var filteredData = allData
                    .Where(x => x.Kategori != null && x.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Filter tambahan berdasarkan status (dummy logic, bisa kamu sesuaikan)
                if (!string.IsNullOrEmpty(status))
                {
                    switch (status)
                    {
                        case "JumlahOP":
                            // Semua data kategori
                            break;

                        case "TerpasangTS":
                            filteredData = filteredData.Where(x => x.IsTerpasangTS).ToList();
                            break;

                        case "TerpasangTB":
                            filteredData = filteredData.Where(x => x.IsTerpasangTB).ToList();
                            break;

                        case "TerpasangSB":
                            filteredData = filteredData.Where(x => x.IsTerpasangSB).ToList();
                            break;
                    }
                }

                return PartialView("../AktivitasOP/PemasanganAlat/_SubDetailModal", filteredData);
            }
            catch (Exception ex)
            {
                return Content("SERVER ERROR: " + ex.Message);
            }
        }






    }
}
