using CCTVParkirManualTarik.JasnitaModels;
using CCTVParkirManualTarik.TelkomModels;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utl = CCTVParkirManualTarik.Utility.Utility;

namespace CCTVParkirManualTarik
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource? _cts;
        private bool _isRunning = false;

        public string _USER_JASNITA = null!;
        public string _PASS_JASNITA = null!;
        public string _USER_JASNITA_2 = null!;
        public string _PASS_JASNITA_2 = null!;
        public string _TOKEN_JASNITA = null!;
        public string _TOKEN_JASNITA_2 = null!;
        public string _USER_TELKOM = null!;
        public string _PASS_TELKOM = null!;
        public string _TOKEN_TELKOM = null!;
        public Form1()
        {
            InitializeComponent();

            //initialize
            _USER_JASNITA = "bapendasby";
            _PASS_JASNITA = "surabaya2025!!";

            _USER_JASNITA_2 = "bapendasby@jastrak.id";
            _PASS_JASNITA_2 = "C8npzZpbJFJYp8Qy";

            _USER_TELKOM = "pemkot_surabaya_va";
            _PASS_TELKOM = "P3mk0tSuR4b4Ya";

            btnTarik.Click += btnTarik_Click;
            btnTarik.Text = "START";
        }

        private async void btnTarik_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                btnTarik.Text = "STOP";
                _cts = new CancellationTokenSource();

                try
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Start Process");

                    DateTime tgl1 = date1.Value.Date;
                    DateTime tgl2 = date2.Value.Date;

                    // Jalankan di thread background
                    await Task.Run(async () =>
                    {
                        var dataList = await DataCctv.GetDataOpCctvAsync();
                        UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Menjalankan proses {dataList.Count} CCTV dari {tgl1:yyyy-MM-dd} sampai {tgl2:yyyy-MM-dd}");

                        for (DateTime tgl = tgl1; tgl <= tgl2; tgl = tgl.AddDays(1))
                        {
                            _cts.Token.ThrowIfCancellationRequested();
                            UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] === Mulai tanggal {tgl:yyyy-MM-dd} ===");

                            var tasks = dataList.Select(op => ProcessDataAsync(op, tgl, _cts.Token)).ToList();
                            await Task.WhenAll(tasks);

                            UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] === Selesai tanggal {tgl:yyyy-MM-dd} ===", Color.Lime);
                            await Task.Delay(TimeSpan.FromSeconds(3), _cts.Token);
                        }

                        UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Proses selesai untuk semua tanggal.", Color.Lime);
                    }, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Proses dibatalkan oleh pengguna.", Color.Red);
                }
                catch (Exception ex)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ERROR: {Utl.GetFullExceptionMessage(ex)}", Color.Red);
                }
                finally
                {
                    _isRunning = false;
                    btnTarik.Text = "START";
                    _cts?.Dispose();
                    _cts = null;
                }
            }
            else
            {
                _cts?.Cancel();
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Membatalkan proses...", Color.Red);
            }
        }


        private async Task ProcessDataAsync(DataCctv.DataOpCctv op, DateTime tanggal, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Start proses {tanggal:yyyy-MM-dd}: {op.Nop};{op.NamaOp};{op.CctvId}");

                    if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Jasnita)
                        await CallApiJasnitaV2GrpcAsync(op, tanggal, cancellationToken);
                    else if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Telkom)
                        await CallApiTelkomAsync(op, tanggal, cancellationToken);
                    else
                    {
                        UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [ERROR] Vendor tidak dikenali: {op.Nop};{op.NamaOp};{op.CctvId}");
                        return;
                    }

                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Selesai proses {tanggal:yyyy-MM-dd}: {op.Nop};{op.NamaOp};{op.CctvId}", Color.Lime);
                    break; // keluar dari while kalau sudah selesai per tanggal
                }
                catch (TaskCanceledException)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [STOP] Dibatalkan: {op.Nop};{op.NamaOp}", Color.Red);
                    break;
                }
                catch (Exception ex)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [ERROR] {op.Nop};{op.NamaOp}: {Utl.GetFullExceptionMessage(ex)}", Color.Red);
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }
        }


        #region Telkom
        private async Task CallApiTelkomAsync(DataCctv.DataOpCctv op, DateTime tanggal, CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTokenTelkom();

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);

                string dateStr = tanggal.Date.ToString("yyyy-MM-dd");
                string url = $"https://bigvision.id/api/analytics/license-plate-recognition/data-tables?date={dateStr}&id_camera={op.CctvId}";

                // 🔹 Panggil API utama
                HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Token kadaluarsa, regenerate & ulangi
                    await GenerateTokenTelkom();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);
                    response = await httpClient.GetAsync(url, cancellationToken);
                }

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Gagal panggil API Telkom. Status: {response.StatusCode}");

                // 🔹 Parsing response JSON
                string json = await response.Content.ReadAsStringAsync(cancellationToken);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<TelkomEvent.TelkomEventResponse>(json, options);

                if (data?.Result == null || data.Result.Count == 0)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ⚠️ Tidak ada event dari API Telkom untuk {op.Nop} ({op.NamaOp})", Color.Yellow);
                    return;
                }

                var rekapResult = new List<RekapTelkom>();

                foreach (var item in data.Result)
                {
                    cancellationToken.ThrowIfCancellationRequested(); // <== 🔹 cancel cepat di setiap iterasi

                    string? platNomor = item.PlatNomor?.ToUpper() == "UNRECOGNIZED" ? null : item.PlatNomor?.ToUpper();
                    DateTime waktuMasuk = Utl.ParseFlexibleDate(item.Timestamp);
                    string id = $"{op.Nop}/{item.Id}";

                    rekapResult.Add(new RekapTelkom
                    {
                        Id = id,
                        Nop = op.Nop,
                        CctvId = op.CctvId ?? "",
                        NamaOp = op.NamaOp,
                        AlamatOp = op.AlamatOp,
                        WilayahPajak = op.WilayahPajak,
                        WaktuMasuk = waktuMasuk,
                        JenisKend = (int)Utl.GetJenisKendaraan(item.TipeKendaraan),
                        PlatNo = platNomor,
                        WaktuKeluar = waktuMasuk,
                        Direction = (int)EnumFactory.CctvParkirDirection.Incoming,
                        Log = item.Id.ToString(),
                        ImageUrl = item.Image,
                        Vendor = (int)EnumFactory.EVendorParkirCCTV.Telkom
                    });
                }

                if (rekapResult.Count > 0)
                {
                    await UpdateDBTelkomRekap(op, rekapResult, tanggal, cancellationToken);
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ✅ Telkom — {rekapResult.Count} data berhasil diproses untuk {op.Nop} ({op.NamaOp})");
                }
                else
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ⚠️ Tidak ada data valid Telkom untuk {op.Nop}");
                }
            }
            catch (OperationCanceledException)
            {
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] 🛑 Dibatalkan: CallApiTelkomAsync {op.Nop}; {op.NamaOp}", Color.Red);
            }
            catch (Exception ex)
            {
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ❌ [ERROR] CallApiTelkomAsync {op.Nop};{op.NamaOp};{op.CctvId} — {Utl.GetFullExceptionMessage(ex)}", Color.Red);
            }
        }

        private async Task UpdateDBTelkomRekap(DataCctv.DataOpCctv op, List<RekapTelkom> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                await context.Database.OpenConnectionAsync(cancellationToken);

                // === HAPUS DATA LAMA (langsung lewat SQL, tanpa load ke memory) ===
                int deletedCount = await context.TOpParkirCctvs
                    .Where(x =>
                        x.Nop == op.Nop &&
                        x.Vendor == (int)op.Vendor &&
                        x.WaktuMasuk.Date == tanggal.Date)
                    .ExecuteDeleteAsync(cancellationToken);

                if (deletedCount > 0)
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Hapus {deletedCount} data lama Telkom Rekap untuk {op.Nop};{op.NamaOp} tanggal {tanggal:yyyy-MM-dd}");

                // === CEK DATA BARU ===
                if (dataList == null || dataList.Count == 0)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Tidak ada data baru untuk Telkom Rekap {op.Nop};{op.NamaOp} tanggal {tanggal:yyyy-MM-dd}");
                    return;
                }

                // === PERSIAPAN DATA BARU ===
                var result = new List<TOpParkirCctv>(dataList.Count);

                foreach (var item in dataList)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    result.Add(new TOpParkirCctv
                    {
                        Id = item.Id,
                        Nop = item.Nop,
                        CctvId = item.CctvId,
                        NamaOp = op.NamaOp,
                        AlamatOp = op.AlamatOp,
                        WilayahPajak = op.WilayahPajak,
                        WaktuMasuk = item.WaktuMasuk,
                        JenisKend = item.JenisKend,
                        PlatNo = item.PlatNo,
                        WaktuKeluar = item.WaktuKeluar ?? item.WaktuMasuk,
                        Direction = item.Direction,
                        Log = item.Log,
                        ImageUrl = item.ImageUrl,
                        Vendor = (int)op.Vendor
                    });
                }

                // === INSERT DATA BARU ===
                await context.TOpParkirCctvs.AddRangeAsync(result, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] Insert {result.Count} data baru Telkom Rekap untuk {op.Nop};{op.NamaOp} tanggal {tanggal:yyyy-MM-dd}");
            }
            catch (OperationCanceledException)
            {
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [STOP] UpdateDBTelkomRekap dibatalkan untuk {op.Nop};{op.NamaOp}", Color.Red);
            }
            catch (Exception ex)
            {
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [ERROR] UpdateDBTelkomRekap {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}", Color.Red);
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }


        private async Task GenerateTokenTelkom()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // URL endpoint login
                    var url = "https://stage1.bigvision.id/api/user/va/login";

                    // Body request (ubah sesuai kredensial sebenarnya)
                    var requestBody = new
                    {
                        username = _USER_TELKOM,
                        password = _PASS_TELKOM
                    };

                    // Serialize body ke JSON
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Kirim POST request
                    var response = await client.PostAsync(url, content);

                    // Pastikan respons sukses
                    response.EnsureSuccessStatusCode();

                    // Baca hasil JSON
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Parse JSON
                    using var doc = JsonDocument.Parse(responseString);
                    var root = doc.RootElement;

                    // Ambil access_token
                    var token = root.GetProperty("result").GetProperty("access_token").GetString();

                    // Simpan ke field global
                    _TOKEN_TELKOM = token ?? "";
                }
                catch (HttpRequestException ex)
                {
                    //Console.WriteLine($"Gagal menghubungi server Telkom: {Utl.GetFullExceptionMessage(ex)}");
                }
                catch (JsonException ex)
                {
                    //Console.WriteLine($"Gagal parsing respon login Telkom: {Utl.GetFullExceptionMessage(ex)}");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error tidak terduga: {Utl.GetFullExceptionMessage(ex)}");
                }
            }
        }

        #endregion

        #region Jasnita
        private async Task CallApiJasnitaV2GrpcAsync(DataCctv.DataOpCctv op, DateTime tanggal, CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTokenJasnita2(cancellationToken);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                var rekapJasnita = new List<RekapJasnita>();

                string webClientUrl = "";
                #region Get Domains
                {
                    // Panggil API Jasnita untuk ambil daftar domain
                    HttpResponseMessage response = await httpClient.GetAsync(
                        "https://hub.jastrak.id/api/v3/ac-backend/domains",
                        cancellationToken
                    );

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Gagal memanggil API Jasnita Domains. Status: {response.StatusCode}");
                    }

                    // Baca dan parsing hasil JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
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
                    throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} Gagal mendapatkan webClientURL dari API Jasnita Domains.");
                }
                if (!webClientUrl.Contains("https"))
                {
                    // Jika webClientUrl tidak valid, hentikan proses
                    throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} webClientURL tidak valid: {webClientUrl}");
                }
                #endregion

                string accessPoint = "";
                #region Cameras
                {
                    // Panggil API Jasnita untuk ambil daftar kamera
                    HttpResponseMessage response = await httpClient.GetAsync($"{webClientUrl}/camera/list", cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Gagal memanggil API Jasnita Cameras. Status: {response.StatusCode}");
                    }
                    // Baca dan parsing hasil JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                    var deserialize = JsonSerializer.Deserialize<CameraJasnita.CameraJasnitaResponse>(jsonResponse);

                    if (deserialize != null)
                    {
                        accessPoint = deserialize.Cameras.FirstOrDefault(x => x.DisplayName == op.DisplayId)?.AccessPoint ?? "";
                    }

                    if (string.IsNullOrEmpty(accessPoint))
                    {
                        throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} Gagal mendapatkan Access Point dari API Jasnita Cameras untuk DisplayId: {op.DisplayId}.");
                    }
                }
                #endregion

                var eventResult = new List<EventAll.EventAllResponse>();
                #region Events
                {
                    int totalData = 0;
                    int limit = 20;
                    int offset = 0;
                    bool hasMore = true;
                    int attempt = 0;
                    const int maxRetry = 5;

                    var result = new List<EventAll.EventAllResponse>();

                    DateTime tglAwal = tanggal.Date.AddDays(0);
                    DateTime tglAkhir = tanggal.Date.AddDays(1).AddTicks(-1);

                    string begin_time = Utl.ConvertWibToUtc(tglAwal).ToString("yyyyMMdd'T'HHmmss.'000'");
                    string end_time = Utl.ConvertWibToUtc(tglAkhir).ToString("yyyyMMdd'T'HHmmss.'999'");

                    while (hasMore)
                    {
                        try
                        {
                            // 🔹 cek cancel sebelum request
                            cancellationToken.ThrowIfCancellationRequested();

                            var resEvent = new EventAll.EventAllResponse();

                            do
                            {
                                // 🔹 cek cancel di setiap iterasi retry
                                cancellationToken.ThrowIfCancellationRequested();

                                var body = new
                                {
                                    method = "axxonsoft.bl.events.EventHistoryService.ReadEvents",
                                    data = new
                                    {
                                        range = new
                                        {
                                            begin_time = begin_time,
                                            end_time = end_time
                                        },
                                        filters = new
                                        {
                                            filters = new[]
                                            {
                                    new {
                                            type = "ET_DetectorEvent",
                                            subjects = accessPoint
                                        }
                                    }
                                        },
                                        limit = limit,
                                        offset = offset,
                                        descending = true
                                    }
                                };

                                var jsonBody = JsonSerializer.Serialize(body);
                                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                                // 🔹 kirim request dengan token
                                HttpResponseMessage response = await httpClient.PostAsync($"{webClientUrl}/grpc", content, cancellationToken);

                                if (response.IsSuccessStatusCode)
                                {
                                    var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                                    var apiResponse = Utl.ConvertSseOutputJson(rawResponse);
                                    var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);

                                    if (res == null)
                                    {
                                        throw new Exception($"Response dari API tidak valid");
                                    }

                                    resEvent = res;
                                    break;
                                }
                                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                                {
                                    await GenerateTokenJasnita2(cancellationToken);
                                }
                                else
                                {
                                    ////Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Error: {response.StatusCode}");
                                }

                                attempt++;
                                if (attempt >= maxRetry)
                                {
                                    ////Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Gagal setelah {maxRetry} percobaan pada offset {offset}");
                                    break;
                                }

                            } while (attempt < maxRetry);

                            // 🔹 setelah selesai mencoba, cek cancel
                            cancellationToken.ThrowIfCancellationRequested();

                            if (resEvent == null || resEvent.Items == null || resEvent.Items.Count == 0)
                            {
                                hasMore = false;
                            }
                            else
                            {
                                eventResult.Add(resEvent);

                                totalData += resEvent.Items.Count;
                                if (resEvent.Items.Count < limit)
                                {
                                    hasMore = false;
                                }
                                else
                                {
                                    offset += limit;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Error: {Utl.GetFullExceptionMessage(ex)}");
                        }

                        // jeda singkat untuk memberi kesempatan pembatalan
                        await Task.Delay(300, cancellationToken);
                    }
                }

                if (eventResult.Count <= 0)
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ⚠️ Tidak ada event dari API Jasnita untuk {op.Nop} ({op.NamaOp})", Color.Yellow);
                    return;
                }

                eventResult = eventResult
                    .Where(p => p.Items.Any(i => i.Body.EventType == "VehicleRecognized"))
                    .ToList();
                #endregion

                var rekapResult = new List<RekapJasnita>();
                var rekapImageResult = new List<RekapJasnitaImage>();
                #region EventSnapshot & Rekap
                foreach (var data in eventResult)
                {
                    int seq = 1;
                    foreach (var item in data.Items)
                    {
                        if (item.Body?.Details?.Count > 0)
                        {
                            foreach (var detail in item.Body.Details)
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                var ar = detail.AutoRecognitionResult;
                                if (ar == null)
                                {
                                    continue;
                                }

                                DateTime waktuMasuk = Utl.ConvertUtcToWib(Utl.ParseFlexibleDate(ar.TimeBegin));
                                var id = $"{op.Nop}/{item.Body.Guid}";

                                if (waktuMasuk.Date == tanggal.Date)
                                {
                                    // ambil access point (hapus "hosts/")
                                    accessPoint = item.Body.OriginExt.AccessPoint.Replace("hosts/", "");

                                    // ambil timestamp
                                    string timestamp = item.Body.Timestamp;

                                    // ambil koordinat cropping
                                    decimal crop_x = 0;
                                    decimal crop_y = 0;
                                    decimal crop_w = 0;
                                    decimal crop_h = 0;
                                    if (item.Body.Details[0].Rectangle != null)
                                    {
                                        var rect = item.Body.Details[0].Rectangle;
                                        crop_x = rect.X;
                                        crop_y = rect.Y;
                                        crop_w = rect.W;
                                        crop_h = rect.H;
                                    }

                                    string cropX = crop_x.ToString(CultureInfo.InvariantCulture);
                                    string cropY = crop_y.ToString(CultureInfo.InvariantCulture);
                                    string cropWidth = crop_w.ToString(CultureInfo.InvariantCulture);
                                    string cropHeight = crop_h.ToString(CultureInfo.InvariantCulture);


                                    var res = new RekapJasnita();

                                    res.Id = id.ToString();
                                    res.Nop = op.Nop;
                                    res.CctvId = op.CctvId ?? "";
                                    res.NamaOp = op.NamaOp;
                                    res.AlamatOp = op.AlamatOp;
                                    res.WilayahPajak = op.WilayahPajak;
                                    res.WaktuMasuk = waktuMasuk;
                                    res.JenisKend = (int)Utl.GetJenisKendaraan(ar.VehicleClass);
                                    res.PlatNo = ar.Hypotheses?.FirstOrDefault()?.PlateFull ?? "";
                                    res.WaktuKeluar = waktuMasuk;
                                    res.Direction = (int)Utl.GetDirection(ar.Direction);
                                    res.Log = item.Body.Guid;
                                    res.ImageUrl = "";
                                    res.Vendor = (int)EnumFactory.EVendorParkirCCTV.Jasnita;


                                    rekapResult.Add(res);


                                    // Rekap Image
                                    string url = $"{webClientUrl}/archive/media/{accessPoint}/{timestamp}?crop_x={cropX}&crop_y={cropY}&crop_width={cropWidth}&crop_height={cropHeight}";
                                    var resImage = new RekapJasnitaImage();
                                    resImage.Id = res.Id;
                                    resImage.Nop = res.Nop;
                                    resImage.CctvId = res.CctvId;
                                    resImage.Url = url;

                                    rekapImageResult.Add(resImage);
                                }
                            }
                        }
                        seq++;
                    }
                }

                rekapResult = rekapResult
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .ToList();

                rekapImageResult = rekapImageResult
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .ToList();
                #endregion


                //INSERT TO DB
                if (rekapResult.Count > 0)
                {
                    await UpdateDBJasnitaRekapFull(op, rekapResult, rekapImageResult, tanggal, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] CallApiJasnitaV2GrpcAsync {op.DisplayId};{op.AccessPoint};{op.NamaOp};{op.AlamatOp};{op.Nop}: {Utl.GetFullExceptionMessage(ex)}");
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] [ERROR] CallApiJasnitaV2GrpcAsync {op.Nop};{op.NamaOp};{op.CctvId} {Utl.GetFullExceptionMessage(ex)}", Color.Red);
                Console.ResetColor();
            }
        }
        private async Task UpdateDBJasnitaRekapFull(DataCctv.DataOpCctv op,List<RekapJasnita> rekapList,List<RekapJasnitaImage> imageList,DateTime tanggal,CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                await context.Database.OpenConnectionAsync(cancellationToken);
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                // 1️⃣ Hapus data lama (anak dulu, karena relasi 1-1)
                var oldParents = await context.TOpParkirCctvs
                    .Where(x =>
                        x.Nop == op.Nop &&
                        x.Vendor == (int)op.Vendor &&
                        x.WaktuMasuk.Date == tanggal.Date)
                    .ToListAsync(cancellationToken);

                if (oldParents.Any())
                {
                    var parentIds = oldParents.Select(p => p.Id).ToList();

                    // Hapus anak (dok) dulu
                    var oldChildren = await context.TOpParkirCctvDoks
                        .Where(d => parentIds.Contains(d.Id))
                        .ToListAsync(cancellationToken);

                    if (oldChildren.Any())
                    {
                        context.TOpParkirCctvDoks.RemoveRange(oldChildren);
                        await context.SaveChangesAsync(cancellationToken);
                        UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] 🧹 Hapus {oldChildren.Count} dokumen lama Jasnita (NOP {op.Nop})");
                    }

                    // Baru hapus parent
                    context.TOpParkirCctvs.RemoveRange(oldParents);
                    await context.SaveChangesAsync(cancellationToken);
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] 🧹 Hapus {oldParents.Count} data parent lama Jasnita (NOP {op.Nop})");
                }

                // 2️⃣ Insert parent baru
                var newParents = rekapList.Select(item => new TOpParkirCctv
                {
                    Id = item.Id,
                    Nop = item.Nop,
                    CctvId = item.CctvId,
                    NamaOp = op.NamaOp,
                    AlamatOp = op.AlamatOp,
                    WilayahPajak = op.WilayahPajak,
                    WaktuMasuk = item.WaktuMasuk,
                    JenisKend = item.JenisKend,
                    PlatNo = item.PlatNo,
                    WaktuKeluar = item.WaktuMasuk,
                    Direction = item.Direction,
                    Log = item.Log,
                    Vendor = (int)op.Vendor
                }).ToList();

                if (newParents.Any())
                {
                    await context.TOpParkirCctvs.AddRangeAsync(newParents, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ✅ Insert {newParents.Count} data parent Jasnita (NOP {op.Nop})");
                }
                else
                {
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ⚠️ Tidak ada data parent baru Jasnita untuk NOP {op.Nop}");
                }

                // 3️ Ambil data image paralel
                var dokList = new ConcurrentBag<TOpParkirCctvDok>();

                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 3,
                    CancellationToken = cancellationToken
                };

                using var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(120)
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                await Parallel.ForEachAsync(imageList, parallelOptions, async (item, ct) =>
                {
                    if (ct.IsCancellationRequested) return;

                    try
                    {
                        var response = await httpClient.GetAsync(item.Url, ct);
                        if (!response.IsSuccessStatusCode)
                        {
                            UpdateConsoleLog($"{DateTime.Now:HH:mm:ss} ⚠️ {op.Nop} gagal ambil snapshot {item.Id}: {response.StatusCode}");
                            if (response.StatusCode == HttpStatusCode.Forbidden)
                                await GenerateTokenJasnita2(ct);
                            return;
                        }

                        var imgData = await response.Content.ReadAsByteArrayAsync(ct);
                        if (imgData?.Length > 0)
                        {
                            dokList.Add(new TOpParkirCctvDok
                            {
                                Id = item.Id,        // sama dengan parent.Id karena 1–1
                                Nop = item.Nop,
                                CctvId = item.CctvId,
                                ImageData = imgData
                            });
                        }
                        else
                        {
                            UpdateConsoleLog($"{DateTime.Now:HH:mm:ss} ⚠️ {op.Nop} snapshot kosong {item.Id}, dilewati.");
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateConsoleLog($"{DateTime.Now:HH:mm:ss} ⚠️ {op.Nop} error ambil snapshot {item.Id}: {Utl.GetFullExceptionMessage(ex)}", Color.Red);
                    }
                });

                // 4️⃣ Insert data dokumen baru
                if (dokList.Any())
                {
                    await context.TOpParkirCctvDoks.AddRangeAsync(dokList, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ✅ Insert {dokList.Count} data dokumen Jasnita (NOP {op.Nop})");
                }

                // 5️⃣ Commit
                await transaction.CommitAsync(cancellationToken);
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] 🎯 Update Jasnita Rekap FULL sukses (NOP {op.Nop}; {op.NamaOp}) — {newParents.Count} parent, {dokList.Count} dokumen");
            }
            catch (Exception ex)
            {
                UpdateConsoleLog($"[{DateTime.Now:HH:mm:ss}] ❌ ERROR UpdateDBJasnitaRekapFull {op.Nop};{op.NamaOp}: {Utl.GetFullExceptionMessage(ex)}", Color.Red);
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }


        private async Task GenerateTokenJasnita2(CancellationToken cancellationToken)
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

                HttpResponseMessage response = await client.PostAsync("https://hub.jastrak.id/api/v3/ac-backend/users/login", content, cancellationToken);

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
        #endregion



        private void UpdateConsoleLog(string message, Color? color = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateConsoleLog(message, color)));
                return;
            }

            // Gunakan warna default putih jika tidak ditentukan
            Color textColor = color ?? Color.White;

            // Pindahkan caret ke akhir teks sebelum menulis
            consoleLog.SelectionStart = consoleLog.TextLength;
            consoleLog.SelectionLength = 0;

            // Tulis teks berwarna
            consoleLog.SelectionColor = textColor;
            consoleLog.AppendText(message + Environment.NewLine);

            // Reset warna ke default
            consoleLog.SelectionColor = consoleLog.ForeColor;

            // === AUTO-SCROLL KE BAWAH ===
            consoleLog.SelectionStart = consoleLog.TextLength;
            consoleLog.ScrollToCaret();
        }
    }
}
