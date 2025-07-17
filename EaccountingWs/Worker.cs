using Microsoft.EntityFrameworkCore.Design;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EaccountingWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //var now = DateTime.Now;

                //var nextRun = now.AddDays(1); // besok jam 00:00
                //var delay = nextRun - now;

                //_logger.LogInformation("Next run scheduled at: {time}", nextRun);

                //await Task.Delay(delay, stoppingToken);

                //if (stoppingToken.IsCancellationRequested)
                //    break;

                // Eksekusi tugas
                try
                {
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR PBB WS",
                    $@"
                            Terjadi exception pada sistem:

                            Pesan Error       : {ex.Message}
                            Tipe Exception    : {ex.GetType().FullName}
                            Source            : {ex.Source}
                            Method            : {ex.TargetSite}
                            Stack Trace       :
                            {ex.StackTrace}

                            Inner Exception   :
                            {ex.InnerException?.Message}
                            {ex.InnerException?.StackTrace}
                            ",
                        null
                    );
                }
            }
        }

        private async Task DoWorkFullScanAsync(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == -1);
            if (thnSetting != null)
            {
                var temp = tglServer.Year - (int)thnSetting.YearBefore;
                if (temp >= 2023)
                {
                    tahunAmbil = temp;
                }
                else
                {
                    tahunAmbil = 2023;
                }
            }
            else
            {
                tahunAmbil = 2023;
            }


            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                string username = "bapenda";
                string password = "Bapenda@sby2022";
                string url = $"https://eaccounting.surabaya.go.id/api-eaccounting{thn}/mutasi";
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("username", username);
                    httpClient.DefaultRequestHeaders.Add("password", password);

                    Console.WriteLine($"eaccounting_api: connecting to {url}");
                    HttpResponseMessage response = httpClient.PostAsync(url, null).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to connect to eaccounting API: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    Console.WriteLine($"eaccounting_api: connected {url}");
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    var result = JsonSerializer.Deserialize<List<API_RESPONSE>>(responseBody);

                    if(result != null)
                    {
                        if(result.Count > 0)
                        {
                            Console.WriteLine($"eaccounting_api: jumlah data {result.Count}");

                            var dataExisting = _contMonPd.TSeriesPendapatans.Where(x => x.Tahun == thn).ToList();
                            _contMonPd.TSeriesPendapatans.RemoveRange(dataExisting);


                            //{ 'NoBukti', 'TgBukti', 'KdRinci', 'KdKegiatan', 'IdSubkegiatan', 'KdOrganisasi', 'Kode', 'NoDokumen'}

                            var groupedData =
                                result.GroupBy(x => new { x.NOBUKTI, x.TGBUKTI, x.KDRINCI, x.KDKEGIATAN, x.IDSUBKEGIATAN, x.KDORGANISASI, x.KODE, x.NODOKUMEN })
                                .Select(x => new
                                {
                                    NOBUKTI = x.Key.NOBUKTI,
                                    TGBUKTI = x.Key.TGBUKTI,
                                    KDRINCI = x.Key.KDRINCI,
                                    KDKEGIATAN = x.Key.KDKEGIATAN,
                                    IDSUBKEGIATAN = x.Key.IDSUBKEGIATAN,
                                    KDORGANISASI = x.Key.KDORGANISASI,
                                    KODE = x.Key.KODE,
                                    NODOKUMEN = x.Key.NODOKUMEN,
                                    URAIAN = string.Join(" ", x.Select(i => i.URAIAN)),
                                    JUMLAH = x.Sum(i => decimal.TryParse(i.JUMLAH ?? "0", System.Globalization.NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var jumlah) ? jumlah : 0),
                                    TAHUN = thn.ToString()
                                }).ToList();

                            foreach (var item in groupedData)
                            {
                                var newRow = new TSeriesPendapatan();
                                newRow.NoBukti = item.NOBUKTI;
                                newRow.TgBukti = DateTime.ParseExact(item.TGBUKTI, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                newRow.KdRinci = item.KDRINCI;
                                newRow.KdKegiatan = item.KDKEGIATAN;
                                newRow.IdSubkegiatan = item.IDSUBKEGIATAN;
                                newRow.KdOrganisasi = item.KDORGANISASI;
                                newRow.Kode = item.KODE;
                                newRow.NoDokumen = item.NODOKUMEN;
                                newRow.Uraian = item.URAIAN;
                                newRow.Jumlah = item.JUMLAH;
                                newRow.Tahun = Convert.ToInt32(item.TAHUN);
                                newRow.InsertDate = DateTime.Now;
                                newRow.InsertBy = "JOB";


                                _contMonPd.TSeriesPendapatans.Add(newRow);
                                Console.WriteLine($"eaccounting_api: insert data {item.NOBUKTI} - {item.TGBUKTI} - {item.KDRINCI} - {item.KDKEGIATAN} - {item.IDSUBKEGIATAN} - {item.KDORGANISASI} - {item.KODE} - {item.NODOKUMEN} - {item.URAIAN} - {item.JUMLAH} - {item.TAHUN}");
                            }

                            _contMonPd.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"eaccounting_api: data saved to database for year {thn}");
                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        public class API_RESPONSE
        {
            [JsonPropertyName("NO_BUKTI")]
            public string NOBUKTI { get; set; } = "";

            [JsonPropertyName("TG_BUKTI")]
            public string TGBUKTI { get; set; } = "";

            [JsonPropertyName("KD_RINCI")]
            public string KDRINCI { get; set; } = "";

            [JsonPropertyName("KD_KEGIATAN")]
            public string KDKEGIATAN { get; set; } = "";

            [JsonPropertyName("ID_SUBKEGIATAN")]
            public int IDSUBKEGIATAN { get; set; }

            [JsonPropertyName("KD_ORGANISASI")]
            public string KDORGANISASI { get; set; } = "";

            [JsonPropertyName("KODE")]
            public string KODE { get; set; } = "";

            [JsonPropertyName("NO_DOKUMEN")]
            public string NODOKUMEN { get; set; } = "";

            [JsonPropertyName("URAIAN")]
            public string URAIAN { get; set; } = "";

            [JsonPropertyName("JUMLAH")]
            public string JUMLAH { get; set; } = "";

            [JsonPropertyName("TAHUN")]
            public string TAHUN { get; set; } = "";
        }
    }
}
