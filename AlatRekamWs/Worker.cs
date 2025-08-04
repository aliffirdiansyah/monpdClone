using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;

namespace AlatRekamWs
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
                        SELECT 	KONDISI, 
			                    KODEREKENING, 
			                    REPLACE(NOP, '.', '') NOP, 
			                    NAMAOP, 
			                    ALAMAT, 
			                    CREATE_DATE, 
			                    NAMAREKENING, 
			                    JENISUSAHA, 
			                    JENIS, 
			                    LOCK_SPTPD, 
			                    OPEN_TS, 
			                    TERPASANG, 
			                    MAX(TERAKHIR_AKTIF) TERAKHIR_AKTIF, 
			                    MAX(HARI_INI) HARI_INI
	                    FROM (
		                    SELECT 
		                        A.KONDISI, 
			                    A.KODEREKENING, 
			                    REPLACE(A.NOP, '.', '') NOP,
			                    A.NAMAOP, 
			                    A.ALAMAT, 
			                    A.CREATE_DATE, 
			                    A.NAMAREKENING, 
			                    A.JENISUSAHA, 
			                    A.JENIS, 
			                    A.LOCK_SPTPD, 
			                    A.OPEN_TS, 
			                    A.TERPASANG,
		                        TO_DATE(B.TERAKHIR_AKTIF, 'DD-MON-YYYY HH24:MI:SS') TERAKHIR_AKTIF, 
		                        TO_DATE(B.HARI_INI, 'DD-MON-YYYY HH24:MI:SS') HARI_INI
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

                    var result = await _contMonitoringDb.Set<DbRekamAlatT>().FromSqlRaw(sql).ToListAsync();

                    var source = _contMonPd.DbRekamAlatTs.ToList();
                    _contMonPd.DbRekamAlatTs.RemoveRange(source);
                    foreach (var item in result)
                    {
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


                        _contMonPd.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now} DB_REKAM_ALAT_TS {item.Nop}");
                        Console.ResetColor();
                    }
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPREKLAMESURAT.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPREKLAMESURAT.ToString().ToUpper();
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
