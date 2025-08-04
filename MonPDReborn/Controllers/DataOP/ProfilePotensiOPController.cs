using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.DataOP;
using static MonPDReborn.Lib.General.ResponseBase;


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
        public IActionResult ShowData(int jenisPajak, int kategori)
        {
            try
            {
                var model = new Models.DataOP.ProfilePotensiOPVM.ShowData((EnumFactory.EPajak)jenisPajak, kategori);

                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (Exception ex)
            {
                // optional: log ex.Message
                throw;
            }
        }
        [HttpGet]
        public object GetDetailPotensi(DataSourceLoadOptions load_options, int JenisPajak)
        {
            var data = Models.DataOP.ProfilePotensiOPVM.Method.GetDetailPotensiList((EnumFactory.EPajak)JenisPajak);
            return DataSourceLoader.Load(data, load_options);
        }
        [HttpGet]
        public async Task<object> GetDataPotensi(DataSourceLoadOptions load_options, int JenisPajak, int kategori)
        {
            await Task.Delay(1000);

            var data = Models.DataOP.ProfilePotensiOPVM.Method.GetDataPotensiList(
                (EnumFactory.EPajak)JenisPajak, kategori
            );

            return DataSourceLoader.Load(data, load_options);
        }
        public IActionResult DetailPotensiOP(string nop, int jenisPajak, int kategori)
        {
            try
            {
                var jenisPajakEnum = (EnumFactory.EPajak)jenisPajak;
                switch (jenisPajakEnum)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var modelMakananMinuman = ProfilePotensiOPVM.Method.GetDataPotensiResto(nop);
                        if (kategori == 4)
                        {
                            return View("~/Views/DataOP/ProfilePotensiOP/DetailCatering.cshtml", modelMakananMinuman);
                        }
                        else
                        {
                            return View("~/Views/DataOP/ProfilePotensiOP/DetailRestoran.cshtml", modelMakananMinuman);
                        }
                    case EnumFactory.EPajak.TenagaListrik:
                        //var modelTenagaListrik = ProfilePotensiOPVM.Method.GetDataPotensiListrik(nop);
                        //return View("~/Views/DataOP/ProfilePotensiOP/DetailPPJ.cshtml", modelTenagaListrik);
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var modelPerhotelan = ProfilePotensiOPVM.Method.GetDataPotensiHotel(nop);
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailHotel.cshtml", modelPerhotelan);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var modelJasaParkir = ProfilePotensiOPVM.Method.GetDataPotensiParkir(nop);
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailParkir.cshtml", modelJasaParkir);
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var modelKesenianHiburan = ProfilePotensiOPVM.Method.GetDataPotensiHiburan(nop);
                        if (kategori == 42)
                        {
                            return View("~/Views/DataOP/ProfilePotensiOP/DetailBioskop.cshtml", modelKesenianHiburan);
                        }
                        else if (kategori == 43)
                        {
                            return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", modelKesenianHiburan);
                        }
                        else
                        {
                            return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", modelKesenianHiburan);
                        }
                    case EnumFactory.EPajak.AirTanah:
                        var modelAbt = ProfilePotensiOPVM.Method.GetDataPotensiABT(nop);
                        return View("~/Views/DataOP/ProfilePotensiOP/DetailAbt.cshtml", modelAbt);
                        break;
                    case EnumFactory.EPajak.Reklame:
                        //var modelReklame = ProfilePotensiOPVM.Method.GetDataPotensiReklame(nop);
                        //return View("~/Views/DataOP/ProfilePotensiOP/DetailPPJ.cshtml", modelReklame);
                        break;
                    case EnumFactory.EPajak.PBB:
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                var defaultModel = new ProfilePotensiOPVM.Detail();
                return View("~/Views/DataOP/ProfilePotensiOP/DetailDefault.cshtml", defaultModel);
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

    // Ubah juga method ShowDetailHiburan untuk lebih fleksibel
    //private IActionResult ShowDetailHiburan(string? kategori, object detailModel, string nop)
    //{
    //    if (string.IsNullOrEmpty(kategori))
    //    {
    //        // Tampilkan halaman hiburan umum jika tidak ada kategori
    //        return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);
    //    }

    //    switch (kategori.ToLower())
    //    {
    //        case "bioskop":
    //            var bioskopModel = new ProfilePotensiOPVM.DetailBioskop(nop);
    //            return View("~/Views/DataOP/ProfilePotensiOP/DetailBioskop.cshtml", bioskopModel);

    //        case "massage":
    //            // var massageModel = new ProfilePotensiOPVM.DetailMassage(nop);
    //            // return View("~/Views/DataOP/ProfilePotensiOP/DetailMassage.cshtml", massageModel);
    //            return View("~/Views/DataOP/ProfilePotensiOP/DetailMassage.cshtml", detailModel);


    //        case "gym":
    //            // var gymModel = new ProfilePotensiOPVM.DetailGym(nop);
    //            // return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", gymModel);
    //            return View("~/Views/DataOP/ProfilePotensiOP/DetailGym.cshtml", detailModel);

    //        default:
    //            return View("~/Views/DataOP/ProfilePotensiOP/DetailHiburan.cshtml", detailModel);
    //    }
    //}
}
