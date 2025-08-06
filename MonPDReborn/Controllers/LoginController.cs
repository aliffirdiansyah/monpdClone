using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models;
using System.Text.Json.Nodes;

namespace MonPDReborn.Controllers
{
    public class LoginController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<LoginController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public const string ERROR_LOGIN = "ERROR_LOGIN";
        private IConfiguration configuration;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            URLView = string.Concat("../", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
            this.configuration = configuration;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM.Index input)
        {
            var response = new ResponseBase();
            try
            {
                var secretKey = configuration["RecaptchaSettings:SecretKey"];
                bool successCapthca = await RecaptchaService.verifyReCaptchaV2((input.RecaptchaToken ?? ""), (secretKey ?? ""));
                if (!successCapthca)
                {
                    throw new ArgumentException("Invalid Captcha");
                }
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
        public class RecaptchaService
        {
            public static async Task<bool> verifyReCaptchaV2(string response, string secret)
            {
                using (var clienct = new HttpClient())
                {
                    string url = "https://www.google.com/recaptcha/api/siteverify";

                    MultipartFormDataContent content = new MultipartFormDataContent();
                    content.Add(new StringContent(response), "response");
                    content.Add(new StringContent(secret), "secret");

                    var result = await clienct.PostAsync(url, content);

                    if (result.IsSuccessStatusCode)
                    {
                        var strResponse = await result.Content.ReadAsStringAsync();

                        var jsonResponse = JsonNode.Parse(strResponse);
                        if (jsonResponse != null)
                        {
                            var success = ((bool?)jsonResponse["success"]);
                            if (success != null && success == true) return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
