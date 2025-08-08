using DevExtreme.AspNet.Mvc;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.ReklamePublic.ReklamePublicVM;
using System.Text.Json.Nodes;
using MonPDReborn.Models.ReklamePublic;

namespace MonPDReborn.Controllers.ReklamePublic
{
    public class ReklamePublicController : Controller
    {
        string URLView = string.Empty;

        private readonly ILogger<ReklamePublicController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        private IConfiguration configuration;
        public ReklamePublicController(ILogger<ReklamePublicController> logger, IConfiguration configuration)
        {
            URLView = string.Concat("../ReklamePublic/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.ReklamePublic.ReklamePublicVM.Index();

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
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Show(string namaJalan, string recaptchaToken)
        {
            var response = new ResponseBase();
            try
            {
                // Ambil secret key dari appsettings.json
                var secretKey = configuration["RecaptchaSettings:SecretKey"];

                // Validasi captcha Google
                bool successCaptcha = await RecaptchaService.verifyReCaptchaV2(
                    recaptchaToken ?? "",
                    secretKey ?? ""
                );

                if (!successCaptcha)
                {
                    throw new ArgumentException("CAPTCHA TIDAK SESUAI");
                }

                // CAPTCHA valid → ambil data model
                var model = new Models.ReklamePublic.ReklamePublicVM.Show(namaJalan);

                // Kembalikan partial view dengan data model
                return PartialView($"{URLView}_{nameof(Show)}", model);
            }
            catch (ArgumentException e)
            {
                response.Status = StatusEnum.Error;
                response.Message = e.InnerException == null ? e.Message : e.InnerException.Message;
                return Json(response);
            }
            catch (Exception)
            {
                response.Status = StatusEnum.Error;
                response.Message = "⚠ Server Error: Internal Server Error";
                return Json(response);
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


        /*public IActionResult Show(string namaJalan)
        {
            try
            {
                var model = new Models.ReklamePublic.ReklamePublicVM.Show(namaJalan);
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
        }*/
        [HttpGet]
        public async Task<object> GetNama(DataSourceLoadOptions loadOptions, string filter)
        {
            var context = DBClass.GetContext();
            var dataList = new List<namaJalanView>();

            if (!string.IsNullOrEmpty(filter))
            {
                try
                {
                    filter = filter.Replace("[[", "")
                                   .Replace("]]", "")
                                   .Replace("\"", "")
                                   .ToUpper();

                    string[] s = filter.Split(',');

                    // Pastikan array s memiliki cukup elemen
                    string keyword = s.Length > 2 ? s[2] : s.FirstOrDefault() ?? string.Empty;

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        dataList = context.MvReklameSummaries
                            .Where(x => !string.IsNullOrEmpty(x.NamaJalan) &&
                                        x.NamaJalan.ToUpper().Contains(keyword))
                            .GroupBy(x => x.NamaJalan)
                            .Select(g => new namaJalanView
                            {
                                Value = g.Key,
                                Text = g.Key
                            })
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    // Logging optional
                    Console.WriteLine("Error filter: " + ex.Message);
                }
            }

            // Jika filter kosong atau tidak valid, tetap kembalikan default kosong
            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

    }
}
