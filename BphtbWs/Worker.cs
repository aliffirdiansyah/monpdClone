using Dapper;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace BphtbWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 12;
        private static EnumFactory.EPajak PAJAK_ENUM = EnumFactory.EPajak.BPHTB;

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
                
                try
                {
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR ABT WS",
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
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

            //await RealisasiProcess();

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                UpdateKoreksi(i);
            }

            MailHelper.SendMail(
            false,
            "DONE BPHTB WS",
            $@"BPHTB WS FINISHED",
            null
            );
        }

        private async Task RealisasiProcess()
        {
            var _contMonPd = DBClass.GetContext();
            using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                        SELECT  
                            NVL(A.IDSSPD, '-') AS IDSSPD, 
                            NVL(A.TGL_BAYAR, SYSDATE) AS TGL_BAYAR, 
                            NVL(A.TGL_DATA, SYSDATE) AS TGL_DATA, 
                            NVL(A.AKUN, '-') AS AKUN, 
                            NVL(A.NAMA_AKUN, '-') AS NAMA_AKUN,
                            NVL(A.KELOMPOK, '-') AS KELOMPOK, 
                            NVL(A.NAMA_KELOMPOK, '-') AS NAMA_KELOMPOK,
                            NVL(A.JENIS, '-') AS JENIS,
                            NVL(A.NAMA_JENIS, '-') AS NAMA_JENIS,
                            NVL(A.OBJEK, '-') AS OBJEK,
                            NVL(A.NAMA_OBJEK, '-') AS NAMA_OBJEK,
                            NVL(A.RINCIAN, '-') AS RINCIAN,
                            NVL(A.NAMA_RINCIAN, '-') AS NAMA_RINCIAN,
                            NVL(A.SUB_RINCIAN, '-') AS SUB_RINCIAN,
                            NVL(A.NAMA_SUB_RINCIAN, '-') AS NAMA_SUB_RINCIAN,
                            NVL(A.SPPT_NOP, '-') AS SPPT_NOP, 
                            NVL(A.NAMA_WP, '-') AS NAMA_WP, 
                            NVL(A.ALAMAT, '-') AS ALAMAT, 
	                        EXTRACT(MONTH FROM NVL(A.TGL_BAYAR, SYSDATE)) AS MASA,
	                        EXTRACT(YEAR FROM NVL(A.TGL_BAYAR, SYSDATE)) AS TAHUN,
                            NVL(A.POKOK, 0) AS POKOK, 
                            NVL(A.SANKSI, 0) AS SANKSI, 
                            NVL(A.NOMORDASARSETOR, '-') AS NOMORDASARSETOR, 
                            NVL(A.TEMPATBAYAR, '-') AS TEMPATBAYAR, 
                            NVL(A.REFSETORAN, '-') AS REFSETORAN, 
                            NVL(A.REKON_DATE, SYSDATE) AS REKON_DATE, 
                            NVL(A.REKON_BY, '-') AS REKON_BY, 
                            NVL(A.KD_PEROLEHAN, '-') AS KD_PEROLEHAN, 
                            NVL(A.KD_BYR, 0) AS KD_BYR, 
                            NVL(A.KODE_NOTARIS, '-') AS KODE_NOTARIS, 
                            NVL(A.KD_PELAYANAN, '-') AS KD_PELAYANAN, 
                            NVL(A.PEROLEHAN, '-') AS PEROLEHAN, 
                            NVL(A.KD_CAMAT, '-') AS KD_CAMAT, 
                            NVL(A.KD_LURAH, '-') AS KD_LURAH
                        FROM (
	                        SELECT 	IDSSPD, 
	                                TGL_PAYMENT TGL_BAYAR, 
	                                TGL_DATA, 
	                                '-' AKUN, 
	                                '-' NAMA_AKUN,
	                                '-' KELOMPOK, 
	                                '-' NAMA_KELOMPOK,
	                                '-' JENIS,
	                                '-' NAMA_JENIS,
	                                '-' OBJEK,
	                                '-' NAMA_OBJEK,
	                                '-' RINCIAN,
	                                '-' NAMA_RINCIAN,
	                                '-' SUB_RINCIAN,
	                                '-' NAMA_SUB_RINCIAN,
	                                NOP SPPT_NOP, 
	                                NAMA_WP, 
	                                ALAMAT, 
	                                MASA, 
	                                TAHUN, 
	                                POKOK, 
	                                SANKSI, 
	                                NOMERDASARSETOR NOMORDASARSETOR, 
	                                TEMPAT_BAYAR TEMPATBAYAR, 
	                                REFSETOR REFSETORAN, 
	                                REKON_DATE, 
	                                REKON_BY, 
	                                KD_PEROLEHAN, 
	                                KD_BAYAR KD_BYR, 
	                                KD_NOTARIS KODE_NOTARIS, 
	                                KD_PELAYANAN, 
	                                JENIS_PEROLEHAN PEROLEHAN, 
	                                KD_CAMAT, 
	                                KD_LURAH
	                        FROM (
	                            SELECT
	                                IDSSPD,TGL_PAYMENT,TGL_DATA,AKUN,NOP,NAMA_WP,ALAMAT,MASA,TAHUN,POKOK,SANKSI,NOMERDASARSETOR,TEMPAT_BAYAR,REFSETOR,REKON_DATE,REKON_BY,KD_PEROLEHAN,KD_BAYAR,
	                                KD_NOTARIS,KD_PELAYANAN,JENIS_PEROLEHAN,KD_CAMAT,KD_LURAH
	                            FROM
	                                (   --JATIM
	                                    SELECT 
	                                                A.VIRTUAL_ACCOUNT AS IDSSPD,
	                                                A.TGL_PAYMENT,
	                                                A.TGL_PAYMENT AS TGL_DATA,
	                                                '-' AS AKUN,
	                                                B.NOP,
	                                                B.NAMA_PEMBELI AS NAMA_WP,
	                                                B.ALAMAT_PEMBELI AS ALAMAT,
	                                                TO_NUMBER(A.MASA) MASA,
	                                                TO_NUMBER(A.TAHUN_SPPT) AS TAHUN,
	                                                TO_NUMBER(A.TOTAL_PAYMENT) AS POKOK,
	                                                0 AS SANKSI,
	                                                A.NO_PELAYANAN AS NOMERDASARSETOR,
	                                                A.TEMPAT_BAYAR,
	                                                '-' AS REFSETOR,
	                                                SYSDATE AS REKON_DATE,
	                                                'JOB' AS REKON_BY,
	                                                C.JENIS_PEROLEHAN_HAK KD_PEROLEHAN,
	                                                1 AS KD_BAYAR,
	                                                '-' AS KD_NOTARIS,
	                                                 A.NO_PELAYANAN AS KD_PELAYANAN,
	                                                D.VALUE JENIS_PEROLEHAN,
	                                                NULL AS KD_CAMAT,
	                                                NULL AS KD_LURAH
	                                    FROM BPHTB.R_BPHTB_BANK@PELAYANANONLINE A
	                                    LEFT JOIN BPHTB.VACODE_BANK@PELAYANANONLINE B ON TRIM(A.VIRTUAL_ACCOUNT)= B.NO_BILL_DIBAYAR-- AND A.VIRTUAL_ACCOUNT=B.NO_BILL AND A.VIRTUAL_ACCOUNT=B.NO_BILL_2
	                                    LEFT JOIN BPHTB.PENILAIAN_BPHTB@PELAYANANONLINE C ON TRIM (B.NO_PELAYANAN)=C.NO_PELAYANAN
	                                    LEFT JOIN BPHTB.JENIS_PEROLEHAN@PELAYANANONLINE D ON TRIM (C.JENIS_PEROLEHAN_HAK)=D.KD_PEROLEHAN
	                                    WHERE TEMPAT_BAYAR IN ('BNI','JATIM','MANDIRI') 
	                                )
	                                UNION ALL
	                                (   --LOKET
	                                    SELECT 
	                                                A.VIRTUAL_ACCOUNT AS IDSSPD,
	                                                A.TGL_PAYMENT,
	                                                A.TGL_PAYMENT AS TGL_DATA,
	                                                 '-' AS AKUN,
	                                                 A.NOP,
	                                                A.NAMA_PEMBELI AS  NAMA_WP,
	                                                D.ALAMAT_WP AS ALAMAT,
	                                                TO_NUMBER(A.MASA),
	                                                TO_NUMBER(A.TAHUN_SPPT) AS TAHUN,
	                                                TO_NUMBER(A.TOTAL_PAYMENT) AS POKOK,
	                                                0 AS SANKSI,
	                                                A.NO_PELAYANAN AS NOMERDASARSETOR,
	                                                A.TEMPAT_BAYAR,
	                                                '-' AS REFSETOR,
	                                                SYSDATE AS REKON_DATE,
	                                                'JOB' AS REKON_BY,
	                                                B.JENIS_PEROLEHAN_HAK KD_PEROLEHAN,
	                                                1 AS KD_BAYAR,
	                                                '-' AS KD_NOTARIS,
	                                                A.NO_PELAYANAN AS KD_PELAYANAN,
	                                                C.KETERANGAN JENIS_PEROLEHAN,
	                                                NULL AS KD_CAMAT,
	                                                NULL AS KD_LURAH
	                                    FROM bphtb.R_BPHTB_BANK@PELAYANANONLINE A
	                                        LEFT JOIN  DEK_RIMA.SSB_BPHTB@LIHATNAKULA B ON TRIM (A.NO_PELAYANAN)=B.NO_BPHTB
	                                        LEFT JOIN  DEK_RIMA.JENIS_PEROLEHAN@LIHATNAKULA C ON TRIM (B.JENIS_PEROLEHAN_HAK)=C.KODE_PEROLEHAN
	                                        LEFT JOIN  DEK_RIMA.SSb_wajib_pajak@LIHATNAKULA D ON D.NO_BPHTB = TRIM (A.NO_PELAYANAN)
	                                        WHERE  TEMPAT_BAYAR = 'LOKET'            
	                                )
	                        ) A
                        ) A
                    ";

                var result = await _contMonitoringDb.Set<OpSkpdBphtb>().FromSqlRaw(sql).ToListAsync();

                var dbMonBphtb = _contMonPd.DbMonBphtbs.ToList();
                _contMonPd.DbMonBphtbs.RemoveRange(dbMonBphtb);
                foreach (var item in result)
                {
                    _contMonPd.DbMonBphtbs.Add(new DbMonBphtb()
                    {
                        Idsspd = item.IDSSPD,
                        TglBayar = item.TGL_BAYAR,
                        TglData = item.TGL_DATA,
                        Akun = item.AKUN,
                        NamaAkun = item.NAMA_AKUN,
                        Jenis = item.JENIS,
                        NamaJenis = item.NAMA_JENIS,
                        Objek = item.OBJEK,
                        NamaObjek = item.NAMA_OBJEK,
                        Rincian = item.RINCIAN,
                        NamaRincian = item.NAMA_RINCIAN,
                        SubRincian = item.SUB_RINCIAN,
                        NamaSubRincian = item.NAMA_SUB_RINCIAN,
                        SpptNop = item.SPPT_NOP,
                        NamaWp = item.NAMA_WP,
                        Alamat = item.ALAMAT,
                        Masa = item.MASA,
                        Tahun = item.TAHUN,
                        Pokok = item.POKOK,
                        Sanksi = item.SANKSI,
                        Nomordasarsetor = item.NOMORDASARSETOR,
                        Tempatbayar = item.TEMPATBAYAR,
                        Refsetoran = item.REFSETORAN,
                        RekonDate = item.REKON_DATE,
                        RekonBy = item.REKON_BY,
                        KdPerolehan = item.KD_PEROLEHAN,
                        KdByr = item.KD_BYR,
                        KodeNotaris = item.KODE_NOTARIS,
                        KdPelayanan = item.KD_PELAYANAN,
                        Perolehan = item.PEROLEHAN,
                        KdCamat = item.KD_CAMAT,
                        KdLurah = item.KD_LURAH,
                        Kelompok = item.KELOMPOK,
                        NamaKelompok = item.NAMA_KELOMPOK,
                    });

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{DateTime.Now} [PROCESS] DB_MON_BPHTB_MONITORINGDB {item.IDSSPD}");
                    Console.ResetColor();
                }
                _contMonPd.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} [SUCCESS] DB_MON_BPHTB_MONITORINGDB");
                Console.ResetColor();
            }
        }

        private void UpdateKoreksi(int tahunBuku)
        {
            Console.WriteLine($"[START] UpdateKoreksi {tahunBuku}");

            var context = DBClass.GetContext();
            var query = @"SELECT 	TAHUN, 
                A.PAJAK_ID, 
                A.SCONTRO, 
                B.REALISASI, 
                (A.SCONTRO-B.REALISASI) SELISIH
        FROM 
        (
            SELECT 
                   P.TAHUN_BUKU AS TAHUN,
                   A.PAJAK_ID,
                   SUM(P.REALISASI) AS SCONTRO
            FROM DB_PENDAPATAN_DAERAH P
            LEFT JOIN DB_PAJAK_MAPPING A 
                 ON P.AKUN = A.AKUN
                 AND P.KELOMPOK = A.KELOMPOK 
                 AND P.JENIS = A.JENIS 
                 AND P.OBJEK = A.OBJEK 
                 AND P.RINCIAN = A.RINCIAN 
                 AND P.SUB_RINCIAN = A.SUB_RINCIAN 
                 AND P.TAHUN_BUKU = A.TAHUN_BUKU
            WHERE P.TAHUN_BUKU = :YEAR
              AND P.OBJEK LIKE '4.1.01%'
              AND A.PAJAK_ID IS NOT NULL
            GROUP BY P.TAHUN_BUKU, A.PAJAK_ID
        ) A 
        JOIN 
        (
            SELECT EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                1 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_RESTO
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                2 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_PPJ
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                3 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_HOTEL
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                4 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_PARKIR
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                5 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_HIBURAN
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                6 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_ABT
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
                7 AS PAJAK_ID, 
                SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
            FROM DB_MON_REKLAME
            WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR) AS TAHUN_BUKU,
                9 AS PAJAK_ID, 
                SUM(NVL(POKOK_PAJAK, 0)) AS REALISASI
            FROM DB_MON_PBB
            WHERE EXTRACT(YEAR FROM TGL_BAYAR) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_BAYAR) AS TAHUN_BUKU,
                12 AS PAJAK_ID, 
                SUM(NVL(POKOK, 0)) AS REALISASI
            FROM DB_MON_BPHTB
            WHERE EXTRACT(YEAR FROM TGL_BAYAR) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_BAYAR)
            UNION ALL
            SELECT 
                EXTRACT(YEAR FROM TGL_SSPD) AS TAHUN_BUKU,
                20 AS PAJAK_ID, 
                SUM(NVL(JML_POKOK, 0)) AS REALISASI
            FROM DB_MON_OPSEN_PKB
            WHERE EXTRACT(YEAR FROM TGL_SSPD) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_SSPD)
            UNION ALL
            SELECT  EXTRACT(YEAR FROM TGL_SSPD) AS TAHUN_BUKU,
                    21 AS PAJAK_ID, 
                    SUM(NVL(JML_POKOK, 0)) AS REALISASI
            FROM DB_MON_OPSEN_BBNKB
            WHERE EXTRACT(YEAR FROM TGL_SSPD) = :YEAR
            GROUP BY EXTRACT(YEAR FROM TGL_SSPD)
        ) B ON A.TAHUN = B.TAHUN_BUKU 
            AND A.PAJAK_ID = B.PAJAK_ID
        WHERE A.PAJAK_ID = :PAJAK";

            var db = getOracleConnection();
            var result = db.Query<MonPDLib.Helper.SCONTROSELISIH>(query, new { YEAR = tahunBuku, PAJAK = (int)PAJAK_ENUM }).ToList();

            decimal selisih = result.FirstOrDefault()?.SELISIH ?? 0;

            int pajakId = (int)PAJAK_ENUM;
            string pajakNama = PAJAK_ENUM.GetDescription();
            var kdPajakString = ((int)PAJAK_ENUM).ToString().PadLeft(2, '0');
            var nop = $"0000000000000000{kdPajakString}";
            var namaop = $"KOREKSI SCONTRO {PAJAK_ENUM.GetDescription()}";
            var tanggal = DateTime.Now.Date;
            if (tahunBuku < DateTime.Now.Year)
            {
                tanggal = new DateTime(tahunBuku, 12, 31);
            }

            var sourceMon = context.DbMonBphtbs.Where(x => x.SpptNop == nop && x.Tahun == tahunBuku).FirstOrDefault();
            if (sourceMon != null)
            {
                sourceMon.Pokok = selisih;
                context.DbMonBphtbs.Update(sourceMon);
                context.SaveChanges();
            }
            else
            {
                var newRow = new DbMonBphtb();

                newRow.Idsspd = nop;
                newRow.TglBayar = tanggal;
                newRow.TglData = null;
                newRow.Akun = "-";
                newRow.NamaAkun = "-";
                newRow.Jenis = "-";
                newRow.NamaJenis = "-";
                newRow.Objek = "-";
                newRow.NamaObjek = "-";
                newRow.Rincian = "-";
                newRow.NamaRincian = "-";
                newRow.SubRincian = "-";
                newRow.NamaSubRincian = "-";
                newRow.SpptNop = nop;
                newRow.NamaWp = namaop;
                newRow.Alamat = "-";
                newRow.Masa = 0;
                newRow.Tahun = tahunBuku;
                newRow.Pokok = selisih;
                newRow.Sanksi = 0;
                newRow.Nomordasarsetor = nop;
                newRow.Tempatbayar = "-";
                newRow.Refsetoran = "-";
                newRow.RekonDate = tanggal;
                newRow.RekonBy = "-";
                newRow.KdPerolehan = "-";
                newRow.KdByr = 0;
                newRow.KodeNotaris = "-";
                newRow.KdPelayanan = "-";
                newRow.Perolehan = "-";
                newRow.KdCamat = "-";
                newRow.KdLurah = "-";
                newRow.Kelompok = "-";
                newRow.NamaKelompok = "-";
                newRow.Seq = -1;

                context.DbMonBphtbs.Add(newRow);
                context.SaveChanges();
            }

            Console.WriteLine($"[FINISHED] UpdateKoreksi {tahunBuku}");
        }
        public static OracleConnection getOracleConnection()
        {
            try
            {
                OracleConnection ret = new OracleConnection(MonPDLib.DBClass.Monpd);
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPLISTRIK.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPLISTRIK.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
        private Helper.DbAkun? GetDbAkun(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.Kategoris).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.Kategoris.Any(y => y.PajakId == idPajak && y.Id == idKategori));

            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }
        }
        private Helper.DbAkun GetDbAkunPokok(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.Kategoris).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.Kategoris.Any(y => y.PajakId == idPajak && y.Id == idKategori));
            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }

        }
        private Helper.DbAkun GetDbAkunSanksi(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriSanksis).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriSanksis.Any(y => y.PajakId == idPajak && y.Id == idKategori));
            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }

        }
        private Helper.DbAkun GetDbAkunKenaikan(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriKenaikans).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriKenaikans.Any(y => y.PajakId == idPajak && y.Id == idKategori));
            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }

        }
    }
}
