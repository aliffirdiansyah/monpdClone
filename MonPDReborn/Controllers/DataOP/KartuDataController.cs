using DevExpress.XtraReports.UI;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using static MonPDReborn.Lib.General.ResponseBase;

namespace MonPDReborn.Controllers.DataOP
{
    public class KartuDataController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<KartuDataController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public KartuDataController(ILogger<KartuDataController> logger)
        {
            URLView = string.Concat("../DataOP/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    if (!nama.Contains("UPTB"))
                    {
                        return RedirectToAction("Error", "Home", new { statusCode = 403 });
                    }
                }
                var model = new Models.DataOP.KartuDataVM.Index();
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
        public IActionResult GetData(string nop)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                if (string.IsNullOrEmpty(nop))
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "OP harap dipilih";
                    return Json(response);
                }
                var model = new Models.DataOP.KartuDataVM.GetData(nop);
                if (model.KartuData == null)
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "Data tidak ditemukan";
                    return Json(response);
                }
                return PartialView(URLView + "_GetData", model);
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = ex.Message;
                return Json(response);
            }
        }
        [HttpGet]
        public async Task<object> GetOP(DataSourceLoadOptions loadOptions, string filter)
        {
            var context = DBClass.GetContext();
            var dataList = new List<System.Web.Mvc.SelectListItem>();

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
                        dataList = Models.DataOP.KartuDataVM.Method.GetOpList(keyword);
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
        public IActionResult KartuReport(string nop)
        {
            ResponseBase response = new();
            try
            {
                var model = new Models.DataOP.KartuDataVM.GetData(nop);
                if (model.KartuData.KartuDataList != null)
                {
                    if (model.KartuData.KartuDataList.Count > 0)
                    {
                        var rptKar = new Reports.KartuData();

                        rptKar.JenisPajak.Text = model.KartuData.JenisPajak.ToUpper();
                        rptKar.Uptb.Text = model.KartuData.UPTB;
                        rptKar.AlamatOP.Text = model.KartuData.AlamatOP;
                        rptKar.NamaOP.Text = model.KartuData.NamaOP;
                        rptKar.Nop.Text = model.KartuData.NOP;

                        var ds = new List<RptList>();
                        int i = 1;
                        foreach (var rpt in model.KartuData.KartuDataList)
                        {
                            ds.Add(new RptList
                            {
                                No = i.ToString(),
                                MasaPajakTahun = rpt.Tahun.ToString(),
                                MasaPajakBulan = rpt.Bulan.ToString(),
                                KetetapanSKPDSPTPD = rpt.KetetapanTotal.ToString("n0"),
                                S = "T",
                                PembayaranPokok = rpt.SetoranPokok.ToString("n0"),
                                PembayaranDenda = rpt.SetoranDenda.ToString("n0"),
                                PembayaranTotal = rpt.SetoranTotal.ToString("n0"),
                                TanggalPembayaran = rpt.TglSetoran,
                                TunggakanPokok = rpt.TunggakanPokok.ToString("n0"),
                                TunggakanPersen = "0",
                                TunggakanDenda = rpt.TunggakanSanksiSK.ToString("n0"),
                                TunggakanTotal = rpt.TunggakanTotal.ToString("n0"),
                                Keterangan = ""
                            });
                            i++;
                        }
                        rptKar.DataSource = ds;

                        XRTableRow row = new XRTableRow();

                        rptKar.No.DataBindings.Add("Text", ds, "No");
                        row.Cells.Add(rptKar.No);

                        rptKar.Tahun.DataBindings.Add("Text", ds, "MasaPajakTahun");
                        row.Cells.Add(rptKar.Tahun);

                        rptKar.Bulan.DataBindings.Add("Text", ds, "MasaPajakBulan");
                        row.Cells.Add(rptKar.Bulan);

                        rptKar.S.DataBindings.Add("Text", ds, "S");
                        row.Cells.Add(rptKar.S);

                        rptKar.KetetapanTotal.DataBindings.Add("Text", ds, "KetetapanSKPDSPTPD");
                        row.Cells.Add(rptKar.KetetapanTotal);

                        rptKar.PokokBayar.DataBindings.Add("Text", ds, "PembayaranPokok");
                        row.Cells.Add(rptKar.PokokBayar);

                        rptKar.DendaBayar.DataBindings.Add("Text", ds, "PembayaranDenda");
                        row.Cells.Add(rptKar.DendaBayar);

                        rptKar.TotalBayar.DataBindings.Add("Text", ds, "PembayaranTotal");
                        row.Cells.Add(rptKar.TotalBayar);

                        rptKar.TglBayar.DataBindings.Add("Text", ds, "TanggalPembayaran");
                        row.Cells.Add(rptKar.TglBayar);

                        rptKar.TunggakPokok.DataBindings.Add("Text", ds, "TunggakanPokok");
                        row.Cells.Add(rptKar.TunggakPokok);

                        rptKar.PersenTunggak.DataBindings.Add("Text", ds, "TunggakanPersen");
                        row.Cells.Add(rptKar.PersenTunggak);

                        rptKar.DendaTunggak.DataBindings.Add("Text", ds, "TunggakanDenda");
                        row.Cells.Add(rptKar.DendaTunggak);

                        rptKar.TunggakTotal.DataBindings.Add("Text", ds, "TunggakanTotal");
                        row.Cells.Add(rptKar.TunggakTotal);

                        rptKar.Ket.DataBindings.Add("Text", ds, "Keterangan");
                        row.Cells.Add(rptKar.Ket);


                        rptKar.xrTable1.Rows.Add(row);
                        model.KartuData.KartuDataList.Sum(x => x.SetoranPokok).ToString("n0");


                        rptKar.SumTotalKet.Text = model.KartuData.KartuDataList.Sum(x => x.KetetapanTotal).ToString("n0");
                        rptKar.SumTotalPokBayar.Text = model.KartuData.KartuDataList.Sum(x => x.SetoranPokok).ToString("n0");
                        rptKar.SumTotalDendaBayar.Text = model.KartuData.KartuDataList.Sum(x => x.SetoranDenda).ToString("n0");
                        rptKar.SumTotalBayar.Text = model.KartuData.KartuDataList.Sum(x => x.SetoranTotal).ToString("n0");
                        rptKar.SumTunggakPokok.Text = model.KartuData.KartuDataList.Sum(x => x.TunggakanPokok).ToString("n0");
                        rptKar.SumTunggakDenda.Text = model.KartuData.KartuDataList.Sum(x => x.TunggakanSanksiSK).ToString("n0");
                        rptKar.SumTunggakTotal.Text = model.KartuData.KartuDataList.Sum(x => x.TunggakanTotal).ToString("n0");

                        rptKar.ExportOptions.PrintPreview.DefaultFileName = "KartuData_" + nop;
                        return PartialView("~/Views/DataOP/KartuData/_KartuDataCetak.cshtml", rptKar);

                    }
                }
                else
                {
                    response.Status = StatusEnum.Error;
                    response.Message = "Data Tidak Ada";
                    return Json(response);
                }
            }
            catch (ArgumentException ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = "Error, message " + ex.Message;
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = "Error, message " + ex.Message;
            }
            return Json(response);
        }
    }
    public class RptList
    {
        public string No { get; set; } = string.Empty;
        public string MasaPajakTahun { get; set; } = string.Empty;
        public string MasaPajakBulan { get; set; } = string.Empty;
        public string S { get; set; } = string.Empty;
        public string KetetapanSKPDSPTPD { get; set; } = string.Empty;
        public string PembayaranPokok { get; set; } = string.Empty;
        public string PembayaranDenda { get; set; } = string.Empty;
        public string PembayaranTotal { get; set; } = string.Empty;
        public string TanggalPembayaran { get; set; } = string.Empty;
        public string TunggakanPokok { get; set; } = string.Empty;
        public string TunggakanPersen { get; set; } = string.Empty;
        public string TunggakanDenda { get; set; } = string.Empty;
        public string TunggakanTotal { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;
    }
}
