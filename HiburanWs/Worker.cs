using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace HiburanWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 5;

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
                    "ERROR HIBURAN WS",
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

            // do fill db op HIBURAN
            if (IsGetDBOp())
            {
                for (var i = tahunAmbil; i <= tglServer.Year; i++)
                {
                    FillOP(i);
                }
            }

            MailHelper.SendMail(
            false,
            "DONE HIBURAN  WS",
            $@"HIBURAN WS FINISHED",
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
        'Pajak Jasa Kesenian Hiburan' PAJAK_NAMA,
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
        CASE D.ID 
            WHEN 64 THEN 48
            WHEN 64 THEN 50
            ELSE 54
        END AS KATEGORI_ID,
        CASE D.ID 
            WHEN 64 THEN 'PANTI PIJAT/THERAPY/SAUNA/SPA'
            WHEN 64 THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
            ELSE 'HIBURAN'
        END AS KATEGORI_NAMA,
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
JOIN OBJEK_PAJAK_HIBURAN B ON A.NOP = B.NOP
JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
JOIN M_KATEGORI_PAJAK D ON D.ID = A.KATEGORI
LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT
WHERE A.NPWPD NOT IN (
    select npwpd_no  
    from npwpd 
    WHERE REF_THN_PEL = 2023 OR NAMA LIKE '%FULAN%' OR NPWPD_NO = '3578200503840003'
)    and  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                    ";

                var result = _contSbyTax.Set<DbOpHiburan>().FromSqlRaw(sql, new[] {
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
                        var sourceRow = _contMonPd.DbOpHiburans.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
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
                            var newRow = new MonPDLib.EF.DbOpHiburan();
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
                            _contMonPd.DbOpHiburans.Add(newRow);
                            _contMonPd.SaveChanges();
                        }

                    }
                    catch (Exception ex)
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

                        var ketetapanSbyTaxOld = _contSbyTaxOld.Set<OPSkpdHiburan>()
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
                            var rowMonHiburan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            bool isOPTutup = false;
                            if (item.TglOpTutup.HasValue)
                            {
                                if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                {
                                    isOPTutup = true;
                                }

                            }


                            if (rowMonHiburan != null)
                            {
                                _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
                            }

                            var newRow = new DbMonHiburan();
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
                            _contMonPd.DbMonHiburans.Add(newRow);
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
                    WHERE     A.JENIS_PAJAK = 5 AND A.NOP = :NOP AND TO_CHAR(TRANSACTION_DATE,'YYYY')=:TAHUN ";

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
                                var ketetapan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
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

                                    var getAkun = GetDbAkun(tahunBuku, KDPajak,(int)item.KategoriId);
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
                    Console.Write($"\rDB_OP_HIBURAN SBYTAX TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
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
SELECT REPLACE(A.FK_NOP, '.', '') NOP,NVL(FK_NPWPD, '-') NPWPD,NAMA_OP, 5 PAJAK_ID,  'Pajak Jasa Kesenian Hiburan' PAJAK_NAMA,
              NVL(ALAMAT_OP, '-') ALAMAT_OP, '-'  ALAMAT_OP_NO,'-' ALAMAT_OP_RT,'-' ALAMAT_OP_RW,NVL(NOMOR_TELEPON, '-') TELP,
              NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH, NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,TGL_TUTUP TGL_OP_TUTUP,
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
             sysdate INS_dATE, 'JOB' INS_BY ,fk_wilayah_pajak WILAYAH_PAJAK   ,
            '-' AKUN,'-'  NAMA_AKUN,'-'  JENIS,'-'  NAMA_JENIS,'-'  OBJEK,'-'  NAMA_OBJEK,'-'  RINCIAN,
'-'  NAMA_RINCIAN,'-'  SUB_RINCIAN,'-'  NAMA_SUB_RINCIAN,'-'  KELOMPOK,
            '-'  NAMA_KELOMPOK,1  IS_TUTUP,'-'  NPWPD_NAMA, '-'  NPWPD_ALAMAT,1 TAHUN_BUKU
FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND A.FK_NOP IS NOT NULL 
)
WHERE  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                    ";

                var result = _contMonitoringDB.Set<DbOpHiburan>().FromSqlRaw(sql, new[] {
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
                        var sourceRow = _contMonPd.DbOpHiburans.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
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
                            var newRow = new MonPDLib.EF.DbOpHiburan();
                            newRow.Nop = item.Nop;
                            newRow.Npwpd = item.Npwpd;
                            // set manual
                            var infoWP = GetInfoWPHPP(newRow.Npwpd);
                            newRow.NpwpdNama =infoWP[0];
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
                            _contMonPd.DbOpHiburans.Add(newRow);
                            _contMonPd.SaveChanges();
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
SELECT REPLACE(FK_NOP,'.','') NOP, TO_NUMBER(TAHUN_PAJAK) TAHUN,TO_NUMBER(BULAN_PAJAK) MASAPAJAK,100 SEQ,1 JENIS_KETETAPAN,TO_DATE(NVL(TGL_SPTPD_DISETOR,MP_AKHIR)) TGL_KETETAPAN,TO_DATE(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO_BAYAR ,0 NILAI_PENGURANG,
            TO_NUMBER(KETETAPAN_TOTAL)  POKOK
FROM VW_SIMPADA_SPTPD@LIHATHPPSERVER
WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND FK_NOP IS NOT NULL and REPLACE(FK_NOP,'.','')=:NOP
)
WHERE  TO_CHAR(TGL_KETETAPAN,'YYYY')=:TAHUN       ";

                        var ketetapanMonitoringDb = _contMonitoringDB.Set<OPSkpdHiburan>()
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
                            var rowMonHiburan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            bool isOPTutup = false;
                            if (item.TglOpTutup.HasValue)
                            {
                                if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                {
                                    isOPTutup = true;
                                }

                            }


                            if (rowMonHiburan != null)
                            {
                                _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
                            }

                            var newRow = new DbMonHiburan();
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
                            _contMonPd.DbMonHiburans.Add(newRow);
                            _contMonPd.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    // realisasi

                    var newRowEx= new DbMonHiburan();
                    try
                    {
                        var sqlRealisasi = @"SELECT  ID_SSPD,
            nvl(SYNC_REFF_BILL,'-') KODE_BILL, 
            '-' NO_KETETAPAN, 
            0 JENIS_PEMBAYARAN,
            5 JENIS_PAJAK,
            1 JENIS_KETETAPAN, 
            TO_DATE(MP_AKHIR) JATUH_TEMPO, 
            REPLACE(FK_NOP,'.','') NOP,
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
WHERE NAMA_PAJAK_DAERAH='HIBURAN'  AND REPLACE(FK_NOP,'.','')=:NOP AND TO_CHAR(TGL_SETORAN,'YYYY')=:TAHUN ";
                        
                        var pembayaranSspdList = _contMonitoringDB.Set<SSPD>()
                            .FromSqlRaw(sqlRealisasi, new[] {
                    new OracleParameter("NOP", item.Nop),
                    new OracleParameter("TAHUN", tahunBuku)
                            }).ToList();

                        

                        if (pembayaranSspdList != null)
                        {
                            foreach (var itemSSPD in pembayaranSspdList)
                            {
                                if (item.Nop == "357810000190301208" && itemSSPD.MASA==12 && itemSSPD.TAHUN==2020)
                                {
                                    string x = "";
                                }

                                var ketetapan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
                                                                                        x.TahunPajakKetetapan == itemSSPD.TAHUN &&
                                                                                        x.MasaPajakKetetapan == itemSSPD.MASA &&
                                                                                        x.SeqPajakKetetapan == itemSSPD.SEQ_KETETAPAN);

                                if (ketetapan ==null)
                                {
                                    ketetapan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
                                                                                        x.TahunPajakKetetapan == itemSSPD.TAHUN &&
                                                                                        x.MasaPajakKetetapan == itemSSPD.MASA &&
                                                                                        x.SeqPajakKetetapan == 101);
                                }
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


                                    var newRow = new DbMonHiburan();
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
                                    newRow.NominalSanksiBayar =itemSSPD.NOMINAL_SANKSI;
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
                                    newRowEx = newRow;
                                    _contMonPd.DbMonHiburans.Add(newRow);
                                    _contMonPd.SaveChanges();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        var nopp = item.Nop;
                        var masa = tahunBuku;
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_OP_HIBURAN HPP TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
                    Thread.Sleep(50);
                    _contMonPd.SaveChanges();

                }
            }
            Console.WriteLine(" ");
        }


        private async Task DoWorkFullScanAsync(CancellationToken stoppingToken)
        {
            int idPajak = 5;
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


            //FILL DB OP
            if (IsGetDBOp())
            {
                //GET DB OP HIBURAN SBYTAX
                using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
                {
                    var sql = @"
                                                                                   SELECT  A.NOP,
        C.NPWPD_NO NPWPD,
        C.NAMA NPWPD_NAMA,
        C.ALAMAT NPWPD_ALAMAT,
        A.PAJAK_ID ,
        'Pajak Jasa Kesenian Hiburan' PAJAK_NAMA,
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
        CASE D.ID 
            WHEN 64 THEN 48
            WHEN 64 THEN 50
            ELSE 54
        END AS KATEGORI_ID,
        CASE D.ID 
            WHEN 64 THEN 'PANTI PIJAT/THERAPY/SAUNA/SPA'
            WHEN 64 THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
            ELSE 'HIBURAN'
        END AS KATEGORI_NAMA,
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
JOIN OBJEK_PAJAK_HIBURAN B ON A.NOP = B.NOP
JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
JOIN M_KATEGORI_PAJAK D ON D.ID = A.KATEGORI
LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT
WHERE A.NPWPD NOT IN (
    select npwpd_no  
    from npwpd 
    WHERE REF_THN_PEL = 2023 OR NAMA LIKE '%FULAN%' OR NPWPD_NO = '3578200503840003'
)    and  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                    ";
                    
                    var result = await _contSbyTax.Set<DbOpHiburan>().FromSqlRaw(sql, new[] {
                    new OracleParameter("TAHUN", tahunAmbil)
                }).ToListAsync();
                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            if (item.TglMulaiBukaOp.Year <= i)
                            {
                                var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                if (sourceRow != null)
                                {

                                    sourceRow.TglOpTutup = item.TglOpTutup;
                                    sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                    var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
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
                                    var newRow = new MonPDLib.EF.DbOpHiburan();
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

                                    newRow.TahunBuku = i;
                                    var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
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
                                    _contMonPd.DbOpHiburans.Add(newRow);
                                }

                                _contMonPd.SaveChanges();
                                Console.WriteLine($"{DateTime.Now} DB_OP {i} {item.Nop}");
                            }
                        }
                    }
                }

                using (var _contMonitoringDb2 = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        	select 	REPLACE(A.FK_NOP, '.', '') NOP,  
	        NVL(FK_NPWPD, '-') NPWPD,
	        NAMA_OP NPWPD_NAMA,
	        NVL(ALAMAT_OP, '-') NPWPD_ALAMAT,
	        '-' ALAMAT_OP_NO,
	        '-' ALAMAT_OP_RT,
	        '-' ALAMAT_OP_RW,
	        NVL(NOMOR_TELEPON, '-') TELP,
	        NAMA_OP,
	        5 PAJAK_ID,
	        'Pajak Jasa Kesenian Hiburan' PAJAK_NAMA,
	        NVL(ALAMAT_OP, '-') ALAMAT_OP,
	        NVL(FK_KELURAHAN, '000') ALAMAT_OP_KD_LURAH,
	        NVL(FK_KECAMATAN, '000') ALAMAT_OP_KD_CAMAT,
	         CASE 
	        WHEN STATUS_OP_DESC <> 'BUKA' THEN TGL_TUTUP 
	        ELSE NULL 
	    END AS TGL_OP_TUTUP,
	    NVL(TGL_BUKA, TO_DATE('1901-01-01', 'YYYY-MM-DD')) TGL_MULAI_BUKA_OP,
	    CASE 
	        WHEN STATUS_OP_DESC <> 'BUKA' THEN 0  
	        ELSE 1 
	    END AS IS_TUTUP,
	    NVL(NAMA_WILAYAH_PAJAK, 'SURABAYA ') WILAYAH_PAJAK,
	    NVL(B.KATEGORI_ID, 54) KATEGORI_ID,
		NVL(B.KATEGORI_NAMA, 'HIBURAN') KATEGORI_NAMA,
	    NAMA_AYAT_PAJAK,
	        0 METODE_PENJUALAN,
	        0 METODE_PEMBAYARAN,
	        0 JUMLAH_KARYAWAN,
	    sysdate INS_dATE, 
	    'JOB' INS_BY,
	    TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')) TAHUN_BUKU,
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
	FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER A
	LEFT JOIN (
	SELECT  FK_NOP, 
			PAJAK_ID, 
			KATEGORI_ID, 
			KATEGORI_NAMA
	FROM (
		SELECT 	FK_NOP, 
				PAJAK_ID,
				CASE 
			        WHEN KATEGORI_STATUS_BARU = 'BAKERY/PASTRY' THEN 1
			        WHEN KATEGORI_STATUS_BARU = 'BUFFET/ALL YOU CAN EAT' THEN 2
			        WHEN KATEGORI_STATUS_BARU = 'CAFE' THEN 3
			        WHEN KATEGORI_STATUS_BARU = 'CATERING' THEN 4
			        WHEN KATEGORI_STATUS_BARU = 'DEPOT/KEDAI' THEN 5
			        WHEN KATEGORI_STATUS_BARU = 'FAST FOOD' THEN 6
			        WHEN KATEGORI_STATUS_BARU = 'RESTORAN' THEN 7
			        WHEN KATEGORI_STATUS_BARU = 'RESTORAN PADA MINIMARKET' THEN 8
			        WHEN KATEGORI_STATUS_BARU = 'RESTORAN PADA OBJEK HIBURAN' THEN 9
			        WHEN KATEGORI_STATUS_BARU = 'RUMAH MAKAN' THEN 10
			        WHEN KATEGORI_STATUS_BARU = 'TENANT MAKANAN/MINUMAN' THEN 11
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL BINTANG DUA' THEN 12
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL BINTANG EMPAT' THEN 13
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL BINTANG LIMA' THEN 14
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL BINTANG SATU' THEN 15
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL BINTANG TIGA' THEN 16
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL NON BINTANG' THEN 17
			        WHEN KATEGORI_STATUS_BARU = 'KATERING' THEN 18
			        WHEN KATEGORI_STATUS_BARU = 'RUMAH KOS' THEN 20
			        WHEN KATEGORI_STATUS_BARU = 'APARTEMEN' THEN 21
			        WHEN KATEGORI_STATUS_BARU = 'APOTIK' THEN 22
			        WHEN KATEGORI_STATUS_BARU = 'HOTEL/PENGINAPAN' THEN 23
			        WHEN KATEGORI_STATUS_BARU = 'KLINIK' THEN 24
			        WHEN KATEGORI_STATUS_BARU = 'LABORATORIUM' THEN 25
			        WHEN KATEGORI_STATUS_BARU = 'MALL/PLAZA' THEN 26
			        WHEN KATEGORI_STATUS_BARU = 'MINIMARKET' THEN 27
			        WHEN KATEGORI_STATUS_BARU = 'PASAR' THEN 28
			        WHEN KATEGORI_STATUS_BARU = 'PERBANKAN' THEN 29
			        WHEN KATEGORI_STATUS_BARU = 'PERGUDANGAN/PABRIK' THEN 30
			        WHEN KATEGORI_STATUS_BARU = 'PERKANTORAN' THEN 31
			        WHEN KATEGORI_STATUS_BARU = 'PERSEWAAN GEDUNG' THEN 32
			        WHEN KATEGORI_STATUS_BARU = 'PERTOKOAN' THEN 33
			        WHEN KATEGORI_STATUS_BARU = 'RUMAH SAKIT' THEN 34
			        WHEN KATEGORI_STATUS_BARU = 'STASIUN' THEN 35
			        WHEN KATEGORI_STATUS_BARU = 'SWALAYAN/SUPERMARKET' THEN 36
			        WHEN KATEGORI_STATUS_BARU = 'USAHA HIBURAN' THEN 37
			        WHEN KATEGORI_STATUS_BARU = 'USAHA LAINNYA' THEN 38
			        WHEN KATEGORI_STATUS_BARU = 'USAHA PARKIR' THEN 39
			        WHEN KATEGORI_STATUS_BARU = 'USAHA RESTORAN' THEN 40
			        WHEN KATEGORI_STATUS_BARU = 'BAR/CAFE/KLAB MALAM/DISKOTIK' THEN 41
			        WHEN KATEGORI_STATUS_BARU = 'BIOSKOP' THEN 42
			        WHEN KATEGORI_STATUS_BARU = 'FITNESS/PUSAT KEBUGARAN' THEN 43
			        WHEN KATEGORI_STATUS_BARU = 'KARAOKE DEWASA' THEN 44
			        WHEN KATEGORI_STATUS_BARU = 'KARAOKE KELUARGA' THEN 45
			        WHEN KATEGORI_STATUS_BARU = 'OLAHRAGA' THEN 46
			        WHEN KATEGORI_STATUS_BARU = 'PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU' THEN 47
			        WHEN KATEGORI_STATUS_BARU = 'PANTI PIJAT/THERAPY/SAUNA/SPA' THEN 48
			        WHEN KATEGORI_STATUS_BARU = 'PERMAINAN ANAK' THEN 49
			        WHEN KATEGORI_STATUS_BARU = 'PERMAINAN ANAK/PERMAINAN KETANGKASAN' THEN 50
			        WHEN KATEGORI_STATUS_BARU = 'RUMAH SAKIT/APOTEK/KLINIK/LABORATORIUM' THEN 51
			        WHEN KATEGORI_STATUS_BARU = 'SWALAYAN/SUPERMARKET/MINIMARKET/PASAR' THEN 52
			        WHEN KATEGORI_STATUS_BARU = 'USAHA RESTORAN/HIBURAN' THEN 53
			        WHEN KATEGORI_STATUS_BARU = 'AIR TANAH' THEN 56
			        WHEN KATEGORI_STATUS_BARU = 'PBB' THEN 57
			        ELSE NULL
			    END AS KATEGORI_ID,
				KATEGORI_STATUS_BARU KATEGORI_NAMA
		FROM (
			SELECT 	FK_NOP,
					3 PAJAK_ID,
					KATEGORI_STATUS,
					CASE
						WHEN KATEGORI_STATUS = 'RUMAH KOS' THEN 'HOTEL NON BINTANG'
						WHEN KATEGORI_STATUS = 'KATERING' THEN 'HOTEL NON BINTANG'
						WHEN KATEGORI_STATUS = 'RESTORAN' THEN 'HOTEL NON BINTANG'
						ELSE KATEGORI_STATUS
					END AS KATEGORI_STATUS_BARU
			FROM T_OP_KATEGORI_STATUS
			WHERE FK_PAJAK_DAERAH = 1
			UNION ALL
			SELECT 	FK_NOP,
					1 PAJAK_ID,
					KATEGORI_STATUS,
					CASE
						WHEN KATEGORI_STATUS = 'RUMAH MAKAN' THEN 'RESTORAN'
						ELSE KATEGORI_STATUS
					END AS KATEGORI_STATUS_BARU
			FROM T_OP_KATEGORI_STATUS
			WHERE FK_PAJAK_DAERAH = 2
			UNION ALL
			SELECT 	FK_NOP,
					4 PAJAK_ID,
					KATEGORI_STATUS,
					CASE
						WHEN KATEGORI_STATUS = 'APOTIK' THEN 'RUMAH SAKIT/APOTEK/KLINIK/LABORATORIUM'
						WHEN KATEGORI_STATUS = 'KLINIK' THEN 'RUMAH SAKIT/APOTEK/KLINIK/LABORATORIUM'
						WHEN KATEGORI_STATUS = 'LABORATORIUM' THEN 'RUMAH SAKIT/APOTEK/KLINIK/LABORATORIUM'
						WHEN KATEGORI_STATUS = 'MINIMARKET' THEN 'SWALAYAN/SUPERMARKET/MINIMARKET/PASAR'
						WHEN KATEGORI_STATUS = 'PASAR' THEN 'SWALAYAN/SUPERMARKET/MINIMARKET/PASAR'
						WHEN KATEGORI_STATUS = 'PERBANKAN' THEN 'PERKANTORAN'
						WHEN KATEGORI_STATUS = 'PERGUDANGAN/PABRIK' THEN 'USAHA LAINNYA'
						WHEN KATEGORI_STATUS = 'PERSEWAAN GEDUNG' THEN 'USAHA LAINNYA'
						WHEN KATEGORI_STATUS = 'RUMAH SAKIT' THEN 'RUMAH SAKIT/APOTEK/KLINIK/LABORATORIUM'
						WHEN KATEGORI_STATUS = 'STASIUN' THEN 'USAHA LAINNYA'
						WHEN KATEGORI_STATUS = 'SWALAYAN/SUPERMARKET' THEN 'SWALAYAN/SUPERMARKET/MINIMARKET/PASAR'
						WHEN KATEGORI_STATUS = 'USAHA PARKIR' THEN 'USAHA LAINNYA'
						WHEN KATEGORI_STATUS = 'USAHA RESTORAN' THEN 'USAHA RESTORAN/HIBURAN'
						ELSE KATEGORI_STATUS
					END AS KATEGORI_STATUS_BARU
			FROM T_OP_KATEGORI_STATUS
			WHERE FK_PAJAK_DAERAH = 7
			UNION ALL
			SELECT 	FK_NOP,
					5 PAJAK_ID,
					KATEGORI_STATUS,
					CASE
						WHEN KATEGORI_STATUS = 'BOWLING' THEN 'OLAHRAGA'
						WHEN KATEGORI_STATUS = 'WISATA TIRTA/REKREASI AIR' THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
						WHEN KATEGORI_STATUS = 'TAMAN SATWA/PEMANDIAN ALAM/TAMAN REKREASI' THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
						WHEN KATEGORI_STATUS = 'PERMAINAN KETANGKASAN' THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
						WHEN KATEGORI_STATUS = 'BILLYARD' THEN 'OLAHRAGA'
						WHEN KATEGORI_STATUS = 'DISKOTIK' THEN 'BAR/CAFE/KLAB MALAM/DISKOTIK'
						WHEN KATEGORI_STATUS = 'PERMAINAN ANAK' THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
						WHEN KATEGORI_STATUS = 'PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU' THEN 'PERMAINAN ANAK/PERMAINAN KETANGKASAN'
						WHEN KATEGORI_STATUS = 'GEDUNG OLAHRAGA' THEN 'OLAHRAGA'
						WHEN KATEGORI_STATUS = 'BAR/CAFE/KLAB MALAM' THEN 'BAR/CAFE/KLAB MALAM/DISKOTIK'
						WHEN KATEGORI_STATUS = 'FUTSAL (OLAHRAGA)' THEN 'OLAHRAGA'
						WHEN KATEGORI_STATUS = 'KOLAM RENANG' THEN 'OLAHRAGA'
						ELSE KATEGORI_STATUS
					END AS KATEGORI_STATUS_BARU
			FROM (
				SELECT DISTINCT FK_NOP, FK_PAJAK_DAERAH, NAMA_JENIS_PAJAK KATEGORI_STATUS
				FROM VW_SIMPADA_OP_all_mon@LIHATHPPSERVER
				WHERE NAMA_PAJAK_DAERAH = 'HIBURAN' AND STATUS_OP = 1 AND KATEGORI_PAJAK != 'INSIDENTIL'
			) A
		) A
	) A
	WHERE PAJAK_ID = 5
	) B ON A.FK_NOP = B.FK_NOP
	WHERE 	NAMA_PAJAK_DAERAH ='HIBURAN' 
			AND KATEGORI_PAJAK <> 'OBJEK TESTING'
			AND A.FK_NOP IS NOT NULL
			AND REPLACE(A.FK_NOP, '.', '') LIKE '3578%'

                    ";

                    var result = await _contMonitoringDb2.Set<DbOpHiburan>().FromSqlRaw(sql).ToListAsync();

                    var distinctNop = result.Select(x => x.Nop).ToList();
                    var dataExisting = _contMonPd.DbOpHiburans.Where(x => distinctNop.Contains(x.Nop)).ToList();


                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            var isExist = dataExisting.Where(x => x.Nop == item.Nop && x.TahunBuku == i).Any();
                            if (!isExist)
                            {
                                if (item.TglMulaiBukaOp.Year <= i)
                                {
                                    var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                    if (sourceRow != null)
                                    {
                                        sourceRow.TglOpTutup = item.TglOpTutup;
                                        sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                        var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
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
                                        var newRow = new MonPDLib.EF.DbOpHiburan();
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

                                        newRow.TahunBuku = i;
                                        var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
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
                                        _contMonPd.DbOpHiburans.Add(newRow);
                                    }

                                    _contMonPd.SaveChanges();
                                    Console.WriteLine($"{DateTime.Now} DB_OP_HPP {tahunAmbil} {item.Nop}");
                                }
                            }

                        }
                    }
                }
            }

            ////FILL KETETAPAN 
            var _contSbyTaxOld = DBClass.GetSurabayaTaxContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                var opList = _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == thn).ToList();
                for (int bln = 1; bln <= 12; bln++)
                {
                    foreach (var op in opList)
                    {
                        bool isOPTutup = false;
                        DateTime tglPenetapan = new DateTime(thn, bln, 1);
                        if (op.TglOpTutup.HasValue)
                        {
                            if (op.TglOpTutup.Value.Date < tglPenetapan.Date)
                            {
                                isOPTutup = true;
                            }

                        }
                        Console.WriteLine($"{DateTime.Now} [QUERY] KETETAPAN SBYTAX HIBURAN {thn}-{bln}-{op.Nop}");
                        var sql = @"
                            SELECT 	A.NOP,
                              A.TAHUN,
                              A.MASAPAJAK,
                              A.SEQ,
                              1 JENIS_KETETAPAN,
                              B.TGL_PENETAPAN TGL_KETETAPAN,
                              C.TGL_JATUH_TEMPO_BAYAR ,
                              0 NILAI_PENGURANG,
                              A.NILAI_PAJAK POKOK
                            FROM (
                             SELECT 	A.NOP, 
                               A.TAHUN, 
                               A.MASAPAJAK,
                               A.SEQ,
                               ((NVL(B.PROSEN_TARIF_PAJAK, 0)/100) * A.TOTAL_OMSET) NILAI_PAJAK
                             FROM (
                              SELECT 	A.NOP, 
                                A.TAHUN, 
                                A.MASAPAJAK, 
                                A.SEQ,
                                SUM(A.OMSET) TOTAL_OMSET
                              FROM OBJEK_PAJAK_SPTPD_DET A
                              WHERE NOP IN (
                               SELECT NOP
                               FROM OBJEK_PAJAK
                               WHERE PAJAK_ID = 5
                              )
                              GROUP BY NOP, TAHUN, MASAPAJAK, SEQ
                             ) A
                             LEFT JOIN (
                              SELECT 	A.NOP, 
                                A.TAHUN, 
                                A.MASAPAJAK,
                                A.SEQ,
                                A.PROSEN_TARIF_PAJAK
                              FROM OBJEK_PAJAK_SPTPD A
                              WHERE NOP IN (
                               SELECT NOP
                               FROM OBJEK_PAJAK
                               WHERE PAJAK_ID = 5
                              )
                              GROUP BY A.NOP, 
                                A.TAHUN, 
                                A.MASAPAJAK,
                                A.SEQ,
                                A.PROSEN_TARIF_PAJAK
                             ) B ON A.NOP = B.NOP AND A.TAHUN = B.TAHUN AND A.MASAPAJAK = B.MASAPAJAK AND A.SEQ = B.SEQ
                            ) A
                            JOIN OBJEK_PAJAK_SPTPD_PENETAPAN B ON A.NOP = B.NOP 
                             AND A.TAHUN = B.TAHUN 
                             AND A.MASAPAJAK = B.MASAPAJAK
                             AND A.SEQ = B.SEQ
                            JOIN OBJEK_PAJAK_SPTPD C ON A.NOP = C.NOP
                             AND A.TAHUN = C.TAHUN 
                             AND A.MASAPAJAK = C.MASAPAJAK
                             AND A.SEQ = C.SEQ
                            WHERE A.NOP = :nop AND A.TAHUN = :tahun AND A.MASAPAJAK = :bulan
                        ";

                        var ketetapanSbyTaxOld = await _contSbyTaxOld.Set<OPSkpdHiburan>()
                            .FromSqlRaw(sql, new[] {
                                new OracleParameter("nop", op.Nop),
                                new OracleParameter("tahun", thn),
                                new OracleParameter("bulan", bln)
                            }).ToListAsync();
                        Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] KETETAPAN SBYTAX HIBURAN {thn}-{bln}-{op.Nop}");

                        var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)op.KategoriId);
                        foreach (var item in ketetapanSbyTaxOld)
                        {
                            string nop = item.NOP;
                            int tahunPajak = item.TAHUN;
                            int masaPajak = item.MASAPAJAK;
                            int seqPajak = item.SEQ;
                            var rowMonHiburan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            if (rowMonHiburan != null)
                            {
                                _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
                            }

                            var newRow = new DbMonHiburan();
                            newRow.Nop = item.NOP;
                            newRow.Npwpd = op.Npwpd;
                            newRow.NpwpdNama = op.NpwpdNama;
                            newRow.NpwpdAlamat = op.NpwpdAlamat;
                            newRow.PajakId = op.PajakId;
                            newRow.PajakNama = op.PajakNama;
                            newRow.NamaOp = op.NamaOp;
                            newRow.AlamatOp = op.AlamatOp;
                            newRow.AlamatOpKdLurah = op.AlamatOpKdLurah;
                            newRow.AlamatOpKdCamat = op.AlamatOpKdCamat;
                            newRow.TglOpTutup = op.TglOpTutup;
                            newRow.TglMulaiBukaOp = op.TglMulaiBukaOp;
                            newRow.IsTutup = isOPTutup ? 1 : 0;
                            newRow.KategoriId = op.KategoriId;
                            newRow.KategoriNama = op.KategoriNama;
                            newRow.TahunBuku = thn;
                            newRow.Akun = op.Akun;
                            newRow.NamaAkun = op.NamaAkun;
                            newRow.Jenis = op.Jenis;
                            newRow.NamaJenis = op.NamaJenis;
                            newRow.Objek = op.Objek;
                            newRow.NamaObjek = op.NamaObjek;
                            newRow.Rincian = op.Rincian;
                            newRow.NamaRincian = op.NamaRincian;
                            newRow.SubRincian = op.SubRincian;
                            newRow.NamaSubRincian = op.NamaSubRincian;
                            newRow.TahunPajakKetetapan = item.TAHUN;
                            newRow.MasaPajakKetetapan = item.MASAPAJAK;
                            newRow.SeqPajakKetetapan = item.SEQ;
                            newRow.KategoriKetetapan = item.JENIS_KETETAPAN.ToString();
                            newRow.TglKetetapan = item.TGL_KETETAPAN;
                            newRow.TglJatuhTempoBayar = item.TGL_JATUH_TEMPO_BAYAR;
                            newRow.PokokPajakKetetapan = item.POKOK - item.NILAI_PENGURANG;
                            newRow.PengurangPokokKetetapan = item.NILAI_PENGURANG;
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

                            GetRealisasi(ref newRow);
                            _contMonPd.DbMonHiburans.Add(newRow);
                            _contMonPd.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"DB_MON_HIBURAN {thn}-{bln}-{item.NOP}-{item.SEQ}");
                            Console.ResetColor();
                        }
                    }
                }
            }

            //FILL KETETAPAN MONITORING DB
            var _contMonitoringDb = DBClass.GetMonitoringDbContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                Console.WriteLine($"{DateTime.Now} [QUERY] OP (SSPD) (PHR) {thn}");
                var sql2 = @"
                SELECT 
    REPLACE(FK_NOP, '.', '') NOP,
    A.BULAN_PAJAK_SSPD  BULAN_PAJAK, 
    TO_NUMBER(TAHUN_PAJAK_SSPD) TAHUN_PAJAK,
    TGL_SSPD TRANSACTION_DATE,  
    JML_POKOK NOMINAL_POKOK, 
    JML_DENDA NOMINAL_SANKSI, 
    0 NOMINAL_ADMINISTRASI,
    0 NOMINAL_LAINNYA,
    0 PENGURANG_POKOK,
    0 PENGURANG_SANSKSI,
    100 SEQ_KETETAPAN
    FROM (
        SELECT ID_SSPD,TGL_SETORAN TGL_SSPD,TGL_SETORAN SSPD_TGL_ENTRY,CASE FK_AYAT_PAJAK
                    WHEN '4.1.1.03.01' THEN '4.1.01.08.01'
                    WHEN '4.1.1.03.21' THEN '4.1.01.08.02'
                    ELSE '4.1.01.08.05'
                END ID_AYAT_PAJAK,FK_NOP,NAMA_OP NAMA_WP,ALAMAT_OP ALAMAT_WP,BULAN_PAJAK BULAN_PAJAK_SSPD,TAHUN_PAJAK TAHUN_PAJAK_SSPD, JML_POKOK,JML_DENDA,REFF_DASAR_SETORAN,NAMA_LOKASI_BAYAR TEMPAT_BAYAR,DASAR_SETORAN SETORAN_BERDASARKAN,SYSDATE REKON_DATE,'JOB' REKON_BY,
                FK_OP,DASAR_SETORAN,NAMA_JENIS_PAJAK                
        FROM VW_SIMPADA_SSPD@LIHATHPPSERVER
        WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND TAHUN_SETOR=:TAHUN AND TO_NUMBER(TAHUN_PAJAK) = :TAHUN
    ) A
                ";
                var pembayaranSspdList = _contMonitoringDb.Set<SSPDPbjt>()
                     .FromSqlRaw(sql2, new[] {
                            new OracleParameter("TAHUN", thn),
                    }).ToList();
                Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] OP (SSPD) (PHR) {thn}");

                var opList = _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == thn).ToList();
                for (int bln = 1; bln <= 12; bln++)
                {
                    Console.WriteLine($"{DateTime.Now} [QUERY] KETETAPAN MONITORING DB {thn}-{bln}");
                    var sql = @"
                             SELECT 	REPLACE(FK_NOP, '.','') NOP,
                               TO_NUMBER(TAHUN_PAJAK) TAHUN,
                               BULAN_PAJAK MASAPAJAK,
                               100 SEQ,
                               100 JENIS_KETETAPAN,
  NVL(TGL_SPTPD_DISETOR, MP_AKHIR) TGL_KETETAPAN,
                               TGL_JATUH_TEMPO TGL_JATUH_TEMPO_BAYAR,
                               0 NILAI_PENGURANG,
                               NVL(KETETAPAN_TOTAL, 0) POKOK
                             FROM (
  	                            SELECT *
                                FROM VW_SIMPADA_SPTPD@LIHATHPPSERVER
                                WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND FK_NOP IS NOT NULL AND TO_NUMBER(TAHUN_PAJAK) = :tahun AND BULAN_PAJAK = :bulan
                             ) A
                        ";

                    var ketetapanSbyTaxOld = await _contMonitoringDb.Set<OPSkpdHiburan>()
                        .FromSqlRaw(sql, new[] {
                                new OracleParameter("tahun", thn),
                                new OracleParameter("bulan", bln)
                        })
                        .ToListAsync();
                    Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] KETETAPAN MONITORING DB {thn}-{bln}");
                    foreach (var op in opList)
                    {
                        bool isOPTutup = false;
                        DateTime tglPenetapan = new DateTime(thn, bln, 1);
                        if (op.TglOpTutup.HasValue)
                        {
                            if (op.TglOpTutup.Value.Date < tglPenetapan.Date)
                            {
                                isOPTutup = true;
                            }
                        }

                        var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)op.KategoriId);
                        foreach (var item in ketetapanSbyTaxOld.Where(x => x.NOP == op.Nop))
                        {
                            string nop = item.NOP;
                            int tahunPajak = item.TAHUN;
                            int masaPajak = item.MASAPAJAK;
                            int seqPajak = item.SEQ;

                            var rowMonHiburan = _contMonPd.DbMonHiburans
                                .SingleOrDefault(x => 
                                x.Nop == nop &&
                                x.TahunPajakKetetapan == tahunPajak &&
                                x.MasaPajakKetetapan == masaPajak &&
                                x.SeqPajakKetetapan == seqPajak
                                );

                            if (rowMonHiburan != null)
                            {
                                _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
                            }

                            var newRow = new DbMonHiburan();
                            newRow.Nop = item.NOP;
                            newRow.Npwpd = op.Npwpd;
                            newRow.NpwpdNama = op.NpwpdNama;
                            newRow.NpwpdAlamat = op.NpwpdAlamat;
                            newRow.PajakId = op.PajakId;
                            newRow.PajakNama = op.PajakNama;
                            newRow.NamaOp = op.NamaOp;
                            newRow.AlamatOp = op.AlamatOp;
                            newRow.AlamatOpKdLurah = op.AlamatOpKdLurah;
                            newRow.AlamatOpKdCamat = op.AlamatOpKdCamat;
                            newRow.TglOpTutup = op.TglOpTutup;
                            newRow.TglMulaiBukaOp = op.TglMulaiBukaOp;
                            newRow.IsTutup = isOPTutup ? 1 : 0;
                            newRow.KategoriId = op.KategoriId;
                            newRow.KategoriNama = op.KategoriNama;
                            newRow.TahunBuku = thn;
                            newRow.Akun = op.Akun;
                            newRow.NamaAkun = op.NamaAkun;
                            newRow.Jenis = op.Jenis;
                            newRow.NamaJenis = op.NamaJenis;
                            newRow.Objek = op.Objek;
                            newRow.NamaObjek = op.NamaObjek;
                            newRow.Rincian = op.Rincian;
                            newRow.NamaRincian = op.NamaRincian;
                            newRow.SubRincian = op.SubRincian;
                            newRow.NamaSubRincian = op.NamaSubRincian;
                            newRow.TahunPajakKetetapan = item.TAHUN;
                            newRow.MasaPajakKetetapan = item.MASAPAJAK;
                            newRow.SeqPajakKetetapan = item.SEQ;
                            newRow.KategoriKetetapan = item.JENIS_KETETAPAN.ToString();
                            newRow.TglKetetapan = item.TGL_KETETAPAN;
                            newRow.TglJatuhTempoBayar = item.TGL_JATUH_TEMPO_BAYAR;
                            newRow.PokokPajakKetetapan = item.POKOK - item.NILAI_PENGURANG;
                            newRow.PengurangPokokKetetapan = item.NILAI_PENGURANG;
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

                            GetRealisasiPhr(ref newRow, pembayaranSspdList);
                            _contMonPd.DbMonHiburans.Add(newRow);
                            _contMonPd.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{DateTime.Now} DB_MON_HIBURAN_MONITORINGDB {thn}-{bln}-{item.NOP}-{item.SEQ}");
                            Console.ResetColor();
                        }
                    }
                }
            }

            MailHelper.SendMail(
            false,
            "DONE HIBURAN WS",
            $@"HIBURAN WS FINISHED",
            null
            );
        }
        private void GetRealisasi(ref DbMonHiburan row)
        {
            //PEMBAYARAN
            var _contBima = DBClass.GetBimaContext();
            var sql = @"SELECT 	ID_SSPD, 
                                      KODE_BILL, 
                                      NO_KETETAPAN, 
                                      JENIS_PEMBAYARAN, 
                                      JENIS_PAJAK, 
                                      JENIS_KETETAPAN, 
                                      JATUH_TEMPO, 
                                      NOP, 
                                      MASA, 
                                      TAHUN, 
                                      NOMINAL_POKOK, 
                                      NOMINAL_SANKSI, 
                                      NOMINAL_ADMINISTRASI, 
                                      NOMINAL_LAINYA, 
                                      PENGURANG_POKOK, 
                                      PENGURANG_SANKSI, 
                                      REFF_PENGURANG_POKOK, 
                                      REFF_PENGURANG_SANKSI, 
                                      AKUN_POKOK, 
                                      AKUN_SANKSI, 
                                      AKUN_ADMINISTRASI, 
                                      AKUN_LAINNYA, 
                                      AKUN_PENGURANG_POKOK, 
                                      AKUN_PENGURANG_SANKSI, 
                                      INVOICE_NUMBER, 
                                      TRANSACTION_DATE, 
                                      NO_NTPD, 
                                      STATUS_NTPD, 
                                      REKON_DATE, 
                                      REKON_BY, 
                                      REKON_REFF, 
                                      SEQ_KETETAPAN, 
                                      INS_DATE	
                                    FROM T_SSPD A
                                    WHERE 	A.JENIS_PAJAK = 5 AND 
                                      A.NOP = :NOP AND
                                      A.TAHUN = :TAHUN AND 
                                      A.MASA = :MASA AND 
                                      A.SEQ_KETETAPAN = :SEQ";

            var pembayaranSspdList = _contBima.Set<SSPD>()
                .FromSqlRaw(sql, new[] {
                                            new OracleParameter("NOP", row.Nop),
                                            new OracleParameter("TAHUN", row.TahunPajakKetetapan),
                                            new OracleParameter("MASA", row.MasaPajakKetetapan),
                                            new OracleParameter("SEQ", row.SeqPajakKetetapan)
                }).ToList();

            if (pembayaranSspdList != null && pembayaranSspdList.Count > 0)
            {
                DateTime tanggalBayarTerakhir = pembayaranSspdList.Max(x => (DateTime)x.TRANSACTION_DATE);
                int maxTahunBayar = pembayaranSspdList.Max(x => ((DateTime)x.TRANSACTION_DATE).Year);
                decimal nominalPokokBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_POKOK);
                decimal nominalSanksiBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_SANKSI);
                decimal nominalAdministrasi = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_ADMINISTRASI);
                decimal nominalLainnya = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_LAINYA);
                decimal pengurangPokok = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_POKOK);
                decimal pengurangSanksi = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_SANKSI);

                string akunBayar = "-";
                string kelompokBayar = "-";
                string jenisBayar = "-";
                string objekBayar = "-";
                string rincianBayar = "-";
                string subrincianBayar = "-";

                var getAkun = GetDbAkun(maxTahunBayar, 6, 56);
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

                var getAkunSanksi = GetDbAkunSanksi(maxTahunBayar, 6, 56);
                if (getAkunSanksi != null)
                {
                    akunSanksi = getAkunSanksi.Akun;
                    kelompokSanksi = getAkunSanksi.Kelompok;
                    jenisSanksi = getAkunSanksi.Jenis;
                    objekSanksi = getAkunSanksi.Objek;
                    rincianSanksi = getAkunSanksi.Rincian;
                    subrincianSanksi = getAkunSanksi.SubRincian;
                }

                if (nominalPokokBayar > 0)
                {
                    DateTime TGL_BAYAR_POKOK = tanggalBayarTerakhir;
                    decimal NOMINAL_POKOK_BAYAR = nominalPokokBayar;
                    string AKUN_POKOK_BAYAR = akunBayar;
                    string KELOMPOK_POKOK_BAYAR = kelompokBayar;
                    string JENIS_POKOK_BAYAR = jenisBayar;
                    string OBJEK_POKOK_BAYAR = objekBayar;
                    string RINCIAN_POKOK_BAYAR = rincianBayar;
                    string SUB_RINCIAN_POKOK_BAYAR = subrincianBayar;

                    row.TglBayarPokok = TGL_BAYAR_POKOK;
                    row.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
                    row.AkunPokokBayar = AKUN_POKOK_BAYAR;
                    row.JenisPokokBayar = JENIS_POKOK_BAYAR;
                    row.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
                    row.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
                    row.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
                }

                if (nominalSanksiBayar > 0 || nominalLainnya > 0 || nominalAdministrasi > 0)
                {
                    DateTime TGL_BAYAR_SANKSI = tanggalBayarTerakhir;
                    decimal NOMINAL_SANKSI_BAYAR = (nominalSanksiBayar + nominalLainnya + nominalAdministrasi);
                    string AKUN_SANKSI_BAYAR = akunSanksi;
                    string KELOMPOK_SANKSI_BAYAR = kelompokSanksi;
                    string JENIS_SANKSI_BAYAR = jenisSanksi;
                    string OBJEK_SANKSI_BAYAR = objekSanksi;
                    string RINCIAN_SANKSI_BAYAR = rincianSanksi;
                    string SUB_RINCIAN_SANKSI_BAYAR = subrincianSanksi;

                    row.TglBayarSanksi = TGL_BAYAR_SANKSI;
                    row.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
                    row.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
                    row.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
                    row.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
                    row.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
                    row.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
                    row.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
                }

                Console.WriteLine($"DB_MON_HIBURAN (SSPD): {row.TahunPajakKetetapan}-{row.MasaPajakKetetapan}-{row.Nop}-{row.SeqPajakKetetapan}-{row.NominalPokokBayar}");
            }
        }
        private void GetRealisasiPhr(ref DbMonHiburan row, List<SSPDPbjt> pembayaranSspdList)
        {
            //PEMBAYARAN PHR
            //var _contPhr = DBClass.GetPhrhContext();
            //Console.WriteLine($"{DateTime.Now} [QUERY] OP (SSPD) (PHR)");
            //var sql = @"
            //    SELECT 	REPLACE(FK_NOP, '.', '') NOP,
	           //         TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,
	           //         BULAN_PAJAK,
            //            TGL_SETORAN TRANSACTION_DATE,
		          //      JML_POKOK NOMINAL_POKOK,
		          //      JML_DENDA NOMINAL_SANKSI,
		          //      0 NOMINAL_ADMINISTRASI,
		          //      0 NOMINAL_LAINNYA,
		          //      0 PENGURANG_POKOK,
		          //      0 PENGURANG_SANSKSI,
		          //      100 SEQ_KETETAPAN
            //    FROM PHRH_USER.VW_SIMPADAHPP_SSPD_PHR A
            //    JOIN PHRH_USER.KODEREKENING_BARU B ON A.FK_AYAT_PAJAK=B.KODE
            //    WHERE NAMA_PAJAK_DAERAH='HIBURAN' 
            //        AND TAHUN_SETOR=:TAHUN
            //        AND  REPLACE(FK_NOP, '.', '') = :NOP AND TO_NUMBER(TAHUN_PAJAK) = :TAHUN AND A.BULAN_PAJAK = :MASA 
            //";

            //var pembayaranSspdList = _contPhr.Set<SSPDPbjt>()
            //     .FromSqlRaw(sql, new[] {
            //            new OracleParameter("NOP", row.Nop),
            //            new OracleParameter("TAHUN", row.TahunPajakKetetapan),
            //            new OracleParameter("MASA", row.MasaPajakKetetapan),
            //    }).ToList();
            //Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] OP (SSPD) (PHR)");

            string nop = row.Nop;
            int thn = Convert.ToInt32(row.TahunPajakKetetapan);
            int bln = Convert.ToInt32(row.MasaPajakKetetapan);
            pembayaranSspdList = pembayaranSspdList.Where(x => x.NOP == nop && x.TAHUN_PAJAK == thn && x.BULAN_PAJAK == bln).ToList();

            if (pembayaranSspdList != null)
            {

                if (pembayaranSspdList.Count > 0)
                {
                    DateTime tanggalBayarTerakhir = pembayaranSspdList.Max(x => (DateTime)x.TRANSACTION_DATE);
                    int maxTahunBayar = pembayaranSspdList.Max(x => ((DateTime)x.TRANSACTION_DATE).Year);
                    decimal nominalPokokBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_POKOK);
                    decimal nominalSanksiBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_SANKSI);
                    decimal nominalAdministrasi = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_ADMINISTRASI);
                    decimal nominalLainnya = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_LAINNYA);
                    decimal pengurangPokok = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_POKOK);
                    decimal pengurangSanksi = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_SANSKSI);

                    string akunBayar = "-";
                    string kelompokBayar = "-";
                    string jenisBayar = "-";
                    string objekBayar = "-";
                    string rincianBayar = "-";
                    string subrincianBayar = "-";

                    var getAkun = GetDbAkun(maxTahunBayar, 6, 56);
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

                    var getAkunSanksi = GetDbAkunSanksi(maxTahunBayar, 6, 56);
                    if (getAkunSanksi != null)
                    {
                        akunSanksi = getAkunSanksi.Akun;
                        kelompokSanksi = getAkunSanksi.Kelompok;
                        jenisSanksi = getAkunSanksi.Jenis;
                        objekSanksi = getAkunSanksi.Objek;
                        rincianSanksi = getAkunSanksi.Rincian;
                        subrincianSanksi = getAkunSanksi.SubRincian;
                    }

                    if (nominalPokokBayar > 0)
                    {
                        DateTime TGL_BAYAR_POKOK = tanggalBayarTerakhir;
                        decimal NOMINAL_POKOK_BAYAR = nominalPokokBayar;
                        string AKUN_POKOK_BAYAR = akunBayar;
                        string KELOMPOK_POKOK_BAYAR = kelompokBayar;
                        string JENIS_POKOK_BAYAR = jenisBayar;
                        string OBJEK_POKOK_BAYAR = objekBayar;
                        string RINCIAN_POKOK_BAYAR = rincianBayar;
                        string SUB_RINCIAN_POKOK_BAYAR = subrincianBayar;

                        row.TglBayarPokok = TGL_BAYAR_POKOK;
                        row.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
                        row.AkunPokokBayar = AKUN_POKOK_BAYAR;
                        row.JenisPokokBayar = JENIS_POKOK_BAYAR;
                        row.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
                        row.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
                        row.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
                    }

                    if (nominalSanksiBayar > 0 || nominalLainnya > 0 || nominalAdministrasi > 0)
                    {
                        DateTime TGL_BAYAR_SANKSI = tanggalBayarTerakhir;
                        decimal NOMINAL_SANKSI_BAYAR = (nominalSanksiBayar + nominalLainnya + nominalAdministrasi);
                        string AKUN_SANKSI_BAYAR = akunSanksi;
                        string KELOMPOK_SANKSI_BAYAR = kelompokSanksi;
                        string JENIS_SANKSI_BAYAR = jenisSanksi;
                        string OBJEK_SANKSI_BAYAR = objekSanksi;
                        string RINCIAN_SANKSI_BAYAR = rincianSanksi;
                        string SUB_RINCIAN_SANKSI_BAYAR = subrincianSanksi;

                        row.TglBayarSanksi = TGL_BAYAR_SANKSI;
                        row.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
                        row.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
                        row.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
                        row.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
                        row.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
                        row.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
                        row.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} [SAVED] DB_MON_HIBURAN (SSPD) (PHR): {row.TahunPajakKetetapan}-{row.MasaPajakKetetapan}-{row.Nop}-{row.SeqPajakKetetapan}");
                    Console.ResetColor();
                }
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

        public static List<string> GetInfoWPHPP(string npwpd)
        {
            var ret = new List<string>();
            var c =  DBClass.GetMonitoringDbContext();
            var connection = c.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @" SELECT  NAMAWP,ALAMAT
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
