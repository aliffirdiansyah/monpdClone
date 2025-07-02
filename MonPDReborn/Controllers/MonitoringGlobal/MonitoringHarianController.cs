using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.MonitoringGlobal
{
    public class MonitoringHarianController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringHarianController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        public ResponseBase response = new ResponseBase();
        public MonitoringHarianController(ILogger<MonitoringHarianController> logger)
        {
            URLView = string.Concat("../MonitoringGlobal/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;

                var hari = new[] { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" };
                var listHarian = new List<MonitoringHarianVM.HarianPajak>();

                var random = new Random(); // Create a single Random instance to avoid issues with repeated instantiation
                for (int i = 1; i <= 31; i++)
                {
                    listHarian.Add(new MonitoringHarianVM.HarianPajak
                    {
                        Tanggal = i,
                        Hari = hari[(i - 1) % 7],
                        Target = random.Next(100000000, 500000000) * 100L, // Adjusted to use int range and scale up
                        Realisasi = (i == 1 ? 3_821_305_874 : 0)
                    });
                }

                var model = new MonitoringHarianVM.Index
                {
                    Bulan = "Juli",
                    JenisPajak = "Semua Pajak",
                    TanggalUpdate = DateTime.Now,
                    Target = 769_810_788_820,
                    Realisasi = 3_821_305_874,
                    ListHarian = listHarian
                };

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
                response.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return Json(response);
            }
        }
    }
}
