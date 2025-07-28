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

            // do fill db op ABT
            if (IsGetDBOp())
            {
                FillOP(2025);
                //for (var i = tahunAmbil; i <= tglServer.Year; i++)
                //{
                //    FillOP(i);
                //}
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
                                B.PERUNTUKAN PERUNTUKAN_ID,
                                case
                                when B.peruntukan=1 then 'NIAGA'
                                when B.peruntukan=2 then 'NON NIAGA'
                                when B.peruntukan=3 then 'BAHAN BAKU AIR' 
                                END PERUNTUKAN_NAMA,
                                56 KATEGORI_ID,
                                'AIR TANAH' KATEGORI_NAMA,
                                1 IS_METERAN_AIR, 0 JUMLAH_KARYAWAN,
                                DECODE(TGL_OP_TUTUP,NULL,0,1) IS_TUTUP,
                                'SURABAYA ' || UPTB_ID AS WILAYAH_PAJAK,
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
                        JOIN OBJEK_PAJAK_ABT B ON A.NOP=B.NOP
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

                                    ketetapan.NominalPokokBayar = (ketetapan.NominalPokokBayar ?? 0) + itemSSPD.NOMINAL_POKOK;
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

                                    ketetapan.NominalSanksiBayar = (ketetapan.NominalSanksiBayar ?? 0) + itemSSPD.NOMINAL_SANKSI;
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

                                    ketetapan.NominalSanksiBayar = (ketetapan.NominalSanksiKenaikanBayar ?? 0) + itemSSPD.NOMINAL_ADMINISTRASI;
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
