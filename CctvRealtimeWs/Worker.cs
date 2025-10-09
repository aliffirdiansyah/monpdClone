using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CctvRealtimeWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public string _USER_JASNITA;
        public string _PASS_JASNITA;
        public string _TOKEN_JASNITA;
        public string _USER_TELKOM;
        public string _PASS_TELKOM;
        public string _TOKEN_TELKOM;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _USER_JASNITA = "bapendasby";
            _PASS_JASNITA = "surabaya2025!!";

            _USER_TELKOM = "pemkot_surabaya_va";
            _PASS_TELKOM = "P3mk0tSuR4b4Ya";

            var prevDataList = new List<DataCctv.DataOpCctv>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var dataList = await DataCctv.GetDataOpCctvAsync();

                    // Deteksi CCTV baru (opsional, untuk logging saja)
                    var newItems = dataList
                        .Where(d => !prevDataList.Any(p => p.CctvId == d.CctvId && p.Nop == d.Nop))
                        .ToList();

                    if (newItems.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Ada {newItems.Count} CCTV baru terdeteksi.");
                        Console.ResetColor();
                    }

                    // Selalu proses SEMUA CCTV (bukan hanya yang baru)
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Memproses {dataList.Count} CCTV aktif...");

                    var tasks = dataList.Select(data =>
                        ProcessDataAsync(data, stoppingToken)
                    ).ToList();

                    await Task.WhenAll(tasks);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Selesai memproses semua CCTV.");
                    Console.ResetColor();

                    // Update cache CCTV lama
                    prevDataList = dataList;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task dibatalkan, menghentikan service...");
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] di ExecuteAsync: {GetFullExceptionMessage(ex)}");
                    Console.ResetColor();
                }

                // Delay 5 detik sebelum loop berikutnya
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Menunggu 5 detik sebelum pengecekan berikutnya...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }


        private async Task ProcessDataAsync(DataCctv.DataOpCctv op, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [START] PROCESS: {op.Nop}-{op.NamaOp}-{op.CctvId}");

            try
            {
                if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Jasnita)
                {
                    await CallApiJasnitaAsync(op, cancellationToken);
                }
                else if (op.Vendor == MonPDLib.General.EnumFactory.EVendorParkirCCTV.Telkom)
                {
                    await CallApiTelkomAsync(op, cancellationToken);
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Vendor tidak dikenali untuk {op.Nop}-{op.NamaOp}-{op.CctvId}");
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] FINISHED PROCESS {op.Nop}-{op.NamaOp}-{op.CctvId}");
                Console.ResetColor();

                // Delay kecil antar proses (biar API tidak kena rate limit)
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {op.Nop}-{op.NamaOp}-{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
        }
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

            DateTime tglAwal = DateTime.Today;
            DateTime tglAkhir = DateTime.Today.AddDays(1).AddTicks(-1);

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
                                    begin_time = tglAwal.ToString("yyyyMMdd'T'HHmmss.'000'"),
                                    end_time = tglAkhir.ToString("yyyyMMdd'T'HHmmss.'999'")
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
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop}-{op.NamaOp}-{op.CctvId} Token kadaluarsa, refresh token dan ulangi...");
                            await GenerateTokenJasnita();
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop}-{op.NamaOp}-{op.CctvId} Error: {response.StatusCode}");
                        }

                        attempt++;
                        if (attempt >= maxRetry)
                        {
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {op.Nop}-{op.NamaOp}-{op.CctvId} Gagal setelah {maxRetry} percobaan pada offset {offset}");
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
                await UpdateDbJasnita(op, result, cancellationToken);
            }
        }
        private async Task CallApiTelkomAsync(DataCctv.DataOpCctv op, CancellationToken cancellationToken)
        {
            await GenerateTokenTelkom();
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);

            string dateStr = DateTime.Now.Date.ToString("yyyy-MM-dd");
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
                await UpdateDbTelkom(op, data, cancellationToken);
            }
        }

        private async Task UpdateDbJasnita(
            DataCctv.DataOpCctv op,
            List<EventAll.EventAllResponse> dataList,
            CancellationToken cancellationToken
            )
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

                                var direction = GetDirection(ar.Direction);

                                if(direction == EnumFactory.CctvParkirDirection.Incoming)
                                {
                                    DateTime waktuMasuk = ParseFlexibleDate(ar.TimeBegin);
                                    var id = $"{seq}{(int)(EnumFactory.EVendorParkirCCTV.Jasnita)}{op.Nop}{waktuMasuk.ToString("yyyyMMddHHmmss")}";

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
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita updated untuk NOP {op.Nop}-{op.NamaOp}-{op.CctvId} ({result.Count} data)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Update DB Jasnita NOP {op.Nop}-{op.NamaOp}-{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Jasnita close conenction {op.Nop}-{op.NamaOp}-{op.CctvId}");
            }
        }

        private async Task UpdateDbTelkom(
            DataCctv.DataOpCctv op,
            TelkomEvent.TelkomEventResponse dataList,
            CancellationToken cancellationToken)
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

                int seq = 1;
                foreach (var item in dataList.Result)
                {

                    string? platNomor = item.PlatNomor?.ToUpper() == "UNRECOGNIZED" ? null : item.PlatNomor?.ToUpper();
                    DateTime waktuMasuk = ParseFlexibleDate(item.Timestamp);
                    var id = $"{seq}{(int)(EnumFactory.EVendorParkirCCTV.Telkom)}{op.Nop}{waktuMasuk.ToString("yyyyMMddHHmmss")}";
                    var res = new TOpParkirCctvRealtime();

                    res.Id = id;
                    res.Nop = op.Nop;
                    res.CctvId = op.CctvId ?? "";
                    res.VendorId = (int)op.Vendor;
                    res.JenisKend = (int)GetJenisKendaraan(item.TipeKendaraan);
                    res.PlatNo = platNomor ?? "";
                    res.WaktuMasuk = waktuMasuk;
                    res.ImageUrl = item.Image;

                    result.Add(res);

                    seq++;
                }

                if (result.Count > 0)
                {
                    await context.TOpParkirCctvRealtimes.AddRangeAsync(result, cancellationToken);
                }

                // Simpan perubahan ke database
                await context.SaveChangesAsync(cancellationToken);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Telkom updated untuk NOP {op.Nop}-{op.NamaOp}-{op.CctvId} ({result.Count} data)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}][Error] Update DB Telkom NOP {op.Nop}-{op.NamaOp}-{op.CctvId}: {GetFullExceptionMessage(ex)}");
                Console.ResetColor();
            }
            finally
            {
                // === CLOSE CONNECTION ===
                await context.Database.CloseConnectionAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Telkom close conenction {op.Nop}-{op.NamaOp}-{op.CctvId}");
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
        private static string GetFullExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            int level = 0;

            while (ex != null)
            {
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
    }
}
