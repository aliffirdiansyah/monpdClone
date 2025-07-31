using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class PotensiUploadController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PotensiUploadController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public PotensiUploadController(ILogger<PotensiUploadController> logger)
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
        [HttpPost]
        public IActionResult SimpanUpaya(Models.AktivitasOP.ReklameSummaryVM.GetDetailUpaya input)
        {
            try
            {
                if (input.Lampiran == null && input.Lampiran.Length <= 0)
                {
                    throw new ArgumentException("Lampiran tidak boleh kosong. Silahkan upload file lampiran yang sesuai.");
                }
                if (input.Lampiran != null && input.Lampiran.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        input.Lampiran.CopyTo(ms);
                        input.Data.NewRowUpaya.Lampiran = ms.ToArray();
                    }
                }
                var insert = new Models.AktivitasOP.ReklameSummaryVM.DetailUpaya.NewRow
                {
                    NoFormulir = input.Data.NewRowUpaya.NoFormulir,
                    IdUpaya = input.SelectedUpaya,
                    IdTindakan = input.SelectedTindakan,
                    NamaPetugas = input.Data.NewRowUpaya.NamaPetugas,
                    TglUpaya = input.Data.NewRowUpaya.TglUpaya,
                    Lampiran = input.Data.NewRowUpaya.Lampiran,
                };
                Models.AktivitasOP.ReklameSummaryVM.Method.SimpanUpaya(insert);

                response.Status = StatusEnum.Success;
                response.Message = "Data Berhasil Disimpan";
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
            return Json(response);
        }
    }
}
