using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MonPDLib.Helper;

namespace AbtWs
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
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                //var now = DateTime.Now;

                //// Hitung waktu untuk 00:00 esok hari
                //var nextRunTime = now.Date.AddHours(1); // Tambah 1 hari dan set jam 00:00
                //var delay = nextRunTime - now;

                //_logger.LogInformation("Next run scheduled at: {time}", nextRunTime);
                //_logger.LogInformation("Next run scheduled : {lama}", delay.Hours + ":" + delay.Minutes);

                //// Tunggu hingga waktu eksekusi
                //await Task.Delay(delay, stoppingToken);

                //if (stoppingToken.IsCancellationRequested)
                //    break;

                //// GUNAKAN KETIKA EKSEKUSI TUGAS MANUAL
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
            int idPajak = 6;
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == 6);
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

            // do fill db op abt
            if (IsGetDBOp())
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
                                D.NAMA KATEGORI_NAMA,
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
                    ";

                    var result = await _contSbyTax.Set<DbOpAbt>().FromSqlRaw(sql).ToListAsync(); //822
                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpAbts.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            if (item.TglMulaiBukaOp.Year <= i)
                            {
                                
                                var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                if (sourceRow != null)
                                {

                                    sourceRow.TglOpTutup = item.TglOpTutup;
                                    sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                    var dbakun = GetDbAkun(i, 6, (int)item.KategoriId);
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

                                    newRow.TahunBuku = i;
                                    var dbakun = GetDbAkun(i, 6, (int)item.KategoriId);
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
                                }
                                Console.WriteLine($"DB_OP_ABT: {i} - {item.Nop}");
                                _contMonPd.SaveChanges();
                            }
                        }
                    }
                }
            }


            // do fill ketetapan
            var _contSbyTaxOld = DBClass.GetSurabayaTaxContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                var opList = _contMonPd.DbOpAbts.Where(x => x.TahunBuku == thn).ToList();
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

                        var sql = @"select   A.NOP, a.TAHUN, a.MASAPAJAK, a.SEQ, JENIS_KETETAPAN, NPWPD, AKUN, AKUN_JENIS, AKUN_JENIS_OBJEK, AKUN_JENIS_OBJEK_RINCIAN, 
        AKUN_JENIS_OBJEK_RINCIAN_SUB, 
        TGL_KETETAPAN, POKOK, SANKSI_TERLAMBAT_LAPOR, SANKSI_ADMINISTRASI, PROSEN_TARIF_PAJAK, PROSEN_SANKSI_TELAT_BAYAR, TGL_JATUH_TEMPO_BAYAR, 
        TGL_JATUH_TEMPO_LAPOR, JATUH_TEMPO_LAPOR_MODE, JATUH_TEMPO_BAYAR, JATUH_TEMPO_BAYAR_MODE, KELOMPOK_ID, KELOMPOK_NAMA, VOL_PENGGUNAAN_AIR, 
        STATUS_BATAL, BATAL_KET, BATAL_DATE, BATAL_BY, BATAL_REF, INS_DATE, INS_BY, PERUNTUKAN, NILAI_PENGURANG, JENIS_PENGURANG, REFF_PENGURANG, NVL(B.NO_KETETAPAN, '-') NO_KETETAPAN
