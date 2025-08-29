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

namespace AbtWs
{
    public class WorkerOld : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<WorkerOld> _logger;
        private static int KDPajak = 6;
        private static string NAMA_PAJAK = "ABT";

        public WorkerOld(ILogger<WorkerOld> logger)
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
                    "ERROR Abt WS",
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
            int tahunAmbil = 2023;

            for (var i = tahunAmbil; i <= 2024; i++)
            {
                GetOPProcess(i);
            }

            for (var i = tahunAmbil; i <= 2024; i++)
            {
                GetRealisasi(i);
            }

            MailHelper.SendMail(
            false,
            "DONE Abt WS",
            $@"Abt WS FINISHED",
            null
            );
        }


        private void GetOPProcess(int tahunBuku)
        {
            var tglMulai = DateTime.Now;
            var sw = new Stopwatch();
            sw.Start();
            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                                                                             SELECT *
FROM (
SELECT NVL(REPLACE(A.FK_NOP, '.', ''), '-') NOP,NVL(FK_NPWPD, '-') NPWPD,NAMA_OP, 5 PAJAK_ID,  'Pajak Jasa Abt' PAJAK_NAMA,
              NVL(ALAMAT_OP, '-') ALAMAT_OP, '-'  ALAMAT_OP_NO,'-' ALAMAT_OP_RT,'-' ALAMAT_OP_RW,NVL(NOMOR_TELEPON, '-') TELP,
              NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH, NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,
              CASE
                  WHEN EXTRACT(YEAR FROM TGL_TUTUP) <= 1990 THEN NULL
                  WHEN EXTRACT(YEAR FROM TGL_TUTUP) IS NULL THEN NULL
                  ELSE TGL_TUTUP
              END AS TGL_OP_TUTUP,
              NVL(TGL_BUKA,TO_DATE('01012000','DDMMYYYY')) TGL_MULAI_BUKA_OP, 0 METODE_PENJUALAN,        0 METODE_PEMBAYARAN,        0 JUMLAH_KARYAWAN,  
             56 KATEGORI_ID,
            'AIR TANAH'   KATEGORI_NAMA,
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
            '-'  NAMA_KELOMPOK,1  IS_TUTUP,'-'  NPWPD_NAMA, '-'  NPWPD_ALAMAT,1 TAHUN_BUKU,'0' DIKELOLA, '0' PUNGUT_TARIF,
0 IS_METERAN_AIR,2 PERUNTUKAN_ID,'NON NIAGA' PERUNTUKAN_NAMA
FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
WHERE NAMA_PAJAK_DAERAH=:PAJAK AND A.FK_NOP IS NOT NULL
)
WHERE  to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND
            (   TGL_OP_TUTUP IS  NULL OR
                 TO_CHAR(TGL_OP_TUTUP,'YYYY') >= :TAHUN OR
                 TO_CHAR(TGL_OP_TUTUP,'YYYY') <=1990
             )
                    ";

                var result = _contMonitoringDB.Set<DbOpAbt>().FromSqlRaw(sql, new[] {
                    new OracleParameter("PAJAK", NAMA_PAJAK),
                    new OracleParameter("TAHUN", tahunBuku)
                }).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbOpAbt>();
                //var updateList = new List<MonPDLib.EF.DbOpAbt>();
                var removeEx = _contMonPd.DbOpAbts.Where(x => x.TahunBuku == tahunBuku).ToList();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        var newRow = new MonPDLib.EF.DbOpAbt();
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
                        newRow.TglOpTutup = item.TglOpTutup;
                        newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                        newRow.PeruntukanId = 2;
                        newRow.PeruntukanNama = "NON NIAGA";
                        newRow.KategoriId = item.KategoriId;
                        newRow.KategoriNama = item.KategoriNama;
                        newRow.JumlahKaryawan = item.JumlahKaryawan;
                        newRow.InsDate = item.InsDate;
                        newRow.InsBy = item.InsBy;
                        newRow.IsTutup = item.IsTutup;
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

                        newRow.Kelompok = "-";
                        newRow.NamaKelompok = "-";
                        newList.Add(newRow);
                    }
                    catch (Exception ex)
                    {
                        var kkk = item.Nop;
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] OP Abt TAHUN {tahunBuku} JML OP {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (removeEx.Any())
                {
                    _contMonPd.DbOpAbts.RemoveRange(removeEx);
                    _contMonPd.SaveChanges();
                }
                if (newList.Any())
                {
                    _contMonPd.DbOpAbts.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                //if (updateList.Any())
                //{
                //    _contMonPd.DbOpAbts.UpdateRange(updateList);
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
           SELECT     NOP,MASA_PAJAK,TAHUN_PAJAK, SEQ ,MAX(JATUH_TEMPO) AS JATUH_TEMPO,JENIS_PERUNTUKAN,
        SUM(NOMINAL_POKOK) NOMINAL_POKOK,
        SUM(NOMINAL_SANKSI) NOMINAL_SANKSI,
        MAX(TRANSACTION_DATE) TRANSACTION_DATE
FROM (            
    select  LAST_DAY(TO_DATE(BULAN || '-' || TAHUN, 'MM-YYYY'))  JATUH_TEMPO,
     REPLACE(NVL(NOP,0),'.','') NOP,
            TO_NUMBER( NVL(BULAN,0)) MASA_PAJAK,
            TO_NUMBER(NVL(TAHUN,0)) TAHUN_PAJAK, 
            1 SEQ, 
             TO_NUMBER(NVL(SKPD,0)) NOMINAL_POKOK, 
               TO_NUMBER(NVL(DENDA,0)) NOMINAL_SANKSI,
               TO_DATE(TANGGAL) TRANSACTION_DATE
               ,JENIS_PERUNTUKAN              
from    PHRH_USER.VW_SIMPADAHPP_SSPD_abt@LIHATHR
WHERE   TO_CHAR(tanggal,'YYYY')=:TAHUN
) A
GROUP BY NOP, MASA_PAJAK, TAHUN_PAJAK,SEQ,JENIS_PERUNTUKAN                                     
                ";

                var realisasiMonitoringDb = _contMonitoringDB.Set<SSPDABT>()
                    .FromSqlRaw(sqlRealisasi, new[] {                                
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();
                
                int jmlData = realisasiMonitoringDb.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonAbt>();
                var updateList = new List<MonPDLib.EF.DbMonAbt>();
         
                foreach (var itemRealisasi in realisasiMonitoringDb)
                {
                    var realisasi = _contMonPd.DbMonAbts.SingleOrDefault(x => x.Nop == itemRealisasi.NOP &&
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
                        switch (itemRealisasi.JENIS_PERUNTUKAN)
                        {
                            case "Niaga":
                                realisasi.PeruntukanId = 1;
                                realisasi.PeruntukanNama = "NIAGA";
                                break;
                            case "Bhn Baku Air":
                                realisasi.PeruntukanId = 3;
                                realisasi.PeruntukanNama = "BAHAN BAKU AIR";
                                break;
                            case "Non Niaga":
                                realisasi.PeruntukanId = 2;
                                realisasi.PeruntukanNama = "NON NIAGA";
                                break;
                            default:
                                realisasi.PeruntukanId = 2;
                                realisasi.PeruntukanNama = "NON NIAGA";
                                break;
                        }
                        
                        updateList.Add(realisasi);
                    }
                    else
                    {
                        var OP = _contMonPd.DbOpAbts.Find(itemRealisasi.NOP, (decimal)tahunBuku);

                        if (OP != null)
                        {                            
                            var AkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)OP.KategoriId);
                            var AkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);

                            var newRow = new DbMonAbt();
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
                            newRow.KategoriKetetapan = "3";
                            newRow.TglKetetapan = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglJatuhTempoBayar = itemRealisasi.JATUH_TEMPO;
                            newRow.PokokPajakKetetapan = itemRealisasi.NOMINAL_POKOK;
                            newRow.PengurangPokokKetetapan = 0;
                            newRow.AkunKetetapan = AkunPokok.Akun;

                            switch (itemRealisasi.JENIS_PERUNTUKAN)
                            {
                                case "Niaga":
                                    newRow.PeruntukanId = 1;
                                    newRow.PeruntukanNama = "NIAGA";
                                    OP.PeruntukanId = 1;
                                    OP.PeruntukanNama= "NIAGA";
                                    break;
                                case "Bhn Baku Air":
                                    newRow.PeruntukanId = 3;
                                    newRow.PeruntukanNama = "BAHAN BAKU AIR";
                                    OP.PeruntukanId = 3;
                                    OP.PeruntukanNama = "BAHAN BAKU AIR";
                                    break;
                                case "Non Niaga":
                                    newRow.PeruntukanId = 2;
                                    newRow.PeruntukanNama = "NON NIAGA";
                                    OP.PeruntukanId = 2;
                                    OP.PeruntukanNama = "NON NIAGA";
                                    break;
                                default:
                                    newRow.PeruntukanId = 2;
                                    newRow.PeruntukanNama = "NON NIAGA";
                                    OP.PeruntukanId = 2;
                                    OP.PeruntukanNama = "NON NIAGA";
                                    break;
                            }

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
                            newRow.NoKetetapan = "-";
                            newList.Add(newRow);
                        }
                        else
                        {

                            var newRowOP = new MonPDLib.EF.DbOpAbt();
                            newRowOP.Nop = itemRealisasi.NOP;
                            newRowOP.Npwpd = "-";
                            newRowOP.NpwpdNama = "-";
                            newRowOP.NpwpdAlamat = "-";
                            newRowOP.PajakId = KDPajak;
                            newRowOP.PajakNama = "Pajak Air Tanah";
                            newRowOP.NamaOp = "-";
                            newRowOP.AlamatOp = "-";
                            newRowOP.AlamatOpNo = "-";
                            newRowOP.AlamatOpRt = "-";
                            newRowOP.AlamatOpRw = "-";
                            newRowOP.Telp = "-";
                            newRowOP.AlamatOpKdLurah = "-";
                            newRowOP.AlamatOpKdCamat = "-";
                            newRowOP.TglOpTutup = new DateTime(tahunBuku, 12, 31);
                            newRowOP.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;
                            newRowOP.KategoriId = 56;
                            newRowOP.KategoriNama = "AIR TANAH";
                            newRowOP.JumlahKaryawan = 0;
                            newRowOP.InsDate = DateTime.Now;
                            newRowOP.InsBy = "JOB";
                            newRowOP.IsTutup = 1;
                            newRowOP.WilayahPajak = "0";
                            newRowOP.TahunBuku = tahunBuku;
                            newRowOP.Akun = "-";
                            newRowOP.NamaAkun = "-";
                            newRowOP.Kelompok = "-";
                            newRowOP.NamaKelompok = "-";
                            newRowOP.Jenis = "-";
                            newRowOP.NamaJenis = "-";
                            newRowOP.Objek = "-";
                            newRowOP.NamaObjek = "-";
                            newRowOP.Rincian = "-";
                            newRowOP.NamaRincian = "-";
                            newRowOP.SubRincian = "-";
                            newRowOP.NamaSubRincian = "-";
                            newRowOP.PeruntukanId = 2;
                            newRowOP.PeruntukanNama = "NON NIAGA";
                            newRowOP.Kelompok = "-";
                            newRowOP.NamaKelompok = "-";                          
                            _contMonPd.DbOpAbts.Add(newRowOP);
                            _contMonPd.SaveChanges();

                            var newRow = new DbMonAbt();
                            newRow.Nop = itemRealisasi.NOP;
                            newRow.Npwpd = "-";
                            newRow.NpwpdNama = "-";
                            newRow.NpwpdAlamat = "-";
                            newRow.PajakId = KDPajak;
                            newRow.PajakNama = "Pajak Jasa Abt";
                            newRow.NamaOp = "-";
                            newRow.AlamatOp = "-";
                            newRow.AlamatOpKdLurah = "-";
                            newRow.AlamatOpKdCamat = "-";
                            
                            newRow.TglOpTutup = itemRealisasi.TRANSACTION_DATE;
                            newRow.TglMulaiBukaOp = itemRealisasi.TRANSACTION_DATE;
                            newRow.IsTutup = 1;
                            newRow.KategoriId = 56;
                            newRow.KategoriNama = "ABT";
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
                            switch (itemRealisasi.JENIS_PERUNTUKAN)
                            {
                                case "Niaga":
                                    newRow.PeruntukanId = 1;
                                    newRow.PeruntukanNama = "NIAGA";                                    
                                    break;
                                case "Bhn Baku Air":
                                    newRow.PeruntukanId = 3;
                                    newRow.PeruntukanNama = "BAHAN BAKU AIR";                                    
                                    break;
                                case "Non Niaga":
                                    newRow.PeruntukanId = 2;
                                    newRow.PeruntukanNama = "NON NIAGA";                                    
                                    break;
                                default:
                                    newRow.PeruntukanId = 2;
                                    newRow.PeruntukanNama = "NON NIAGA";                                    
                                    break;
                            }
                            newList.Add(newRow);
                        }
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] REALISASI ABT TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}     [({persen:F2}%)]");
                }
                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbMonAbts.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbMonAbts.UpdateRange(updateList);
                    _contMonPd.SaveChanges();
                }
                sw.Stop();
                Console.Write($"Done {sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik");
                Console.WriteLine($"");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error processing REALISASI {ex.Message} | Inner: {ex.InnerException?.Message ?? ""}");
            }
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
