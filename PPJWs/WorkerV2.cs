using Microsoft.EntityFrameworkCore;
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

namespace PPJWs
{
    public class WorkerV2 : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<WorkerV2> _logger;
        private static int KDPajak = 2;
        private static string NAMA_PAJAK = "PPJ";

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
            }

            MailHelper.SendMail(
            false,
            "DONE PPJ WS",
            $@"PPJ WS FINISHED",
            null
            );
        }


        private void GetOPProcess(int tahunBuku)
        {
            var tglMulai = DateTime.Now;
            var sw = new Stopwatch();
            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                                                                      SELECT *
FROM (
SELECT REPLACE(A.FK_NOP, '.', '') NOP,NVL(FK_NPWPD, '-') NPWPD,NAMA_OP, 5 PAJAK_ID,  'Pajak Tenaga Listrik' PAJAK_NAMA,
              NVL(ALAMAT_OP, '-') ALAMAT_OP, '-'  ALAMAT_OP_NO,'-' ALAMAT_OP_RT,'-' ALAMAT_OP_RW,NVL(NOMOR_TELEPON, '-') TELP,
              NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH, NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,TGL_TUTUP TGL_OP_TUTUP,
              NVL(TGL_BUKA,TO_DATE('01012000','DDMMYYYY')) TGL_MULAI_BUKA_OP, 0 METODE_PENJUALAN,        0 METODE_PEMBAYARAN,        0 JUMLAH_KARYAWAN,  
                CASE                             
                        WHEN NAMA_JENIS_PAJAK = 'PPJ NON PLN' THEN 55
                        WHEN NAMA_JENIS_PAJAK = 'PPJ PLN' THEN 58
                ELSE 58
            END AS KATEGORI_ID,
            NVL(NAMA_JENIS_PAJAK,'-') KATEGORI_NAMA,
                CASE                             
                        WHEN NAMA_JENIS_PAJAK = 'PPJ NON PLN' THEN 55
                        WHEN NAMA_JENIS_PAJAK = 'PPJ PLN' THEN 58
                ELSE 58
            END AS PERUNTUKAN,
            NVL(NAMA_JENIS_PAJAK, '-')   PERUNTUKAN_NAMA,
                CASE                             
                        WHEN NAMA_JENIS_PAJAK = 'PPJ NON PLN' THEN 55
                        WHEN NAMA_JENIS_PAJAK = 'PPJ PLN' THEN 58
                ELSE 58
            END AS SUMBER,
            NVL(NAMA_JENIS_PAJAK, '-')   SUMBER_NAMA,
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
WHERE NAMA_PAJAK_DAERAH=:PAJAK AND A.FK_NOP IS NOT NULL AND A.FK_NOP IS NOT NULL  AND FK_NOP NOT LIKE '00%'    AND FK_NOP NOT LIKE '-%'
)
WHERE  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN) OR  TO_CHAR(TGL_OP_TUTUP,'YYYY') <=1990
                    ";

                var result = _contMonitoringDB.Set<DbOpListrik>().FromSqlRaw(sql, new[] {
                    new OracleParameter("PAJAK", NAMA_PAJAK),
                    new OracleParameter("TAHUN", tahunBuku)
                }).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbOpListrik>();
                var updateList = new List<MonPDLib.EF.DbOpListrik>();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        var sourceRow = _contMonPd.DbOpListriks.Find(item.Nop, (decimal)tahunBuku);
                        if (sourceRow != null)
                        {
                            sourceRow.TglOpTutup = item.TglOpTutup;
                            sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                            sourceRow.IsTutup = item.TglOpTutup == null ? 0 : item.TglOpTutup.Value.Year <= tahunBuku ? 1 : 0;
                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                            sourceRow.Akun = dbakun.Akun;
                            sourceRow.NamaAkun = dbakun.NamaAkun;
                            sourceRow.Kelompok = dbakun.Kelompok;
                            sourceRow.NamaKelompok = dbakun.NamaKelompok;
                            sourceRow.Jenis = dbakun.Jenis;
                            sourceRow.NamaJenis = dbakun.NamaJenis;
                            sourceRow.Objek = dbakun.Objek;
                            sourceRow.NamaObjek = dbakun.NamaObjek;
                            sourceRow.Rincian = dbakun.Rincian;
                            sourceRow.NamaRincian = dbakun.NamaRincian;
                            sourceRow.SubRincian = dbakun.SubRincian;
                            sourceRow.NamaSubRincian = dbakun.NamaSubRincian;
                            updateList.Add(sourceRow);
                        }
                        else
                        {
                            var newRow = new MonPDLib.EF.DbOpListrik();
                            newRow.Nop = item.Nop;
                            newRow.Npwpd = item.Npwpd;
                            // set manual
                            var infoWP = GetInfoWPHPP(newRow.Npwpd);
                            newRow.NpwpdNama = infoWP[0];
                            newRow.NpwpdAlamat = infoWP[1];
                            newRow.JumlahKaryawan = item.JumlahKaryawan;
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
                            newRow.TglOpTutup = item.TglOpTutup;
                            newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.Sumber = item.Sumber;
                            newRow.SumberNama = item.SumberNama;
                            newRow.JumlahKaryawan = item.JumlahKaryawan;
                            newRow.InsDate = item.InsDate;
                            newRow.InsBy = item.InsBy;
                            newRow.IsTutup = item.IsTutup;
                            newRow.WilayahPajak = item.WilayahPajak;
                            newRow.Peruntukan = item.Peruntukan;
                            newRow.PeruntukanNama = item.PeruntukanNama;
                            newRow.Sumber = item.Sumber;
                            newRow.SumberNama = item.SumberNama;

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


                    }
                    catch (Exception ex)
                    {
                        var kkk = item.Nop;
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] OP LISTRIK TAHUN {tahunBuku} JML OP {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbOpListriks.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbOpListriks.UpdateRange(updateList);
                    _contMonPd.SaveChanges();
                }
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
                var newList = new List<MonPDLib.EF.DbMonPpj>();
                var updateList = new List<MonPDLib.EF.DbMonPpj>();

                foreach (var itemRealisasi in realisasiMonitoringDb)
                {
                    var realisasi = _contMonPd.DbMonPpjs.SingleOrDefault(x => x.Nop == itemRealisasi.NOP &&
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
                        var OP = _contMonPd.DbOpListriks.Find(itemRealisasi.NOP, (decimal)tahunBuku);

                        if (OP != null)
                        {                            
                            var AkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)OP.KategoriId);
                            var AkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);

                            var newRow = new DbMonPpj();
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
                            newRow.SumberNama = OP.SumberNama;
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
                            newRow.PeruntukanNama = "-";
                            newRow.SumberNama = "-";


                            newRow.TglBayarPokok = itemRealisasi.TRANSACTION_DATE;
                            newRow.NominalPokokBayar = itemRealisasi.NOMINAL_POKOK;
                            newRow.AkunPokokBayar = AkunPokok.Akun;
                            newRow.Kelompok = AkunPokok.Kelompok;
                            newRow.JenisPokokBayar = AkunPokok.Jenis;
                            newRow.ObjekPokokBayar = AkunPokok.Objek;
                            newRow.RincianPokokBayar = AkunPokok.Rincian;
                            newRow.SubRincianPokokBayar = AkunPokok.SubRincian;
                            newRow.TglBayarSanksi = itemRealisasi.TRANSACTION_DATE;
                            newRow.NominalSanksiBayar = itemRealisasi.NOMINAL_SANKSI;
                            newRow.AkunSanksiBayar = AkunSanksi.Akun;
                            newRow.KelompokSanksiBayar = AkunSanksi.Kelompok;
                            newRow.JenisSanksiBayar = AkunSanksi.Jenis;
                            newRow.ObjekSanksiBayar = AkunSanksi.Objek;
                            newRow.RincianSanksiBayar = AkunSanksi.Rincian;
                            newRow.SubRincianSanksiBayar = AkunSanksi.SubRincian;
                            newRow.TglBayarSanksiKenaikan = itemRealisasi.TRANSACTION_DATE;

                            newRow.NominalSanksiKenaikanBayar = 0;
                            newRow.AkunKenaikanBayar = AkunSanksi.Akun;
                            newRow.KelompokKenaikanBayar = AkunSanksi.Kelompok;
                            newRow.JenisKenaikanBayar = AkunSanksi.Jenis;
                            newRow.ObjekKenaikanBayar = AkunSanksi.Objek;
                            newRow.RincianKenaikanBayar = AkunSanksi.Rincian;
                            newRow.SubRincianKenaikanBayar = AkunSanksi.SubRincian;
                            newList.Add(newRow);
                        }
                        else
                        {
                            var newRowOp = new MonPDLib.EF.DbOpListrik();
                            newRowOp.Nop = itemRealisasi.NOP;
                            newRowOp.Npwpd = "-";
                            newRowOp.NpwpdNama = "-";
                            newRowOp.NpwpdAlamat = "-";
                            newRowOp.JumlahKaryawan = 0;
                            newRowOp.PajakId = KDPajak;
                            newRowOp.PajakNama = "Pajak Jasa Listrik";
                            newRowOp.NamaOp = "-"; 
                            newRowOp.AlamatOp = "-" ;
                            newRowOp.AlamatOpNo = "-";
                            newRowOp.AlamatOpRt = "-";
                            newRowOp.AlamatOpRw = "-";
                            newRowOp.Telp = "-";
                            newRowOp.AlamatOpKdLurah = "-";
                            newRowOp.AlamatOpKdCamat = "-";
                            newRowOp.TglOpTutup = new DateTime(tahunBuku, 12, 31);
                            newRowOp.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;
                            newRowOp.KategoriId = 55;
                            newRowOp.KategoriNama = "PPJ NON PLN";
                            newRowOp.Sumber = 0;
                            newRowOp.SumberNama = "-";
                            newRowOp.JumlahKaryawan = 0;
                            newRowOp.InsDate = DateTime.Now;
                            newRowOp.InsBy = "JOB";
                            newRowOp.IsTutup = 1;
                            newRowOp.WilayahPajak = "0";
                            newRowOp.Peruntukan = 0;
                            newRowOp.PeruntukanNama = "-" ;
                            newRowOp.Sumber = 0;
                            newRowOp.SumberNama = "-";

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


                            _contMonPd.DbOpListriks.Add(newRowOp);
                            _contMonPd.SaveChanges();

                            var newRow = new DbMonPpj();
                            newRow.Nop = itemRealisasi.NOP;
                            newRow.Npwpd = "-";
                            newRow.NpwpdNama = "-";
                            newRow.NpwpdAlamat = "-";
                            newRow.PajakId = KDPajak;
                            newRow.PajakNama = "Pajak Jasa PPJ";
                            newRow.NamaOp = "-";
                            newRow.AlamatOp = "-";
                            newRow.AlamatOpKdLurah = "-";
                            newRow.AlamatOpKdCamat = "-";
                            newRow.TglOpTutup = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;
                            newRow.IsTutup =1;
                            newRow.SumberNama = "-";
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
                            newRow.PeruntukanNama = "-";
                            newRow.SumberNama = "-";

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

                            newRow.NominalSanksiKenaikanBayar = 0;
                            newRow.AkunKenaikanBayar = "-";
                            newRow.KelompokKenaikanBayar = "-";
                            newRow.JenisKenaikanBayar = "-";
                            newRow.ObjekKenaikanBayar = "-";
                            newRow.RincianKenaikanBayar = "-";
                            newRow.SubRincianKenaikanBayar = "-";
                            newList.Add(newRow);
                        }
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r{tglMulai.ToString("dd MMM yyyy HH:mm:ss")} REALISASI PPJ TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }
                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbMonPpjs.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbMonPpjs.UpdateRange(updateList);
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