from objek_pajak_skpd_abt a
LEFT JOIN (
	SELECT nop, tahun, masapajak, seq, (A.SURAT_KLASIFIKASI || '/' || A.SURAT_PAJAK || A.SURAT_DOKUMEN || A.SURAT_BIDANG || A.SURAT_AGENDA || '/' || A.SURAT_OPD || '/' || A.SURAT_TAHUN) no_Ketetapan
	from OBJEK_PAJAK_SKPD_ABT_PNTPN a
) b ON a.nop = b.nop and a.tahun = b.tahun AND a.MASAPAJAK = b.MASAPAJAK AND a.SEQ = b.SEQ
WHERE a.NOP=:nop AND a.TAHUN=:tahun AND a.MASAPAJAK=:bulan AND a.STATUS_BATAL=0";

                        var ketetapanSbyTaxOld = await _contSbyTaxOld.Set<OPSkpdAbt>()
                            .FromSqlRaw(sql, new[] {
                                new OracleParameter("nop", op.Nop),
                                new OracleParameter("tahun", thn),
                                new OracleParameter("bulan", bln)
                            }).ToListAsync();
                        var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)op.KategoriId);
                        foreach (var item in ketetapanSbyTaxOld)
                        {
                            string nop = item.NOP;
                            int tahunPajak = item.TAHUN;
                            int masaPajak = item.MASAPAJAK;
                            int seqPajak = item.SEQ;
                            var rowMonAbt = _contMonPd.DbMonAbts.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            if (rowMonAbt != null)
                            {
                                _contMonPd.DbMonAbts.Remove(rowMonAbt);
                            }
                            _contMonPd.DbMonAbts.Add(new DbMonAbt()
                            {
                                Nop = item.NOP,
                                Npwpd = op.Npwpd,
                                NpwpdNama = op.NpwpdNama,
                                NpwpdAlamat = op.NpwpdAlamat,
                                PajakId = op.PajakId,
                                PajakNama = op.PajakNama,
                                NamaOp = op.NamaOp,
                                AlamatOp = op.AlamatOp,
                                AlamatOpKdLurah = op.AlamatOpKdLurah,
                                AlamatOpKdCamat = op.AlamatOpKdCamat,
                                TglOpTutup = op.TglOpTutup,
                                TglMulaiBukaOp = op.TglMulaiBukaOp,
                                IsTutup = isOPTutup ? 1 : 0,
                                PeruntukanId = item.PERUNTUKAN,
                                PeruntukanNama = item.PERUNTUKAN == 1 ? "NIAGA" : item.PERUNTUKAN == 2 ? "NON NIAGA" : item.PERUNTUKAN == 3 ? "BAHAN BAKU AIR" : "",
                                KategoriId = op.KategoriId,
                                KategoriNama = op.KategoriNama,
                                TahunBuku = thn,
                                Akun = op.Akun,
                                NamaAkun = op.NamaAkun,
                                Kelompok = op.Kelompok,
                                KelompokNama = op.NamaKelompok,
                                Jenis = op.Jenis,
                                NamaJenis = op.NamaJenis,
                                Objek = op.Objek,
                                NamaObjek = op.NamaObjek,
                                Rincian = op.Rincian,
                                NamaRincian = op.NamaRincian,
                                SubRincian = op.SubRincian,
                                NamaSubRincian = op.NamaSubRincian,
                                TahunPajakKetetapan = item.TAHUN,
                                MasaPajakKetetapan = item.MASAPAJAK,
                                SeqPajakKetetapan = item.SEQ,
                                KategoriKetetapan = item.JENIS_KETETAPAN.ToString(),
                                TglKetetapan = item.TGL_KETETAPAN,
                                TglJatuhTempoBayar = item.TGL_JATUH_TEMPO_BAYAR,
                                PokokPajakKetetapan = item.POKOK - item.NILAI_PENGURANG,
                                PengurangPokokKetetapan = item.NILAI_PENGURANG,
                                AkunKetetapan = dbAkunPokok.Akun,
                                KelompokKetetapan = dbAkunPokok.Kelompok,
                                JenisKetetapan = dbAkunPokok.Jenis,
                                ObjekKetetapan = dbAkunPokok.Objek,
                                RincianKetetapan = dbAkunPokok.Rincian,
                                SubRincianKetetapan = dbAkunPokok.SubRincian,
                                InsDate = DateTime.Now,
                                InsBy = "JOB",
                                UpdDate = DateTime.Now,
                                UpdBy = "JOB",
                                NoKetetapan = item.NO_KETETAPAN
                            });

                            Console.WriteLine($"DB_MON_ABT {thn}-{bln}-{item.NOP}-{item.SEQ}");
                            _contMonPd.SaveChanges();
                        }
                    }
                }
            }

            // do fill realisasi
            var _contBima = DBClass.GetBimaContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {                
                for (int bln = 1; bln <= 12; bln++)
                {
                    var opList = _contMonPd.DbMonAbts.Where(x => x.TahunPajakKetetapan == thn && x.MasaPajakKetetapan==bln).ToList();

                    foreach (var op in opList)
                    {
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
                                    WHERE 	A.JENIS_PAJAK = 6 AND 
		                                    A.NOP = :NOP AND
		                                    A.TAHUN = :TAHUN AND 
		                                    A.MASA = :MASA AND 
		                                    A.SEQ_KETETAPAN = :SEQ";

                        var pembayaranSspdList = await _contBima.Set<SSPD>()
                            .FromSqlRaw(sql, new[] {
                                            new OracleParameter("NOP", op.Nop),
                                            new OracleParameter("TAHUN", thn),
                                            new OracleParameter("MASA", bln),
                                            new OracleParameter("SEQ", op.SeqPajakKetetapan)
                            }).ToListAsync();

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
                            if(getAkun != null)
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
                            if(getAkunSanksi != null)
                            {
                                akunSanksi = getAkunSanksi.Akun;
                                kelompokSanksi = getAkunSanksi.Kelompok;
                                jenisSanksi = getAkunSanksi.Jenis;
                                objekSanksi = getAkunSanksi.Objek;
                                rincianSanksi = getAkunSanksi.Rincian;
                                subrincianSanksi = getAkunSanksi.SubRincian;
                            }

                            if(nominalPokokBayar > 0)
                            {
                                DateTime TGL_BAYAR_POKOK = tanggalBayarTerakhir;
                                decimal NOMINAL_POKOK_BAYAR = nominalPokokBayar;
                                string AKUN_POKOK_BAYAR = akunBayar;
                                string KELOMPOK_POKOK_BAYAR = kelompokBayar;
                                string JENIS_POKOK_BAYAR = jenisBayar;
                                string OBJEK_POKOK_BAYAR = objekBayar;
                                string RINCIAN_POKOK_BAYAR = rincianBayar;
                                string SUB_RINCIAN_POKOK_BAYAR = subrincianBayar;

                                op.TglBayarPokok = TGL_BAYAR_POKOK;
                                op.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
                                op.AkunPokokBayar = AKUN_POKOK_BAYAR;
                                op.JenisPokokBayar = JENIS_POKOK_BAYAR;
                                op.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
                                op.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
                                op.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
                            }

                            if(nominalSanksiBayar > 0 || nominalLainnya > 0 || nominalAdministrasi > 0)
                            {
                                DateTime TGL_BAYAR_SANKSI = tanggalBayarTerakhir;
                                decimal NOMINAL_SANKSI_BAYAR = (nominalSanksiBayar + nominalLainnya + nominalAdministrasi);
                                string AKUN_SANKSI_BAYAR = akunSanksi;
                                string KELOMPOK_SANKSI_BAYAR = kelompokSanksi;
                                string JENIS_SANKSI_BAYAR = jenisSanksi;
                                string OBJEK_SANKSI_BAYAR = objekSanksi;
                                string RINCIAN_SANKSI_BAYAR = rincianSanksi;
                                string SUB_RINCIAN_SANKSI_BAYAR = subrincianSanksi;

                                op.TglBayarSanksi = TGL_BAYAR_SANKSI;
                                op.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
                                op.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
                                op.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
                                op.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
                                op.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
                                op.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
                                op.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
                            }
                            Console.WriteLine($"DB_MON_ABT (SSPD): {thn}-{bln}-{op.Nop}-{op.SeqPajakKetetapan}");
                            _contMonPd.SaveChanges();
                        }
                    }
                }
            }

            MailHelper.SendMail(
            false,
            "DONE ABT WS",
            $@"ABT WS FINISHED",
            null
            );
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
