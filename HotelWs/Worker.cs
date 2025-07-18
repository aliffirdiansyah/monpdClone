using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace HotelWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 3;

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

        private async Task DoWorkNewMeta(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            if (thnSetting != null)
            {
                var temp = tglServer.Year - (int)thnSetting.YearBefore;
                if (temp >= 2021)
                {
                    tahunAmbil = temp;
                }
                else
                {
                    tahunAmbil = 2021;
                }
            }

            // do fill db op HOTEL
            if (IsGetDBOp())
            {
                for (var i = tahunAmbil; i <= tglServer.Year; i++)
                {
                    FillOP(i);
                }
            }

            MailHelper.SendMail(
            false,
            "DONE HOTEL  WS",
            $@"HOTEL WS FINISHED",
            null
            );
        }

        private void FillOP(int tahunBuku)
        {
            // SURABAYA TAX PROCESS
            using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
            {
                var sql = @"
                                                                                                                                                              SELECT  A.NOP,
        C.NPWPD_NO NPWPD,
        C.NAMA NPWPD_NAMA,
        C.ALAMAT NPWPD_ALAMAT,
        A.PAJAK_ID ,
        'Pajak Jasa Perhotelan' PAJAK_NAMA,
        A.NAMA NAMA_OP,
        A.ALAMAT ALAMAT_OP,
        A.ALAMAT_NO ALAMAT_OP_NO,
        A.RT ALAMAT_OP_RT,
        A.RW ALAMAT_OP_RW,
        A.TELP,
        A.KD_LURAH ALAMAT_OP_KD_LURAH,
        A.KD_CAMAT ALAMAT_OP_KD_CAMAT,
        TGL_OP_TUTUP,
        TGL_MULAI_BUKA_OP,
        0 METODE_PENJUALAN,
        B.BUKTI_BAYAR METODE_PEMBAYARAN,
        B.JUMLAH_KARYAWAN,
        CASE A.KATEGORI 
            WHEN 48 THEN 17
            WHEN 43 THEN 16
            WHEN 1 THEN 17
            WHEN 7 THEN 17
            WHEN 44 THEN 15
            WHEN 46 THEN 13
            WHEN 45 THEN 14
            WHEN 47 THEN 12
            ELSE 17
        END AS KATEGORI_ID,
        D.NAMA KATEGORI_NAMA,
        sysdate INS_dATE, 
        'JOB' INS_BY,
        TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')) TAHUN_BUKU,
        CASE 
            WHEN TGL_OP_TUTUP IS NOT NULL THEN 1
        ELSE 0
        END AS IS_TUTUP,
        'SURABAYA ' || UPTB_ID AS WILAYAH_PAJAK,
        '-'  AKUN  ,
        '-'  NAMA_AKUN         ,
        '-'  KELOMPOK      ,
        '-'  NAMA_KELOMPOK     ,
        '-'  JENIS             ,
        '-'  NAMA_JENIS        ,
        '-'  OBJEK            ,
        '-'  NAMA_OBJEK       ,
        '-'  RINCIAN         ,
        '-'  NAMA_RINCIAN     ,
        '-'  SUB_RINCIAN      ,
        '-'  NAMA_SUB_RINCIAN    
FROM OBJEK_PAJAK A
JOIN OBJEK_PAJAK_HOTEL B ON A.NOP = B.NOP
JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
JOIN M_KATEGORI_PAJAK D ON D.ID = A.KATEGORI
LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT
WHERE A.NPWPD NOT IN (
    select npwpd_no  
    from npwpd 
    WHERE REF_THN_PEL = 2023 OR NAMA LIKE '%FULAN%' OR NPWPD_NO = '3578200503840003'
)    and  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                    ";

                var result = _contSbyTax.Set<DbOpHotel>().FromSqlRaw(sql, new[] {
                    new OracleParameter("TAHUN", tahunBuku)
                }).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        var sourceRow = _contMonPd.DbOpHotels.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
                        if (sourceRow != null)
                        {
                            sourceRow.TglOpTutup = item.TglOpTutup;
                            sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                            if (dbakun != null)
                            {
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
                            }
                            else
                            {
                                sourceRow.Akun = item.Akun;
                                sourceRow.NamaAkun = item.NamaAkun;
                                sourceRow.Kelompok = item.Kelompok;
                                sourceRow.NamaKelompok = item.NamaKelompok;
                                sourceRow.Jenis = item.Jenis;
                                sourceRow.NamaJenis = item.NamaJenis;
                                sourceRow.Objek = item.Objek;
                                sourceRow.NamaObjek = item.NamaObjek;
                                sourceRow.Rincian = item.Rincian;
                                sourceRow.NamaRincian = item.NamaRincian;
                                sourceRow.SubRincian = item.SubRincian;
                                sourceRow.NamaSubRincian = item.NamaSubRincian;
                            }
                        }
                        else
                        {
                            var newRow = new MonPDLib.EF.DbOpHotel();
                            newRow.Nop = item.Nop;
                            newRow.Npwpd = item.Npwpd;
                            newRow.NpwpdNama = item.NpwpdNama;
                            newRow.NpwpdAlamat = item.NpwpdAlamat;
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
                            newRow.MetodePenjualan = item.MetodePenjualan;
                            newRow.MetodePembayaran = item.MetodePembayaran;
                            newRow.JumlahKaryawan = item.JumlahKaryawan;
                            newRow.InsDate = item.InsDate;
                            newRow.InsBy = item.InsBy;
                            newRow.IsTutup = item.IsTutup;
                            newRow.WilayahPajak = item.WilayahPajak;

                            newRow.TahunBuku = tahunBuku;
                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                            if (dbakun != null)
                            {
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
                            }
                            else
                            {
                                newRow.Akun = item.Akun;
                                newRow.NamaAkun = item.NamaAkun;
                                newRow.Kelompok = item.Kelompok;
                                newRow.NamaKelompok = item.NamaKelompok;
                                newRow.Jenis = item.Jenis;
                                newRow.NamaJenis = item.NamaJenis;
                                newRow.Objek = item.Objek;
                                newRow.NamaObjek = item.NamaObjek;
                                newRow.Rincian = item.Rincian;
                                newRow.NamaRincian = item.NamaRincian;
                                newRow.SubRincian = item.SubRincian;
                                newRow.NamaSubRincian = item.NamaSubRincian;
                            }
                            _contMonPd.DbOpHotels.Add(newRow);
                            _contMonPd.SaveChanges();
                        }

                    }
                    catch
                    {

                    }

                    // ketetapan sbytax
                    try
                    {
                        var _contSbyTaxOld = DBClass.GetSurabayaTaxContext();
                        var sqlKetetapan = @"SELECT  NOP,TAHUN,MASAPAJAK,SEQ,1 JENIS_KETETAPAN,TGL_KETETAPAN,TGL_JATUH_TEMPO_BAYAR ,0 NILAI_PENGURANG,((NVL(PROSEN_TARIF_PAJAK, 0)/100) * OMSET)  POKOK
 FROM (
 SELECT A.NOP,A.TAHUN,A.MASAPAJAK,A.SEQ,A.TGL_SPTPD TGL_KETETAPAN,A.TGL_JATUH_TEMPO_BAYAR ,A.PROSEN_TARIF_PAJAK, SUM(B.OMSET) OMSET
 FROM OBJEK_PAJAK_SPTPD A
 JOIN OBJEK_PAJAK_SPTPD_DET B ON A.NOP=B.NOP AND A.TAHUN=B.TAHUN AND A.MASAPAJAK=B.MASAPAJAK AND A.SEQ=B.SEQ
 WHERE A.NOP=:NOP    AND  A.STATUS =1 AND TO_CHAR(TGL_SPTPD,'YYYY')=:TAHUN                          
 GROUP BY A.NOP,A.TAHUN,A.MASAPAJAK,A.SEQ,A.TGL_SPTPD ,A.TGL_JATUH_TEMPO_BAYAR ,A.PROSEN_TARIF_PAJAK)";

                        var ketetapanSbyTaxOld = _contSbyTaxOld.Set<OPSkpdHotel>()
                            .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("NOP", item.Nop),
                                new OracleParameter("TAHUN", tahunBuku)
                            }).ToList();
                        var dbAkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)item.KategoriId);
                        foreach (var itemKetetapan in ketetapanSbyTaxOld)
                        {
                            string nop = item.Nop;
                            int tahunPajak = itemKetetapan.TAHUN;
                            int masaPajak = itemKetetapan.MASAPAJAK;
                            int seqPajak = itemKetetapan.SEQ;
                            var rowMonHHotel = _contMonPd.DbMonHotels.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            bool isOPTutup = false;
                            if (item.TglOpTutup.HasValue)
                            {
                                if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                {
                                    isOPTutup = true;
                                }

                            }


                            if (rowMonHHotel != null)
                            {
                                _contMonPd.DbMonHotels.Remove(rowMonHHotel);
                            }

                            var newRow = new DbMonHotel();
                            newRow.Nop = item.Nop;
                            newRow.Npwpd = item.Npwpd;
                            newRow.NpwpdNama = item.NpwpdNama;
                            newRow.NpwpdAlamat = item.NpwpdAlamat;
                            newRow.PajakId = item.PajakId;
                            newRow.PajakNama = item.PajakNama;
                            newRow.NamaOp = item.NamaOp;
                            newRow.AlamatOp = item.AlamatOp;
                            newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
                            newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
                            newRow.TglOpTutup = item.TglOpTutup;
                            newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                            newRow.IsTutup = isOPTutup ? 1 : 0;
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.TahunBuku = tahunBuku;
                            newRow.Akun = item.Akun;
                            newRow.NamaAkun = item.NamaAkun;
                            newRow.Jenis = item.Jenis;
                            newRow.NamaJenis = item.NamaJenis;
                            newRow.Objek = item.Objek;
                            newRow.NamaObjek = item.NamaObjek;
                            newRow.Rincian = item.Rincian;
                            newRow.NamaRincian = item.NamaRincian;
                            newRow.SubRincian = item.SubRincian;
                            newRow.NamaSubRincian = item.NamaSubRincian;
                            newRow.TahunPajakKetetapan = itemKetetapan.TAHUN;
                            newRow.MasaPajakKetetapan = itemKetetapan.MASAPAJAK;
                            newRow.SeqPajakKetetapan = itemKetetapan.SEQ;
                            newRow.KategoriKetetapan = itemKetetapan.JENIS_KETETAPAN.ToString();
                            newRow.TglKetetapan = itemKetetapan.TGL_KETETAPAN;
                            newRow.TglJatuhTempoBayar = itemKetetapan.TGL_JATUH_TEMPO_BAYAR;
                            newRow.PokokPajakKetetapan = itemKetetapan.POKOK - itemKetetapan.NILAI_PENGURANG;
                            newRow.PengurangPokokKetetapan = itemKetetapan.NILAI_PENGURANG;
                            newRow.AkunKetetapan = dbAkunPokok.Akun;
                            newRow.KelompokKetetapan = dbAkunPokok.Kelompok;
                            newRow.JenisKetetapan = dbAkunPokok.Jenis;
                            newRow.ObjekKetetapan = dbAkunPokok.Objek;
                            newRow.RincianKetetapan = dbAkunPokok.Rincian;
                            newRow.SubRincianKetetapan = dbAkunPokok.SubRincian;
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";
                            newRow.UpdDate = DateTime.Now;
                            newRow.UpdBy = "JOB";
                            _contMonPd.DbMonHotels.Add(newRow);
                            _contMonPd.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    // realisasi
                    try
                    {
                        var sqlRealisasi = @"SELECT     ID_SSPD,KODE_BILL, NO_KETETAPAN, JENIS_PEMBAYARAN, JENIS_PAJAK, JENIS_KETETAPAN, 
                                            JATUH_TEMPO, NOP, MASA, TAHUN, NOMINAL_POKOK, NOMINAL_SANKSI, NOMINAL_ADMINISTRASI, NOMINAL_LAINYA, PENGURANG_POKOK, 
                                PENGURANG_SANKSI, REFF_PENGURANG_POKOK, REFF_PENGURANG_SANKSI, AKUN_POKOK, AKUN_SANKSI, AKUN_ADMINISTRASI, 
                                AKUN_LAINNYA, AKUN_PENGURANG_POKOK, AKUN_PENGURANG_SANKSI, INVOICE_NUMBER, TRANSACTION_DATE, NO_NTPD, 
                                STATUS_NTPD, REKON_DATE, REKON_BY, REKON_REFF, SEQ_KETETAPAN, INS_DATE    
                    FROM T_SSPD A            
                    WHERE     A.JENIS_PAJAK = 3 AND A.NOP = :NOP AND TO_CHAR(TRANSACTION_DATE,'YYYY')=:TAHUN ";

                        var _contBima = DBClass.GetBimaContext();
                        var pembayaranSspdList = _contBima.Set<SSPD>()
                            .FromSqlRaw(sqlRealisasi, new[] {
                    new OracleParameter("NOP", item.Nop),
                    new OracleParameter("TAHUN", tahunBuku)
                            }).ToList();

                        if (pembayaranSspdList != null)
                        {
                            foreach (var itemSSPD in pembayaranSspdList)
                            {
                                var ketetapan = _contMonPd.DbMonHotels.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
                                                                                        x.TahunPajakKetetapan == itemSSPD.TAHUN &&
                                                                                        x.MasaPajakKetetapan == itemSSPD.MASA &&
                                                                                        x.SeqPajakKetetapan == itemSSPD.SEQ_KETETAPAN);
                                if (ketetapan != null)
                                {
                                    string akunBayar = "-";
                                    string kelompokBayar = "-";
                                    string jenisBayar = "-";
                                    string objekBayar = "-";
                                    string rincianBayar = "-";
                                    string subrincianBayar = "-";

                                    var getAkun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkun != null)
                                    {
                                        akunBayar = getAkun.Akun;
                                        kelompokBayar = getAkun.Kelompok;
                                        jenisBayar = getAkun.Jenis;
                                        objekBayar = getAkun.Objek;
                                        rincianBayar = getAkun.Rincian;
                                        subrincianBayar = getAkun.SubRincian;
                                    }

                                    string akunSanksi = "-";
                                    string kelompokSanksi = "-";
                                    string jenisSanksi = "-";
                                    string objekSanksi = "-";
                                    string rincianSanksi = "-";
                                    string subrincianSanksi = "-";

                                    var getAkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkunSanksi != null)
                                    {
                                        akunSanksi = getAkunSanksi.Akun;
                                        kelompokSanksi = getAkunSanksi.Kelompok;
                                        jenisSanksi = getAkunSanksi.Jenis;
                                        objekSanksi = getAkunSanksi.Objek;
                                        rincianSanksi = getAkunSanksi.Rincian;
                                        subrincianSanksi = getAkunSanksi.SubRincian;
                                    }



                                    if (!ketetapan.TglBayarPokok.HasValue)
                                    {
                                        ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarPokok.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalPokokBayar = ketetapan.NominalPokokBayar + itemSSPD.NOMINAL_POKOK;
                                    ketetapan.AkunPokokBayar = akunBayar;
                                    ketetapan.Kelompok = kelompokBayar;
                                    ketetapan.JenisPokokBayar = jenisBayar;
                                    ketetapan.ObjekPokokBayar = objekBayar;
                                    ketetapan.RincianPokokBayar = rincianBayar;
                                    ketetapan.SubRincianPokokBayar = subrincianBayar;

                                    if (!ketetapan.TglBayarSanksi.HasValue)
                                    {
                                        ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarSanksi.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiBayar + itemSSPD.NOMINAL_SANKSI;
                                    ketetapan.AkunSanksiBayar = akunSanksi;
                                    ketetapan.KelompokSanksiBayar = kelompokSanksi;
                                    ketetapan.JenisSanksiBayar = jenisSanksi;
                                    ketetapan.ObjekSanksiBayar = objekSanksi;
                                    ketetapan.RincianSanksiBayar = rincianSanksi;
                                    ketetapan.SubRincianSanksiBayar = subrincianSanksi;


                                    if (!ketetapan.TglBayarSanksiKenaikan.HasValue)
                                    {
                                        ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarSanksiKenaikan.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiKenaikanBayar + itemSSPD.NOMINAL_ADMINISTRASI;
                                    ketetapan.AkunSanksiBayar = akunSanksi;
                                    ketetapan.KelompokSanksiBayar = kelompokSanksi;
                                    ketetapan.JenisSanksiBayar = jenisSanksi;
                                    ketetapan.ObjekSanksiBayar = objekSanksi;
                                    ketetapan.RincianSanksiBayar = rincianSanksi;
                                    ketetapan.SubRincianSanksiBayar = subrincianSanksi;
                                    _contMonPd.SaveChanges();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_OP_HOTEL SBYTAX TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
                    Thread.Sleep(50);
                    _contMonPd.SaveChanges();

                }
            }


            Console.WriteLine("");
            // HPP PROCESS
            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                                                                                   SELECT *
FROM (
SELECT REPLACE(A.FK_NOP, '.', '') NOP,NVL(FK_NPWPD, '-') NPWPD,NAMA_OP, 5 PAJAK_ID,  'Pajak Jasa Perhotelan' PAJAK_NAMA,
              NVL(ALAMAT_OP, '-') ALAMAT_OP, '-'  ALAMAT_OP_NO,'-' ALAMAT_OP_RT,'-' ALAMAT_OP_RW,NVL(NOMOR_TELEPON, '-') TELP,
              NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH, NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,TGL_TUTUP TGL_OP_TUTUP,
              NVL(TGL_BUKA,TO_DATE('01012000','DDMMYYYY')) TGL_MULAI_BUKA_OP, 0 METODE_PENJUALAN,        0 METODE_PEMBAYARAN,        0 JUMLAH_KARYAWAN,  
              CASE                             
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG TIGA' THEN 14
                        WHEN NAMA_JENIS_PAJAK = 'RESTORAN' THEN 17
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL NON BINTANG' THEN 17
                        WHEN NAMA_JENIS_PAJAK = 'KATERING' THEN 17
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG LIMA' THEN 16
                        WHEN NAMA_JENIS_PAJAK = 'RUMAH KOS' THEN 17
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG SATU' THEN 12
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG LIMA BERLIAN' THEN 16
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG EMPAT' THEN 15    
                        WHEN NAMA_JENIS_PAJAK = 'HOTEL BINTANG DUA' THEN 13
            ELSE 17
            END AS KATEGORI_ID,
            NAMA_JENIS_PAJAK   KATEGORI_NAMA,
             sysdate INS_dATE, 'JOB' INS_BY ,fk_wilayah_pajak WILAYAH_PAJAK   ,
            '-' AKUN,'-'  NAMA_AKUN,'-'  JENIS,'-'  NAMA_JENIS,'-'  OBJEK,'-'  NAMA_OBJEK,'-'  RINCIAN,
'-'  NAMA_RINCIAN,'-'  SUB_RINCIAN,'-'  NAMA_SUB_RINCIAN,'-'  KELOMPOK,
            '-'  NAMA_KELOMPOK,1  IS_TUTUP,'-'  NPWPD_NAMA, '-'  NPWPD_ALAMAT,1 TAHUN_BUKU
FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
WHERE NAMA_PAJAK_DAERAH='HOTEL' AND A.FK_NOP IS NOT NULL
)
WHERE  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                    ";

                var result = _contMonitoringDB.Set<DbOpHotel>().FromSqlRaw(sql, new[] {
                    new OracleParameter("TAHUN", tahunBuku)
                }).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        if (item.Nop== "357802200290100001")
                        {
                            var kk = 1;
                        }
                        var sourceRow = _contMonPd.DbOpHotels.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
                        if (sourceRow != null)
                        {
                            sourceRow.TglOpTutup = item.TglOpTutup;
                            sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                            if (dbakun != null)
                            {
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
                            }
                            else
                            {
                                sourceRow.Akun = item.Akun;
                                sourceRow.NamaAkun = item.NamaAkun;
                                sourceRow.Kelompok = item.Kelompok;
                                sourceRow.NamaKelompok = item.NamaKelompok;
                                sourceRow.Jenis = item.Jenis;
                                sourceRow.NamaJenis = item.NamaJenis;
                                sourceRow.Objek = item.Objek;
                                sourceRow.NamaObjek = item.NamaObjek;
                                sourceRow.Rincian = item.Rincian;
                                sourceRow.NamaRincian = item.NamaRincian;
                                sourceRow.SubRincian = item.SubRincian;
                                sourceRow.NamaSubRincian = item.NamaSubRincian;
                            }
                        }
                        else
                        {
                            var newRow = new MonPDLib.EF.DbOpHotel();
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
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.MetodePenjualan = item.MetodePenjualan;
                            newRow.MetodePembayaran = item.MetodePembayaran;
                            newRow.JumlahKaryawan = item.JumlahKaryawan;
                            newRow.InsDate = item.InsDate;
                            newRow.InsBy = item.InsBy;
                            newRow.IsTutup = item.IsTutup;
                            newRow.WilayahPajak = item.WilayahPajak;

                            newRow.TahunBuku = tahunBuku;
                            var dbakun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                            if (dbakun != null)
                            {
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
                            }
                            else
                            {
                                newRow.Akun = item.Akun;
                                newRow.NamaAkun = item.NamaAkun;
                                newRow.Kelompok = item.Kelompok;
                                newRow.NamaKelompok = item.NamaKelompok;
                                newRow.Jenis = item.Jenis;
                                newRow.NamaJenis = item.NamaJenis;
                                newRow.Objek = item.Objek;
                                newRow.NamaObjek = item.NamaObjek;
                                newRow.Rincian = item.Rincian;
                                newRow.NamaRincian = item.NamaRincian;
                                newRow.SubRincian = item.SubRincian;
                                newRow.NamaSubRincian = item.NamaSubRincian;
                            }
                            _contMonPd.DbOpHotels.Add(newRow);
                        }

                    }
                    catch (Exception ex)
                    {
                        var kkk = item.Nop;
                    }

                    // ketetapan 
                    try
                    {
                        var sqlKetetapan = @"SELECT *
FROM (            
SELECT FK_NOP NOP, TO_NUMBER(TAHUN_PAJAK) TAHUN,TO_NUMBER(BULAN_PAJAK) MASAPAJAK,100 SEQ,1 JENIS_KETETAPAN,TO_DATE(NVL(TGL_SPTPD_DISETOR,MP_AKHIR)) TGL_KETETAPAN,TO_DATE(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO_BAYAR ,0 NILAI_PENGURANG,
            TO_NUMBER(KETETAPAN_TOTAL)  POKOK
FROM VW_SIMPADA_SPTPD@LIHATHPPSERVER
WHERE NAMA_PAJAK_DAERAH='HOTEL' AND FK_NOP IS NOT NULL and REPLACE(FK_NOP,'.','')=:NOP
)
WHERE  TO_CHAR(TGL_KETETAPAN,'YYYY')=:TAHUN             ";

                        var ketetapanMonitoringDb = _contMonitoringDB.Set<OPSkpdHotel>()
                            .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("NOP", item.Nop),
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                            }).ToList();
                        var dbAkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)item.KategoriId);
                        foreach (var itemKetetapan in ketetapanMonitoringDb)
                        {
                            string nop = item.Nop;
                            int tahunPajak = itemKetetapan.TAHUN;
                            int masaPajak = itemKetetapan.MASAPAJAK;
                            int seqPajak = itemKetetapan.SEQ;
                            var rowMonHotel = _contMonPd.DbMonHotels.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            bool isOPTutup = false;
                            if (item.TglOpTutup.HasValue)
                            {
                                if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                {
                                    isOPTutup = true;
                                }

                            }


                            if (rowMonHotel != null)
                            {
                                _contMonPd.DbMonHotels.Remove(rowMonHotel);
                            }

                            var newRow = new DbMonHotel();
                            newRow.Nop = item.Nop;
                            newRow.Npwpd = item.Npwpd;
                            newRow.NpwpdNama = item.NpwpdNama;
                            newRow.NpwpdAlamat = item.NpwpdAlamat;
                            newRow.PajakId = item.PajakId;
                            newRow.PajakNama = item.PajakNama;
                            newRow.NamaOp = item.NamaOp;
                            newRow.AlamatOp = item.AlamatOp;
                            newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
                            newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
                            newRow.TglOpTutup = item.TglOpTutup;
                            newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                            newRow.IsTutup = isOPTutup ? 1 : 0;
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.TahunBuku = tahunBuku;
                            newRow.Akun = item.Akun;
                            newRow.NamaAkun = item.NamaAkun;
                            newRow.Jenis = item.Jenis;
                            newRow.NamaJenis = item.NamaJenis;
                            newRow.Objek = item.Objek;
                            newRow.NamaObjek = item.NamaObjek;
                            newRow.Rincian = item.Rincian;
                            newRow.NamaRincian = item.NamaRincian;
                            newRow.SubRincian = item.SubRincian;
                            newRow.NamaSubRincian = item.NamaSubRincian;
                            newRow.TahunPajakKetetapan = itemKetetapan.TAHUN;
                            newRow.MasaPajakKetetapan = itemKetetapan.MASAPAJAK;
                            newRow.SeqPajakKetetapan = itemKetetapan.SEQ;
                            newRow.KategoriKetetapan = itemKetetapan.JENIS_KETETAPAN.ToString();
                            newRow.TglKetetapan = itemKetetapan.TGL_KETETAPAN;
                            newRow.TglJatuhTempoBayar = itemKetetapan.TGL_JATUH_TEMPO_BAYAR;
                            newRow.PokokPajakKetetapan = itemKetetapan.POKOK - itemKetetapan.NILAI_PENGURANG;
                            newRow.PengurangPokokKetetapan = itemKetetapan.NILAI_PENGURANG;
                            newRow.AkunKetetapan = dbAkunPokok.Akun;
                            newRow.KelompokKetetapan = dbAkunPokok.Kelompok;
                            newRow.JenisKetetapan = dbAkunPokok.Jenis;
                            newRow.ObjekKetetapan = dbAkunPokok.Objek;
                            newRow.RincianKetetapan = dbAkunPokok.Rincian;
                            newRow.SubRincianKetetapan = dbAkunPokok.SubRincian;
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";
                            newRow.UpdDate = DateTime.Now;
                            newRow.UpdBy = "JOB";
                            _contMonPd.DbMonHotels.Add(newRow);
                            _contMonPd.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    // realisasi
                    try
                    {
                        var sqlRealisasi = @"SELECT  ID_SSPD,
            nvl(SYNC_REFF_BILL,'-') KODE_BILL, 
            '-' NO_KETETAPAN, 
            0 JENIS_PEMBAYARAN,
            5 JENIS_PAJAK,
            1 JENIS_KETETAPAN, 
            TO_DATE(MP_AKHIR) JATUH_TEMPO, 
            FK_NOP NOP,
            TO_NUMBER( BULAN_PAJAK) MASA, 
            TO_NUMBER(TAHUN_PAJAK) TAHUN, 
           TO_NUMBER(JML_POKOK) NOMINAL_POKOK, 
           TO_NUMBER(JML_DENDA) NOMINAL_SANKSI,
           0 NOMINAL_ADMINISTRASI, 
           0 NOMINAL_LAINYA,
           0 PENGURANG_POKOK, 
           0 PENGURANG_SANKSI,
           '-' REFF_PENGURANG_POKOK,'-'   REFF_PENGURANG_SANKSI,'-'   AKUN_POKOK,'-'   AKUN_SANKSI,'-'   AKUN_ADMINISTRASI, 
                                '-'  AKUN_LAINNYA,'-'   AKUN_PENGURANG_POKOK,'-'   AKUN_PENGURANG_SANKSI,'-'  INVOICE_NUMBER,TO_DATE(TGL_SETORAN) TRANSACTION_DATE, 
                                '-'  NO_NTPD,1  STATUS_NTPD,SYSDATE  REKON_DATE,'-'   REKON_BY,'-'   REKON_REFF,100 SEQ_KETETAPAN,SYSDATE INS_DATE                                                                 
FROM VW_SIMPADA_SSPD@LIHATHPPSERVER
WHERE NAMA_PAJAK_DAERAH='HOTEL'  AND REPLACE(FK_NOP,'.','')=:NOP AND TO_CHAR(TGL_SETORAN,'YYYY')=:TAHUN ";

                        var pembayaranSspdList = _contMonitoringDB.Set<SSPD>()
                            .FromSqlRaw(sqlRealisasi, new[] {
                    new OracleParameter("NOP", item.Nop),
                    new OracleParameter("TAHUN", tahunBuku)
                            }).ToList();

                        if (pembayaranSspdList != null)
                        {
                            foreach (var itemSSPD in pembayaranSspdList)
                            {
                                var ketetapan = _contMonPd.DbMonHotels.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
                                                                                        x.TahunPajakKetetapan == itemSSPD.TAHUN &&
                                                                                        x.MasaPajakKetetapan == itemSSPD.MASA &&
                                                                                        x.SeqPajakKetetapan == itemSSPD.SEQ_KETETAPAN);
                                if (ketetapan != null)
                                {
                                    string akunBayar = "-";
                                    string kelompokBayar = "-";
                                    string jenisBayar = "-";
                                    string objekBayar = "-";
                                    string rincianBayar = "-";
                                    string subrincianBayar = "-";

                                    var getAkun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkun != null)
                                    {
                                        akunBayar = getAkun.Akun;
                                        kelompokBayar = getAkun.Kelompok;
                                        jenisBayar = getAkun.Jenis;
                                        objekBayar = getAkun.Objek;
                                        rincianBayar = getAkun.Rincian;
                                        subrincianBayar = getAkun.SubRincian;
                                    }

                                    string akunSanksi = "-";
                                    string kelompokSanksi = "-";
                                    string jenisSanksi = "-";
                                    string objekSanksi = "-";
                                    string rincianSanksi = "-";
                                    string subrincianSanksi = "-";

                                    var getAkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkunSanksi != null)
                                    {
                                        akunSanksi = getAkunSanksi.Akun;
                                        kelompokSanksi = getAkunSanksi.Kelompok;
                                        jenisSanksi = getAkunSanksi.Jenis;
                                        objekSanksi = getAkunSanksi.Objek;
                                        rincianSanksi = getAkunSanksi.Rincian;
                                        subrincianSanksi = getAkunSanksi.SubRincian;
                                    }



                                    if (!ketetapan.TglBayarPokok.HasValue)
                                    {
                                        ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarPokok.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarPokok = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalPokokBayar = ketetapan.NominalPokokBayar + itemSSPD.NOMINAL_POKOK;
                                    ketetapan.AkunPokokBayar = akunBayar;
                                    ketetapan.Kelompok = kelompokBayar;
                                    ketetapan.JenisPokokBayar = jenisBayar;
                                    ketetapan.ObjekPokokBayar = objekBayar;
                                    ketetapan.RincianPokokBayar = rincianBayar;
                                    ketetapan.SubRincianPokokBayar = subrincianBayar;

                                    if (!ketetapan.TglBayarSanksi.HasValue)
                                    {
                                        ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarSanksi.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiBayar + itemSSPD.NOMINAL_SANKSI;
                                    ketetapan.AkunSanksiBayar = akunSanksi;
                                    ketetapan.KelompokSanksiBayar = kelompokSanksi;
                                    ketetapan.JenisSanksiBayar = jenisSanksi;
                                    ketetapan.ObjekSanksiBayar = objekSanksi;
                                    ketetapan.RincianSanksiBayar = rincianSanksi;
                                    ketetapan.SubRincianSanksiBayar = subrincianSanksi;


                                    if (!ketetapan.TglBayarSanksiKenaikan.HasValue)
                                    {
                                        ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
                                    }
                                    else
                                    {
                                        if (ketetapan.TglBayarSanksiKenaikan.Value < itemSSPD.TRANSACTION_DATE)
                                        {
                                            ketetapan.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;
                                        }
                                    }

                                    ketetapan.NominalSanksiBayar = ketetapan.NominalSanksiKenaikanBayar + itemSSPD.NOMINAL_ADMINISTRASI;
                                    ketetapan.AkunSanksiBayar = akunSanksi;
                                    ketetapan.KelompokSanksiBayar = kelompokSanksi;
                                    ketetapan.JenisSanksiBayar = jenisSanksi;
                                    ketetapan.ObjekSanksiBayar = objekSanksi;
                                    ketetapan.RincianSanksiBayar = rincianSanksi;
                                    ketetapan.SubRincianSanksiBayar = subrincianSanksi;
                                    _contMonPd.SaveChanges();
                                }
                                else
                                {
                                    bool isOPTutup = false;
                                    if (item.TglOpTutup.HasValue)
                                    {
                                        if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                        {
                                            isOPTutup = true;
                                        }

                                    }


                                    string akunBayar = "-";
                                    string kelompokBayar = "-";
                                    string jenisBayar = "-";
                                    string objekBayar = "-";
                                    string rincianBayar = "-";
                                    string subrincianBayar = "-";

                                    var getAkun = GetDbAkun(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkun != null)
                                    {
                                        akunBayar = getAkun.Akun;
                                        kelompokBayar = getAkun.Kelompok;
                                        jenisBayar = getAkun.Jenis;
                                        objekBayar = getAkun.Objek;
                                        rincianBayar = getAkun.Rincian;
                                        subrincianBayar = getAkun.SubRincian;
                                    }

                                    string akunSanksi = "-";
                                    string kelompokSanksi = "-";
                                    string jenisSanksi = "-";
                                    string objekSanksi = "-";
                                    string rincianSanksi = "-";
                                    string subrincianSanksi = "-";

                                    var getAkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)item.KategoriId);
                                    if (getAkunSanksi != null)
                                    {
                                        akunSanksi = getAkunSanksi.Akun;
                                        kelompokSanksi = getAkunSanksi.Kelompok;
                                        jenisSanksi = getAkunSanksi.Jenis;
                                        objekSanksi = getAkunSanksi.Objek;
                                        rincianSanksi = getAkunSanksi.Rincian;
                                        subrincianSanksi = getAkunSanksi.SubRincian;
                                    }


                                    var newRow = new DbMonHotel();
                                    newRow.Nop = item.Nop;
                                    newRow.Npwpd = item.Npwpd;
                                    newRow.NpwpdNama = item.NpwpdNama;
                                    newRow.NpwpdAlamat = item.NpwpdAlamat;
                                    newRow.PajakId = item.PajakId;
                                    newRow.PajakNama = item.PajakNama;
                                    newRow.NamaOp = item.NamaOp;
                                    newRow.AlamatOp = item.AlamatOp;
                                    newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
                                    newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
                                    newRow.TglOpTutup = item.TglOpTutup;
                                    newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                                    newRow.IsTutup = isOPTutup ? 1 : 0;
                                    newRow.KategoriId = item.KategoriId;
                                    newRow.KategoriNama = item.KategoriNama;
                                    newRow.TahunBuku = tahunBuku;
                                    newRow.Akun = item.Akun;
                                    newRow.NamaAkun = item.NamaAkun;
                                    newRow.Jenis = item.Jenis;
                                    newRow.NamaJenis = item.NamaJenis;
                                    newRow.Objek = item.Objek;
                                    newRow.NamaObjek = item.NamaObjek;
                                    newRow.Rincian = item.Rincian;
                                    newRow.NamaRincian = item.NamaRincian;
                                    newRow.SubRincian = item.SubRincian;
                                    newRow.NamaSubRincian = item.NamaSubRincian;
                                    newRow.TahunPajakKetetapan = itemSSPD.TAHUN;
                                    newRow.MasaPajakKetetapan = itemSSPD.MASA;
                                    newRow.SeqPajakKetetapan = 101;
                                    newRow.KategoriKetetapan = "4";
                                    newRow.TglKetetapan = itemSSPD.TRANSACTION_DATE;
                                    newRow.TglJatuhTempoBayar = itemSSPD.JATUH_TEMPO;
                                    newRow.PokokPajakKetetapan = itemSSPD.NOMINAL_POKOK;
                                    newRow.PengurangPokokKetetapan = 0;
                                    newRow.AkunKetetapan = akunBayar;
                                    newRow.KelompokKetetapan = kelompokBayar;
                                    newRow.JenisKetetapan = jenisBayar;
                                    newRow.ObjekKetetapan = objekBayar;
                                    newRow.RincianKetetapan = rincianBayar;
                                    newRow.SubRincianKetetapan = subrincianBayar;
                                    newRow.InsDate = DateTime.Now;
                                    newRow.InsBy = "JOB";
                                    newRow.UpdDate = DateTime.Now;
                                    newRow.UpdBy = "JOB";


                                    newRow.NominalPokokBayar = itemSSPD.NOMINAL_POKOK;
                                    newRow.AkunPokokBayar = akunBayar;
                                    newRow.Kelompok = kelompokBayar;
                                    newRow.JenisPokokBayar = jenisBayar;
                                    newRow.ObjekPokokBayar = objekBayar;
                                    newRow.RincianPokokBayar = rincianBayar;
                                    newRow.SubRincianPokokBayar = subrincianBayar;
                                    newRow.TglBayarSanksi = itemSSPD.TRANSACTION_DATE;
                                    newRow.NominalSanksiBayar = itemSSPD.NOMINAL_SANKSI;
                                    newRow.AkunSanksiBayar = akunSanksi;
                                    newRow.KelompokSanksiBayar = kelompokSanksi;
                                    newRow.JenisSanksiBayar = jenisSanksi;
                                    newRow.ObjekSanksiBayar = objekSanksi;
                                    newRow.RincianSanksiBayar = rincianSanksi;
                                    newRow.SubRincianSanksiBayar = subrincianSanksi;
                                    newRow.TglBayarSanksiKenaikan = itemSSPD.TRANSACTION_DATE;

                                    newRow.NominalSanksiBayar = itemSSPD.NOMINAL_ADMINISTRASI;
                                    newRow.AkunSanksiBayar = akunSanksi;
                                    newRow.KelompokSanksiBayar = kelompokSanksi;
                                    newRow.JenisSanksiBayar = jenisSanksi;
                                    newRow.ObjekSanksiBayar = objekSanksi;
                                    newRow.RincianSanksiBayar = rincianSanksi;
                                    newRow.SubRincianSanksiBayar = subrincianSanksi;
                                    _contMonPd.DbMonHotels.Add(newRow);
                                    _contMonPd.SaveChanges();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_OP_HOTEL HPP TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
                    Thread.Sleep(50);
                    _contMonPd.SaveChanges();

                }
            }
            Console.WriteLine(" ");
        }
    
        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPHOTEL.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPHOTEL.ToString().ToUpper();
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
