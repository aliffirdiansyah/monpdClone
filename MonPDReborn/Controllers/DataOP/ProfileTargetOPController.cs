using DevExpress.DataAccess.Native.Web;
using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.DataOP.ProfileTargetOPVM;

namespace MonPDReborn.Controllers.DataOP
{
    public class ProfileTargetOPController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfileTargetOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfileTargetOPController(ILogger<ProfileTargetOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfileTargetOPVM.Index();
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

        public JsonResult GetTargetChartData()
        {
            var data = GetDummyTargetPajakBulanan();

            var result = new
            {
                categories = data.Select(x => x.Bulan),
                series = new[]
                {
                    new {
                        name = "Target Pajak Bulanan",
                        data = data.Select(x => x.Target)
                    }
                }
            };

            return new JsonResult(result); // atau: return Json(result);
        }

        public static List<TargetPajakBulanan> GetDummyTargetPajakBulanan()
        {
            return new List<TargetPajakBulanan>
                {
                    new() { Tahun = 2025, Bulan = "Jan", Target = 400_000_000 },
                    new() { Tahun = 2025, Bulan = "Feb", Target = 430_000_000 },
                    new() { Tahun = 2025, Bulan = "Mar", Target = 470_000_000 },
                    new() { Tahun = 2025, Bulan = "Apr", Target = 540_000_000 },
                    new() { Tahun = 2025, Bulan = "Mei", Target = 580_000_000 },
                    new() { Tahun = 2025, Bulan = "Jun", Target = 690_000_000 },
                    new() { Tahun = 2025, Bulan = "Jul", Target = 690_000_000 },
                    new() { Tahun = 2025, Bulan = "Agu", Target = 710_000_000 },
                    new() { Tahun = 2025, Bulan = "Sep", Target = 760_000_000 },
                    new() { Tahun = 2025, Bulan = "Okt", Target = 800_000_000 },
                    new() { Tahun = 2025, Bulan = "Nov", Target = 850_000_000 },
                    new() { Tahun = 2025, Bulan = "Des", Target = 900_000_000 }
                };
        }
        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new Models.DataOP.ProfileTargetOPVM.Show(keyword);
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


      /*  public IActionResult Detail(string nop)
        {
            try
            {
                var model = new Models.DataOP.ProfileTargetOPVM.Detail(nop);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception)
            {

                throw;
            }
        }*/
    }
}
