using CctvRealtimeWs.JasnitaModels;
using CctvRealtimeWs.TelkomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utl = CctvRealtimeWs.Utility.Utility;

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
        public string _DOMAIN_JASNITA_2 = null!;
        public string _USER_TELKOM = null!;
        public string _PASS_TELKOM = null!;
        public string _TOKEN_TELKOM = null!;
        private List<DataCctv.DataOpCctv> _dataList = new();
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
            _dataList = await DataCctv.GetDataOpCctvAsync();

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Menjalankan proses {_dataList.Count} CCTV aktif...");

            // Jalankan masing-masing CCTV dalam task sendiri (loop internal)
            var tasks = _dataList.Select(data => Task.Run(() => ProcessDataAsync(data, stoppingToken), stoppingToken)).ToList();

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

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] FINISHED PROCESS {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();

                    // Tunggu 30 detik sebelum mulai loop berikutnya
                    await Task.Delay(TimeSpan.FromSeconds(300), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [STOP] Dibatalkan: {op.Nop};{op.NamaOp};{op.CctvId}");
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
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
                        DateTime waktuMasuk = Utl.ParseFlexibleDate(item.Timestamp);
                        var id = $"{op.Nop}/{item.Id.ToString()}";

                        var res = new RekapTelkom();

                        res.Id = id;
                        res.Nop = op.Nop;
                        res.CctvId = op.CctvId ?? "";
                        res.NamaOp = op.NamaOp;
                        res.AlamatOp = op.AlamatOp;
                        res.WilayahPajak = op.WilayahPajak;
                        res.WaktuMasuk = waktuMasuk;
                        res.JenisKend = (int)Utl.GetJenisKendaraan(item.TipeKendaraan);
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
                        //await UpdateDbTelkomRealtime(op, rekapResult, cancellationToken);
                        //await UpdateDBTelkomRekap(op, rekapResult, tanggal, cancellationToken);

                        await UpdateDBTelkomCombined(op, rekapResult, tanggal, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] CallApiTelkomAsync {op.NamaOp};{op.AlamatOp};{op.Nop}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
        }
        private async Task UpdateDBTelkomRekap(DataCctv.DataOpCctv op, List<RekapTelkom> dataList, DateTime tanggal, CancellationToken cancellationToken)
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

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBTelkomRekap Inserted {result.Count} new Telkom Rekap data untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBTelkomRekap Tidak ada data baru untuk Telkom Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBTelkomRekap Update DB Telkom Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();

            }
        }
        private async Task UpdateDbTelkomRealtime(DataCctv.DataOpCctv op, List<RekapTelkom> dataList, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                var backDateDataList = await context.TOpParkirCctvRealtimes
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor && x.WaktuMasuk.Date < DateTime.Now.Date)
                    .ToListAsync(cancellationToken);

                // Hapus data lama beserta dok-nya
                foreach (var item in backDateDataList)
                {
                    if (item.TOpParkirCctvRealtimeDok != null)
                    {
                        context.TOpParkirCctvRealtimeDoks.Remove(item.TOpParkirCctvRealtimeDok);
                    }
                    context.TOpParkirCctvRealtimes.Remove(item);
                }

                context.TOpParkirCctvRealtimes.RemoveRange(backDateDataList);

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
                await context.SaveChangesAsync(cancellationToken);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDbTelkomRealtime DB Telkom Realtime updated untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} (insert {insertCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDbTelkomRealtime Update DB Telkom Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
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

        private async Task UpdateDBTelkomCombined(DataCctv.DataOpCctv op,List<RekapTelkom> dataList,DateTime tanggal,CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                await context.Database.OpenConnectionAsync(cancellationToken);
                await context.Database.BeginTransactionAsync(cancellationToken);

                // -----------------------------
                // 1. REaltime
                // -----------------------------
                var backDateDataList = await context.TOpParkirCctvRealtimes
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor && x.WaktuMasuk.Date < DateTime.Now.Date)
                    .ToListAsync(cancellationToken);

                // Hapus data lama beserta dok-nya
                foreach (var item in backDateDataList)
                {
                    if (item.TOpParkirCctvRealtimeDok != null)
                        context.TOpParkirCctvRealtimeDoks.Remove(item.TOpParkirCctvRealtimeDok);

                    context.TOpParkirCctvRealtimes.Remove(item);
                }

                // Ambil existing IDs Realtime
                var existingIdsRealtime = new HashSet<string>(
                    await context.TOpParkirCctvRealtimes
                        .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor)
                        .Select(x => x.Id)
                        .ToListAsync(cancellationToken)
                );

                int insertedRealtimeCount = 0;
                foreach (var item in dataList)
                {
                    if (existingIdsRealtime.Contains(item.Id))
                        continue;

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
                    insertedRealtimeCount++;
                }

                // -----------------------------
                // 2. Rekap
                // -----------------------------
                var existingIdsRekap = new HashSet<string>(
                    await context.TOpParkirCctvs
                        .Where(x => x.Nop == op.Nop && x.WaktuMasuk.Date == tanggal.Date && x.Vendor == (int)op.Vendor)
                        .Select(x => x.Id)
                        .ToListAsync(cancellationToken)
                );

                var rekapResult = new List<TOpParkirCctv>();
                foreach (var item in dataList.Where(x => !existingIdsRekap.Contains(x.Id)))
                {
                    rekapResult.Add(new TOpParkirCctv
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
                        Vendor = (int)op.Vendor
                    });
                }

                if (rekapResult.Count > 0)
                    await context.TOpParkirCctvs.AddRangeAsync(rekapResult, cancellationToken);

                // -----------------------------
                // 3. Simpan semua perubahan
                // -----------------------------
                await context.SaveChangesAsync(cancellationToken);
                await context.Database.CommitTransactionAsync();

                // -----------------------------
                // 4. Logging
                // -----------------------------
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Telkom Combined Update Realtime ({insertedRealtimeCount} new) & Rekap ({rekapResult.Count} new) untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Telkom Combined Update NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
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
                            var apiResponse = Utl.ConvertSseOutputJson(rawResponse);
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
            string _DOMAIN_JASNITA_2 = "";
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

                _DOMAIN_JASNITA_2 = rawWebClientUrl;
            }
            if (string.IsNullOrEmpty(_DOMAIN_JASNITA_2))
            {
                // Jika _DOMAIN_JASNITA_2 kosong, hentikan proses
                throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} Gagal mendapatkan webClientURL dari API Jasnita Domains.");
            }
            if (!_DOMAIN_JASNITA_2.Contains("https"))
            {
                // Jika _DOMAIN_JASNITA_2 tidak valid, hentikan proses
                throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp} webClientURL tidak valid: {_DOMAIN_JASNITA_2}");
            }
            #endregion

            var eventResult = new List<EventV2.Event>();
            #region Events
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

                string timestamp_start = Utl.ConvertWibToUtc(DateTime.Now.Date).ToString("yyyyMMdd'T'HHmmss.'000'");
                string timestamp_end = Utl.ConvertWibToUtc(DateTime.Now.AddDays(1).AddTicks(-1)).ToString("yyyyMMdd'T'HHmmss.'000'");
                string cameraId = op.DisplayId ?? "";
                string url = $"{_DOMAIN_JASNITA_2}/archive/events/detectors/{cameraId}/{timestamp_start}/{timestamp_end}";

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
                string url = $"{_DOMAIN_JASNITA_2}/archive/media/{item.Source.Replace("hosts/", "")}/{item.Timestamp}?crop_x={item.Rectangles[0].Left}&crop_y={item.Rectangles[0].Top}&crop_width=0.1&crop_height=0.1";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{DateTime.Now} {op.Nop};{op.NamaOp};{item.Id} Gagal memanggil API Jasnita Event Snapshot. Status: {response.StatusCode}");
                }

                // Baca stream image-nya
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                DateTime parsedTime = Utl.ConvertUtcToWib(Utl.ParseFlexibleDate(item.Timestamp));

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
                await GenerateDomainJasnita2();

                var rekapJasnita = new List<RekapJasnita>();

                string accessPoint = "";
                #region Cameras
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");
                    // Panggil API Jasnita untuk ambil daftar kamera
                    HttpResponseMessage response = await httpClient.GetAsync($"{_DOMAIN_JASNITA_2}/camera/list", cancellationToken);
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
                                HttpResponseMessage response = await httpClient.PostAsync($"{_DOMAIN_JASNITA_2}/grpc", content, cancellationToken);

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
                                    await GenerateTokenJasnita2();
                                    await GenerateDomainJasnita2();
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

                                DateTime waktuMasuk = Utl.ConvertUtcToWib(Utl.ParseFlexibleDate(ar.TimeBegin));
                                var id = $"{op.Nop}/{item.Body.Guid}";

                                if (waktuMasuk.Date == DateTime.Now.Date)
                                {
                                    // Ambil snapshot image dari API Jasnita
                                    using var httpClient = new HttpClient();
                                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

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
                                    string url = $"{_DOMAIN_JASNITA_2}/archive/media/{accessPoint}/{timestamp}?crop_x={cropX}&crop_y={cropY}&crop_width={cropWidth}&crop_height={cropHeight}";
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
                    //await UpdateDBJasnitaRealtimeV2(op, rekapResult, cancellationToken);
                    //await UpdateDBJasnitaRealtimeImageV2(op, rekapImageResult, cancellationToken);

                    //await UpdateDBJasnitaRekap(op, rekapResult, tanggal, cancellationToken);
                    //await UpdateDBJasnitaRekapImageV2(op, rekapImageResult, tanggal, cancellationToken);

                    await UpdateDBJasnitaCombined(op, rekapResult, tanggal, cancellationToken);
                    await UpdateDBJasnitaCombinedImageOptimized(op, rekapImageResult, tanggal, cancellationToken);

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] CallApiJasnitaV2GrpcAsync {op.DisplayId};{op.AccessPoint};{op.NamaOp};{op.AlamatOp};{op.Nop}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
        }
        private async Task UpdateDBJasnitaRekap(DataCctv.DataOpCctv op, List<RekapJasnita> dataList, DateTime tanggal, CancellationToken cancellationToken)
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
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBJasnitaRekap Inserted {result.Count} new Jasnita Rekap data untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
                else
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBJasnitaRekap Tidak ada data baru untuk Jasnita Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaRekap Update DB Jasnita Rekap NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                await context.Database.CloseConnectionAsync();

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
                            await GenerateTokenJasnita2();
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
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Update DB Jasnita Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
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

        private async Task UpdateDbJasnitaRealtime(DataCctv.DataOpCctv op, List<EventAll.EventAllResponse> dataList, CancellationToken cancellationToken)
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

                                DateTime waktuMasuk = Utl.ConvertUtcToWib(Utl.ParseFlexibleDate(ar.TimeBegin));
                                var id = $"{seq}{(int)(EnumFactory.EVendorParkirCCTV.Jasnita)}{op.Nop}{waktuMasuk.ToString("yyyyMMddHHmmss")}";

                                if (waktuMasuk.Date == DateTime.Now.Date)
                                {
                                    var res = new TOpParkirCctvRealtime();

                                    res.Id = id.ToString();
                                    res.Nop = op.Nop;
                                    res.CctvId = op.CctvId ?? "";
                                    res.VendorId = (int)op.Vendor;
                                    res.JenisKend = (int)Utl.GetJenisKendaraan(ar.VehicleClass);
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

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDbJasnitaRealtime DB Jasnita updated untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} ({result.Count} data)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDbJasnitaRealtime Update DB Jasnita NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();
                //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita close conenction {op.Nop};{op.NamaOp};{op.CctvId}");
            }
        }
        private async Task UpdateDBJasnitaRealtimeV2(DataCctv.DataOpCctv op, List<RekapJasnita> dataList, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);

                var backDateDataList = await context.TOpParkirCctvRealtimes
                    .Include(x => x.TOpParkirCctvRealtimeDok)
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor && x.WaktuMasuk.Date < DateTime.Now.Date)
                    .ToListAsync(cancellationToken);

                // Hapus data lama beserta dok-nya
                foreach (var item in backDateDataList)
                {
                    if (item.TOpParkirCctvRealtimeDok != null)
                    {
                        context.TOpParkirCctvRealtimeDoks.Remove(item.TOpParkirCctvRealtimeDok);
                    }
                    context.TOpParkirCctvRealtimes.Remove(item);
                }


                int insertedCount = 0;
                foreach (var item in dataList)
                {
                    // Cek apakah ID sudah ada
                    bool exists = await context.TOpParkirCctvRealtimes.AnyAsync(x => x.Id == item.Id, cancellationToken);

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

                // Simpan perubahan ke database
                await context.SaveChangesAsync(cancellationToken);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBJasnitaRealtimeV2 DB Jasnita Realtime insert-only untuk NOP {op.Nop};{op.NamaOp};{op.CctvId} ({insertedCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaRealtimeV2 Update DB Jasnita Realtime NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
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
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error insert UpdateDBJasnitaRealtimeImageV2 {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                        Console.ResetColor();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] UpdateDBJasnitaRealtimeImageV2 Selesai update DB Jasnita Realtime Image NOP {op.Nop};{op.NamaOp};{op.CctvId} (Insert {insertedCount} data baru)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaRealtimeImageV2 Update DB Jasnita Realtime Image NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
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


        private async Task UpdateDBJasnitaCombined(DataCctv.DataOpCctv op, List<RekapJasnita> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();

            try
            {
                // === OPEN CONNECTION ===
                await context.Database.OpenConnectionAsync(cancellationToken);
                await context.Database.BeginTransactionAsync(cancellationToken);

                // -----------------------------
                // 1. UPDATE REALTIME
                // -----------------------------
                var backDateDataList = await context.TOpParkirCctvRealtimes
                    .Include(x => x.TOpParkirCctvRealtimeDok)
                    .Where(x => x.Nop == op.Nop && x.VendorId == (int)op.Vendor && x.WaktuMasuk.Date < DateTime.Now.Date)
                    .ToListAsync(cancellationToken);

                // Hapus data lama beserta dok-nya
                foreach (var item in backDateDataList)
                {
                    if (item.TOpParkirCctvRealtimeDok != null)
                    {
                        context.TOpParkirCctvRealtimeDoks.Remove(item.TOpParkirCctvRealtimeDok);
                    }
                    context.TOpParkirCctvRealtimes.Remove(item);
                }

                int insertedRealtimeCount = 0;
                foreach (var item in dataList)
                {
                    bool exists = await context.TOpParkirCctvRealtimes.AnyAsync(x => x.Id == item.Id, cancellationToken);

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
                        insertedRealtimeCount++;
                    }
                }

                // -----------------------------
                // 2. UPDATE REKAP
                // -----------------------------
                var existingIds = await context.TOpParkirCctvs
                    .Where(x => x.Nop == op.Nop && x.WaktuMasuk.Date == tanggal.Date && x.Vendor == (int)op.Vendor)
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                var rekapResult = new List<TOpParkirCctv>();

                foreach (var item in dataList)
                {
                    if (!existingIds.Contains(item.Id))
                    {
                        rekapResult.Add(new TOpParkirCctv
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
                        });
                    }
                }

                if (rekapResult.Count > 0)
                {
                    await context.TOpParkirCctvs.AddRangeAsync(rekapResult, cancellationToken);
                }

                // -----------------------------
                // 3. SAVE CHANGES
                // -----------------------------
                await context.SaveChangesAsync(cancellationToken);
                await context.Database.CommitTransactionAsync();

                // -----------------------------
                // 4. LOGGING
                // -----------------------------
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Combined Update Jasnita Realtime ({insertedRealtimeCount} new) & Rekap ({rekapResult.Count} new) untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaCombined NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }
        private async Task UpdateDBJasnitaCombinedImageOptimized(DataCctv.DataOpCctv op, List<RekapJasnitaImage> dataList, DateTime tanggal, CancellationToken cancellationToken)
        {
            await using var context = DBClass.GetContext();
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);

            try
            {
                await context.Database.OpenConnectionAsync(cancellationToken);
                await context.Database.BeginTransactionAsync(cancellationToken);

                // Ambil ID existing Realtime & Rekap
                var existingIdsRealtime = new HashSet<string>(
                    await context.TOpParkirCctvRealtimeDoks
                        .Where(x => x.IdNavigation.Nop == op.Nop && x.IdNavigation.VendorId == (int)op.Vendor)
                        .Select(x => x.Id)
                        .ToListAsync(cancellationToken)
                );

                var existingIdsRekap = new HashSet<string>(
                    await context.TOpParkirCctvDoks
                        .Where(x => x.TOpParkirCctv.Nop == op.Nop &&
                                    x.TOpParkirCctv.Vendor == (int)op.Vendor &&
                                    x.TOpParkirCctv.WaktuMasuk.Date == tanggal.Date)
                        .Select(x => x.Id)
                        .ToListAsync(cancellationToken)
                );

                int insertedRealtimeCount = 0;
                var rekapResult = new List<TOpParkirCctvDok>();

                // Filter dataList → hanya ID baru
                var newDataList = dataList
                    .Where(x => !existingIdsRealtime.Contains(x.Id) || !existingIdsRekap.Contains(x.Id))
                    .ToList();

                foreach (var item in newDataList)
                {
                    byte[]? imageBytes = await DownloadImageWithRetry(httpClient, item.Url, 3, cancellationToken);
                    if (imageBytes == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{DateTime.Now} {op.Nop};{op.NamaOp};{item.Id} Gagal ambil snapshot setelah 3 percobaan");
                        Console.ResetColor();
                        continue; // skip jika tetap gagal
                    }

                    // Insert Realtime jika belum ada
                    if (!existingIdsRealtime.Contains(item.Id))
                    {
                        await context.TOpParkirCctvRealtimeDoks.AddAsync(new TOpParkirCctvRealtimeDok
                        {
                            Id = item.Id,
                            ImageData = imageBytes
                        }, cancellationToken);
                        insertedRealtimeCount++;
                    }

                    // Insert Rekap jika belum ada
                    if (!existingIdsRekap.Contains(item.Id))
                    {
                        rekapResult.Add(new TOpParkirCctvDok
                        {
                            Id = item.Id,
                            Nop = item.Nop,
                            CctvId = item.CctvId,
                            ImageData = imageBytes
                        });
                    }
                }

                if (rekapResult.Count > 0)
                    await context.TOpParkirCctvDoks.AddRangeAsync(rekapResult, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
                await context.Database.CommitTransactionAsync();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Update Combined Image Realtime ({insertedRealtimeCount} new) & Rekap ({rekapResult.Count} new) untuk NOP {op.Nop};{op.NamaOp};{op.CctvId}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] UpdateDBJasnitaCombinedImageFiltered NOP {op.Nop};{op.NamaOp};{op.CctvId}: {Utl.GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }

        private async Task<byte[]?> DownloadImageWithRetry(HttpClient httpClient, string url, int maxRetry = 1, CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            while (attempt < maxRetry)
            {
                attempt++;
                try
                {
                    var response = await httpClient.GetAsync(url, cancellationToken);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsByteArrayAsync(cancellationToken);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{DateTime.Now} Warning: Attempt {attempt} failed for {url} - {response.StatusCode}");
                    Console.ResetColor();

                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await GenerateTokenJasnita2();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{DateTime.Now} Warning: Attempt {attempt} exception for {url} - {ex.Message}");
                    Console.ResetColor();
                }

                // delay before retry, exponential backoff
                await Task.Delay(500 * attempt, cancellationToken);
            }

            // jika tetap gagal setelah maxRetry
            return null;
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
        private async Task GenerateDomainJasnita2()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN_JASNITA_2);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

            // Panggil API Jasnita untuk ambil daftar domain
            HttpResponseMessage response = await httpClient.GetAsync(
                "https://hub.jastrak.id/api/v3/ac-backend/domains");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gagal memanggil API Jasnita Domains. Status: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonObject.Parse(jsonResponse);

            string rawWebClientUrl = jsonObject?["domains"]?[0]?["domain"]?["webClientURL"]?.ToString() ?? "";

            if (!string.IsNullOrWhiteSpace(rawWebClientUrl))
            {
                if (rawWebClientUrl.StartsWith("//"))
                    rawWebClientUrl = "https:" + rawWebClientUrl;

                rawWebClientUrl = rawWebClientUrl.TrimEnd('/');
            }

            if (string.IsNullOrEmpty(rawWebClientUrl) || !rawWebClientUrl.Contains("https"))
                throw new Exception($"webClientURL tidak valid: {rawWebClientUrl}");

            _DOMAIN_JASNITA_2 = rawWebClientUrl;
        }
        #endregion






    }
}
