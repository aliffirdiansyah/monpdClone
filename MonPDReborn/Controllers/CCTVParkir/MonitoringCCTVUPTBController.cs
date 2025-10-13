using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.CCTVParkir;
using System.Text;
using System.Text.Json;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.CCTVParkir.MonitoringCCTVUPTBVM;

namespace MonPDReborn.Controllers.CCTVParkir
{
    public class MonitoringCCTVUPTBController : BaseController
    {

        string URLView = string.Empty;

        private readonly ILogger<MonitoringCCTVUPTBController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();
        
        public MonitoringCCTVUPTBController(ILogger<MonitoringCCTVUPTBController> logger)
        {
            URLView = string.Concat("../CCTVParkir/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                string lastPart = nama.Split(' ').Last();
                ViewData["NamaUPTB"] = nama;

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("UPTB"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }


                if (!int.TryParse(lastPart, out int wilayah))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }

                
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.Index();
                return PartialView($"{URLView}{actionName}", model);
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

        public IActionResult Show()
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                int uptbnomor = int.Parse(nama.Split(' ').Last());

                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.Show(uptbnomor);
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

        public IActionResult Detail(string nop, int vendorId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.Detail(nop, vendorId);
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
        //ini sebagai kapasistas bulanan
        public IActionResult Kapasitas(string nop, int vendorId)
        {
            try
            {
                var nama = HttpContext.Session.GetString(Utility.SESSION_NAMA).ToString();
                string lastPart = nama.Split(' ').Last();
                ViewData["NamaUPTB"] = nama;
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.Kapasitas(nop, vendorId);
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
        public IActionResult KapasitasHarian(string nop, int tahun, int bulan)
        {
            try
            {
                var data = Method.GetMonitoringCCTVHarian(nop, tahun, bulan);
                return Json(data);
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Error;
                response.Message = ex.Message;
                return Json(response);
            }
        }

        public IActionResult KapasitasHarianDetail(string nop, int vendorId, DateTime tgl)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.KapasitasHarianDetail(nop, vendorId, tgl);

                // pastikan URLView dan actionName sesuai convention di BaseController
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
        public IActionResult DataKapasitasParkir(string nop, DateTime tanggalAwal, DateTime tanggalAkhir)
        {
            var response = new ResponseBase();
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.DataKapasitasParkir(nop, tanggalAwal, tanggalAkhir);
                return PartialView($"{URLView}_{actionName}", model);
            }
            catch (ArgumentException e)
            {
                return Json(response.ToErrorInfoMessage(e.Message));
            }
            catch (Exception ex)
            {
                return Json(response.ToInternalServerError);
            }
        }
        public IActionResult Log(string nop, int jenisKend, DateTime tanggalAwal, DateTime tanggalAkhir)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.Log(nop, jenisKend, tanggalAwal, tanggalAkhir);
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
        public IActionResult LiveStreaming(string nop, int vendorId, DateTime tgl)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.LiveStreaming(nop, vendorId, tgl);
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult LiveStreamingVideo(string nop, string cctvId)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.LiveStreamingVideo(nop, cctvId);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult LiveStreamingVideoCounting(string nop, DateTime tgl)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVUPTBVM.LiveStreamingCounting(nop, tgl);
                //return PartialView($"{URLView}{actionName}", model);
                return PartialView("../CCTVParkir/MonitoringCCTVUPTB/LiveStreamingVideoCounting", model);

            }
            catch (Exception ex)
            {

                return Content($"<div class='text-danger p-3 text-center'>Terjadi error: {ex.Message}</div>", "text/html");
            }
        }

        [HttpGet]
        public async Task GetCctvEvents(CancellationToken cancellationToken, string nop, DateTime tgl)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
            Response.Headers.Add("X-Accel-Buffering", "no");

            // Ini menjamin kita mengambil semua data yang masuk saat halaman sedang loading (menutup data gap).
            DateTime lastActivityCheckTime = DateTime.Now.AddSeconds(-10);

            // 💡 KONTROL FREKUENSI CHART: Hanya hit logic chart setiap 5 loop (10 detik)
            int chartUpdateCounter = 0;
            object lastKapasitasChartDetail = null; // Menyimpan snapshot terakhir dari data Chart

            try
            {
                await Response.Body.FlushAsync(cancellationToken);

                await SendSseAsync(new { status = "connected", timestamp = DateTime.Now }, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // --- 1.  Data Analisis (Card) ---
                        var analysis = MonitoringCCTVUPTBVM.Method.GetLiveStreamingAnalysis(nop, tgl);

                        // --- 2.  Data Delta (DataGrid) ---
                        var newActivityEntries = MonitoringCCTVUPTBVM.Method.GetNewActivityEntries(nop, lastActivityCheckTime);

                        // UNTUK SINKRONISASI WAKTU:
                        if (newActivityEntries.Count > 0)
                        {
                            // Gunakan waktu masuk TERBARU dari data yang BARU SAJA diambil sebagai acuan berikutnya.
                            // Ini mengabaikan jam server dan hanya mengikuti waktu database.
                            lastActivityCheckTime = newActivityEntries.Max(e => e.TanggalMasuk);
                        }
                        // Jika tidak ada entri baru (Panjang: 0), waktu pengecekan tetap pada nilai sebelumnya.

                        // --- 3. Ambil Data Chart (Throttled) ---
                        chartUpdateCounter++;
                        if (chartUpdateCounter >= 5 || lastKapasitasChartDetail == null)
                        {
                            var aktivitasListFull = MonitoringCCTVUPTBVM.Method.GetAktivitasHarian(nop, tgl);
                            lastKapasitasChartDetail = MonitoringCCTVUPTBVM.Method.GetKapasitasChart(aktivitasListFull, tgl);
                            chartUpdateCounter = 0;
                        }

                        // --- 4. Kirim Payload ---
                        var payload = new
                        {
                            AnalysisResult = analysis,
                            NewActivityEntries = newActivityEntries,
                            KapasitasChartDetail = lastKapasitasChartDetail,
                            Timestamp = DateTime.Now
                        };

                        await SendSseAsync(payload, cancellationToken);
                        await Task.Delay(2000, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error fetching data for NOP {Nop}", nop);
                        await SendSseAsync(new { status = "error", message = "Data fetch error" }, cancellationToken);
                        await Task.Delay(5000, cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("SSE connection closed for NOP {Nop}", nop); // ← INFO, BUKAN ERROR
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in GetCctvEvents for NOP {Nop}", nop);
            }
        }

        private async Task SendSseAsync(object data, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // ← TAMBAHAN
            });

            await Response.WriteAsync($"data: {json}\n\n", Encoding.UTF8, ct);
            await Response.Body.FlushAsync(ct);
        }


    }
}
