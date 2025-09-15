using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace HiburanWs
{
    public class WorkerV2 : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<WorkerV2> _logger;
        private static int KDPajak = 5;
        private static string NAMA_PAJAK = "HIBURAN";
        private static EnumFactory.EPajak PAJAK_ENUM = EnumFactory.EPajak.JasaKesenianHiburan;

        public WorkerV2(ILogger<WorkerV2> logger)
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
                    GetOPProcess(i);
                }

            }

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                GetRealisasi(i);
                UpdateKoreksi(i);
            }


            MailHelper.SendMail(
            false,
            "DONE PBB WS",
            $@"PBB WS FINISHED",
            null
            );
        }

        private void UpdateKoreksi(int tahunBuku)
        {
            bool isTahunSekarang = tahunBuku == DateTime.Now.Year;
            if (!isTahunSekarang)
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
         WHERE A.PAJAK_ID = :PAJAK AND (A.SCONTRO-B.REALISASI) <> 0";

                var db = getOracleConnection();
                var result = db.Query<MonPDLib.Helper.SCONTROSELISIH>(query, new { YEAR = tahunBuku, PAJAK = (int)PAJAK_ENUM }).ToList();

                if (result.Count > 0)
                {
                    decimal selisih = result.FirstOrDefault()?.SELISIH ?? 0;

                    int pajakId = (int)PAJAK_ENUM;
                    string pajakNama = PAJAK_ENUM.GetDescription();
                    var kdPajakString = ((int)PAJAK_ENUM).ToString().PadLeft(2, '0');
                    var nop = $"0000000000000000{kdPajakString}";
                    var namaop = $"KOREKSI SCONTRO {PAJAK_ENUM.GetDescription()}";

                    int kategoriId = 54;
                    string kategoriNama = "HIBURAN";

                    var tanggal = DateTime.Now.Date;
                    if (tahunBuku < DateTime.Now.Year)
                    {
                        tanggal = new DateTime(tahunBuku, 12, 31);
                    }

                    var source = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahunBuku);
                    if (source != null)
                    {
                        source.NamaOp = namaop;
                        source.TahunBuku = tahunBuku;

                        context.DbOpHiburans.Update(source);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbOpHiburan();

                        newRow.Nop = nop;
                        newRow.NamaOp = namaop;
                        newRow.TahunBuku = tahunBuku;
                        newRow.Npwpd = "KOREKSI";
                        newRow.NpwpdNama = "KOREKSI";
                        newRow.NpwpdAlamat = "-";
                        newRow.PajakId = pajakId;
                        newRow.PajakNama = pajakNama;
                        newRow.AlamatOp = "-";
                        newRow.AlamatOpNo = "-";
                        newRow.AlamatOpRt = "-";
                        newRow.AlamatOpRw = "-";
                        newRow.Telp = "-";
                        newRow.AlamatOpKdLurah = "-";
                        newRow.AlamatOpKdCamat = "-";
                        newRow.TglOpTutup = null;
                        newRow.TglMulaiBukaOp = tanggal;
                        newRow.KategoriId = kategoriId;
                        newRow.KategoriNama = kategoriNama;
                        newRow.MetodePembayaran = "-";
                        newRow.MetodePenjualan = "-";
                        newRow.JumlahKaryawan = 0;

                        newRow.InsDate = tanggal;
                        newRow.InsBy = "-";
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
                        newRow.Kelompok = "-";
                        newRow.NamaKelompok = "-";
                        newRow.WilayahPajak = "-";
                        newRow.IsTutup = 0;


                        context.DbOpHiburans.Add(newRow);
                        context.SaveChanges();
                    }

                    source = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahunBuku);
                    if (source == null)
                    {
                        throw new Exception("Gagal membuat data OP untuk koreksi scontro");
                    }
                    var sourceMon = context.DbMonHiburans.Where(x => x.Nop == nop && x.TahunBuku == tahunBuku).FirstOrDefault();
                    if (sourceMon != null)
                    {
                        sourceMon.NominalPokokBayar = selisih;
                        context.DbMonHiburans.Update(sourceMon);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbMonHiburan();

                        newRow.Nop = source.Nop;
                        newRow.Npwpd = source.Npwpd;
                        newRow.NpwpdNama = source.NpwpdNama;
                        newRow.NpwpdAlamat = source.NpwpdAlamat;
                        newRow.PajakId = source.PajakId;
                        newRow.PajakNama = source.PajakNama;
                        newRow.NamaOp = source.NamaOp;
                        newRow.AlamatOp = source.AlamatOp;
                        newRow.AlamatOpKdLurah = source.AlamatOpKdLurah;
                        newRow.AlamatOpKdCamat = source.AlamatOpKdCamat;
                        newRow.TglOpTutup = source.TglOpTutup;
                        newRow.TglMulaiBukaOp = source.TglMulaiBukaOp;
                        newRow.IsTutup = source.TglOpTutup == null ? 0 : source.TglOpTutup.Value.Year <= tahunBuku ? 1 : 0;
                        newRow.KategoriId = source.KategoriId;
                        newRow.KategoriNama = source.KategoriNama;
                        newRow.TahunBuku = tahunBuku;
                        newRow.Akun = source.Akun;
                        newRow.NamaAkun = source.NamaAkun;
                        newRow.Jenis = source.Jenis;
                        newRow.NamaJenis = source.NamaJenis;
                        newRow.Objek = source.Objek;
                        newRow.NamaObjek = source.NamaObjek;
                        newRow.Rincian = source.Rincian;
                        newRow.NamaRincian = source.NamaRincian;
                        newRow.SubRincian = source.SubRincian;
                        newRow.NamaSubRincian = source.NamaSubRincian;
                        newRow.TahunPajakKetetapan = tanggal.Year;
                        newRow.MasaPajakKetetapan = tanggal.Month;
                        newRow.SeqPajakKetetapan = 1;
                        newRow.KategoriKetetapan = "4";
                        newRow.TglKetetapan = tanggal;
                        newRow.TglJatuhTempoBayar = tanggal;
                        newRow.PokokPajakKetetapan = selisih;
                        newRow.PengurangPokokKetetapan = 0;
                        newRow.AkunKetetapan = "-";
                        newRow.KelompokKetetapan = "-";
                        newRow.JenisKetetapan = "-";
                        newRow.ObjekKetetapan = "-";
                        newRow.RincianKetetapan = "-";
                        newRow.SubRincianKetetapan = "-";
                        newRow.InsDate = tanggal;
                        newRow.InsBy = "JOB";
                        newRow.UpdDate = tanggal;
                        newRow.UpdBy = "JOB";
                        newRow.TglBayarPokok = tanggal;
                        newRow.NominalPokokBayar = selisih;
                        newRow.AkunPokokBayar = "-";
                        newRow.Kelompok = "-";
                        newRow.JenisPokokBayar = "-";
                        newRow.ObjekPokokBayar = "-";
                        newRow.RincianPokokBayar = "-";
                        newRow.SubRincianPokokBayar = "-";
                        newRow.TglBayarSanksi = null;
                        newRow.NominalSanksiBayar = null;
                        newRow.AkunSanksiBayar = "-";
                        newRow.KelompokSanksiBayar = "-";
                        newRow.JenisSanksiBayar = "-";
                        newRow.ObjekSanksiBayar = "-";
                        newRow.RincianSanksiBayar = "-";
                        newRow.SubRincianSanksiBayar = "-";
                        newRow.TglBayarSanksiKenaikan = null;

                        newRow.NominalSanksiBayar = 0;
                        newRow.AkunSanksiBayar = "-";
                        newRow.KelompokSanksiBayar = "-";
                        newRow.JenisSanksiBayar = "-";
                        newRow.ObjekSanksiBayar = "-";
                        newRow.RincianSanksiBayar = "-";
                        newRow.SubRincianSanksiBayar = "-";

                        context.DbMonHiburans.Add(newRow);
                        context.SaveChanges();
                    }
                }

                Console.WriteLine($"[FINISHED] UpdateKoreksi {tahunBuku}");
            }
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


        private void GetOPProcess(int tahunBuku)
        {
            var tglMulai = DateTime.Now;
            var sw = new Stopwatch();
            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
            {
                sw.Start();
                var sql = @"
                                                                               SELECT *
FROM (
SELECT REPLACE(A.FK_NOP, '.', '') NOP,NVL(FK_NPWPD, '-') NPWPD,NAMA_OP, 5 PAJAK_ID,  'Pajak Jasa Kesenian Hiburan' PAJAK_NAMA,
              NVL(ALAMAT_OP, '-') ALAMAT_OP, '-'  ALAMAT_OP_NO,'-' ALAMAT_OP_RT,'-' ALAMAT_OP_RW,NVL(NOMOR_TELEPON, '-') TELP,
              NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH, NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,CASE
	              WHEN TGL_TUTUP IS NULL THEN NULL 
	              WHEN TO_CHAR(TGL_TUTUP,'YYYY') <= 1990 THEN NULL
	              WHEN STATUS_OP != 0 THEN NULL
	              ELSE TGL_TUTUP
	              END  TGL_OP_TUTUP,
              NVL(TGL_BUKA,TO_DATE('01012000','DDMMYYYY')) TGL_MULAI_BUKA_OP, 0 METODE_PENJUALAN,        0 METODE_PEMBAYARAN,        0 JUMLAH_KARYAWAN,  
              CASE                             
                        WHEN NAMA_JENIS_PAJAK = 'FITNESS/PUSAT KEBUGARAN' THEN 43
                        WHEN NAMA_JENIS_PAJAK = 'KARAOKE KELUARGA' THEN 45
                        WHEN NAMA_JENIS_PAJAK = 'PANTI PIJAT/THERAPY/SAUNA/SPA' THEN 48
                        WHEN NAMA_JENIS_PAJAK = 'PERMAINAN ANAK' THEN 50
                        WHEN NAMA_JENIS_PAJAK = 'OLAHRAGA' THEN 46
                        WHEN NAMA_JENIS_PAJAK = 'PERMAINAN ANAK/PERMAINAN KETANGKASAN' THEN 50
                        WHEN NAMA_JENIS_PAJAK = 'BAR/CAFE/KLAB MALAM/DISKOTIK' THEN 41
                        WHEN NAMA_JENIS_PAJAK = 'BIOSKOP' THEN 42
                        WHEN NAMA_JENIS_PAJAK = 'KARAOKE DEWASA' THEN 44    
            ELSE -2
            END AS KATEGORI_ID,
            NAMA_JENIS_PAJAK   KATEGORI_NAMA,
             sysdate INS_dATE, 'JOB' INS_BY ,CASE
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 1' THEN '1'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 2' THEN '2'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 3' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 4' THEN '4'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 5' THEN '5'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 6' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 7' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = 'SURABAYA 8' THEN '2'
    WHEN TO_CHAR(fk_wilayah_pajak) = '01' THEN '1'
    WHEN TO_CHAR(fk_wilayah_pajak) = '02' THEN '2'
    WHEN TO_CHAR(fk_wilayah_pajak) = '03' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '04' THEN '4'
    WHEN TO_CHAR(fk_wilayah_pajak) = '05' THEN '5'
    WHEN TO_CHAR(fk_wilayah_pajak) = '07' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '06' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '08' THEN '2'
    WHEN TO_CHAR(fk_wilayah_pajak) = '1' THEN '1'
    WHEN TO_CHAR(fk_wilayah_pajak) = '2' THEN '2'
    WHEN TO_CHAR(fk_wilayah_pajak) = '3' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '4' THEN '4'
    WHEN TO_CHAR(fk_wilayah_pajak) = '5' THEN '5'
    WHEN TO_CHAR(fk_wilayah_pajak) = '7' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '6' THEN '3'
    WHEN TO_CHAR(fk_wilayah_pajak) = '8' THEN '2'
    ELSE NULL
END AS WILAYAH_PAJAK,
            '-' AKUN,'-'  NAMA_AKUN,'-'  JENIS,'-'  NAMA_JENIS,'-'  OBJEK,'-'  NAMA_OBJEK,'-'  RINCIAN,
'-'  NAMA_RINCIAN,'-'  SUB_RINCIAN,'-'  NAMA_SUB_RINCIAN,'-'  KELOMPOK,
            '-'  NAMA_KELOMPOK,1  IS_TUTUP,'-'  NPWPD_NAMA, '-'  NPWPD_ALAMAT,1 TAHUN_BUKU
FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
where fk_pajak_daerah = '03' and status_op != 0 and kategori_pajak <> 'INSIDENTIL'
)
                    ";

                var result = _contMonitoringDB.Set<DbOpHiburan>().FromSqlRaw(sql, new[] {
                    new OracleParameter("PAJAK", NAMA_PAJAK),
                    new OracleParameter("TAHUN", tahunBuku)
                }).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbOpHiburan>();
                //var updateList = new List<MonPDLib.EF.DbOpHiburan>();
                var removeEx = _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == tahunBuku).ToList();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        var newRow = new MonPDLib.EF.DbOpHiburan();
                        newRow.Nop = item.Nop;
                        newRow.Npwpd = item.Npwpd;
                        // set manual
                        var infoWP = GetInfoWPHPP(newRow.Npwpd);
                        newRow.NpwpdNama = infoWP[0];
                        newRow.NpwpdAlamat = infoWP[1];
                        //
                        newRow.PajakId = item.PajakId;
                        newRow.PajakNama = item.PajakNama;
                        newRow.NamaOp = item.NamaOp;
                        newRow.AlamatOp = item.AlamatOp;
                        newRow.AlamatOpNo = item.AlamatOpNo;
                        newRow.AlamatOpRt = item.AlamatOpRt;
                        newRow.AlamatOpRw = item.AlamatOpRw;
                        newRow.Telp = item.Telp;
                        newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
                        newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
                        newRow.TglOpTutup = null;
                        if (item.TglOpTutup.HasValue && item.TglOpTutup.Value.Year <= 1990)
                        {
                            newRow.TglOpTutup = null;
                        }
                        else
                        {
                            newRow.TglOpTutup = item.TglOpTutup;
                        }
                        newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                        var kategori = GetKategoriOvveride(item.Nop, item.KategoriNama);
                        item.KategoriId = Convert.ToInt32(kategori[0] ?? "54");
                        item.KategoriNama = kategori[1] ?? "HIBURAN";

                        newRow.KategoriId = item.KategoriId;
                        newRow.KategoriNama = item.KategoriNama;

                        newRow.MetodePenjualan = item.MetodePenjualan;
                        newRow.MetodePembayaran = item.MetodePembayaran;
                        newRow.JumlahKaryawan = item.JumlahKaryawan;
                        newRow.InsDate = item.InsDate;
                        newRow.InsBy = item.InsBy;
                        newRow.IsTutup = item.TglOpTutup == null ? 0 : item.TglOpTutup.Value.Year <= tahunBuku ? 1 : 0;
                        newRow.WilayahPajak = item.WilayahPajak;

                        newRow.TahunBuku = tahunBuku;
                        var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                        newRow.Akun = dbakun.Akun;
                        newRow.NamaAkun = dbakun.NamaAkun;
                        newRow.Kelompok = dbakun.Kelompok;
                        newRow.NamaKelompok = dbakun.NamaKelompok;
                        newRow.Jenis = dbakun.Jenis;
                        newRow.NamaJenis = dbakun.NamaJenis;
                        newRow.Objek = dbakun.Objek;
                        newRow.NamaObjek = dbakun.NamaObjek;
                        newRow.Rincian = dbakun.Rincian;
                        newRow.NamaRincian = dbakun.NamaRincian;
                        newRow.SubRincian = dbakun.SubRincian;
                        newRow.NamaSubRincian = dbakun.NamaSubRincian;
                        newList.Add(newRow);
                    }
                    catch (Exception ex)
                    {
                        var kkk = item.Nop;
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] OP HIBURAN TAHUN {tahunBuku} JML OP {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")},    [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (removeEx.Any())
                {
                    _contMonPd.DbOpHiburans.RemoveRange(removeEx);
                    _contMonPd.SaveChanges();
                }
                if (newList.Any())
                {
                    _contMonPd.DbOpHiburans.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                //if (updateList.Any())
                //{
                //    _contMonPd.DbOpHiburans.UpdateRange(updateList);
                //    _contMonPd.SaveChanges();
                //}
                sw.Stop();
                Console.Write($"Done {sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik");
                Console.WriteLine($"");
            }
        }


        private void GetRealisasi(int tahunBuku)
        {
            var tglMulai = DateTime.Now;
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlRealisasi = @"
           SELECT     NOP,MASA_PAJAK,TAHUN_PAJAK, SEQ ,MAX(JATUH_TEMPO) AS JATUH_TEMPO,
        SUM(NOMINAL_POKOK) NOMINAL_POKOK,
        SUM(NOMINAL_SANKSI) NOMINAL_SANKSI,
        MAX(TRANSACTION_DATE) TRANSACTION_DATE
FROM (            
    SELECT            TO_DATE(nvl( MP_AKHIR, LAST_DAY(TO_DATE(bulan_pajak || '-' || tahun_pajak, 'MM-YYYY')) ) ) JATUH_TEMPO,  
            REPLACE(NVL(FK_NOP,0),'.','') NOP,
            TO_NUMBER( NVL(BULAN_PAJAK,0)) MASA_PAJAK,
            TO_NUMBER(NVL(TAHUN_PAJAK,0)) TAHUN_PAJAK, 
            1 SEQ, 
             TO_NUMBER(NVL(JML_POKOK,0)) NOMINAL_POKOK, 
               TO_NUMBER(NVL(JML_DENDA,0)) NOMINAL_SANKSI,
               TO_DATE(TGL_SETORAN) TRANSACTION_DATE                                                                 
FROM VW_SIMPADA_SSPD@LIHATHPPSERVER
WHERE NAMA_PAJAK_DAERAH=:PAJAK  AND TO_CHAR(TGL_SETORAN,'YYYY')=:TAHUN
) A
GROUP BY NOP, MASA_PAJAK, TAHUN_PAJAK,SEQ                                         
                ";

                var realisasiMonitoringDb = _contMonitoringDB.Set<SSPDHPP>()
                    .FromSqlRaw(sqlRealisasi, new[] {
                                new OracleParameter("PAJAK", NAMA_PAJAK),
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();
                
                int jmlData = realisasiMonitoringDb.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonHiburan>();
                var updateList = new List<MonPDLib.EF.DbMonHiburan>();
         
                foreach (var itemRealisasi in realisasiMonitoringDb)
                {
                    var realisasi = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == itemRealisasi.NOP &&
                                                                           x.TahunPajakKetetapan == itemRealisasi.TAHUN_PAJAK &&
                                                                           x.MasaPajakKetetapan == itemRealisasi.MASA_PAJAK &&
                                                                           x.SeqPajakKetetapan == itemRealisasi.SEQ &&
                                                                           x.TahunBuku == tahunBuku);
                    if (realisasi != null)
                    {
                        realisasi.NominalPokokBayar = itemRealisasi.NOMINAL_POKOK;
                        realisasi.TglBayarPokok = itemRealisasi.TRANSACTION_DATE;
                        realisasi.NominalSanksiBayar = itemRealisasi.NOMINAL_SANKSI;
                        realisasi.TglBayarSanksi = itemRealisasi.TRANSACTION_DATE;
                        realisasi.NominalSanksiKenaikanBayar = 0;
                        realisasi.TglBayarSanksiKenaikan = itemRealisasi.TRANSACTION_DATE;
                        updateList.Add(realisasi);
                    }
                    else
                    {
                        var OP = _contMonPd.DbOpHiburans.Find(itemRealisasi.NOP, (decimal)tahunBuku);

                        if (OP != null)
                        {                            
                            var AkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)OP.KategoriId);
                            var AkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);

                            var newRow = new DbMonHiburan();
                            newRow.Nop = OP.Nop;
                            newRow.Npwpd = OP.Npwpd;
                            newRow.NpwpdNama = OP.NpwpdNama;
                            newRow.NpwpdAlamat = OP.NpwpdAlamat;
                            newRow.PajakId = OP.PajakId;
                            newRow.PajakNama = OP.PajakNama;
                            newRow.NamaOp = OP.NamaOp;
                            newRow.AlamatOp = OP.AlamatOp;
                            newRow.AlamatOpKdLurah = OP.AlamatOpKdLurah;
                            newRow.AlamatOpKdCamat = OP.AlamatOpKdCamat;
                            newRow.TglOpTutup = OP.TglOpTutup;
                            newRow.TglMulaiBukaOp = OP.TglMulaiBukaOp;
                            newRow.IsTutup = OP.TglOpTutup == null ? 0 : OP.TglOpTutup.Value.Year <= tahunBuku ? 1 : 0;
                            newRow.KategoriId = OP.KategoriId;
                            newRow.KategoriNama = OP.KategoriNama;
                            newRow.TahunBuku = tahunBuku;
                            newRow.Akun = OP.Akun;
                            newRow.NamaAkun = OP.NamaAkun;
                            newRow.Jenis = OP.Jenis;
                            newRow.NamaJenis = OP.NamaJenis;
                            newRow.Objek = OP.Objek;
                            newRow.NamaObjek = OP.NamaObjek;
                            newRow.Rincian = OP.Rincian;
                            newRow.NamaRincian = OP.NamaRincian;
                            newRow.SubRincian = OP.SubRincian;
                            newRow.NamaSubRincian = OP.NamaSubRincian;
                            newRow.TahunPajakKetetapan = itemRealisasi.TAHUN_PAJAK;
                            newRow.MasaPajakKetetapan = itemRealisasi.MASA_PAJAK;
                            newRow.SeqPajakKetetapan = itemRealisasi.SEQ;
                            newRow.KategoriKetetapan = "4";
                            newRow.TglKetetapan = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglJatuhTempoBayar = itemRealisasi.JATUH_TEMPO;
                            newRow.PokokPajakKetetapan = itemRealisasi.NOMINAL_POKOK;
                            newRow.PengurangPokokKetetapan = 0;
                            newRow.AkunKetetapan = AkunPokok.Akun;
                            newRow.KelompokKetetapan = AkunPokok.Kelompok;
                            newRow.JenisKetetapan = AkunPokok.Jenis;
                            newRow.ObjekKetetapan = AkunPokok.Objek;
                            newRow.RincianKetetapan = AkunPokok.Rincian;
                            newRow.SubRincianKetetapan = AkunPokok.SubRincian;
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";
                            newRow.UpdDate = DateTime.Now;
                            newRow.UpdBy = "JOB";
                            newRow.NominalPokokBayar = itemRealisasi.NOMINAL_POKOK;
                            newRow.AkunPokokBayar = AkunPokok.Akun;
                            newRow.Kelompok = AkunPokok.Kelompok;
                            newRow.JenisPokokBayar = AkunPokok.Jenis;
                            newRow.ObjekPokokBayar = AkunPokok.Objek;
                            newRow.RincianPokokBayar = AkunPokok.Rincian;
                            newRow.SubRincianPokokBayar = AkunPokok.SubRincian;
                            newRow.TglBayarPokok = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglBayarSanksi = itemRealisasi.TRANSACTION_DATE;
                            newRow.NominalSanksiBayar = itemRealisasi.NOMINAL_SANKSI;
                            newRow.AkunSanksiBayar = AkunSanksi.Akun;
                            newRow.KelompokSanksiBayar = AkunSanksi.Kelompok;
                            newRow.JenisSanksiBayar = AkunSanksi.Jenis;
                            newRow.ObjekSanksiBayar = AkunSanksi.Objek;
                            newRow.RincianSanksiBayar = AkunSanksi.Rincian;
                            newRow.SubRincianSanksiBayar = AkunSanksi.SubRincian;
                            newRow.TglBayarSanksiKenaikan = itemRealisasi.TRANSACTION_DATE;

                            newRow.NominalSanksiBayar = 0;
                            newRow.AkunSanksiBayar = AkunSanksi.Akun;
                            newRow.KelompokSanksiBayar = AkunSanksi.Kelompok;
                            newRow.JenisSanksiBayar = AkunSanksi.Jenis;
                            newRow.ObjekSanksiBayar = AkunSanksi.Objek;
                            newRow.RincianSanksiBayar = AkunSanksi.Rincian;
                            newRow.SubRincianSanksiBayar = AkunSanksi.SubRincian;
                            newList.Add(newRow);
                        }
                        else
                        {
                            var newRowOp = new DbOpHiburan();
                            newRowOp.Nop = itemRealisasi.NOP;
                            newRowOp.Npwpd = "-";
                            newRowOp.NpwpdNama = "-";
                            newRowOp.NpwpdAlamat = "-";
                            newRowOp.PajakId = KDPajak;
                            newRowOp.PajakNama = "Pajak Jasa Hiburan";
                            newRowOp.NamaOp = "-";
                            newRowOp.AlamatOp = "-";
                            newRowOp.AlamatOpNo = "-";
                            newRowOp.AlamatOpRt = "-";
                            newRowOp.AlamatOpRw = "-";
                            newRowOp.Telp = "-";
                            newRowOp.AlamatOpKdLurah = "-";
                            newRowOp.AlamatOpKdCamat = "-";
                            newRowOp.TglOpTutup = new DateTime(tahunBuku, 12, 31);
                            newRowOp.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;                            
                            newRowOp.KategoriId = 54;
                            newRowOp.KategoriNama = "HIBURAN";
                            newRowOp.MetodePenjualan = "0";
                            newRowOp.MetodePembayaran = "0";
                            newRowOp.JumlahKaryawan = 0;
                            newRowOp.InsDate = DateTime.Now;
                            newRowOp.InsBy = "JOB";
                            newRowOp.IsTutup = 1;
                            newRowOp.WilayahPajak ="0";

                            newRowOp.TahunBuku = tahunBuku;
                            newRowOp.Akun = "-";
                            newRowOp.NamaAkun = "-";
                            newRowOp.Kelompok = "-";
                            newRowOp.NamaKelompok = "-";
                            newRowOp.Jenis = "-";
                            newRowOp.NamaJenis = "-";
                            newRowOp.Objek = "-";
                            newRowOp.NamaObjek = "-";
                            newRowOp.Rincian = "-";
                            newRowOp.NamaRincian = "-";
                            newRowOp.SubRincian = "-";
                            newRowOp.NamaSubRincian = "-";

                            _contMonPd.DbOpHiburans.Add(newRowOp);
                            _contMonPd.SaveChanges();

                            var newRow = new DbMonHiburan();
                            newRow.Nop = itemRealisasi.NOP;
                            newRow.Npwpd = "-";
                            newRow.NpwpdNama = "-";
                            newRow.NpwpdAlamat = "-";
                            newRow.PajakId = KDPajak;
                            newRow.PajakNama = "Pajak Jasa Hiburan";
                            newRow.NamaOp = "-";
                            newRow.AlamatOp = "-";
                            newRow.AlamatOpKdLurah = "-";
                            newRow.AlamatOpKdCamat = "-";
                            newRow.TglOpTutup = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;
                            newRow.IsTutup = 1;
                            newRow.KategoriId = -1;
                            newRow.KategoriNama = "-";
                            newRow.TahunBuku = tahunBuku;
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
                            newRow.TahunPajakKetetapan = itemRealisasi.TAHUN_PAJAK;
                            newRow.MasaPajakKetetapan = itemRealisasi.MASA_PAJAK;
                            newRow.SeqPajakKetetapan = itemRealisasi.SEQ;
                            newRow.KategoriKetetapan = "4";
                            newRow.TglKetetapan = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglJatuhTempoBayar = itemRealisasi.JATUH_TEMPO;
                            newRow.PokokPajakKetetapan = itemRealisasi.NOMINAL_POKOK;
                            newRow.PengurangPokokKetetapan = 0;
                            newRow.AkunKetetapan = "-";
                            newRow.KelompokKetetapan = "-";
                            newRow.JenisKetetapan = "-";
                            newRow.ObjekKetetapan = "-";
                            newRow.RincianKetetapan = "-";
                            newRow.SubRincianKetetapan = "-";
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";
                            newRow.UpdDate = DateTime.Now;
                            newRow.UpdBy = "JOB";
                            newRow.TglBayarPokok = itemRealisasi.TRANSACTION_DATE;
                            newRow.NominalPokokBayar = itemRealisasi.NOMINAL_POKOK;
                            newRow.AkunPokokBayar = "-";
                            newRow.Kelompok = "-";
                            newRow.JenisPokokBayar = "-";
                            newRow.ObjekPokokBayar = "-";
                            newRow.RincianPokokBayar = "-";
                            newRow.SubRincianPokokBayar = "-";
                            newRow.TglBayarSanksi = itemRealisasi.TRANSACTION_DATE;
                            newRow.NominalSanksiBayar = itemRealisasi.NOMINAL_SANKSI;
                            newRow.AkunSanksiBayar = "-";
                            newRow.KelompokSanksiBayar = "-";
                            newRow.JenisSanksiBayar = "-";
                            newRow.ObjekSanksiBayar = "-";
                            newRow.RincianSanksiBayar = "-";
                            newRow.SubRincianSanksiBayar = "-";
                            newRow.TglBayarSanksiKenaikan = itemRealisasi.TRANSACTION_DATE;

                            newRow.NominalSanksiBayar = 0;
                            newRow.AkunSanksiBayar = "-";
                            newRow.KelompokSanksiBayar = "-";
                            newRow.JenisSanksiBayar = "-";
                            newRow.ObjekSanksiBayar = "-";
                            newRow.RincianSanksiBayar = "-";
                            newRow.SubRincianSanksiBayar = "-";
                            newList.Add(newRow);
                        }




                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] REALISASI HIBURAN TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}     [({persen:F2}%)]");
                }
                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbMonHiburans.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbMonHiburans.UpdateRange(updateList);
                    _contMonPd.SaveChanges();
                }
                sw.Stop();
                Console.Write($"Done {sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik");
                Console.WriteLine($"");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing REALISASI {ex.Message}");
            }
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPHIBURAN.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPHIBURAN.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }

        private List<string> GetKategoriOvveride(string nop, string kategori)
        {
            var ret = new List<string>();
            ret.Add("54");
            ret.Add("HIBURAN");

            var c = DBClass.GetMonitoringDbContext();
            var connection = c.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @" SELECT *
                                        FROM T_OP_KATEGORI_STATUS
                                        WHERE REPLACE(FK_NOP,'.','')=:NOP  AND ROWNUM=1";
                var param = command.CreateParameter();
                param.ParameterName = "NOP";
                param.Value = nop;
                command.Parameters.Add(param);
                var dr = command.ExecuteReader();

                if (dr.Read())
                {
                    var katname = dr.GetString(2);
                    if (!string.IsNullOrEmpty(katname))
                    {
                        katname = katname.Replace(" ", "").ToUpper().Trim();
                        switch (katname)
                        {
                            case "PERMAINANANAK/PERMAINANKETANGKASAN":
                                ret[0] = "50";
                                ret[1] = "PERMAINAN ANAK/PERMAINAN KETANGKASAN";
                                break;
                            case "GEDUNGOLAHRAGA":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "KARAOKEKELUARGA":
                                ret[0] = "45";
                                ret[1] = "KARAOKE KELUARGA";
                                break;
                            case "PANTIPIJAT/THERAPY/SAUNA/SPA":
                                ret[0] = "48";
                                ret[1] = "PANTI PIJAT/THERAPY/SAUNA/SPA";
                                break;
                            case "TAMANSATWA/PEMANDIANALAM/TAMANREKREASI/WISATATIRTA/REKREASIAIR":
                                ret[0] = "50";
                                ret[1] = "PERMAINAN ANAK/PERMAINAN KETANGKASAN";
                                break;
                            case "BILLYARD":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "KOLAMRENANG":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "KARAOKEDEWASA":
                                ret[0] = "44";
                                ret[1] = "KARAOKE DEWASA";
                                break;
                            case "BIOSKOP":
                                ret[0] = "42";
                                ret[1] = "BIOSKOP";
                                break;
                            case "PAMERANSENIBUDAYA,SENIUKIR,BARANGSENI,TUMBU":
                                ret[0] = "50";
                                ret[1] = "PERMAINAN ANAK/PERMAINAN KETANGKASAN";
                                break;
                            case "BAR/CAFE/KLABMALAM/DISKOTIK":
                                ret[0] = "41";
                                ret[1] = "BAR/CAFE/KLAB MALAM/DISKOTIK";
                                break;
                            case "FUTSAL(OLAHRAGA)":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "FITNESS/PUSATKEBUGARAN":
                                ret[0] = "43";
                                ret[1] = "FITNESS/PUSAT KEBUGARAN";
                                break;
                            case "BOWLING":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "OLAHRAGA":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            default:
                                ret[0] = "54";
                                ret[1] = "HIBURAN";
                                break;
                        }
                    }
                    else
                    {
                        katname = kategori.Replace(" ", "").ToUpper().Trim();
                        switch (katname)
                        {
                            case "FITNESS/PUSATKEBUGARAN":
                                ret[0] = "43";
                                ret[1] = "FITNESS/PUSAT KEBUGARAN";
                                break;
                            case "KARAOKEKELUARGA":
                                ret[0] = "45";
                                ret[1] = "KARAOKE KELUARGA";
                                break;
                            case "PANTIPIJAT/THERAPY/SAUNA/SPA":
                                ret[0] = "48";
                                ret[1] = "PANTI PIJAT/THERAPY/SAUNA/SPA";
                                break;
                            case "OLAHRAGA":
                                ret[0] = "46";
                                ret[1] = "OLAHRAGA";
                                break;
                            case "PERMAINANANAK":
                                case "PERMAINANKETANGKASAN":
                                ret[0] = "50";
                                ret[1] = "PERMAINAN ANAK/PERMAINAN KETANGKASAN";
                                break;
                            case "BAR/CAFE/KLABMALAM/DISKOTIK":
                                ret[0] = "41";
                                ret[1] = "BAR/CAFE/KLAB MALAM/DISKOTIK";
                                break;
                            case "PERMAINANANAK/PERMAINANKETANGKASAN":
                                ret[0] = "50";
                                ret[1] = "PERMAINAN ANAK/PERMAINAN KETANGKASAN";
                                break;
                            case "BIOSKOP":
                                ret[0] = "42";
                                ret[1] = "BIOSKOP";
                                break;
                            case "KARAOKEDEWASA":
                                ret[0] = "44";
                                ret[1] = "KARAOKE DEWASA";
                                break;
                            default:
                                ret[0] = "54";
                                ret[1] = "HIBURAN";
                                break;
                        }
                    }
                }

                var command2 = connection.CreateCommand();
                command2.CommandText = @" 
                                SELECT DISTINCT REPLACE(FK_NOP, '.', '') FK_NOP, NAMA_AYAT_PAJAK
                                FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
                                WHERE NAMA_PAJAK_DAERAH='HIBURAN' 
	                                AND NAMA_AYAT_PAJAK = 'Pajak Hiburan Insidentil' 
	                                AND A.FK_NOP IS NOT NULL 
	                                AND REPLACE(FK_NOP, '.', '') = :NOP
	                                AND ROWNUM=1
                            ";
                var param2 = command2.CreateParameter();
                param2.ParameterName = "NOP";
                param2.Value = nop;
                command2.Parameters.Add(param2);
                var dr2 = command2.ExecuteReader();

                if (dr2.Read())
                {
                    var insidentil = dr2.GetString(0);
                    if (!string.IsNullOrEmpty(insidentil))
                    {
                        ret[0] = "64";
                        ret[1] = "INSIDENTIL";
                    }
                }

                dr.Close();
            }
            catch
            {

            }

            connection.Close();
            return ret;
        }
        public static List<string> GetInfoWPHPP(string npwpd)
        {
            var ret = new List<string>();
            var c = DBClass.GetMonitoringDbContext();
            var connection = c.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @" SELECT  NVL(NAMAWP,'-'),NVL(ALAMAT,'-')
                                    FROM PHRH_USER.npwpd_baru@LIHATHR
                                    WHERE NPWPD=:NPWPD  AND ROWNUM=1";
                var param = command.CreateParameter();
                param.ParameterName = "NPWPD";
                param.Value = npwpd;
                command.Parameters.Add(param);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    ret.Add(dr.GetString(0));
                    ret.Add(dr.GetString(1));
                }
                else
                {
                    ret.Add("-");
                    ret.Add("-");
                }
                dr.Close();
            }
            catch
            {

            }

            connection.Close();
            return ret;
        }
        private Helper.DbAkun GetDbAkun(int tahun, int idPajak, int idKategori)
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
    }
}
