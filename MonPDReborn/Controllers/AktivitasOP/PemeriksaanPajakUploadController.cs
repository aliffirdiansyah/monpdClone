using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.AktivitasOP
{
    public class PemeriksaanPajakUploadController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PemeriksaanPajakUploadController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public PemeriksaanPajakUploadController(ILogger<PemeriksaanPajakUploadController> logger)
        {
            URLView = string.Concat("../AktivitasOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.PotensiUploadVM.Index
                {
                    TahunList = Enumerable.Range(DateTime.Now.Year - 5, 6)
                        .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                        .ToList(),
                    Tahun = DateTime.Now.Year
                };
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
