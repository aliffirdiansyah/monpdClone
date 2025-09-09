using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace AbtWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 6;
        private static EnumFactory.EPajak PAJAK_ENUM = EnumFactory.EPajak.AirTanah;

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
        private async Task DoWorkNewMeta(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

            // do fill db op ABT
            if (IsGetDBOp())
            {
                for (var i = tahunAmbil; i <= tglServer.Year; i++)
                {
                    FillOP(i);
                }
            }

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                UpdateKoreksi(i);
            }

            MailHelper.SendMail(
            false,
            "DONE ABT  WS",
            $@"ABT WS FINISHED",
            null
            );
        }
        private void FillOP(int tahunBuku)
        {
            Console.WriteLine("");

            // SURABAYA TAX PROCESS
            SBYTaxProcess(tahunBuku);

            Console.WriteLine(" ");
        }
        private void SBYTaxProcess(int tahunBuku)
        {
            using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
            {
                var sql = @"
                                        SELECT  A.NOP,
                                C.NPWPD_NO NPWPD,
                                C.NAMA NPWPD_NAMA,
                                C.ALAMAT NPWPD_ALAMAT,
                                6 PAJAK_ID,
                                'PAJAK AIR TANAH' PAJAK_NAMA,
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
                                NVL(B.PERUNTUKAN, 1) PERUNTUKAN_ID,
                                case
                                when B.peruntukan=1 then 'NIAGA'
                                when B.peruntukan=2 then 'NON NIAGA'
                                when B.peruntukan=3 then 'BAHAN BAKU AIR' 
                                else 'NIAGA'
                                END PERUNTUKAN_NAMA,
                                56 KATEGORI_ID,
                                'AIR TANAH' KATEGORI_NAMA,
                                1 IS_METERAN_AIR, 0 JUMLAH_KARYAWAN,
                                DECODE(TGL_OP_TUTUP,NULL,0,1) IS_TUTUP,
                                CASE
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 1' THEN '1'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 2' THEN '2'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 3' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 4' THEN '4'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 5' THEN '5'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 6' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 7' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = 'SURABAYA 8' THEN '2'
                                    WHEN TO_CHAR(UPTB_ID) = '01' THEN '1'
                                    WHEN TO_CHAR(UPTB_ID) = '02' THEN '2'
                                    WHEN TO_CHAR(UPTB_ID) = '03' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '04' THEN '4'
                                    WHEN TO_CHAR(UPTB_ID) = '05' THEN '5'
                                    WHEN TO_CHAR(UPTB_ID) = '07' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '06' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '08' THEN '2'
                                    WHEN TO_CHAR(UPTB_ID) = '1' THEN '1'
                                    WHEN TO_CHAR(UPTB_ID) = '2' THEN '2'
                                    WHEN TO_CHAR(UPTB_ID) = '3' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '4' THEN '4'
                                    WHEN TO_CHAR(UPTB_ID) = '5' THEN '5'
                                    WHEN TO_CHAR(UPTB_ID) = '7' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '6' THEN '3'
                                    WHEN TO_CHAR(UPTB_ID) = '8' THEN '2'
                                    ELSE NULL
                                END AS WILAYAH_PAJAK,
                                sysdate INS_dATE, 'JOB' INS_BY,
                                TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')) TAHUN_BUKU,
                                '-'  AKUN  ,
                                '-'  NAMA_AKUN           ,
                                '-'  JENIS             ,
                                '-'  NAMA_JENIS        ,
                                '-'  OBJEK            ,
                                '-'  NAMA_OBJEK       ,
                                '-'  RINCIAN         ,
                                '-'  NAMA_RINCIAN     ,
                                '-'  SUB_RINCIAN      ,
                                '-'  NAMA_SUB_RINCIAN ,
                                '-'  KELOMPOK      ,
                                '-'  NAMA_KELOMPOK        
                        FROM OBJEK_PAJAK A
                        LEFT JOIN OBJEK_PAJAK_ABT B ON A.NOP=B.NOP
                        JOIN NPWPD  C ON A.NPWPD=C.NPWPD_no
                        JOIN M_KATEGORI_PAJAK D ON D.ID=A.KATEGORI
                        LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT
WHERE A.NPWPD NOT IN (
    select npwpd_no  
    from npwpd 
    WHERE REF_THN_PEL = 2023 OR NAMA LIKE '%FULAN%' OR NPWPD_NO = '3578200503840003'
) and  TGL_OP_TUTUP IS  NULL OR ( to_char(tgl_mulai_buka_op,'YYYY') <=:TAHUN AND to_char(TGL_OP_TUTUP,'YYYY') >= :TAHUN)
                                ";

                var result = _contSbyTax.Set<DbOpAbt>().FromSqlRaw(sql, new[] {
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
                        var sourceRow = _contMonPd.DbOpAbts.SingleOrDefault(x => x.Nop == item.Nop && x.TahunBuku == tahunBuku);
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
                            var newRow = new MonPDLib.EF.DbOpAbt();
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
                            newRow.PeruntukanId = item.PeruntukanId;
                            newRow.PeruntukanNama = item.PeruntukanNama;
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.IsMeteranAir = item.IsMeteranAir;
                            newRow.JumlahKaryawan = item.JumlahKaryawan;
                            newRow.IsTutup = item.IsTutup;
                            newRow.InsDate = item.InsDate;
                            newRow.InsBy = item.InsBy;
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
                            _contMonPd.DbOpAbts.Add(newRow);
                            _contMonPd.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing NOP {ex.Message}");
                    }

                    // ketetapan sbytax
                    try
                    {
                        var _contSbyTaxOld = DBClass.GetSurabayaTaxContext();
                        var sqlKetetapan = @"select   A.NOP, a.TAHUN, a.MASAPAJAK, a.SEQ, JENIS_KETETAPAN, NPWPD, AKUN, AKUN_JENIS, AKUN_JENIS_OBJEK, AKUN_JENIS_OBJEK_RINCIAN, 
        AKUN_JENIS_OBJEK_RINCIAN_SUB, 
        TGL_KETETAPAN, POKOK, SANKSI_TERLAMBAT_LAPOR, SANKSI_ADMINISTRASI, PROSEN_TARIF_PAJAK, PROSEN_SANKSI_TELAT_BAYAR, TGL_JATUH_TEMPO_BAYAR, 
        TGL_JATUH_TEMPO_LAPOR, JATUH_TEMPO_LAPOR_MODE, JATUH_TEMPO_BAYAR, JATUH_TEMPO_BAYAR_MODE, KELOMPOK_ID, KELOMPOK_NAMA, VOL_PENGGUNAAN_AIR, 
        STATUS_BATAL, BATAL_KET, BATAL_DATE, BATAL_BY, BATAL_REF, INS_DATE, INS_BY, PERUNTUKAN, NILAI_PENGURANG, JENIS_PENGURANG, REFF_PENGURANG, NVL(B.NO_KETETAPAN, '-') NO_KETETAPAN
from objek_pajak_skpd_abt a
LEFT JOIN (
    SELECT nop, tahun, masapajak, seq, (A.SURAT_KLASIFIKASI || '/' || A.SURAT_PAJAK || A.SURAT_DOKUMEN || A.SURAT_BIDANG || A.SURAT_AGENDA || '/' || A.SURAT_OPD || '/' || A.SURAT_TAHUN) no_Ketetapan
    from OBJEK_PAJAK_SKPD_ABT_PNTPN a
) b ON a.nop = b.nop and a.tahun = b.tahun AND a.MASAPAJAK = b.MASAPAJAK AND a.SEQ = b.SEQ
WHERE a.NOP=:NOP AND a.STATUS_BATAL=0 AND TO_CHAR(TGL_KETETAPAN,'YYYY')=:TAHUN";

                        var ketetapanSbyTaxOld = _contSbyTaxOld.Set<OPSkpdAbt>()
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
                            var rowMonHAbt = _contMonPd.DbMonAbts.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            bool isOPTutup = false;
                            if (item.TglOpTutup.HasValue)
                            {
                                if (item.TglOpTutup.Value.Date.Year <= tahunBuku)
                                {
                                    isOPTutup = true;
                                }

                            }


                            if (rowMonHAbt != null)
                            {
                                _contMonPd.DbMonAbts.Remove(rowMonHAbt);
                            }

                            var newRow = new DbMonAbt();
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
                            newRow.PeruntukanId = itemKetetapan.PERUNTUKAN;
                            newRow.PeruntukanNama = itemKetetapan.PERUNTUKAN == 1 ? "NIAGA" : itemKetetapan.PERUNTUKAN == 2 ? "NON NIAGA" : itemKetetapan.PERUNTUKAN == 3 ? "BAHAN BAKU AIR" : "";
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.TahunBuku = tahunBuku;
                            newRow.Akun = item.Akun;
                            newRow.NamaAkun = item.NamaAkun;
                            newRow.Kelompok = item.Kelompok;
                            newRow.KelompokNama = item.NamaKelompok;
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
                            newRow.NoKetetapan = itemKetetapan.NO_KETETAPAN;
                            newRow.KelompokAbtNama = itemKetetapan.KELOMPOK_NAMA;
                            newRow.VolPenggunaanAir = itemKetetapan.VOL_PENGGUNAAN_AIR;
                            _contMonPd.DbMonAbts.Add(newRow);
                            _contMonPd.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing NOP {ex.Message}");
                    }

                    //                    // realisasi
                    try
                    {
                        var sqlRealisasi = @"SELECT     ID_SSPD,KODE_BILL, NO_KETETAPAN, JENIS_PEMBAYARAN, JENIS_PAJAK, JENIS_KETETAPAN, 
                                            JATUH_TEMPO, NOP, MASA, TAHUN, NOMINAL_POKOK, NOMINAL_SANKSI, NOMINAL_ADMINISTRASI, NOMINAL_LAINYA, PENGURANG_POKOK, 
                                PENGURANG_SANKSI, REFF_PENGURANG_POKOK, REFF_PENGURANG_SANKSI, AKUN_POKOK, AKUN_SANKSI, AKUN_ADMINISTRASI, 
                                AKUN_LAINNYA, AKUN_PENGURANG_POKOK, AKUN_PENGURANG_SANKSI, INVOICE_NUMBER, TRANSACTION_DATE, NO_NTPD, 
                                STATUS_NTPD, REKON_DATE, REKON_BY, REKON_REFF, SEQ_KETETAPAN, INS_DATE    
                    FROM T_SSPD A            
                    WHERE     A.JENIS_PAJAK = 6 AND A.NOP = :NOP AND TO_CHAR(TRANSACTION_DATE,'YYYY')=:TAHUN ";

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
                                var ketetapan = _contMonPd.DbMonAbts.SingleOrDefault(x => x.Nop == itemSSPD.NOP &&
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

                                    ketetapan.NominalPokokBayar = itemSSPD.NOMINAL_POKOK;
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

                                    ketetapan.NominalSanksiBayar = itemSSPD.NOMINAL_SANKSI;
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

                                    ketetapan.NominalSanksiBayar = itemSSPD.NOMINAL_ADMINISTRASI;
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
                        Console.WriteLine($"Error processing NOP {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_OP_ABT SBYTAX TAHUN {tahunBuku} JML OP {jmlData} : {item.Nop}  {persen:F2}%   ");
                    Thread.Sleep(50);
                    _contMonPd.SaveChanges();

                }
            }
        }
        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPABT.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPABT.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
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
                    int kategoriId = 56;
                    string kategoriNama = "AIR TANAH";
                    var tanggal = DateTime.Now.Date;
                    if (tahunBuku < DateTime.Now.Year)
                    {
                        tanggal = new DateTime(tahunBuku, 12, 31);
                    }

                    var source = context.DbOpAbts.FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahunBuku);
                    if (source != null)
                    {
                        source.NamaOp = namaop;
                        source.TahunBuku = tahunBuku;

                        context.DbOpAbts.Update(source);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbOpAbt();

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
                        newRow.JumlahKaryawan = 0;
                        newRow.PeruntukanId = kategoriId;
                        newRow.PeruntukanNama = kategoriNama;
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


                        context.DbOpAbts.Add(newRow);
                        context.SaveChanges();
                    }

                    source = context.DbOpAbts.FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahunBuku);
                    if (source == null)
                    {
                        throw new Exception("Gagal membuat data OP untuk koreksi scontro");
                    }

                    var sourceMon = context.DbMonAbts.Where(x => x.Nop == nop && x.TahunBuku == tahunBuku).FirstOrDefault();
                    if (sourceMon != null)
                    {
                        sourceMon.NominalPokokBayar = selisih;
                        context.DbMonAbts.Update(sourceMon);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbMonAbt();

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
                        newRow.PeruntukanId = source.PeruntukanId;
                        newRow.PeruntukanNama = source.PeruntukanNama;
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

                        context.DbMonAbts.Add(newRow);
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
        private Helper.DbAkun GetDbAkunPokok(int tahun, int idPajak, string nop)
        {
            var _contMonPd = DBClass.GetContext();
            var n = _contMonPd.DbOpAbts.Single(x => x.Nop == nop.Replace(".", ""));
            var query = _contMonPd.DbAkuns.Include(x => x.Kategoris).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.Kategoris.Any(y => y.PajakId == idPajak && y.Id == n.KategoriId));
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
        private Helper.DbAkun GetDbAkunSanksi(int tahun, int idPajak, string nop)
        {
            var _contMonPd = DBClass.GetContext();
            var n = _contMonPd.DbOpAbts.Single(x => x.Nop == nop.Replace(".", ""));
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriSanksis).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriSanksis.Any(y => y.PajakId == idPajak && y.Id == n.KategoriId));
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
        private Helper.DbAkun GetDbAkunKenaikan(int tahun, int idPajak, string nop)
        {
            var _contMonPd = DBClass.GetContext();
            var n = _contMonPd.DbOpAbts.Single(x => x.Nop == nop.Replace(".", ""));
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriKenaikans).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriKenaikans.Any(y => y.PajakId == idPajak && y.Id == n.KategoriId));
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
