using DevExtreme.AspNet.Mvc;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.AktivitasOP.ReklameSummaryVM;
using static MonPDReborn.Models.ReklamePublic.ReklamePublicVM;

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
        public ReklamePublicController(ILogger<ReklamePublicController> logger)
        {
            URLView = string.Concat("../ReklamePublic/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.ReklamePublic.ReklamePublicVM.Index();

                HttpContext.Session.SetInt32("CaptchaAnswer", model.CaptchaAnswer);
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
        public IActionResult Show(string namaJalan, int userAnswer)
        {
            try
            {
                var captchaAnswer = HttpContext.Session.GetInt32("CaptchaAnswer");
                if(userAnswer != captchaAnswer)
                {
                    throw new ArgumentException("CAPTCHA TIDAK SESUAI");
                }

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
        }
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
