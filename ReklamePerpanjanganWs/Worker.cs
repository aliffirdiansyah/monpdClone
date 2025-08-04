using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;

namespace ReklamePerpanjanganWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 2;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
                    "ERROR REKLAME_SURAT WS",
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
            if (IsGetDBOp())
            {
                var tglServer = DateTime.Now;
                var _contMonPd = DBClass.GetContext();

                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        SELECT NOFORM_S NO_FORMULIR, MAX(IS_PERPANJANGAN) IS_PERPANJANGAN 
                        FROM MONITORING.T_KETETAPAN_REKLAME_EXP 
                        GROUP BY NOFORM_S
                    ";

                    var result = await _contMonitoringDb.Set<DbMonReklamePerpanjangan>().FromSqlRaw(sql).ToListAsync();

                    var source = _contMonPd.DbMonReklamePerpanjangans.ToList();
                    int total = result.Count;
                    int current = 0;

                    foreach (var item in result)
                    {
                        current++;

                        var rowMonReklamePerpanjangan = source.FirstOrDefault(x => x.NoFormulir == item.NoFormulir);
                        if (rowMonReklamePerpanjangan != null)
                        {
                            _contMonPd.DbMonReklamePerpanjangans.Remove(rowMonReklamePerpanjangan);
                        }

                        _contMonPd.DbMonReklamePerpanjangans.Add(new DbMonReklamePerpanjangan()
                        {
                            NoFormulir = item.NoFormulir,
                            IsPerpanjangan = item.IsPerpanjangan
                        });

                        _contMonPd.SaveChanges();

                        
                        double percent = ((double)current / total) * 100;
                        Console.Write($"\r TOTAL DATA : {total.ToString("n0")} Progress: {item.NoFormulir} {current.ToString("n0")} - {percent:0.00}%   ");
                    }
                    Console.WriteLine(); // ganti baris setelah selesai
                }

                MailHelper.SendMail(
                false,
                "DONE REKLAME_SURAT WS",
                $@"REKLAME_SURAT WS FINISHED",
                null
                );
            }
        }

        public static OracleConnection getOracleConnection()
        {
            try
            {
                OracleConnection ret = new OracleConnection(MonPDLib.DBClass.MonitoringDb);
                ret.Open();
                ret.Close();
                return ret;
            }
            catch (Exception ex)
            {
                return new OracleConnection();
            }
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPREKLAMEPERPANJANGAN.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPREKLAMEPERPANJANGAN.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }

        public class TeguranFile
        {
            public string KLASIFIKASI { get; set; } = null!;
            public int TAHUN_SURAT { get; set; }
            public int PAJAK { get; set; }
            public string KODE_DOKUMEN { get; set; } = null!;
            public string BIDANG { get; set; } = null!;
            public int AGENDA { get; set; }
            public byte[] ISI_FILE { get; set; } = null!;

        }
    }
}
