using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace PbbWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 9;

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

                //// GUNAKAN KETIKA EKSEKUSI TUGAS MANUAL
                try
                {
                    await DoWorkNewMeta(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR PBB WS",
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

        private async Task DoWorkNewMeta(CancellationToken stoppingToken)
        {
            //var tglServer = DateTime.Now;
            //var _contMonPd = DBClass.GetContext();
            //int tahunAmbil = tglServer.Year;
            //var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            //tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

            //// do fill db op HOTEL
            //if (IsGetDBOp())
            //{
            //    GetOPProcess();
            //}

            //MailHelper.SendMail(
            //false,
            //"DONE HOTEL  WS",
            //$@"HOTEL WS FINISHED",
            //null
            //);
        }

//        private void GetOPProcess()
//        {
//            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
//            {
//                var sql = @"
//                            select  T_PROP_KD || T_DATI2_KD || T_KEC_KD || T_KEL_KD || D_NOP_BLK || D_NOP_URUT ||  D_NOP_JNS      NOP,
//          57 KATEGORI_ID,'PBB' KATEGORI_NAMA,
//          D_OP_JLN ALAMAT_OP,
//           D_OP_JLNO  ALAMAT_OP_NO,
//           D_OP_RT   ALAMAT_OP_RT  ,
//           D_OP_RW     ALAMAT_OP_RW ,
//          KD_CAMAT   ALAMAT_KD_CAMAT  ,
//  KD_LURAH ALAMAT_KD_LURAH  ,
//  UPTD UPTB     ,
// D_TNH_LUAS LUAS_TANAH,
//  D_WP_JLN  ALAMAT_WP ,
//  D_WP_JLNO ALAMAT_WP_NO,
// D_WP_KEL  ALAMAT_WP_KEL,
//  D_WP_KOTA ALAMAT_WP_KOTA ,
//  D_WP_KTP WP_KTP        ,
//  D_WP_NAMA WP_NAMA       ,
//  D_WP_NPWP WP_NPWP       ,
//  D_WP_RT WP_RT         ,
//  D_WP_RW  WP_RW         ,
//  STATUSAKTIF STATUS        ,
// SYSDATE INS_DATE      ,
//  'JOB 'INS_BY        
//  FROM DATAOP@LIHATGATOTKACA A
//    JOIN M_WILAYAHV1 B ON KD_CAMAT=KD_KECAMATAN AND KD_LURAH=KD_KELURAHAN
//                    ";

//                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).ToList();
//                var _contMonPd = DBClass.GetContext();
//                int jmlData = result.Count;
//                int index = 0;
//                foreach (var item in result)
//                {
//                    // DATA OP
//                    try
//                    {
//                        if (item.Nop == "000000000090302748")
//                        {
//                            var kk = 1;
//                        }
//                        var sourceRow = _contMonPd.DbOpHiburans.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
//                        if (sourceRow != null)
//                        {
//                            sourceRow.TglOpTutup = item.TglOpTutup;
//                            sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

//                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
//                            if (dbakun != null)
//                            {
//                                sourceRow.Akun = dbakun.Akun;
//                                sourceRow.NamaAkun = dbakun.NamaAkun;
//                                sourceRow.Kelompok = dbakun.Kelompok;
//                                sourceRow.NamaKelompok = dbakun.NamaKelompok;
//                                sourceRow.Jenis = dbakun.Jenis;
//                                sourceRow.NamaJenis = dbakun.NamaJenis;
//                                sourceRow.Objek = dbakun.Objek;
//                                sourceRow.NamaObjek = dbakun.NamaObjek;
//                                sourceRow.Rincian = dbakun.Rincian;
//                                sourceRow.NamaRincian = dbakun.NamaRincian;
//                                sourceRow.SubRincian = dbakun.SubRincian;
//                                sourceRow.NamaSubRincian = dbakun.NamaSubRincian;
//                            }
//                            else
//                            {
//                                sourceRow.Akun = item.Akun;
//                                sourceRow.NamaAkun = item.NamaAkun;
//                                sourceRow.Kelompok = item.Kelompok;
//                                sourceRow.NamaKelompok = item.NamaKelompok;
//                                sourceRow.Jenis = item.Jenis;
//                                sourceRow.NamaJenis = item.NamaJenis;
//                                sourceRow.Objek = item.Objek;
//                                sourceRow.NamaObjek = item.NamaObjek;
//                                sourceRow.Rincian = item.Rincian;
//                                sourceRow.NamaRincian = item.NamaRincian;
//                                sourceRow.SubRincian = item.SubRincian;
//                                sourceRow.NamaSubRincian = item.NamaSubRincian;
//                            }
//                        }
//                        else
//                        {
//                            var newRow = new MonPDLib.EF.DbOpHiburan();
//                            newRow.Nop = item.Nop;
//                            newRow.Npwpd = item.Npwpd;
//                            // set manual
//                            var infoWP = GetInfoWPHPP(newRow.Npwpd);
//                            newRow.NpwpdNama = infoWP[0];
//                            newRow.NpwpdAlamat = infoWP[1];
//                            //
//                            newRow.PajakId = item.PajakId;
//                            newRow.PajakNama = item.PajakNama;
//                            newRow.NamaOp = item.NamaOp;
//                            newRow.AlamatOp = item.AlamatOp;
//                            newRow.AlamatOpNo = item.AlamatOpNo;
//                            newRow.AlamatOpRt = item.AlamatOpRt;
//                            newRow.AlamatOpRw = item.AlamatOpRw;
//                            newRow.Telp = item.Telp;
//                            newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
//                            newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
//                            newRow.TglOpTutup = item.TglOpTutup;
//                            newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

//                            var kategori = GetKategoriOvveride(item.Nop);
//                            item.KategoriId = Convert.ToInt32(kategori[0] ?? "54");
//                            item.KategoriNama = kategori[1] ?? "HIBURAN";

//                            newRow.KategoriId = item.KategoriId;
//                            newRow.KategoriNama = item.KategoriNama;
//                            newRow.MetodePenjualan = item.MetodePenjualan;
//                            newRow.MetodePembayaran = item.MetodePembayaran;
//                            newRow.JumlahKaryawan = item.JumlahKaryawan;
//                            newRow.InsDate = item.InsDate;
//                            newRow.InsBy = item.InsBy;
//                            newRow.IsTutup = item.IsTutup;
//                            newRow.WilayahPajak = item.WilayahPajak;

//                            newRow.TahunBuku = tahunBuku;
//                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
//                            if (dbakun != null)
//                            {
//                                newRow.Akun = dbakun.Akun;
//                                newRow.NamaAkun = dbakun.NamaAkun;
//                                newRow.Kelompok = dbakun.Kelompok;
//                                newRow.NamaKelompok = dbakun.NamaKelompok;
//                                newRow.Jenis = dbakun.Jenis;
//                                newRow.NamaJenis = dbakun.NamaJenis;
//                                newRow.Objek = dbakun.Objek;
//                                newRow.NamaObjek = dbakun.NamaObjek;
//                                newRow.Rincian = dbakun.Rincian;
//                                newRow.NamaRincian = dbakun.NamaRincian;
//                                newRow.SubRincian = dbakun.SubRincian;
//                                newRow.NamaSubRincian = dbakun.NamaSubRincian;
//                            }
//                            else
//                            {
//                                newRow.Akun = item.Akun;
//                                newRow.NamaAkun = item.NamaAkun;
//                                newRow.Kelompok = item.Kelompok;
//                                newRow.NamaKelompok = item.NamaKelompok;
//                                newRow.Jenis = item.Jenis;
//                                newRow.NamaJenis = item.NamaJenis;
//                                newRow.Objek = item.Objek;
//                                newRow.NamaObjek = item.NamaObjek;
//                                newRow.Rincian = item.Rincian;
//                                newRow.NamaRincian = item.NamaRincian;
//                                newRow.SubRincian = item.SubRincian;
//                                newRow.NamaSubRincian = item.NamaSubRincian;
//                            }
//                            _contMonPd.DbOpHiburans.Add(newRow);
//                            _contMonPd.SaveChanges();
//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"Error processing NOP {ex.Message}");
//                        var kkk = item.Nop;
//                    }
//                    _contMonPd.SaveChanges();
//                    index++;
//                    double persen = ((double)index / jmlData) * 100;
//                    Console.Write($"\rDB_OP_HIBURAN HPP TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
//                    Thread.Sleep(50);


//                }
//            }
//        }

//        private void HPPKetetapanProcess(int tahunBuku)
//        {
//            var kkk = new OPSkpdHiburan();
//            try
//            {
//                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
//                var _contMonPd = DBClass.GetContext();
//                var sqlKetetapan = @"
//                SELECT 	NVL(NOP, '-') NOP,
//	                    TAHUN,
//	                    MASAPAJAK,
//	                    100 SEQ,
//	                    1 JENIS_KETETAPAN,
//	                    MAX(TGL_KETETAPAN) TGL_KETETAPAN,
//	                    MAX(TGL_JATUH_TEMPO_BAYAR) TGL_JATUH_TEMPO_BAYAR,
//	                    0 NILAI_PENGURANG,
//	                    SUM(NVL(POKOK, 0)) POKOK
//	            FROM ( 
//	            SELECT  REPLACE(FK_NOP,'.','') NOP, 
//                        TO_NUMBER(TAHUN_PAJAK) TAHUN,TO_NUMBER(BULAN_PAJAK) MASAPAJAK,100 SEQ,1 JENIS_KETETAPAN,TO_DATE(NVL(TGL_SPTPD_DISETOR,MP_AKHIR)) TGL_KETETAPAN,TO_DATE(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO_BAYAR ,0 NILAI_PENGURANG,
//                        TO_NUMBER(KETETAPAN_TOTAL)  POKOK
//                FROM VW_SIMPADA_SPTPD@LIHATHPPSERVER
//                WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND EXTRACT(YEAR FROM TO_DATE(NVL(TGL_SPTPD_DISETOR,MP_AKHIR))) = :TAHUN
//	            ) A
//	            GROUP BY NOP,
//	                    TAHUN,
//	                    MASAPAJAK
//                ";

//                var ketetapanMonitoringDb = _contMonitoringDB.Set<OPSkpdHiburan>()
//                    .FromSqlRaw(sqlKetetapan, new[] {
//                                new OracleParameter("TAHUN", tahunBuku.ToString())
//                    }).ToList();

//                int jmlData = ketetapanMonitoringDb.Count;
//                int index = 0;
//                decimal jml = 0;
//                var kkkkkk = ketetapanMonitoringDb.Sum(x => x.POKOK);
//                foreach (var itemKetetapan in ketetapanMonitoringDb)
//                {
//                    jml = jml + itemKetetapan.POKOK;
//                    kkk = itemKetetapan;
//                    var OP = _contMonPd.DbOpHiburans.FirstOrDefault(x => x.Nop == itemKetetapan.NOP.Replace(".", ""));
//                    if (OP != null)
//                    {
//                        var dbAkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)OP.KategoriId);
//                        string nop = itemKetetapan.NOP;
//                        int tahunPajak = itemKetetapan.TAHUN;
//                        int masaPajak = itemKetetapan.MASAPAJAK;
//                        int seqPajak = itemKetetapan.SEQ;
//                        var rowMonHiburan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == nop.Replace(".", "") && x.TahunPajakKetetapan == tahunPajak &&
//                                                                                x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

//                        bool isOPTutup = false;
//                        if (OP.TglOpTutup.HasValue)
//                        {
//                            if (OP.TglOpTutup.Value.Date.Year <= tahunBuku)
//                            {
//                                isOPTutup = true;
//                            }

//                        }


//                        if (rowMonHiburan != null)
//                        {
//                            _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
//                            _contMonPd.SaveChanges();
//                        }

//                        var newRow = new DbMonHiburan();
//                        newRow.Nop = OP.Nop;
//                        newRow.Npwpd = OP.Npwpd;
//                        newRow.NpwpdNama = OP.NpwpdNama;
//                        newRow.NpwpdAlamat = OP.NpwpdAlamat;
//                        newRow.PajakId = OP.PajakId;
//                        newRow.PajakNama = OP.PajakNama;
//                        newRow.NamaOp = OP.NamaOp;
//                        newRow.AlamatOp = OP.AlamatOp;
//                        newRow.AlamatOpKdLurah = OP.AlamatOpKdLurah;
//                        newRow.AlamatOpKdCamat = OP.AlamatOpKdCamat;
//                        newRow.TglOpTutup = OP.TglOpTutup;
//                        newRow.TglMulaiBukaOp = OP.TglMulaiBukaOp;
//                        newRow.IsTutup = isOPTutup ? 1 : 0;
//                        newRow.KategoriId = OP.KategoriId;
//                        newRow.KategoriNama = OP.KategoriNama;
//                        newRow.TahunBuku = tahunBuku;
//                        newRow.Akun = OP.Akun;
//                        newRow.NamaAkun = OP.NamaAkun;
//                        newRow.Jenis = OP.Jenis;
//                        newRow.NamaJenis = OP.NamaJenis;
//                        newRow.Objek = OP.Objek;
//                        newRow.NamaObjek = OP.NamaObjek;
//                        newRow.Rincian = OP.Rincian;
//                        newRow.NamaRincian = OP.NamaRincian;
//                        newRow.SubRincian = OP.SubRincian;
//                        newRow.NamaSubRincian = OP.NamaSubRincian;
//                        newRow.TahunPajakKetetapan = itemKetetapan.TAHUN;
//                        newRow.MasaPajakKetetapan = itemKetetapan.MASAPAJAK;
//                        newRow.SeqPajakKetetapan = itemKetetapan.SEQ;
//                        newRow.KategoriKetetapan = itemKetetapan.JENIS_KETETAPAN.ToString();
//                        newRow.TglKetetapan = itemKetetapan.TGL_KETETAPAN;
//                        newRow.TglJatuhTempoBayar = itemKetetapan.TGL_JATUH_TEMPO_BAYAR;
//                        newRow.PokokPajakKetetapan = itemKetetapan.POKOK - itemKetetapan.NILAI_PENGURANG;
//                        newRow.PengurangPokokKetetapan = itemKetetapan.NILAI_PENGURANG;
//                        newRow.AkunKetetapan = dbAkunPokok.Akun;
//                        newRow.KelompokKetetapan = dbAkunPokok.Kelompok;
//                        newRow.JenisKetetapan = dbAkunPokok.Jenis;
//                        newRow.ObjekKetetapan = dbAkunPokok.Objek;
//                        newRow.RincianKetetapan = dbAkunPokok.Rincian;
//                        newRow.SubRincianKetetapan = dbAkunPokok.SubRincian;
//                        newRow.InsDate = DateTime.Now;
//                        newRow.InsBy = "JOB";
//                        newRow.UpdDate = DateTime.Now;
//                        newRow.UpdBy = "JOB";
//                        _contMonPd.DbMonHiburans.Add(newRow);
//                        _contMonPd.SaveChanges();
//                        index++;
//                        double persen = ((double)index / jmlData) * 100;
//                        Console.Write($"\rDB_KETETAPAN_HIBURAN HPP TAHUN {tahunBuku} JML OP {jmlData} : {jml.ToString("n0")}  {persen:F2}%   ");
//                        Thread.Sleep(50);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error processing NOP {ex.Message}");
//            }
//        }

//        private void HPPRealisasiProcess(int tahunBuku)
//        {
//            try
//            {
//                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
//                var _contMonPd = DBClass.GetContext();
//                var sqlRealisasi = @"SELECT 	NVL(NOP, '-') NOP, 
//		MASA, 
//		TAHUN, 
//		'-' ID_SSPD,
//        '-' KODE_BILL,
//        0 JENIS_PEMBAYARAN,
//        3 JENIS_PAJAK,
//        1 JENIS_KETETAPAN,
//		MAX(JATUH_TEMPO) AS JATUH_TEMPO,
//		SUM(NOMINAL_POKOK) NOMINAL_POKOK,
//		SUM(NOMINAL_SANKSI) NOMINAL_SANKSI,
//'-' NO_KETETAPAN,
//        0 NOMINAL_ADMINISTRASI, 
//        0 NOMINAL_LAINYA,
//        0 PENGURANG_POKOK, 
//        0 PENGURANG_SANKSI,
//        '-' REFF_PENGURANG_POKOK,
//        '-'   REFF_PENGURANG_SANKSI,
//        '-'   AKUN_POKOK,
//        '-'   AKUN_SANKSI,
//        '-'   AKUN_ADMINISTRASI, 
//        '-'  AKUN_LAINNYA,
//        '-'   AKUN_PENGURANG_POKOK,
//        '-'   AKUN_PENGURANG_SANKSI,
//        '-'  INVOICE_NUMBER, 
//        MAX(TRANSACTION_DATE) TRANSACTION_DATE, 
//        '-'  NO_NTPD,
//        1  STATUS_NTPD,
//        SYSDATE  REKON_DATE,
//        '-'   REKON_BY,
//        '-'   REKON_REFF,
//        100 SEQ_KETETAPAN,
//        SYSDATE INS_DATE           
//FROM (
//	SELECT  ID_SSPD,
//            nvl(SYNC_REFF_BILL,'-') KODE_BILL, 
//            '-' NO_KETETAPAN, 
//            0 JENIS_PEMBAYARAN,
//            5 JENIS_PAJAK,
//            1 JENIS_KETETAPAN, 
//            TO_DATE(MP_AKHIR) JATUH_TEMPO, 
//            REPLACE(FK_NOP,'.','') NOP,
//            TO_NUMBER( BULAN_PAJAK) MASA, 
//            TO_NUMBER(TAHUN_PAJAK) TAHUN, 
//           TO_NUMBER(JML_POKOK) NOMINAL_POKOK, 
//           TO_NUMBER(JML_DENDA) NOMINAL_SANKSI,
//           0 NOMINAL_ADMINISTRASI, 
//           0 NOMINAL_LAINYA,
//           0 PENGURANG_POKOK, 
//           0 PENGURANG_SANKSI,
//           '-' REFF_PENGURANG_POKOK,'-'   REFF_PENGURANG_SANKSI,'-'   AKUN_POKOK,'-'   AKUN_SANKSI,'-'   AKUN_ADMINISTRASI, 
//                                '-'  AKUN_LAINNYA,'-'   AKUN_PENGURANG_POKOK,'-'   AKUN_PENGURANG_SANKSI,'-'  INVOICE_NUMBER,TO_DATE(TGL_SETORAN) TRANSACTION_DATE, 
//                                '-'  NO_NTPD,1  STATUS_NTPD,SYSDATE  REKON_DATE,'-'   REKON_BY,'-'   REKON_REFF,100 SEQ_KETETAPAN,SYSDATE INS_DATE                                                                 
//FROM VW_SIMPADA_SSPD@LIHATHPPSERVER
//WHERE NAMA_PAJAK_DAERAH='HIBURAN'  AND TO_CHAR(TGL_SETORAN,'YYYY')=:TAHUN
//) A
//GROUP BY NOP, MASA, TAHUN  ";

//                var pembayaranSspdList = _contMonitoringDB.Set<SSPD>()
//                    .FromSqlRaw(sqlRealisasi, new[] {
//                    new OracleParameter("TAHUN", tahunBuku)
//                    }).ToList();

//                if (pembayaranSspdList != null)
//                {
//                    int jmlData = pembayaranSspdList.Count;
//                    int index = 0;
//                    foreach (var itemSSPD in pembayaranSspdList)
//                    {
//                        var OP = _contMonPd.DbOpHiburans.FirstOrDefault(x => x.Nop == itemSSPD.NOP.Replace(".", ""));
//                        if (OP != null)
//                        {
//                            var ketetapan = _contMonPd.DbMonHiburans.FirstOrDefault(x => x.Nop == itemSSPD.NOP.Replace(".", "") &&
//                                                                                    x.TahunPajakKetetapan == itemSSPD.TAHUN &&
//                                                                                    x.MasaPajakKetetapan == itemSSPD.MASA &&
//                                                                                    x.SeqPajakKetetapan == itemSSPD.SEQ_KETETAPAN);
//                            if (ketetapan == null)
//                            {
//                                ketetapan = _contMonPd.DbMonHiburans.FirstOrDefault(x => x.Nop == itemSSPD.NOP.Replace(".", "") &&
//                                                                                    x.TahunPajakKetetapan == itemSSPD.TAHUN &&
//                                                                                    x.MasaPajakKetetapan == itemSSPD.MASA &&
//                                                                                    x.SeqPajakKetetapan == 101);
//                            }
//                            if (ketetapan != null)
//                            {
//                                string akunBayar = "-";
//                                string kelompokBayar = "-";
//                                string jenisBayar = "-";
//                                string objekBayar = "-";
//                                string rincianBayar = "-";
//                                string subrincianBayar = "-";

//                                var getAkun = GetDbAkun(tahunBuku, KDPajak, (int)OP.KategoriId);
//                                if (getAkun != null)
//                                {
//                                    akunBayar = getAkun.Akun;
//                                    kelompokBayar = getAkun.Kelompok;
//                                    jenisBayar = getAkun.Jenis;
//                                    objekBayar = getAkun.Objek;
//                                    rincianBayar = getAkun.Rincian;
//                                    subrincianBayar = getAkun.SubRincian;
//                                }

//                                string akunSanksi = "-";
//                                string kelompokSanksi = "-";
//                                string jenisSanksi = "-";
//                                string objekSanksi = "-";
//                                string rincianSanksi = "-";
//                                string subrincianSanksi = "-";

//                                var getAkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);
//                                if (getAkunSanksi != null)
//                                {
//                                    akunSanksi = getAkunSanksi.Akun;
//                                    kelompokSanksi = getAkunSanksi.Kelompok;
//                                    jenisSanksi = getAkunSanksi.Jenis;
//                                    objekSanksi = getAkunSanksi.Objek;
//                                    rincianSanksi = getAkunSanksi.Rincian;
//                                    subrincianSanksi = getAkunSanksi.SubRincian;
//                                }



//                                if (!ketetapan.TglBayarPokok.HasValue)
//                                {
//                                    ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
//                                }
//                                else
//                                {
//                                    if (ketetapan.TglBayarPokok.Value < itemSSPD.TRANSACTION_DATE)
//                                    {
//                                        ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
//                                    }
//                                }

//                                ketetapan.NominalPokokBayar = (ketetapan.NominalPokokBayar ?? 0) + itemSSPD.NOMINAL_POKOK;
//                                ketetapan.AkunPokokBayar = akunBayar;
//                                ketetapan.Kelompok = kelompokBayar;
//                                ketetapan.JenisPokokBayar = jenisBayar;
//                                ketetapan.ObjekPokokBayar = objekBayar;
//                                ketetapan.RincianPokokBayar = rincianBayar;
//                                ketetapan.SubRincianPokokBayar = subrincianBayar;

//                                if (!ketetapan.TglBayarSanksi.HasValue)
//                                {
//                                    ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
//                                }
//                                else
//                                {
//                                    if (ketetapan.TglBayarSanksi.Value < itemSSPD.TRANSACTION_DATE)
//                                    {
//                                        ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
//                                    }
//                                }

//                                ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiBayar + itemSSPD.NOMINAL_SANKSI;
//                                ketetapan.AkunSanksiBayar = akunSanksi;
//                                ketetapan.KelompokSanksiBayar = kelompokSanksi;
//                                ketetapan.JenisSanksiBayar = jenisSanksi;
//                                ketetapan.ObjekSanksiBayar = objekSanksi;
//                                ketetapan.RincianSanksiBayar = rincianSanksi;
//                                ketetapan.SubRincianSanksiBayar = subrincianSanksi;


//                                if (!ketetapan.TglBayarSanksiKenaikan.HasValue)
//                                {
//                                    ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
//                                }
//                                else
//                                {
//                                    if (ketetapan.TglBayarSanksiKenaikan.Value < itemSSPD.TRANSACTION_DATE)
//                                    {
//                                        ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
//                                    }
//                                }

//                                ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiKenaikanBayar + itemSSPD.NOMINAL_ADMINISTRASI;
//                                ketetapan.AkunSanksiBayar = akunSanksi;
//                                ketetapan.KelompokSanksiBayar = kelompokSanksi;
//                                ketetapan.JenisSanksiBayar = jenisSanksi;
//                                ketetapan.ObjekSanksiBayar = objekSanksi;
//                                ketetapan.RincianSanksiBayar = rincianSanksi;
//                                ketetapan.SubRincianSanksiBayar = subrincianSanksi;
//                                _contMonPd.SaveChanges();
//                            }
//                            else
//                            {
//                                bool isOPTutup = false;
//                                if (OP.TglOpTutup.HasValue)
//                                {
//                                    if (OP.TglOpTutup.Value.Date.Year <= tahunBuku)
//                                    {
//                                        isOPTutup = true;
//                                    }

//                                }


//                                string akunBayar = "-";
//                                string kelompokBayar = "-";
//                                string jenisBayar = "-";
//                                string objekBayar = "-";
//                                string rincianBayar = "-";
//                                string subrincianBayar = "-";

//                                var getAkun = GetDbAkun(tahunBuku, KDPajak, (int)OP.KategoriId);
//                                if (getAkun != null)
//                                {
//                                    akunBayar = getAkun.Akun;
//                                    kelompokBayar = getAkun.Kelompok;
//                                    jenisBayar = getAkun.Jenis;
//                                    objekBayar = getAkun.Objek;
//                                    rincianBayar = getAkun.Rincian;
//                                    subrincianBayar = getAkun.SubRincian;
//                                }

//                                string akunSanksi = "-";
//                                string kelompokSanksi = "-";
//                                string jenisSanksi = "-";
//                                string objekSanksi = "-";
//                                string rincianSanksi = "-";
//                                string subrincianSanksi = "-";

//                                var getAkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);
//                                if (getAkunSanksi != null)
//                                {
//                                    akunSanksi = getAkunSanksi.Akun;
//                                    kelompokSanksi = getAkunSanksi.Kelompok;
//                                    jenisSanksi = getAkunSanksi.Jenis;
//                                    objekSanksi = getAkunSanksi.Objek;
//                                    rincianSanksi = getAkunSanksi.Rincian;
//                                    subrincianSanksi = getAkunSanksi.SubRincian;
//                                }


//                                var newRow = new DbMonHiburan();
//                                newRow.Nop = OP.Nop;
//                                newRow.Npwpd = OP.Npwpd;
//                                newRow.NpwpdNama = OP.NpwpdNama;
//                                newRow.NpwpdAlamat = OP.NpwpdAlamat;
//                                newRow.PajakId = OP.PajakId;
//                                newRow.PajakNama = OP.PajakNama;
//                                newRow.NamaOp = OP.NamaOp;
//                                newRow.AlamatOp = OP.AlamatOp;
//                                newRow.AlamatOpKdLurah = OP.AlamatOpKdLurah;
//                                newRow.AlamatOpKdCamat = OP.AlamatOpKdCamat;
//                                newRow.TglOpTutup = OP.TglOpTutup;
//                                newRow.TglMulaiBukaOp = OP.TglMulaiBukaOp;
//                                newRow.IsTutup = isOPTutup ? 1 : 0;
//                                newRow.KategoriId = OP.KategoriId;
//                                newRow.KategoriNama = OP.KategoriNama;
//                                newRow.TahunBuku = tahunBuku;
//                                newRow.Akun = OP.Akun;
//                                newRow.NamaAkun = OP.NamaAkun;
//                                newRow.Jenis = OP.Jenis;
//                                newRow.NamaJenis = OP.NamaJenis;
//                                newRow.Objek = OP.Objek;
//                                newRow.NamaObjek = OP.NamaObjek;
//                                newRow.Rincian = OP.Rincian;
//                                newRow.NamaRincian = OP.NamaRincian;
//                                newRow.SubRincian = OP.SubRincian;
//                                newRow.NamaSubRincian = OP.NamaSubRincian;
//                                newRow.TahunPajakKetetapan = itemSSPD.TAHUN;
//                                newRow.MasaPajakKetetapan = itemSSPD.MASA;
//                                newRow.SeqPajakKetetapan = 101;
//                                newRow.KategoriKetetapan = "4";
//                                newRow.TglKetetapan = itemSSPD.TRANSACTION_DATE;
//                                newRow.TglJatuhTempoBayar = itemSSPD.JATUH_TEMPO;
//                                newRow.PokokPajakKetetapan = itemSSPD.NOMINAL_POKOK;
//                                newRow.PengurangPokokKetetapan = 0;
//                                newRow.AkunKetetapan = akunBayar;
//                                newRow.KelompokKetetapan = kelompokBayar;
//                                newRow.JenisKetetapan = jenisBayar;
//                                newRow.ObjekKetetapan = objekBayar;
//                                newRow.RincianKetetapan = rincianBayar;
//                                newRow.SubRincianKetetapan = subrincianBayar;
//                                newRow.InsDate = DateTime.Now;
//                                newRow.InsBy = "JOB";
//                                newRow.UpdDate = DateTime.Now;
//                                newRow.UpdBy = "JOB";


//                                newRow.NominalPokokBayar = itemSSPD.NOMINAL_POKOK;
//                                newRow.AkunPokokBayar = akunBayar;
//                                newRow.Kelompok = kelompokBayar;
//                                newRow.JenisPokokBayar = jenisBayar;
//                                newRow.ObjekPokokBayar = objekBayar;
//                                newRow.RincianPokokBayar = rincianBayar;
//                                newRow.SubRincianPokokBayar = subrincianBayar;
//                                newRow.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
//                                newRow.NominalSanksiBayar = itemSSPD.NOMINAL_SANKSI;
//                                newRow.AkunSanksiBayar = akunSanksi;
//                                newRow.KelompokSanksiBayar = kelompokSanksi;
//                                newRow.JenisSanksiBayar = jenisSanksi;
//                                newRow.ObjekSanksiBayar = objekSanksi;
//                                newRow.RincianSanksiBayar = rincianSanksi;
//                                newRow.SubRincianSanksiBayar = subrincianSanksi;
//                                newRow.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;

//                                newRow.NominalSanksiBayar = itemSSPD.NOMINAL_ADMINISTRASI;
//                                newRow.AkunSanksiBayar = akunSanksi;
//                                newRow.KelompokSanksiBayar = kelompokSanksi;
//                                newRow.JenisSanksiBayar = jenisSanksi;
//                                newRow.ObjekSanksiBayar = objekSanksi;
//                                newRow.RincianSanksiBayar = rincianSanksi;
//                                newRow.SubRincianSanksiBayar = subrincianSanksi;
//                                _contMonPd.DbMonHiburans.Add(newRow);
//                                _contMonPd.SaveChanges();
//                            }
//                            index++;
//                            double persen = ((double)index / jmlData) * 100;
//                            Console.Write($"\rDB_REALISASI_HIBURAN HPP TAHUN {tahunBuku} JML OP {jmlData} : {OP.Nop}  {persen:F2}%   ");
//                            Thread.Sleep(50);
//                        }
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error processing NOP {ex.Message}");
//            }
//        }

//        private bool IsGetDBOp()
//        {
//            var _contMonPd = DBClass.GetContext();
//            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPHIBURAN.ToString().ToUpper());
//            if (row != null)
//            {
//                if (row.InsDate.HasValue)
//                {
//                    var tglTarik = row.InsDate.Value.Date;
//                    var tglServer = DateTime.Now.Date;
//                    if (tglTarik >= tglServer)
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        row.InsDate = DateTime.Now;
//                        _contMonPd.SaveChanges();
//                        return true;
//                    }
//                }
//                else
//                {
//                    row.InsDate = DateTime.Now;
//                    _contMonPd.SaveChanges();
//                    return true;
//                }
//            }
//            var newRow = new MonPDLib.EF.SetLastRun();
//            newRow.Job = EnumFactory.EJobName.DBOPHIBURAN.ToString().ToUpper();
//            newRow.InsDate = DateTime.Now;
//            _contMonPd.SetLastRuns.Add(newRow);
//            _contMonPd.SaveChanges();
//            return true;
//        }
    }
}
