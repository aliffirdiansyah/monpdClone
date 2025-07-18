using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;
using MonPDReborn.Models.DataOP;


namespace MonPDReborn.Controllers.DataOP
{
    public class ProfilePotensiOPController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<ProfilePotensiOPController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public ProfilePotensiOPController(ILogger<ProfilePotensiOPController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.DataOP.ProfilePotensiOPVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ShowRekap(string jenisPajak)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowRekap(jenisPajak);
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

        public IActionResult ShowDetail(string jenisPajak)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowDetail
                {
                    JenisPajak = jenisPajak,
                    DataDetailPotensi = Models.DataOP.ProfilePotensiOPVM.Method.GetDetailPotensiList(jenisPajak)
                };
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
        public IActionResult ShowData(string jenisPajak, string kategori)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowData
                {
                    JenisPajak = jenisPajak,
                    Kategori = kategori,
                    DataPotensiList = Models.DataOP.ProfilePotensiOPVM.Method.GetDataPotensiList(jenisPajak, kategori)
                };

                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                // optional: log ex.Message
                throw;
            }
        }

        public IActionResult Detail(string nop, string jenisPajak, string? kategoriHiburan = null)
        {
            var detailModel = new Models.DataOP.ProfilePotensiOPVM.Detail();

            switch (jenisPajak.ToLower())
            {
                case "hotel":
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailHotel.cshtml", detailModel);

                case "parkir":
                    var parkirModel = new ProfilePotensiOPVM.DetailParkir(nop);
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailParkir.cshtml", parkirModel);


                case "restoran":
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailRestoran.cshtml", detailModel);

                case "hiburan":
                    return ShowDetailHiburan(kategoriHiburan, detailModel);

                default:
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailDefault.cshtml", detailModel);
            }
        }

        private IActionResult ShowDetailHiburan(string? kategori, object detailModel)
        {
            if (string.IsNullOrEmpty(kategori))
                return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);

            switch (kategori.ToLower())
            {
                case "massage":
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailMassage.cshtml", detailModel);

                case "gym":
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", detailModel);

                case "bioskop":
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailBioskop.cshtml", detailModel);

                default:
                    return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);
            }
        }

    }
}
