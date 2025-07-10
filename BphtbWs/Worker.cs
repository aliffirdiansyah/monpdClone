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
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                var nextRun = now.AddDays(1); // besok jam 00:00
                var delay = nextRun - now;

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
                    "ERROR BPHTB WS",
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
            int idPajak = 12;
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == idPajak);
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
                foreach (var item in result)
                {
                    var rowMonBphtb = dbMonBphtb.FirstOrDefault(x => x.Idsspd == item.IDSSPD);
                    if (rowMonBphtb != null)
                    {
                        _contMonPd.DbMonBphtbs.Remove(rowMonBphtb);
                    }

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

                    _contMonPd.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} DB_MON_BPHTB_MONITORINGDB {item.IDSSPD}");
                    Console.ResetColor();
                }
            }

            MailHelper.SendMail(
            false,
            "DONE BPHTB WS",
            $@"BPHTB WS FINISHED",
            null
            );
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
