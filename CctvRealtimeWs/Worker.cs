using CctvRealtimeWs.JasnitaModels;
using CctvRealtimeWs.TelkomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;

namespace CctvRealtimeWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public string _USER_JASNITA = null!;
        public string _PASS_JASNITA = null!;
        public string _USER_JASNITA_2 = null!;
        public string _PASS_JASNITA_2 = null!;
        public string _TOKEN_JASNITA = null!;
        public string _TOKEN_JASNITA_2 = null!;
        public string _USER_TELKOM = null!;
        public string _PASS_TELKOM = null!;
        public string _TOKEN_TELKOM = null!;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            _USER_JASNITA = "bapendasby";
            _PASS_JASNITA = "surabaya2025!!";

            _USER_JASNITA_2 = "bapendasby@jastrak.id";
            _PASS_JASNITA_2 = "C8npzZpbJFJYp8Qy";

            _USER_TELKOM = "pemkot_surabaya_va";
            _PASS_TELKOM = "P3mk0tSuR4b4Ya";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dataList = await DataCctv.GetDataOpCctvAsync();

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Menjalankan proses {dataList.Count} CCTV aktif...");

            // Jalankan masing-masing CCTV dalam task sendiri (loop internal)
            var tasks = dataList.Select(data => Task.Run(() => ProcessDataAsync(data, stoppingToken), stoppingToken)).ToList();

            // Tunggu sampai service dimatikan
            await Task.WhenAll(tasks);
        }


        private async Task ProcessDataAsync(DataCctv.DataOpCctv op, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [START] PROCESS: {op.Nop};{op.NamaOp};{op.CctvId}");

                try
                {
                    if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Jasnita)
                    {
                        await CallApiJasnitaV2GrpcAsync(op, DateTime.Now, cancellationToken);
                    }
                    else if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Telkom)
                    {
                        await CallApiTelkomAsync(op, DateTime.Now, cancellationToken);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Vendor tidak dikenali untuk {op.Nop};{op.NamaOp};{op.CctvId}");
                        Console.ResetColor();
                        return;
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] FINISHED PROCESS {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();

                    // Tunggu 30 detik sebelum mulai loop berikutnya
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [STOP] Dibatalkan: {op.Nop};{op.NamaOp};{op.CctvId}");
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                    Console.ResetColor();

                    // Tunggu sebentar sebelum mencoba lagi supaya tidak spam error
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }
        }





        #region Cctv Telkomm 
        private async Task CallApiTelkomAsync(DataCctv.DataOpCctv op, DateTime tanggal, CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTokenTelkom();
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);

                string dateStr = tanggal.Date.ToString("yyyy-MM-dd");
                string url = $"https://bigvision.id/api/analytics/license-plate-recognition/data-tables?date={dateStr}&id_camera={op.CctvId}";
                var response = await httpClient.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Gagal panggil API Telkom. Status: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<TelkomEvent.TelkomEventResponse>(json, options);
                if (data?.Result != null && data.Result.Count > 0)
                {
                    var rekapResult = new List<RekapTelkom>();

                    int seq = 1;
                    foreach (var item in data.Result)
                    {
                        string? platNomor = item.PlatNomor?.ToUpper() == "UNRECOGNIZED" ? null : item.PlatNomor?.ToUpper();
                        DateTime waktuMasuk = ParseFlexibleDate(item.Timestamp);
                        var id = $"{item.Id.ToString()}";

                        var res = new RekapTelkom();

                        res.Id = id;
                        res.Nop = op.Nop;
                        res.CctvId = op.CctvId ?? "";
                        res.NamaOp = op.NamaOp;
                        res.AlamatOp = op.AlamatOp;
                        res.WilayahPajak = op.WilayahPajak;
                        res.WaktuMasuk = waktuMasuk;
                        res.JenisKend = (int)GetJenisKendaraan(item.TipeKendaraan);
                        res.PlatNo = platNomor;
                        res.WaktuKeluar = waktuMasuk;
                        res.Direction = (int)EnumFactory.CctvParkirDirection.Incoming;
                        res.Log = item.Id.ToString();
                        res.ImageUrl = item.Image;
                        res.Vendor = (int)EnumFactory.EVendorParkirCCTV.Telkom;


                        rekapResult.Add(res);

                        seq++;
                    }

                    if (rekapResult.Count > 0)
                    {
                        await UpdateDbTelkomRealtime(op, rekapResult, cancellationToken);
                        await UpdateDBTelkomRekap(op, rekapResult, tanggal, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] CallApiTelkomAsync {op.NamaOp};{op.AlamatOp};{op.Nop}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
        }
        private async Task UpdateDBTelkomRekap(DataCctv.DataOpCctv op,List<RekapTelkom> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil daftar ID existing untuk NOP & Vendor hari ini
                var existingIds = await context.TOpParkirCctvs
                    .Where(x =>
                        x.Nop == op.Nop &&
                        x.WaktuMasuk.Date == tanggal.Date &&
                        x.Vendor == (int)op.Vendor)
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                // Filter hanya yang belum ada di database
                var newItems = dataList
                    .Where(item => !existingIds.Contains(item.Id))
                    .ToList();

                var result = new List<TOpParkirCctv>();
                int seq = 1;

                foreach (var item in newItems)
                {
                    var res = new TOpParkirCctv
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
                        ImageUrl = item.ImageUrl,
                        Vendor = (int)op.Vendor // pastikan field ini diisi
                    };

                    result.Add(res);
                    seq++;
                }

                if (result.Count > 0)
                {
                    await context.TOpParkirCctvs.AddRangeAsync(result, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Inserted {result.Count} new Telkom Rekap data untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Tidak ada data baru untuk Telkom Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBTelkomRekap Update DB Telkom Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Telkom Rekap close connection {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
            }
        }
        private async Task UpdateDbTelkomRealtime(DataCctv.DataOpCctv op, List<RekapTelkom> dataList, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil semua ID yang sudah ada di DB untuk NOP dan Vendor yang sama
                var existingIds = await context.TOpParkirCctvRealtimes
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor)
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                int insertCount = 0;

                foreach (var item in dataList)
                {
                    // Cek apakah ID sudah ada
                    if (existingIds.Contains(item.Id))
                        continue; // skip kalau sudah ada

                    var res = new TOpParkirCctvRealtime
                    {
                        Id = item.Id,
                        Nop = item.Nop,
                        CctvId = item.CctvId,
                        VendorId = (int)EnumFactory.EVendorParkirCCTV.Telkom,
                        JenisKend = item.JenisKend,
                        PlatNo = item.PlatNo,
                        WaktuMasuk = item.WaktuMasuk,
                        ImageUrl = item.ImageUrl
                    };

                    await context.TOpParkirCctvRealtimes.AddAsync(res, cancellationToken);
                    insertCount++;
                }

                // Simpan perubahan ke database
                if (insertCount > 0)
                    await context.SaveChangesAsync(cancellationToken);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDbTelkomRealtime DB Telkom Realtime updated untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} (insert {insertCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDbTelkomRealtime Update DB Telkom Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Telkom Realtime close connection {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
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
                    Console.WriteLine($"Gagal menghubungi server Telkom: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Gagal parsing respon login Telkom: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error tidak terduga: {ex.Message}");
                }
            }
        }
        #endregion




        #region Cctv Jasnita
        private async Task CallApiJasnitaAsync(DataCctv.DataOpCctv op, CancellationToken cancellationToken)
        {
            await GenerateTokenJasnita();
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

            int totalData = 0;
            int limit = 20;
            int offset = 0;
            bool hasMore = true;
            int attempt = 0;
            const int maxRetry = 5;

            var result = new List<EventAll.EventAllResponse>();

            DateTime tglAwal = DateTime.Today.AddDays(-1);
            DateTime tglAkhir = DateTime.Today.AddDays(1).AddTicks(-1);

            string begin_time = ConvertWibToUtc(tglAwal).ToString("yyyyMMdd'T'HHmmss.'000'");
            string end_time = ConvertWibToUtc(tglAkhir).ToString("yyyyMMdd'T'HHmmss.'999'");

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
                                            subjects = op.AccessPoint
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
                        HttpResponseMessage response = await httpClient.PostAsync("http://202.146.133.26/grpc", content, cancellationToken);

                        if (response.IsSuccessStatusCode)
                        {
                            var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                            var apiResponse = ConvertSseOutputJson(rawResponse);
                            var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);

                            if (res == null)
                                throw new Exception($"Response dari API tidak valid");

                            resEvent = res;
                            break;
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Token kadaluarsa, refresh token dan ulangi...");
                            await GenerateTokenJasnita();
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Error: {response.StatusCode}");
                        }

                        attempt++;
                        if (attempt >= maxRetry)
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Gagal setelah {maxRetry} percobaan pada offset {offset}");
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
                        result.Add(resEvent);

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
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }


            //INSERT TO DB
            if (result.Count > 0)
            {
                await UpdateDbJasnitaRealtime(op, result, cancellationToken);
            }
        }
        private async Task CallApiJasnitaV2Async(DataCctv.DataOpCctv op, CancellationToken cancellationToken)
        {
            await GenerateTokenJasnita2();

            var rekapJasnita = new List<RekapJasnita>();

            #region Get Domains
            string webClientUrl = "";
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

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

            var eventResult = new List<EventV2.Event>();
            #region Events
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                string timestamp_start = ConvertWibToUtc(DateTime.Now.Date).ToString("yyyyMMdd'T'HHmmss.'000'");
                string timestamp_end = ConvertWibToUtc(DateTime.Now.AddDays(1).AddTicks(-1)).ToString("yyyyMMdd'T'HHmmss.'000'");
                string cameraId = op.DisplayId ?? "";
                string url = $"{webClientUrl}/archive/events/detectors/{cameraId}/{timestamp_start}/{timestamp_end}";

                // Panggil API Jasnita untuk ambil daftar domain
                HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Gagal memanggil API Jasnita Event. Status: {response.StatusCode}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<EventV2.EventV2Response>(jsonResponse);
                if (result != null && result.Events != null && result.Events.Count > 0)
                {
                    eventResult.AddRange(result.Events);
                }
            }

            if (eventResult.Count <= 0)
            {
                throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} Tidak ada event dari API Jasnita Events.");
            }
            #endregion


            #region EventSnapshot
            foreach (var item in eventResult)
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                string url = $"{webClientUrl}/archive/media/{item.Source.Replace("hosts/", "")}/{item.Timestamp}?crop_x={item.Rectangles[0].Left}&crop_y={item.Rectangles[0].Top}&crop_width=0.1&crop_height=0.1";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp};{item.Id} Gagal memanggil API Jasnita Event Snapshot. Status: {response.StatusCode}");
                }

                // Baca stream image-nya
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                DateTime parsedTime = ConvertUtcToWib(ParseFlexibleDate(item.Timestamp));

                var rekap = new RekapJasnita();
                rekap.Id = item.Id;
                rekap.Nop = op.Nop;
                rekap.CctvId = op.CctvId ?? "-";
                rekap.Vendor = (int)op.Vendor;
                rekap.JenisKend = (int)EnumFactory.EJenisKendParkirCCTV.Unknown;
                rekap.PlatNo = "-";
                rekap.WaktuMasuk = parsedTime;

                rekapJasnita.Add(rekap);
            }
            #endregion
        }
        private async Task CallApiJasnitaV2GrpcAsync(DataCctv.DataOpCctv op, DateTime tanggal, CancellationToken cancellationToken)
        {
            try
            {
                await GenerateTokenJasnita2();

                var rekapJasnita = new List<RekapJasnita>();

                string webClientUrl = "";
                #region Get Domains
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

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

                var eventResult = new List<EventAll.EventAllResponse>();
                #region Events
                {
                    using var httpClient = new HttpClient();

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                    int totalData = 0;
                    int limit = 20;
                    int offset = 0;
                    bool hasMore = true;
                    int attempt = 0;
                    const int maxRetry = 5;

                    var result = new List<EventAll.EventAllResponse>();

                    DateTime tglAwal = tanggal.Date.AddDays(0);
                    DateTime tglAkhir = tanggal.Date.AddDays(1).AddTicks(-1);

                    string begin_time = ConvertWibToUtc(tglAwal).ToString("yyyyMMdd'T'HHmmss.'000'");
                    string end_time = ConvertWibToUtc(tglAkhir).ToString("yyyyMMdd'T'HHmmss.'999'");

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
                                            subjects = op.AccessPoint
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
                                    var apiResponse = ConvertSseOutputJson(rawResponse);
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
                                    await GenerateTokenJasnita();
                                }
                                else
                                {
                                    //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Error: {response.StatusCode}");
                                }

                                attempt++;
                                if (attempt >= maxRetry)
                                {
                                    //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop};{op.NamaOp};{op.CctvId} Gagal setelah {maxRetry} percobaan pada offset {offset}");
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
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }

                if (eventResult.Count <= 0)
                {
                    throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} Tidak ada event dari API Jasnita Events.");
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

                                DateTime waktuMasuk = ConvertUtcToWib(ParseFlexibleDate(ar.TimeBegin));
                                var id = $"{item.Body.Guid}";

                                if (waktuMasuk.Date == DateTime.Now.Date)
                                {
                                    // Ambil snapshot image dari API Jasnita
                                    using var httpClient = new HttpClient();
                                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                                    // ambil access point (hapus "hosts/")
                                    string accessPoint = item.Body.OriginExt.AccessPoint.Replace("hosts/", "");

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
                                    res.JenisKend = (int)GetJenisKendaraan(ar.VehicleClass);
                                    res.PlatNo = ar.Hypotheses?.FirstOrDefault()?.PlateFull ?? "";
                                    res.WaktuKeluar = waktuMasuk;
                                    res.Direction = (int)GetDirection(ar.Direction);
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
                #endregion


                //INSERT TO DB
                if (rekapResult.Count > 0)
                {
                    await UpdateDBJasnitaRealtimeV2(op, rekapResult, cancellationToken);
                    await UpdateDBJasnitaRealtimeImageV2(op, rekapImageResult, cancellationToken);

                    await UpdateDBJasnitaRekap(op, rekapResult, tanggal, cancellationToken);
                    await UpdateDBJasnitaRekapImageV2(op, rekapImageResult, tanggal, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] CallApiJasnitaV2GrpcAsync {op.DisplayId};{op.AccessPoint};{op.NamaOp};{op.AlamatOp};{op.Nop}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
        }
        private async Task UpdateDBJasnitaRekap(DataCctv.DataOpCctv op,List<RekapJasnita> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil daftar ID existing untuk NOP & Vendor hari ini
                var existingIds = await context.TOpParkirCctvs
                    .Where(x =>
                        x.Nop == op.Nop &&
                        x.WaktuMasuk.Date == tanggal.Date &&
                        x.Vendor == (int)op.Vendor)
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var newItems = dataList
                    .Where(item => !existingIds.Contains(item.Id))
                    .ToList();

                // Ambil ID dok yang sudah ada untuk mencegah constraint error
                var existingDokIds = await context.TOpParkirCctvDoks
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                var result = new List<TOpParkirCctv>();

                foreach (var item in dataList)
                {
                    // kalau Id CCTV belum ada → insert record baru
                    if (!existingIds.Contains(item.Id))
                    {
                        var res = new TOpParkirCctv();
                        
                        res.Id = item.Id;
                        res.Nop = item.Nop;
                        res.CctvId = item.CctvId;
                        res.NamaOp = op.NamaOp;
                        res.AlamatOp = op.AlamatOp;
                        res.WilayahPajak = op.WilayahPajak;
                        res.WaktuMasuk = item.WaktuMasuk;
                        res.JenisKend = item.JenisKend;
                        res.PlatNo = item.PlatNo;
                        res.WaktuKeluar = item.WaktuMasuk;
                        res.Direction = item.Direction;
                        res.Log = item.Log;
                        res.Vendor = (int)op.Vendor;
                        
                        result.Add(res);
                    }
                }

                if (result.Count > 0)
                {
                    await context.TOpParkirCctvs.AddRangeAsync(result, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);


                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Inserted {result.Count} new Jasnita Rekap data untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
                else
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Tidak ada data baru untuk Jasnita Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Update DB Jasnita Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                await context.Database.CloseConnectionAsync();

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Rekap close connection {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
            }
        }
        private async Task UpdateDBJasnitaRekapImageV2(DataCctv.DataOpCctv op, List<RekapJasnitaImage> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil dan hapus data lama
                var oldData = await context.TOpParkirCctvDoks
                    .Where(x => x.TOpParkirCctv.Nop == op.Nop && x.TOpParkirCctv.Vendor == (int)op.Vendor && x.TOpParkirCctv.WaktuMasuk.Date == tanggal.Date)
                    .ToListAsync(cancellationToken);

                // Baru hapus data utamanya (parent)
                context.TOpParkirCctvDoks.RemoveRange(oldData);

                // Siapkan data baru
                var result = new List<TOpParkirCctvDok>();

                foreach (var item in dataList)
                {
                    var res = new TOpParkirCctvDok();

                    // bentuk URL akhir
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    string url = $"{item.Url}";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{DateTime.Now} {op.Nop};{op.NamaOp};{item.Id} UpdateDBJasnitaRekapImageV2 Gagal ambil snapshot: {response.StatusCode}");
                        Console.ResetColor();

                        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            await GenerateTokenJasnita();
                        }

                        continue;
                    }
                    // Baca stream image-nya
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    res.Id = item.Id;
                    res.Nop = item.Nop;
                    res.CctvId = item.CctvId;
                    res.ImageData = imageBytes;

                    result.Add(res);
                }

                if (result.Count > 0)
                {
                    await context.TOpParkirCctvDoks.AddRangeAsync(result, cancellationToken);
                }

                // Simpan perubahan ke database
                await context.SaveChangesAsync(cancellationToken);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Realtime updated untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} ({result.Count} data)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Update DB Jasnita Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Realtime close conenction {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
            }
        }

        private async Task UpdateDbJasnitaRealtime(DataCctv.DataOpCctv op, List<EventAll.EventAllResponse> dataList,CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil dan hapus data lama
                var oldData = await context.TOpParkirCctvRealtimes
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor)
                    .ToListAsync(cancellationToken);

                context.TOpParkirCctvRealtimes.RemoveRange(oldData);

                // Siapkan data baru
                var result = new List<TOpParkirCctvRealtime>();

                foreach (var data in dataList)
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
                                    continue;

                                DateTime waktuMasuk = ConvertUtcToWib(ParseFlexibleDate(ar.TimeBegin));
                                var id = $"{seq}{(int)(EnumFactory.EVendorParkirCCTV.Jasnita)}{op.Nop}{waktuMasuk.ToString("yyyyMMddHHmmss")}";

                                if (waktuMasuk.Date == DateTime.Now.Date)
                                {
                                    var res = new TOpParkirCctvRealtime();

                                    res.Id = id.ToString();
                                    res.Nop = op.Nop;
                                    res.CctvId = op.CctvId ?? "";
                                    res.VendorId = (int)op.Vendor;
                                    res.JenisKend = (int)GetJenisKendaraan(ar.VehicleClass);
                                    res.PlatNo = ar.Hypotheses?.FirstOrDefault()?.PlateFull ?? "";
                                    res.WaktuMasuk = waktuMasuk;
                                    res.ImageUrl = "";

                                    result.Add(res);
                                }
                            }
                        }
                        seq++;
                    }
                }

                if (result.Count > 0)
                {
                    await context.TOpParkirCctvRealtimes.AddRangeAsync(result, cancellationToken);
                }

                // Simpan perubahan ke database
                await context.SaveChangesAsync(cancellationToken);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDbJasnitaRealtime DB Jasnita updated untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} ({result.Count} data)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDbJasnitaRealtime Update DB Jasnita NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita close conenction {op.Nop};{op.NamaOp};{op.CctvId}");
            }
        }
        private async Task UpdateDBJasnitaRealtimeV2(DataCctv.DataOpCctv op,List<RekapJasnita> dataList,CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                int insertedCount = 0;

                foreach (var item in dataList)
                {
                    // Cek apakah ID sudah ada
                    bool exists = await context.TOpParkirCctvRealtimes
                        .AnyAsync(x => x.Id == item.Id && x.Nop == item.Nop && x.CctvId == item.CctvId, cancellationToken);

                    if (!exists)
                    {
                        var res = new TOpParkirCctvRealtime
                        {
                            Id = item.Id,
                            Nop = item.Nop,
                            CctvId = item.CctvId,
                            VendorId = Convert.ToInt32(item.Vendor),
                            JenisKend = item.JenisKend,
                            PlatNo = item.PlatNo,
                            WaktuMasuk = item.WaktuMasuk
                        };

                        await context.TOpParkirCctvRealtimes.AddAsync(res, cancellationToken);
                        insertedCount++;
                    }
                }

                if (insertedCount > 0)
                {
                    await context.SaveChangesAsync(cancellationToken);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Realtime insert-only untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} ({insertedCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaRealtimeV2 Update DB Jasnita Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Realtime close connection {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
            }
        }
        private async Task UpdateDBJasnitaRealtimeImageV2(DataCctv.DataOpCctv op, List<RekapJasnitaImage> dataList, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                // Ambil ID yang sudah ada untuk NOP dan Vendor ini (pakai HashSet biar cepat)
                var existingIds = new HashSet<string>(
                    await context.TOpParkirCctvRealtimeDoks
                        .Where(x => x.IdNavigation.Nop == op.Nop && x.IdNavigation.VendorId == (int)op.Vendor)
                        .Select(x => x.Id)
                        .ToListAsync(cancellationToken)
                );

                int insertedCount = 0;

                foreach (var item in dataList)
                {
                    // Skip jika ID sudah ada
                    if (existingIds.Contains(item.Id))
                        continue;

                    try
                    {
                        using var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                        string url = item.Url;
                        var response = await httpClient.GetAsync(url, cancellationToken);

                        if (!response.IsSuccessStatusCode)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{DateTime.Now} {op.Nop};{op.NamaOp};{item.Id} UpdateDBJasnitaRealtimeImageV2 Gagal ambil snapshot: {response.StatusCode}");
                            Console.ResetColor();

                            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                            {
                                await GenerateTokenJasnita();
                            }

                            continue;
                        }

                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

                        var newData = new TOpParkirCctvRealtimeDok
                        {
                            Id = item.Id,
                            ImageData = imageBytes
                        };

                        // langsung insert ke DB
                        await context.TOpParkirCctvRealtimeDoks.AddAsync(newData, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        insertedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error insert UpdateDBJasnitaRealtimeImageV2 {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                        Console.ResetColor();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Selesai update DB Jasnita Realtime Image NOP {op.Nop};{op.NamaOp};{op.CctvId} (Insert {insertedCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaRealtimeImageV2 Update DB Jasnita Realtime Image NOP {op.Nop};{op.NamaOp};{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                await context.Database.CloseConnectionAsync();

                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita Realtime Image close connection {op.Nop};{op.NamaOp};{op.CctvId}");
                //Console.ResetColor();
            }
        }



        private async Task GenerateTokenJasnita()
        {
            using (var client = new HttpClient())
            {
                // Basic Auth
                var byteArray = Encoding.ASCII.GetBytes($"{_USER_JASNITA}:{_PASS_JASNITA}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Headers
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                var body = new
                {
                    method = "axxonsoft.bl.auth.AuthenticationService.AuthenticateEx",
                    data = new
                    {
                        user_name = _USER_JASNITA,
                        password = _PASS_JASNITA
                    }
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("http://202.146.133.26/grpc", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonObject.Parse(result);

                    _TOKEN_JASNITA = obj?["token_value"]?.ToString() ?? ""; // ambil token
                }
                else
                {
                    throw new Exception("Gagal ambil cancellationToken. Status: " + response.StatusCode);
                }
            }
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
        #endregion




        #region Utility
        public EnumFactory.EJenisKendParkirCCTV GetJenisKendaraan(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "CAR":
                    return EnumFactory.EJenisKendParkirCCTV.Mobil;
                case "MOTORCYCLE":
                    return EnumFactory.EJenisKendParkirCCTV.Motor;
                case "TRUCK":
                    return EnumFactory.EJenisKendParkirCCTV.Truck;
                case "BUS":
                    return EnumFactory.EJenisKendParkirCCTV.Bus;
                default:
                    return EnumFactory.EJenisKendParkirCCTV.Unknown;
            }
        }
        public EnumFactory.CctvParkirDirection GetDirection(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "INCOMING":
                    return EnumFactory.CctvParkirDirection.Incoming;
                case "OUTGOING":
                    return EnumFactory.CctvParkirDirection.Outgoing;
                default:
                    return EnumFactory.CctvParkirDirection.Unknown;
            }
        }
        public EnumFactory.EStatusCCTV GetStatusCctv(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "IPDS_SIGNAL_RESTORED":
                    return EnumFactory.EStatusCCTV.Aktif;
                default:
                    return EnumFactory.EStatusCCTV.NonAktif;
            }
        }
        public static DateTime ParseFlexibleDate(string timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr))
            {
                throw new ArgumentException("timeStr tidak boleh kosong");
            }

            var formats = new[]
            {
        "yyyyMMdd'T'HHmmss.ffffff",  // 20250924T083638.867000
        "yyyyMMdd'T'HHmmss.fff",     // 20250924T083638.867
        "yyyyMMdd'T'HHmmss",         // ✅ 20250925T073926 (kasus kamu)
        "yyyy-MM-dd'T'HH:mm:ss.ffffff",
        "yyyy-MM-dd'T'HH:mm:ss.fff",
        "yyyy-MM-dd'T'HH:mm:ss",
        "yyyyMMddHHmmss",            // 20250925073926 (tanpa 'T')
        "yyyy-MM-dd HH:mm:ss",       // 2025-09-25 07:39:26
        "yyyy/MM/dd HH:mm:ss",       // 2025/09/25 07:39:26
        "yyyyMMdd'T'HHmmssZ",        // 20250925T073926Z
        "yyyy-MM-dd'T'HH:mm:ssZ",    // 2025-09-25T07:39:26Z
        "yyyy-MM-dd'T'HH:mm:ssK"     // 2025-09-25T07:39:26+07:00
    };

            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(
                    timeStr,
                    fmt,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal,
                    out DateTime result))
                {
                    return result;
                }
            }

            throw new Exception(timeStr + " tidak sesuai format yang dikenali.");
        }
        private static string ConvertSseOutputJson(string output)
        {
            try
            {
                var lines = output.Split('\n')
                            .Where(l => l.StartsWith("data:"))
                            .Select(l => l.Substring("data:".Length).Trim())
                            .ToList();
                var dtResponse = lines[0];

                return dtResponse;
            }
            catch (Exception ex)
            {
                return output;
            }
        }
        private static string ConvertSseOutputJson2(string output)
        {
            try
            {
                var jsonObjects = output.Split('\n')
                    .Where(l => l.StartsWith("data:"))
                    .Select(l => l.Substring("data:".Length).Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Where(l => !l.Contains("\"items\":[]"))
                    .ToList();

                if (jsonObjects.Count == 0)
                    return "[]";

                var jsonArray = "[" + string.Join(",", jsonObjects) + "]";
                return jsonArray;
            }
            catch (Exception)
            {
                return output;
            }
        }
        private static string GetFullExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            int level = 0;

            while (ex != null)
            {
                // Pilih warna berdasarkan level
                var color = level switch
                {
                    0 => ConsoleColor.Red,
                    1 => ConsoleColor.Yellow,
                    2 => ConsoleColor.Cyan,
                    _ => ConsoleColor.Gray
                };

                // Tulis ke console langsung (lebih interaktif)
                Console.ForegroundColor = color;
                Console.WriteLine($"[Level {level}] {ex.GetType().Name}: {ex.Message}");
                Console.ResetColor();

                // Simpan juga di string builder kalau mau di-log ke file
                sb.AppendLine($"[Level {level}] {ex.GetType().Name}: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    sb.AppendLine(ex.StackTrace);
                }

                ex = ex.InnerException;
                level++;
            }

            return sb.ToString();
        }
        // Konversi dari UTC (+0) ke WIB (+7)
        public static DateTime ConvertUtcToWib(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(utcTime, DateTimeKind.Utc),
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
        }

        // Konversi dari WIB (+7) ke UTC (+0)
        public static DateTime ConvertWibToUtc(DateTime wibTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(wibTime, DateTimeKind.Local)
            );
        }
        #endregion



    }
}
