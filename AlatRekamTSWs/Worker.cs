using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;

namespace AlatRekamTSWs
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
                    "ERROR TS WS",
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
                        SELECT  KONDISI, 
		                        KODEREKENING, 
		                        NOP, 
		                        NAMAOP, 
		                        ALAMAT, 
		                        CREATE_DATE, 
		                        NAMAREKENING, 
		                        JENISUSAHA, 
		                        JENIS, 
		                        LOCK_SPTPD, 
		                        OPEN_TS,
		                        TO_NUMBER(TERPASANG) TERPASANG,
		                        MAX(TO_DATE(TERAKHIR_AKTIF, 'DD-MON-YYYY HH24:MI:SS')) TERAKHIR_AKTIF, 
		                        MAX(TO_DATE(HARI_INI, 'DD-MON-YYYY HH24:MI:SS')) HARI_INI
                        FROM(
	                        SELECT 
	                            A.*, 
	                            B.TERAKHIR_AKTIF, 
	                            B.HARI_INI
	                        FROM (
	                            SELECT 
	                                A.*, 
	                                NVL(B.KONDISI, 0) AS TERPASANG
	                            FROM (
	                                SELECT *
	                                FROM detail_wp_ts@lihatelang
	                                WHERE KONDISI = 0
	                            ) A
	                            LEFT JOIN (
	                                SELECT *
	                                FROM detail_wp_ts@lihatelang
	                                WHERE KONDISI = 1
	                            ) B 
	                            ON A.NOP = B.NOP
	                        ) A
	                        LEFT JOIN USER_ONLINE_TS@lihatelang B 
	                        ON A.NOP = B.NOP
                        ) A
                        GROUP BY KONDISI, 
		                        KODEREKENING, 
		                        NOP, 
		                        NAMAOP, 
		                        ALAMAT, 
		                        CREATE_DATE, 
		                        NAMAREKENING, 
		                        JENISUSAHA, 
		                        JENIS, 
		                        LOCK_SPTPD, 
		                        OPEN_TS,
		                        TERPASANG
                    ";

                    Console.WriteLine($@"{DateTime.Now} TS WS STARTED");
                    var result = await _contMonitoringDb.Set<DbRekamAlatT>().FromSqlRaw(sql).ToListAsync();
                    int jmlData = result.Count;
                    int index = 0;

                    var source = _contMonPd.DbRekamAlatTs.ToList();
                    Console.WriteLine($@"{DateTime.Now} TS EXISTING REMOVED");

                    foreach (var item in result)
                    {
                        index++;
                        var existing = source.FirstOrDefault(x => x.Nop == item.Nop);
                        if (existing != null)
                        {
                            _contMonPd.DbRekamAlatTs.Remove(existing);
                        }

                        var newRow = new DbRekamAlatT();
                        newRow.Kondisi = item.Kondisi;
                        newRow.Koderekening = item.Koderekening;
                        newRow.Nop = item.Nop;
                        newRow.Namaop = item.Namaop;
                        newRow.Alamat = item.Alamat;
                        newRow.CreateDate = item.CreateDate;
                        newRow.Namarekening = item.Namarekening;
                        newRow.Jenisusaha = item.Jenisusaha;
                        newRow.Jenis = item.Jenis;
                        newRow.LockSptpd = item.LockSptpd;
                        newRow.OpenTs = item.OpenTs;
                        newRow.Terpasang = item.Terpasang;
                        newRow.TerakhirAktif = item.TerakhirAktif;
                        newRow.HariIni = item.HariIni;
                        _contMonPd.DbRekamAlatTs.Add(newRow);

                        double persen = ((double)index / jmlData) * 100;
                        Console.Write($"\rTS MONITORINGDB JML OP {jmlData.ToString("n0")} {item.Nop} : {persen:F2}%   ");
                        _contMonPd.SaveChanges();
                    }
                }


                MailHelper.SendMail(
                false,
                "DONE TS WS",
                $@"TS WS FINISHED",
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPALATREKAMTS.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPALATREKAMTS.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
    }
}
