using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDReborn.Lib.General;
using static MonPDReborn.Models.PengawasanReklame.PencarianReklameVM;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.PengawasanReklame
{
    public class PencarianReklameController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<PencarianReklameController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        public PencarianReklameController(ILogger<PencarianReklameController> logger)
        {
            URLView = string.Concat("../PengawasanReklame/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.PengawasanReklame.PencarianReklameVM.Index();
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
        public IActionResult Show(string namaJalan, string detailLokasi, string status)
        {
            try
            {
                var model = new Models.PengawasanReklame.PencarianReklameVM.Show(namaJalan, detailLokasi, status);
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
        [HttpGet]
        public async Task<object> GetLokasi(DataSourceLoadOptions loadOptions, string filter)
        {
            var context = DBClass.GetContext();
            var dataList = new List<lokasiReklameView>();

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
                            .Where(x => !string.IsNullOrEmpty(x.DetailLokasi))
                            .Select(x => new
                            {
                                Original = x.DetailLokasi,
                                // Ambil substring sebelum koma pertama
                                FirstPart = x.DetailLokasi.Contains(",")
                                    ? x.DetailLokasi.Substring(0, x.DetailLokasi.IndexOf(","))
                                    : x.DetailLokasi
                            })
                            .Where(x => x.FirstPart.ToUpper().Contains(keyword))
                            .GroupBy(x => x.FirstPart)
                            .Select(g => new lokasiReklameView
                            {
                                Value = g.Key,
                                Text = g.Key
                            })
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error filter: " + ex.Message);
                }
            }

            return DevExtreme.AspNet.Data.DataSourceLoader.Load(dataList, loadOptions);
        }

    }
}
