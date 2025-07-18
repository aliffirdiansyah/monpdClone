using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models;

namespace MonPDReborn.Controllers
{
    public class LoginController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<LoginController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public const string ERROR_LOGIN = "ERROR_LOGIN";
        public LoginController(ILogger<LoginController> logger)
        {
            URLView = string.Concat("../", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }

        public IActionResult Index()
        {
            var response = new ResponseBase();
            try
            {
                if (HttpContext.IsUserLoggedIn())
                {
                    return RedirectToAction("Index", "Home");
                }

                var model = new LoginVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Index(LoginVM.Index input)
        {
            var response = new ResponseBase();
            try
            {
                var login = LoginVM.DoLogin(input);

                HttpContext.Session.SetString(Utility.SESSION_USER, login.Username);
                HttpContext.Session.SetInt32(Utility.SESSION_ROLE, Convert.ToInt32(login.RoleId));

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                TempData[ERROR_LOGIN] = ex.Message;
                return View($"{URLView}{actionName}", input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error di {controllerName} - {actionName}: {ex.Message}");

                TempData[ERROR_LOGIN] = response.InternalServerErrorMessage;
                return View($"{URLView}{actionName}", input);
            }
        }
    }
}
