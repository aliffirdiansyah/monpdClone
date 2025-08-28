using MonPDLib;
using MonPDLib.General;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Transactions;
using static MutasiBankWs.Worker;

namespace MutasiBankWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = -5;
        private static string NAMA_PAJAK = "MUTASI BANK";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                DateTime nextRun = now.AddSeconds(1); // besok jam 00:00
                TimeSpan delay = nextRun - now;
                if (isFirst)
                {
                    nextRun = now.AddSeconds(1); // besok jam 00:00
                    delay = nextRun - now;
                    isFirst = false;
                }
                else
                {
                    nextRun = now.AddHours(1); // next jam 00:00
                    delay = nextRun - now;
                }


                _logger.LogInformation("Next run scheduled at: {time}", nextRun);

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

                //// GUNAKAN KETIKA EKSEKUSI TUGAS MANUAL
                try
                {
                    DoWorkNewMeta(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR HOTEL WS",
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

        private void DoWorkNewMeta(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

            if (IsGetDBOp())
            {
                for (var i = tahunAmbil; i <= tglServer.Year; i++)
                {
                    ProcessDataMutasi(i);
                }
            }

            MailHelper.SendMail(
            false,
            "DONE MUTASI BANK WS",
            $@"MUTASI BANK WS FINISHED",
            null
            );
        }

        public void ProcessDataMutasi(int tahun)
        {
            var _contMonPd = DBClass.GetContext();
            var listNorek = Enum.GetValues(typeof(EnumFactory.EBankRekening)).Cast<EnumFactory.EBankRekening>()
                .Select(x => new
                {
                    Kode = $"00{(int)x}",
                    Nama = x.GetDescription()
                })
                .OrderBy(x => x.Kode)
                .ToList();

            
            foreach (var rekBank in listNorek)
            {
                int endMonth;

                // jika tahun ambil = tahun sekarang ? sampai bulan berjalan
                if (tahun == DateTime.Now.Year)
                {
                    //endMonth = DateTime.Now.Month;
                    endMonth = 8;
                }
                else
                {
                    endMonth = 12; // kalau tahun sebelumnya ? sampai desember
                }

                for (int bulan = 6; bulan <= endMonth; bulan++)
                {
                    var totalDaysInMonth = DateTime.DaysInMonth(tahun, bulan);
                    var startDate = new DateTime(tahun, bulan, 1);
                    var endDate = new DateTime(tahun, bulan, totalDaysInMonth);

                    // looping tanggal per 2 hari
                    for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        var sw = new Stopwatch();
                        sw.Start();
                        var newList = new List<MonPDLib.EF.DbMutasiRekening>();
                        var updateList = new List<MonPDLib.EF.DbMutasiRekening>();
                        var failedList = new List<History>();

                        var rangeStart = dt;
                        var rangeEnd = dt.AddDays(0);

                        if (rangeEnd > endDate)
                        {
                            rangeEnd = endDate;
                        }
                        DataResponse? result = null;
                        int maxRetry = 6;
                        int attempt = 0;

                        int jmlData = 0;
                        int index = 0;
                        bool isSuccessGetData = true;
                        while (attempt < maxRetry)
                        {
                            attempt++;

                            Console.WriteLine($"SEND API {rekBank.Kode}-{rekBank.Nama}, tgl {rangeStart.ToString("yyyy-MM-dd")} s/d {rangeEnd.ToString("yyyy-MM-dd")}");
                            result = GetDataApi(
                                rekBank.Kode.ToString(),
                                rangeStart.ToString("yyyy-MM-dd"),
                                rangeEnd.ToString("yyyy-MM-dd")
                            );

                            if(result == null)
                            {
                                throw new Exception("FAILED");
                            }

                            if(result.ResponseCode == "-1")
                            {
                                if (!result.ResponseDesc.Contains("majapahit.bankjatim.co.id:443"))
                                {
                                    isSuccessGetData = false;
                                    break;
                                }
                            }


                            if (result != null && result.ResponseCode == "00" && result.ResponseDesc == "Success")
                            {
                                jmlData = result.History.Count;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"SEND API {rekBank.Kode}-{rekBank.Nama}, tgl {rangeStart.ToString("yyyy-MM-dd")} s/d {rangeEnd.ToString("yyyy-MM-dd")}");
                                Console.ResetColor();

                                attempt = 0;
                                break; // sukses, keluar dari loop
                            }

                            // kalau belum max attempt, tunggu sebentar dulu biar nggak spam (opsional)
                            if (attempt < maxRetry)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine($"retry ke {attempt}");
                                Console.ResetColor();
                                Thread.Sleep(1000); // delay 1 detik
                            }
                        }

                        if (attempt > 1)
                        {
                            // setelah loop selesai, kalau masih gagal lempar exception
                            if (result == null || result.ResponseCode != "00" || result.ResponseDesc != "Success")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"NoRek : {rekBank.Kode} - {rekBank.Nama} tgl : {tahun}-{bulan} Gagal Terhubung setelah dicoba {attempt}x, karena {result?.ResponseDesc ?? ""}, Mohon Ulangi Beberapa Saat.");
                                Console.ResetColor();

                                break;
                            }
                        }


                        if (isSuccessGetData)
                        {
                            if(result != null)
                            {
                                foreach (var item in result.History)
                                {
                                    try
                                    {
                                        //var tanggalTransaksi = DateTime.ParseExact(item.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                        var tanggalTransaksi = MethodSawuangarDateTime(rangeStart, item.DateTime);

                                        var transactionCode = Regex.Replace($"{item.Reffno}.{item.TransactionCode}.{tanggalTransaksi.ToString("ddMMyyyyHHmmss")}.{item.Amount}", @"\s+", "");
                                        var description = item.Description;
                                        var amount = item.Amount;
                                        var flag = item.Flag;
                                        var ccy = item.Ccy;
                                        var reffno = item.Reffno;
                                        var rekeningBank = rekBank.Kode.ToString();
                                        var rekeningBankNama = rekBank.Nama;

                                        var sourceRow = _contMonPd.DbMutasiRekenings.Find(transactionCode);
                                        if (sourceRow != null)
                                        {
                                            sourceRow.TanggalTransaksi = tanggalTransaksi;
                                            sourceRow.Description = description;
                                            sourceRow.Amount = amount;
                                            sourceRow.Flag = flag;
                                            sourceRow.Ccy = ccy;
                                            sourceRow.Reffno = reffno;
                                            sourceRow.RekeningBank = rekeningBank;
                                            sourceRow.RekeningBankNama = rekeningBankNama;

                                            updateList.Add(sourceRow);
                                        }
                                        else
                                        {
                                            var newRow = new MonPDLib.EF.DbMutasiRekening();

                                            newRow.TransactionCode = transactionCode;
                                            newRow.TanggalTransaksi = tanggalTransaksi;
                                            newRow.Description = description;
                                            newRow.Amount = amount;
                                            newRow.Flag = flag;
                                            newRow.Ccy = ccy;
                                            newRow.Reffno = reffno;
                                            newRow.RekeningBank = rekeningBank;
                                            newRow.RekeningBankNama = rekeningBankNama;


                                            newList.Add(newRow);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"error : {ex.Message}");
                                        Console.ResetColor();

                                        failedList.Add(item);
                                    }

                                    index++;
                                    double persen = ((double)index / jmlData) * 100;
                                    Console.Write($"\r[{rangeStart.ToString("yyyy-MM-dd")} s/d {rangeEnd.ToString("yyyy-MM-dd")} JML DATA {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")} Failed : {failedList.Count.ToString("n0")} [({persen:F2}%)]");
                                }
                            }

                            Console.WriteLine("Updating DB!");
                            if (newList.Any())
                            {
                                var checkDoble = newList
                                    .GroupBy(x => new { x.TransactionCode })
                                    .Select(x => new { x.Key.TransactionCode, Jml = x.Count() })
                                    .Where(x => x.Jml > 1).ToList();

                                if(checkDoble.Any())
                                {
                                    string x = "";
                                }

                                _contMonPd.DbMutasiRekenings.AddRange(newList);
                                _contMonPd.SaveChanges();
                            }


                            if (updateList.Any())
                            {
                                _contMonPd.DbMutasiRekenings.UpdateRange(updateList);
                                _contMonPd.SaveChanges();
                            }
                            sw.Stop();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"NoRek : {rekBank.Kode} - {rekBank.Nama} tgl : {rangeStart.ToString("yyyy-MM-dd")} s/d {rangeEnd.ToString("yyyy-MM-dd")} Finished [{sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik]");
                            Console.WriteLine($"");
                            Console.ResetColor();
                        }
                    }
                }
            }
        }


        public static string? GetToken()
        {
            try
            {
                

                // 1. Buat credential
                var credential = new TOKEN_CREDENTIAL();
                credential.UserName = "PBJT";
                credential.Password = "PBJT12_3";
                credential.AppKey = "SBYTAXBAYAR*12345678*";
                credential.Audience = "TAX";
                

                // 2. Serialize jadi JSON
                var jsonBody = JsonSerializer.Serialize(credential);
                

                // 3. Tentukan URL API
                var url = "http://10.21.31.181:8444/api/Token";
                

                using (HttpClient client = new HttpClient())
                {
                    // 4. Siapkan content request
                    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    

                    // 5. Kirim request POST
                    
                    var responseApi = client.PostAsync(url, content).Result;
                    

                    // 6. Periksa status response
                    if (responseApi.IsSuccessStatusCode)
                    {
                        
                        string responseContent = responseApi.Content.ReadAsStringAsync().Result;
                        
                        return responseContent;
                    }
                    else
                    {
                        
                        string errorContent = responseApi.Content.ReadAsStringAsync().Result;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            
            return null;
        }
        public static DataResponse? GetDataApi(string noRek, string tglAwal, string tglAkhir)
        {
            var token = GetToken();
            var reqBody = new
            {
                noRekening = noRek,
                tanggalAwal = tglAwal,
                tanggalAkhir = tglAkhir,
            };
            string url = "http://10.21.31.181:8444/api/BankJatim/GetMutasiBJ";
            var GetDataMutasi = SendApi(url, (token ?? ""), reqBody);
            if(GetDataMutasi == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now} BROKEN");
                Console.ResetColor();
                throw new Exception($"{DateTime.Now} BROKEN");
            }
            if (GetDataMutasi.Code == 0)
            {
                if (GetDataMutasi.ResponeValue != null)
                {
                    var responseValue = JsonSerializer.Deserialize<DataResponse>(GetDataMutasi.ResponeValue);
                    if (responseValue != null)
                    {
                        return responseValue;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now} Failed - {GetDataMutasi.Response}");
                Console.ResetColor();

                return new DataResponse() 
                { 
                    ResponseCode = GetDataMutasi.Code.ToString(),
                    ResponseDesc = GetDataMutasi.Response
                };
            }

            return null;
        }
        public static RESPONSE_API? SendApi(string url, string jwt, object reqbody)
        {
            var resp = new RESPONSE_API();

            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header with the JWT token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

                // Serialize the request body to JSON
                string jsonBody = JsonSerializer.Serialize(reqbody);

                // Content to be sent in the request body
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Make an authenticated POST request
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and display the response content
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    resp = JsonSerializer.Deserialize<RESPONSE_API>(responseContent);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }

            return resp;
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBMUTASIBANK.ToString().ToUpper());
            if (row != null)
            {
                if (row.InsDate.HasValue)
                {
                    var tglTarik = row.InsDate.Value.Date;
                    var tglServer = DateTime.Now.Date;
                    if (tglTarik >= tglServer)
                    {
                        return false;
                    }
                    else
                    {
                        row.InsDate = DateTime.Now;
                        _contMonPd.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    row.InsDate = DateTime.Now;
                    _contMonPd.SaveChanges();
                    return true;
                }
            }
            var newRow = new MonPDLib.EF.SetLastRun();
            newRow.Job = EnumFactory.EJobName.DBMUTASIBANK.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
        public static DateTime MethodSawuangarDateTime(DateTime originalDate, string inputDateTimeBJ)
        {
            if (string.IsNullOrWhiteSpace(inputDateTimeBJ) || !inputDateTimeBJ.Contains(" "))
            {
                throw new ArgumentException("Format string tidak valid", nameof(inputDateTimeBJ));
            }

            string timePart = inputDateTimeBJ.Substring(inputDateTimeBJ.LastIndexOf(' ') + 1);

            var newTime = TimeOnly.ParseExact(timePart, "HH:mm:ss");

            return new DateTime(
                originalDate.Year,
                originalDate.Month,
                originalDate.Day,
                newTime.Hour,
                newTime.Minute,
                newTime.Second,
                newTime.Millisecond,
                originalDate.Kind
            );
        }

        public class TOKEN_CREDENTIAL
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string AppKey { get; set; }
            public string Audience { get; set; }
        }

        public class RESPONSE_API
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }

            [JsonPropertyName("response")]
            public string Response { get; set; }

            [JsonPropertyName("responeValue")]
            public string ResponeValue { get; set; }
        }

        public class DataResponse
        {
            [JsonPropertyName("responseCode")]
            public string ResponseCode { get; set; }

            [JsonPropertyName("responseDesc")]
            public string ResponseDesc { get; set; }

            [JsonPropertyName("history")]
            public List<History> History { get; set; }
        }

        public class History
        {
            [JsonPropertyName("dateTime")]
            public string DateTime { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("transactionCode")]
            public string TransactionCode { get; set; }

            [JsonPropertyName("amount")]
            public decimal Amount { get; set; }

            [JsonPropertyName("flag")]
            public string Flag { get; set; }

            [JsonPropertyName("ccy")]
            public string Ccy { get; set; }

            [JsonPropertyName("reffno")]
            public string Reffno { get; set; }
        }
    }
}
