using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using MonPDReborn.Lib.General;
using MonPDReborn.Models.CCTVParkir;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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
        public string _USER_JASNITA_2 = null!;
        public string _PASS_JASNITA_2 = null!;
        public string _TOKEN_JASNITA_2 = null!;
        ResponseBase response = new ResponseBase();
        
        public MonitoringCCTVUPTBController(ILogger<MonitoringCCTVUPTBController> logger)
        {
            URLView = string.Concat("../CCTVParkir/", GetType().Name.Replace("Controller", ""), "/");
            _logger = logger;


            _USER_JASNITA_2 = "bapendasby@jastrak.id";
            _PASS_JASNITA_2 = "C8npzZpbJFJYp8Qy";
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
        public async Task<IActionResult> GetLiveStreamingVideo(string displayId)
        {
            try
            {

                await GenerateTokenJasnita2();
                string webClientUrl = "";
                #region Get Domains
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                    // Panggil API Jasnita untuk ambil daftar domain
                    HttpResponseMessage response = await httpClient.GetAsync(
                        "https://hub.jastrak.id/api/v3/ac-backend/domains");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Gagal memanggil API Jasnita Domains. Status: {response.StatusCode}");
                    }

                    // Baca dan parsing hasil JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonObject.Parse(jsonResponse);

                    // Ambil field webClientURL dari domain pertama
                    string rawWebClientUrl = jsonObject?["domains"]?[0]?["domain"]?["webClientURL"]?.ToString() ?? "";

                    // Normalisasi URL (tambahkan https jika perlu dan hapus slash di akhir)
                    if (!string.IsNullOrWhiteSpace(rawWebClientUrl))
                    {
                        if (rawWebClientUrl.StartsWith("//"))
                        {
                            rawWebClientUrl = "https:" + rawWebClientUrl;
                        }

                        rawWebClientUrl = rawWebClientUrl.TrimEnd('/');
                    }

                    webClientUrl = rawWebClientUrl;
                }
                if (string.IsNullOrEmpty(webClientUrl))
                {
                    // Jika webClientUrl kosong, hentikan proses
                    throw new Exception($"{DateTime.Now} Gagal mendapatkan webClientURL dari API Jasnita Domains.");
                }
                if (!webClientUrl.Contains("https"))
                {
                    // Jika webClientUrl tidak valid, hentikan proses
                    throw new Exception($"{DateTime.Now} webClientURL tidak valid: {webClientUrl}");
                }
                #endregion

                string accessPoint = "";
                #region Cameras
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");
                    // Panggil API Jasnita untuk ambil daftar kamera
                    HttpResponseMessage response = await httpClient.GetAsync($"{webClientUrl}/camera/list");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Gagal memanggil API Jasnita Cameras. Status: {response.StatusCode}");
                    }
                    // Baca dan parsing hasil JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var deserialize = JsonSerializer.Deserialize<CameraJasnita.CameraJasnitaResponse>(jsonResponse);

                    if (deserialize != null)
                    {
                        accessPoint = (deserialize.Cameras.FirstOrDefault(x => x.DisplayName == displayId)?.AccessPoint ?? "").Replace("hosts/", "");
                    }

                    if (string.IsNullOrEmpty(accessPoint))
                    {
                        throw new Exception($"{DateTime.Now}  Gagal mendapatkan Access Point dari API Jasnita Cameras untuk DisplayId: {displayId}.");
                    }
                }
                #endregion

                {
                    var url = $"{webClientUrl}/live/media/{accessPoint}";

                    var _client = new System.Net.Http.HttpClient();
                    // Tambahkan header Authorization
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                    var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                    if (!response.IsSuccessStatusCode)
                        return Content($"Gagal ambil stream. Status: {response.StatusCode}");

                    var stream = await response.Content.ReadAsStreamAsync();
                    var contentType = response.Content.Headers.ContentType?.ToString() ?? "video/mp4";

                    return File(stream, contentType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal ambil video stream");
                return Content($"Terjadi error: {ex.Message}");
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

        private async Task GenerateTokenJasnita2()
        {
            using (var client = new HttpClient())
            {
                // Headers
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                // Body
                var body = new
                {
                    email = _USER_JASNITA_2,
                    password = _PASS_JASNITA_2
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://hub.jastrak.id/api/v3/ac-backend/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonObject.Parse(result);

                    _TOKEN_JASNITA_2 = obj?["accessToken"]?.ToString() ?? ""; // ambil token
                }
                else
                {
                    throw new Exception("Gagal ambil cancellationToken. Status: " + response.StatusCode);
                }
            }
        }
        public class CameraJasnita
        {
            public class Archive
            {
                [JsonPropertyName("accessPoint")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("default")]
                public bool Default { get; set; }

                [JsonPropertyName("incomplete")]
                public bool Incomplete { get; set; }

                [JsonPropertyName("isEmbedded")]
                public bool IsEmbedded { get; set; }

                [JsonPropertyName("storage")]
                public string Storage { get; set; }

                [JsonPropertyName("storageDisplayName")]
                public string StorageDisplayName { get; set; }
            }

            public class AudioStream
            {
                [JsonPropertyName("accessPoint")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("isActivated")]
                public bool IsActivated { get; set; }
            }

            public class Camera
            {
                [JsonPropertyName("accessPoint")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("archives")]
                public List<Archive> Archives { get; set; }

                [JsonPropertyName("arm_state")]
                public string ArmState { get; set; }

                [JsonPropertyName("audioStreams")]
                public List<AudioStream> AudioStreams { get; set; }

                [JsonPropertyName("azimuth")]
                public string Azimuth { get; set; }

                [JsonPropertyName("camera_access")]
                public string CameraAccess { get; set; }

                [JsonPropertyName("comment")]
                public string Comment { get; set; }

                [JsonPropertyName("detectors")]
                public List<Detector> Detectors { get; set; }

                [JsonPropertyName("displayId")]
                public string DisplayId { get; set; }

                [JsonPropertyName("displayName")]
                public string DisplayName { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("groups")]
                public List<string> Groups { get; set; }

                [JsonPropertyName("ipAddress")]
                public string IpAddress { get; set; }

                [JsonPropertyName("isActivated")]
                public bool IsActivated { get; set; }

                [JsonPropertyName("latitude")]
                public string Latitude { get; set; }

                [JsonPropertyName("longitude")]
                public string Longitude { get; set; }

                [JsonPropertyName("model")]
                public string Model { get; set; }

                [JsonPropertyName("offlineDetectors")]
                public List<object> OfflineDetectors { get; set; }

                [JsonPropertyName("panomorph")]
                public bool Panomorph { get; set; }

                [JsonPropertyName("ptzs")]
                public List<object> Ptzs { get; set; }

                [JsonPropertyName("rays")]
                public List<object> Rays { get; set; }

                [JsonPropertyName("speakers")]
                public List<object> Speakers { get; set; }

                [JsonPropertyName("textSources")]
                public List<object> TextSources { get; set; }

                [JsonPropertyName("vendor")]
                public string Vendor { get; set; }

                [JsonPropertyName("videoStreams")]
                public List<VideoStream> VideoStreams { get; set; }
            }

            public class Detector
            {
                [JsonPropertyName("accessPoint")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("displayName")]
                public string DisplayName { get; set; }

                [JsonPropertyName("events")]
                public List<string> Events { get; set; }

                [JsonPropertyName("isActivated")]
                public bool IsActivated { get; set; }

                [JsonPropertyName("parentDetector")]
                public string ParentDetector { get; set; }

                [JsonPropertyName("type")]
                public string Type { get; set; }
            }

            public class CameraJasnitaResponse
            {
                [JsonPropertyName("cameras")]
                public List<Camera> Cameras { get; set; }

                [JsonPropertyName("search_meta_data")]
                public List<SearchMetaDatum> SearchMetaData { get; set; }
            }

            public class SearchMetaDatum
            {
                [JsonPropertyName("matches")]
                public List<object> Matches { get; set; }

                [JsonPropertyName("score")]
                public int Score { get; set; }
            }

            public class VideoStream
            {
                [JsonPropertyName("accessPoint")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }
            }


        }
    }
}
