using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.StrukPBJT
{
    public class StrukPBJTController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<StrukPBJTController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public StrukPBJTController(ILogger<StrukPBJTController> logger)
        {
            URLView = string.Concat("../StrukPBJT/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index(string? keyword)
        {
            try
            {
                ViewData["Title"] = controllerName;
                if (string.IsNullOrEmpty(keyword))
                {
                    var model = new Models.StrukPBJT.StrukPBJTVM.Index(string.Empty);
                    return View($"{URLView}{actionName}", model);
                }
                else
                {
                    var model = new Models.StrukPBJT.StrukPBJTVM.Index(keyword);
                    return View($"{URLView}{actionName}", model);
                }
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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        public IActionResult Show(string noStruk)
        {
            try
            {
                var model = new Models.StrukPBJT.StrukPBJTVM.Show(noStruk);
                return PartialView($"{URLView}_{actionName}", model);
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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }

        
    }
}
