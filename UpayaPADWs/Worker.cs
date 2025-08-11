using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;

namespace UpayaPADWs
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

                //using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                //{
                //    var sql = @"
                //        SELECT  
                //            PAJAK_ID, 
                //            NOP, 
                //            BULAN, 
                //            TAHUN, 
                //            MAX(IS_HIMBAUAN)     AS IS_HIMBAUAN, 
                //            MAX(IS_PENYILANGAN)  AS IS_PENYILANGAN, 
                //            MAX(IS_TEGURAN)      AS IS_TEGURAN, 
                //            MAX(IS_PANGGILAN)    AS IS_PANGGILAN, 
                //            MAX(IS_KEJAKSAAN)    AS IS_KEJAKSAAN, 
                //            MAX(IS_ANGSURAN)     AS IS_ANGSURAN, 
                //            MAX(IS_BANTIP)       AS IS_BANTIP, 
                //            MAX(IS_PEMBONGKARAN) AS IS_PEMBONGKARAN, 
                //            MAX(IS_RENCANA_TS)   AS IS_RENCANA_TS, 
                //            MAX(IS_TS)           AS IS_TS, 
                //            MIN(CREATE_DATE)     AS CREATE_DATE, 
                //            MIN(CREATE_BY)       AS CREATE_BY, 
                //            MAX(MODI_DATE)       AS MODI_DATE, 
                //            MAX(MODI_BY)         AS MODI_BY
                //        FROM (
	               //         SELECT PAJAK_ID, 
	               //         NOP, 
	               //         BULAN, 
	               //         TAHUN, 
	               //         IS_HIMBAUAN, 
	               //         IS_PENYILANGAN, 
	               //         IS_TEGURAN, 
	               //         IS_PANGGILAN, 
	               //         IS_KEJAKSAAN, 
	               //         IS_ANGSURAN, 
	               //         IS_BANTIP, 
	               //         IS_PEMBONGKARAN, 
	               //         IS_RENCANA_TS, 
	               //         IS_TS, 
	               //         CREATE_DATE, 
	               //         CREATE_BY, 
	               //         MODI_DATE, 
	               //         MODI_BY
	               //         FROM
	               //         (
	               //             SELECT 
	               //                 CASE 
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 1 THEN 3
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 2 THEN 1
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 3 THEN 5
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 4 THEN 7
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 5 THEN 2
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 7 THEN 4
	               //                     WHEN TO_NUMBER(a.FK_PAJAK_DAERAH) = 8 THEN 6
	               //                     ELSE 0
	               //                 END AS PAJAK_ID,
	               //                 REPLACE(A.FK_NOP,'.','') NOP,
	               //                 BULAN_SURAT AS BULAN, 
	               //                 TAHUN_SURAT AS TAHUN, 
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT HIMBAUAN' THEN 1 ELSE 0 END AS IS_HIMBAUAN,                          
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT PENYILANGAN' THEN 1 ELSE 0 END AS IS_PENYILANGAN,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT TEGURAN' THEN 1 ELSE 0 END AS IS_TEGURAN,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT PANGGILAN' THEN 1 ELSE 0 END AS IS_PANGGILAN,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'KEJAKSAAN' THEN 1 ELSE 0 END AS IS_KEJAKSAAN,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'ANGSURAN' THEN 1 ELSE 0 END AS IS_ANGSURAN,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT BANTIP' THEN 1 ELSE 0 END AS IS_BANTIP,
	               //                 CASE WHEN JENIS_PENAGIHAN = 'SURAT BONGKAR' THEN 1 ELSE 0 END AS IS_PEMBONGKARAN,
	               //                 0 AS IS_RENCANA_TS, 
	               //                 0 AS IS_TS,
	               //                 SYSDATE AS CREATE_DATE, 
	               //                 '-' AS CREATE_BY, 
	               //                 SYSDATE AS MODI_DATE, 
	               //                 '-' AS MODI_BY
	               //             FROM UPAYA_PENAGIHAN_PAD A 
	               //         ) A
                //        ) A
                //        GROUP BY 
                //            PAJAK_ID, 
                //            NOP, 
                //            BULAN, 
                //            TAHUN
                //    ";

                //    Console.WriteLine($@"{DateTime.Now} TS WS STARTED");
                //    var result = await _contMonitoringDb.Set<DbMonUpayaPad>().FromSqlRaw(sql).ToListAsync();
                //    int jmlData = result.Count;
                //    int index = 0;

                //    var source = _contMonPd.DbMonUpayaPads.ToList();
                //    Console.WriteLine($@"{DateTime.Now} TS EXISTING REMOVED");

                //    foreach (var item in result)
                //    {
                //        index++;
                //        var existing = source.FirstOrDefault(x => x.Nop == item.Nop);
                //        if (existing != null)
                //        {
                //            _contMonPd.DbMonUpayaPads.Remove(existing);
                //        }

                //        var newRow = new DbMonUpayaPad();
                //        newRow.Kondisi = item.Kondisi;
                //        newRow.Koderekening = item.Koderekening;
                //        newRow.Nop = item.Nop;
                //        newRow.Namaop = item.Namaop;
                //        newRow.Alamat = item.Alamat;
                //        newRow.CreateDate = item.CreateDate;
                //        newRow.Namarekening = item.Namarekening;
                //        newRow.Jenisusaha = item.Jenisusaha;
                //        newRow.Jenis = item.Jenis;
                //        newRow.LockSptpd = item.LockSptpd;
                //        newRow.OpenTs = item.OpenTs;
                //        newRow.Terpasang = item.Terpasang;
                //        newRow.TerakhirAktif = item.TerakhirAktif;
                //        newRow.HariIni = item.HariIni;
                //        _contMonPd.DbMonUpayaPads.Add(newRow);

                //        double persen = ((double)index / jmlData) * 100;
                //        Console.Write($"\rTS MONITORINGDB JML OP {jmlData.ToString("n0")} {item.Nop} : {persen:F2}%   ");
                //        _contMonPd.SaveChanges();
                //    }
                //}


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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBMONUPAYAPAD.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBMONUPAYAPAD.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
    }
}
