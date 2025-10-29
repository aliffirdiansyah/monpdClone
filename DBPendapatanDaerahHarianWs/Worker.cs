using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DBPendapatanDaerahHarianWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
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
                //DateTime nextRun = now.AddMinutes(30); // besok jam 00:00
                //TimeSpan delay = nextRun - now;

                //Console.WriteLine($"esidatra_api: next run at {nextRun}");
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
                if (temp >= DateTime.Now.Year)
                {
                    tahunAmbil = temp;
                }
                else
                {
                    tahunAmbil = DateTime.Now.Year;
                }
            }
            else
            {
                tahunAmbil = DateTime.Now.Year;
            }


            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                string username = "monpd_api";
                string password = "e10adc3949ba59abbe56e057f20f883e";
                string url = $"https://esidatra.surabaya.go.id/api/MonPdRealisasiPendapatanDaerahHarian{thn}";
                using (HttpClient httpClient = new HttpClient())
                {
                    // Encode username:password ke base64
                    var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    Console.WriteLine($"esidatra_api: connecting to {url}");

                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    string content = response.Content.ReadAsStringAsync().Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to connect to eaccounting API: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    Console.WriteLine($"esidatra_api: connected {url}");
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    var result = JsonSerializer.Deserialize<List<API_RESPONSE>>(responseBody);

                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            Console.WriteLine($"esidatra_api: jumlah data {result.Count}");

                            var dataExisting = _contMonPd.DbPendapatanDaerahHarians.Where(x => x.TahunBuku == thn).ToList();
                            _contMonPd.DbPendapatanDaerahHarians.RemoveRange(dataExisting);

                            int index = 0;
                            int jmlData = result.Count;
                            foreach (var item in result)
                            {
                                var res = new DbPendapatanDaerahHarian();
                                res.TahunBuku = item.TAHUNBUKU;
                                res.Akun = item.AKUN;
                                res.NamaAkun = item.NAMAAKUN;
                                res.Kelompok = item.KELOMPOK;
                                res.NamaKelompok = item.NAMAKELOMPOK;
                                res.Jenis = item.JENIS;
                                res.NamaJenis = item.NAMAJENIS;
                                res.Objek = item.OBJEK;
                                res.NamaObjek = item.NAMAOBJEK;
                                res.Rincian = item.RINCIAN;
                                res.NamaRincian = item.NAMARINCIAN;
                                res.SubRincian = item.SUBRINCIAN;
                                res.NamaSubRincian = item.NAMASUBRINCIAN;
                                res.KodeOpd = item.KODEOPD;
                                res.NamaOpd = item.NAMAOPD;
                                res.KodeSubOpd = item.KODESUBOPD;
                                res.NamaSubOpd = item.NAMASUBOPD;
                                res.Tanggal = item.TANGGAL;
                                res.Target = item.TARGET;
                                res.Realisasi = item.REALISASI;


                                _contMonPd.DbPendapatanDaerahHarians.Add(res);

                                index++;
                                double persen = ((double)index / jmlData) * 100;
                                Console.Write($"\rDB PENDAPATAN DAERAH  JML DATA {jmlData.ToString("n0")} [({persen:F2}%)]");
                            }


                            Console.WriteLine($"\nesidatra_api: saving...");
                            _contMonPd.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"esidatra_api: data saved to database for year {thn}");
                            Console.ResetColor();
                            Console.WriteLine($"");
                        }
                    }
                }
            }
        }

        public class API_RESPONSE
        {
            [JsonPropertyName("TAHUN_BUKU")]
            public int TAHUNBUKU { get; set; }

            [JsonPropertyName("AKUN")]
            public string AKUN { get; set; }

            [JsonPropertyName("NAMA_AKUN")]
            public string NAMAAKUN { get; set; }

            [JsonPropertyName("KELOMPOK")]
            public string KELOMPOK { get; set; }

            [JsonPropertyName("NAMA_KELOMPOK")]
            public string NAMAKELOMPOK { get; set; }

            [JsonPropertyName("JENIS")]
            public string JENIS { get; set; }

            [JsonPropertyName("NAMA_JENIS")]
            public string NAMAJENIS { get; set; }

            [JsonPropertyName("OBJEK")]
            public string OBJEK { get; set; }

            [JsonPropertyName("NAMA_OBJEK")]
            public string NAMAOBJEK { get; set; }

            [JsonPropertyName("RINCIAN")]
            public string RINCIAN { get; set; }

            [JsonPropertyName("NAMA_RINCIAN")]
            public string NAMARINCIAN { get; set; }

            [JsonPropertyName("SUB_RINCIAN")]
            public string SUBRINCIAN { get; set; }

            [JsonPropertyName("NAMA_SUB_RINCIAN")]
            public string NAMASUBRINCIAN { get; set; }

            [JsonPropertyName("KODE_OPD")]
            public string KODEOPD { get; set; }

            [JsonPropertyName("NAMA_OPD")]
            public string NAMAOPD { get; set; }

            [JsonPropertyName("KODE_SUB_OPD")]
            public string KODESUBOPD { get; set; }

            [JsonPropertyName("NAMA_SUB_OPD")]
            public string NAMASUBOPD { get; set; }

            [JsonPropertyName("TANGGAL")]
            public DateTime TANGGAL { get; set; }

            [JsonPropertyName("TARGET")]
            public decimal TARGET { get; set; }

            [JsonPropertyName("REALISASI")]
            public decimal REALISASI { get; set; }
        }
    }
}
