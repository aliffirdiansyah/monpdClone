using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Controllers.Aktivitas;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers.AktivitasOP
{
    public class SeriesPendapatanDaerahScontroController : Controller
    {
        private string URLView => $"../AktivitasOP/{nameof(SeriesPendapatanDaerahScontroController).Replace("Controller", "")}/";
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";
        public IActionResult Index()
        {
            var model = new Models.AktivitasOP.SeriesPendapatanDaerahScontroVM.Index();
            return View($"{URLView}{actionName}", model);
        }

        public IActionResult Show()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.AktivitasOP.SeriesPendapatanDaerahScontroVM.Show();
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex) 
            { 
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(response.ToInternalServerError());
            }
        }

        public IActionResult ShowPrint()
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.AktivitasOP.SeriesPendapatanDaerahScontroVM.Show();
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex) 
            { 
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(response.ToInternalServerError());
            }
        }
    }
}
