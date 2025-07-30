using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using static MonPDLib.General.EnumFactory;

namespace ReklameSurveyWs
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
                    "ERROR REKLAME_SURVEY WS",
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
                        SELECT DISTINCT NOR, 
                            IDTRANS_SUVEY, 
                            SURVEYOR, 
                            TGL_SURVEY, 
                            VERIFIKATOR, 
                            TGL_VERIF, 
                            NILAI_VERIF, 
                            IS_BARU, 
                            IS_TUTUP, 
                            NO_FORMULIR, 
                            KODE_OBYEK, 
                            NO_SKPD, 
                            TGL_SKPD, 
                            NILAI_PAJAK,
                            1 SEQ
                        FROM DETAIL_REKLAME_SURVEY
                    ";

                    Console.WriteLine($@"{DateTime.Now} REKLAME_SURVEY WS STARTED");
                    var result = await _contMonitoringDb.Set<DbMonReklameSurvey>().FromSqlRaw(sql).ToListAsync();
                    int jmlData = result.Count;
                    int index = 0;

                    var source = _contMonPd.DbMonReklameSurveys.ToList();
                    _contMonPd.DbMonReklameSurveys.RemoveRange(source);
                    Console.WriteLine($@"{DateTime.Now} REKLAME_SURVEY EXISTING REMOVED");

                    foreach (var item in result)
                    {

                        var newRow = new DbMonReklameSurvey();
                        newRow.Nor = item.Nor;
                        newRow.IdtransSuvey = item.IdtransSuvey;
                        newRow.Surveyor = item.Surveyor;
                        newRow.TglSurvey = item.TglSurvey;
                        newRow.Verifikator = item.Verifikator;
                        newRow.TglVerif = item.TglVerif;
                        newRow.NilaiVerif = item.NilaiVerif;
                        newRow.IsBaru = item.IsBaru;
                        newRow.IsTutup = item.IsTutup;
                        newRow.NoFormulir = item.NoFormulir;
                        newRow.KodeObyek = item.KodeObyek;
                        newRow.NoSkpd = item.NoSkpd;
                        newRow.TglSkpd = item.TglSkpd;
                        newRow.NilaiPajak = item.NilaiPajak;
                        _contMonPd.DbMonReklameSurveys.Add(newRow);

                        double persen = ((double)index / jmlData) * 100;
                        Console.Write($"\rREKLAME_SURVEY MONITORINGDB JML OP {jmlData.ToString("n0")} {item.Nor} : {persen:F2}%   ");

                        index++;
                    }
                    _contMonPd.SaveChanges();
                }


                MailHelper.SendMail(
                false,
                "DONE REKLAME_SURVEY WS",
                $@"REKLAME_SURVEY WS FINISHED",
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPREKLAMESURVEY.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPREKLAMESURVEY.ToString().ToUpper();
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
