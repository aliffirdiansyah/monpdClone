using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Controllers.Aktivitas;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.CCTVParkir;
using System.Text;
using System.Text.Json;
using static MonPDReborn.Lib.General.ResponseBase;
using static MonPDReborn.Models.CCTVParkir.MonitoringCCTVVM;

namespace MonPDReborn.Controllers.CCTVParkir
{
    public class MonitoringCCTVController : BaseController
    {
        string URLView = string.Empty;

        private readonly ILogger<MonitoringCCTVController> _logger;
        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        const string TD_KEY = "TD_KEY";
        const string MONITORING_ERROR_MESSAGE = "MONITORING_ERROR_MESSAGE";
        ResponseBase response = new ResponseBase();

        public MonitoringCCTVController(ILogger<MonitoringCCTVController> logger)
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

                if (string.IsNullOrEmpty(nama))
                {
                    throw new ArgumentException("Session tidak ditemukan dalam sesi.");
                }

                if (!nama.Contains("BAPENDA"))
                {
                    return RedirectToAction("Error", "Home", new { statusCode = 403 });
                }
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Index();
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
        public IActionResult Show(int uptb)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Show(uptb);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Detail(nop, vendorId);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Kapasitas(nop, vendorId);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.KapasitasHarianDetail(nop, vendorId, tgl);

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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.DataKapasitasParkir(nop, tanggalAwal, tanggalAkhir);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.Log(nop, jenisKend, tanggalAwal, tanggalAkhir);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreaming(nop, vendorId, tgl);
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
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreamingVideo(nop, cctvId);
                return PartialView($"{URLView}{actionName}", model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult LiveStreamingVideoCounting(string nop , DateTime tgl)
        {
            try
            {
                var model = new Models.CCTVParkir.MonitoringCCTVVM.LiveStreamingCounting(nop , tgl);
                //return PartialView($"{URLView}{actionName}", model);
                return PartialView("../CCTVParkir/MonitoringCCTV/LiveStreamingVideoCounting", model);

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

            DateTime lastActivityCheckTime = DateTime.Now.AddMinutes(-5);
            try
            {
                await Response.Body.FlushAsync(cancellationToken);

                await SendSseAsync(new { status = "connected", timestamp = DateTime.Now }, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // 1. Dapatkan data baru (Hanya Analysis dan Delta Aktivitas)
                        var analysis = MonitoringCCTVVM.Method.GetLiveStreamingAnalysis(nop, tgl);

                        // 💡 PANGGIL METHOD BARU UNTUK DELTA DATA
                        // Kita gunakan waktu saat ini sebagai acuan pemanggilan berikutnya
                        var newActivityEntries = MonitoringCCTVVM.Method.GetNewActivityEntries(nop, lastActivityCheckTime);

                        // 💡 PENTING: Perbarui waktu pemeriksaan terakhir
                        lastActivityCheckTime = DateTime.Now;

                        // 2. Dapatkan data Chart (Jika Anda ingin Chart juga Delta, kita bahas di Langkah 2)
                        // KARENA CHART JUGA MEMBUTUHKAN SELURUH RIWAYAT data aktivitas harian untuk plot 96 titik,
                        // kita harus memisahkan pengambilan data CHART dari data DataGrid.
                        // UNTUK SEMENTARA, kita akan tetap mengirim data chart secara lengkap (Langkah 1A).
                        var aktivitasListFull = MonitoringCCTVVM.Method.GetAktivitasHarian(nop, tgl); // Data penuh untuk Chart
                        var kapasitasChart = MonitoringCCTVVM.Method.GetKapasitasChart(aktivitasListFull, tgl);

                        var payload = new
                        {
                            AnalysisResult = analysis,
                            // 💡 KIRIM DATA DELTA untuk DataGrid (Gantikan AktivitasHarianList yang berat)
                            NewActivityEntries = newActivityEntries,
                            KapasitasChartDetail = kapasitasChart, // TETAP DIKIRIM LENGKAP SEMENTARA
                            Timestamp = DateTime.Now
                        };

                        await SendSseAsync(payload, cancellationToken);
                        await Task.Delay(2000, cancellationToken);
                    
                    }
                    catch (Exception ex) // ← ERROR DI LOOP TIDAK BREAK KONEKSI
                    {
                        _logger.LogWarning(ex, "Error fetching data for NOP {Nop}", nop);
                        await SendSseAsync(new { status = "error", message = "Data fetch error" }, cancellationToken);
                        await Task.Delay(5000, cancellationToken); // ← RETRY LEBIH LAMA
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
