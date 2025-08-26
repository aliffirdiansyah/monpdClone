using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Models.DataOP.UploadDataKategoriPajakVM.ViewModel;

namespace MonPDReborn.Controllers.DataOP
{
    public class UploadDataKategoriPajakController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<UploadDataKategoriPajakController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public UploadDataKategoriPajakController(ILogger<UploadDataKategoriPajakController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var model = new Models.DataOP.UploadDataKategoriPajakVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult Show(Models.DataOP.UploadDataKategoriPajakVM.Index input)
        {
            try
            {
                var model = new Models.DataOP.UploadDataKategoriPajakVM.Show(input.FileExcel);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Update(Models.DataOP.UploadDataKategoriPajakVM.Index input)
        {
            try
            {
                
                Models.DataOP.UploadDataKategoriPajakVM.Method.UpdateKategoriOp(input.FileExcel);
                return Json(response.ToSuccessInfoMessage("Data Berhasil Di Update"));
            }
            catch (Exception ex)
            {
                return Json(response.ToErrorInfoMessage(ex.Message));
            }
        }
    }
}
