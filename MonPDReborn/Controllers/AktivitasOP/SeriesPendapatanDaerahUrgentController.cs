using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Controllers.Aktivitas;

namespace MonPDReborn.Controllers.AktivitasOP
{
    public class SeriesPendapatanDaerahUrgentController : Controller
    {
        private string URLView => $"../AktivitasOP/{nameof(SeriesPendapatanDaerahUrgentController).Replace("Controller", "")}/";
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";
        public IActionResult Index()
        {
            var model = new Models.AktivitasOP.SeriesPendapatanDaerahUrgentVM.Index();
            return View($"{URLView}{actionName}", model);
        }
    }
}
