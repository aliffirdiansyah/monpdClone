using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace PPJWs
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
                    "ERROR PPJ WS",
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
            int idPajak = 2;
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
                //GET DB OP LISTRIK SBYTAX
                using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
                {
                    var sql = @"
                                            SELECT  A.NOP,
        C.NPWPD_NO NPWPD,
        C.NAMA NPWPD_NAMA,
        C.ALAMAT NPWPD_ALAMAT,
        A.PAJAK_ID ,
        'PBJT ATAS TENAGA LISTRIK' PAJAK_NAMA,
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
        B.SUMBER SUMBER,
        CASE SUMBER
        	WHEN 0 THEN 'SENDIRI'
        	ELSE 'SUMBER LAIN'
        END AS SUMBER_NAMA,
        B.PERUNTUKAN PERUNTUKAN,
        CASE PERUNTUKAN
        	WHEN 0 THEN 'UMUM'
        	WHEN 1 THEN 'INDUSTRI'
        	WHEN 2 THEN 'PERTAMBANGAN BUMI GAS ALAM'
        	ELSE '-'
        END AS PERUNTUKAN_NAMA,
        CASE SUMBER
        	WHEN 0 THEN 55
        	ELSE 58
        END AS KATEGORI_ID,
        CASE SUMBER
        	WHEN 0 THEN 'SENDIRI'
        	ELSE 'SUMBER LAIN'
        END AS KATEGORI_NAMA,
        0 JUMLAH_KARYAWAN,
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
JOIN OBJEK_PAJAK_LISTRIK B ON A.NOP = B.NOP
JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT     
WHERE A.NPWPD NOT IN (
	select npwpd_no  
	from npwpd 
	WHERE REF_THN_PEL = 2023 OR NAMA LIKE '%FULAN%' OR NPWPD_NO = '3578200503840003'
)
                    ";

                    var result = await _contSbyTax.Set<DbOpListrik>().FromSqlRaw(sql).ToListAsync();
                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpListriks.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            if (item.TglMulaiBukaOp.Year <= i)
                            {
                                var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                if (sourceRow != null)
                                {

                                    sourceRow.TglOpTutup = item.TglOpTutup;
                                    sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                    var dbakun = GetDbAkun(i, idPajak, (int)12);
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
                                    var newRow = new MonPDLib.EF.DbOpListrik();
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
                                    newRow.JumlahKaryawan = item.JumlahKaryawan;
                                    newRow.InsDate = item.InsDate;
                                    newRow.InsBy = item.InsBy;
                                    newRow.IsTutup = item.IsTutup;
                                    newRow.WilayahPajak = item.WilayahPajak;
                                    newRow.Sumber = item.Sumber;
                                    newRow.SumberNama = item.SumberNama;
                                    newRow.Peruntukan = item.Peruntukan;
                                    newRow.PeruntukanNama = item.PeruntukanNama;
                                    newRow.JumlahKaryawan = item.JumlahKaryawan;
                                    newRow.TahunBuku = i;
                                    var dbakun = GetDbAkun(i, idPajak, (int)12);
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
                                    _contMonPd.DbOpListriks.Add(newRow);
                                }

                                _contMonPd.SaveChanges();
                                Console.WriteLine($"{DateTime.Now} DB_OP {i} {item.Nop}");
                            }
                        }
                    }
                }

                using (var _contHpp = DBClass.GetHppContext())
                {
                    var sql = @"
                        	                       		                        	                       	                        	                       		                        	                       	select 	REPLACE(FK_NOP, '.', '') NOP,  
        NVL(FK_NPWPD, '-') NPWPD,
        NAMA_OP NPWPD_NAMA,
        NVL(ALAMAT_OP, '-') NPWPD_ALAMAT,
        '-' ALAMAT_OP_NO,
        '-' ALAMAT_OP_RT,
        '-' ALAMAT_OP_RW,
        NVL(NOMOR_TELEPON, '-') TELP,
        NAMA_OP,
        5 PAJAK_ID,
        'Pajak Jasa Kesenian Listrik' PAJAK_NAMA,
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
    NAMA_AYAT_PAJAK,
    CASE NAMA_JENIS_PAJAK
		WHEN 'PPJ NON PLN' THEN 55
		WHEN 'PPJ PLN' THEN 58
		ELSE NULL
	END AS SUMBER,
	CASE NAMA_JENIS_PAJAK
		WHEN 'PPJ NON PLN' THEN 'SENDIRI'
		WHEN 'PPJ PLN' THEN 'SUMBER LAIN'
		ELSE NULL
	END AS SUMBER_NAMA,
    CASE NAMA_JENIS_PAJAK
		WHEN 'PPJ NON PLN' THEN 55
		WHEN 'PPJ PLN' THEN 58
		ELSE NULL
	END AS KATEGORI_ID,
    CASE NAMA_JENIS_PAJAK
		WHEN 'PPJ NON PLN' THEN 'SENDIRI'
		WHEN 'PPJ PLN' THEN 'SUMBER LAIN'
		ELSE NULL
	END AS KATEGORI_NAMA,
	    0 PERUNTUKAN,
	    '-' PERUNTUKAN_NAMA,
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
FROM VW_SIMPADA_OP_all_mon
WHERE 	NAMA_PAJAK_DAERAH ='PPJ' 
		AND KATEGORI_PAJAK <> 'OBJEK TESTING'
		AND FK_NOP IS NOT NULL
		AND REPLACE(FK_NOP, '.', '') LIKE '3578%'
                    ";

                    var result = await _contHpp.Set<DbOpListrik>().FromSqlRaw(sql).ToListAsync();

                    var distinctNop = result.Select(x => x.Nop).ToList();
                    var dataExisting = _contMonPd.DbOpListriks.Where(x => distinctNop.Contains(x.Nop)).ToList();


                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpListriks.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            var isExist = dataExisting.Where(x => x.Nop == item.Nop).Any();
                            if (!isExist)
                            {
                                if (item.TglMulaiBukaOp.Year <= i)
                                {
                                    var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                    if (sourceRow != null)
                                    {
                                        sourceRow.TglOpTutup = item.TglOpTutup;
                                        sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                        var dbakun = GetDbAkun(i, idPajak, (int)12);
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
                                        var newRow = new MonPDLib.EF.DbOpListrik();
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
                                        newRow.Sumber = item.Sumber;
                                        newRow.SumberNama = item.SumberNama;
                                        newRow.Peruntukan = item.Peruntukan;
                                        newRow.PeruntukanNama = item.PeruntukanNama;
                                        newRow.JumlahKaryawan = item.JumlahKaryawan;
                                        newRow.InsDate = item.InsDate;
                                        newRow.InsBy = item.InsBy;
                                        newRow.IsTutup = item.IsTutup;
                                        newRow.WilayahPajak = item.WilayahPajak;

                                        newRow.TahunBuku = i;
                                        var dbakun = GetDbAkun(i, idPajak, (int)12);
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
                                        _contMonPd.DbOpListriks.Add(newRow);
                                    }

                                    _contMonPd.SaveChanges();
                                    Console.WriteLine($"{DateTime.Now} DB_OP_HPP {tahunAmbil} {item.Nop}");
                                }
                            }

                        }
                    }
                }
            }

            //////FILL KETETAPAN 
            var _contSbyTaxOld = DBClass.GetSurabayaTaxContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                var opList = _contMonPd.DbOpListriks.Where(x => x.TahunBuku == thn).ToList();
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
                        Console.WriteLine($"{DateTime.Now} [QUERY] KETETAPAN SBYTAX LISTRIK {thn}-{bln}-{op.Nop}");
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
                               WHERE PAJAK_ID = 2
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
                               WHERE PAJAK_ID = 2
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

                        var ketetapanSbyTaxOld = await _contSbyTaxOld.Set<OPSkpdListrik>()
                            .FromSqlRaw(sql, new[] {
                                new OracleParameter("nop", op.Nop),
                                new OracleParameter("tahun", thn),
                                new OracleParameter("bulan", bln)
                            }).ToListAsync();
                        Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] KETETAPAN SBYTAX LISTRIK {thn}-{bln}-{op.Nop}");

                        var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)12);
                        foreach (var item in ketetapanSbyTaxOld)
                        {
                            string nop = item.NOP;
                            int tahunPajak = item.TAHUN;
                            int masaPajak = item.MASAPAJAK;
                            int seqPajak = item.SEQ;
                            var rowMonListrik = _contMonPd.DbMonPpjs.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
                                                                                    x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            if (rowMonListrik != null)
                            {
                                _contMonPd.DbMonPpjs.Remove(rowMonListrik);
                            }

                            var newRow = new DbMonPpj();
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
                            newRow.SumberNama = op.SumberNama;
                            newRow.PeruntukanNama = op.PeruntukanNama;
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

                            _contMonPd.DbMonPpjs.Add(newRow);
                            _contMonPd.SaveChanges();
                            Console.WriteLine($"DB_MON_LISTRIK {thn}-{bln}-{item.NOP}-{item.SEQ}");
                        }
                    }
                }
            }

            //FILL KETETAPAN MONITORING DB
            var _contMonitoringDb = DBClass.GetMonitoringDbContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                var opList = _contMonPd.DbOpListriks.Where(x => x.TahunBuku == thn).ToList();
                for (int bln = 1; bln <= 12; bln++)
                {
                    Console.WriteLine($"{DateTime.Now} [QUERY] KETETAPAN MONITORING DB {thn}-{bln}");
                    var sql = @"
                            SELECT 	REPLACE(NOP, '.','') NOP,
                              TAHUN,
                              MASAPAJAK,
                              1 SEQ,
                              1 JENIS_KETETAPAN,
                              TANGGALENTRY TGL_KETETAPAN,
                              TANGGALJATUHTEMPO TGL_JATUH_TEMPO_BAYAR,
                              0 NILAI_PENGURANG,
                              NVL(PAJAK_TERUTANG, 0) POKOK
                            FROM (
                             select  NO_SPTPD, A.NPWPD, IDAYAT, 
                                     TAHUN, MASAPAJAK,MASAPAJAKAWAL, MASAPAJAKAKHIR, OMSET, 
                                     RUMUS_PROSEN, PAJAK_TERUTANG + PAJAK_TERUTANG1 PAJAK_TERUTANG,
                                     A.NOP, NPWPD2, TANGGALJATUHTEMPO, TANGGALENTRY, A.MODIDATE, TEMPATENTRY, PENGENTRY, A.KETERANGAN,'MANUAL' JENIS_LAPOR
                             from PHRH_USER.sptpd_new@LIHATHR A
                             JOIN PHRH_USER.NOP_BARU@LIHATHR B ON A.NOP=B.NOP AND JENISUSAHA='LISTRIK'
                             WHERE STATUS=0
                             UNION ALL
                             select KD_BILL,NPWPD,KODEREKENING,
                                     TAHUNPAJAK,MASAPAJAK,PERIODE_AWAL,PERIODE_AKHIR,0 OMSET,
                                     PROSEN,PAJAK,A.NOP,NPWPD NPWPD2,JATUH_TEMPO,A.CREATEDATE,A.CREATEDATE,'ONLINE','-','-','ONLINE' JENIS_LAPOR 
                             from sptpd_payment@LIHATBONANG A
                             JOIN PHRH_USER.NOP_BARU@LIHATHR B ON A.NOP=B.NOP AND JENISUSAHA='LISTRIK'
                             where STATUS_HAPUS=0
                            ) A
                            WHERE A.TAHUN = :tahun AND A.MASAPAJAK = :bulan
                        ";

                    var ketetapanSbyTaxOld = await _contMonitoringDb.Set<OPSkpdListrik>()
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

                        var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)12);
                        foreach (var item in ketetapanSbyTaxOld.Where(x => x.NOP == op.Nop))
                        {
                            string nop = item.NOP;
                            int tahunPajak = item.TAHUN;
                            int masaPajak = item.MASAPAJAK;
                            int seqPajak = item.SEQ;
                            var rowMonListrik = _contMonPd.DbMonPpjs.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak && x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);
                            if (rowMonListrik != null)
                            {
                                _contMonPd.DbMonPpjs.Remove(rowMonListrik);
                            }

                            var newRow = new DbMonPpj();

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
                            newRow.SumberNama = op.SumberNama;
                            newRow.PeruntukanNama = op.PeruntukanNama;
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

                            GetRealisasiPhr(ref newRow);

                            _contMonPd.DbMonPpjs.Add(newRow);
                            _contMonPd.SaveChanges();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{DateTime.Now} DB_MON_LISTRIK_MONITORINGDB {thn}-{bln}-{item.NOP}-{item.SEQ}");
                            Console.ResetColor();
                        }
                    }
                }
            }


            MailHelper.SendMail(
            false,
            "DONE PPJ WS",
            $@"PPJ WS FINISHED",
            null
            );
        }
        private void GetRealisasi(ref DbMonPpj row)
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
                                    WHERE 	A.JENIS_PAJAK = 2 AND 
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

                Console.WriteLine($"DB_MON_PPJ (SSPD): {row.TahunPajakKetetapan}-{row.MasaPajakKetetapan}-{row.Nop}-{row.SeqPajakKetetapan}-{row.NominalPokokBayar}");
            }
        }
        private void GetRealisasiPhr(ref DbMonPpj row)
        {
            //PEMBAYARAN PHR
            var _contMonitoringDb = DBClass.GetMonitoringDbContext();

            Console.WriteLine($"{DateTime.Now} [QUERY] OP (SSPD) (PHR)");
            var sql = @"
               SELECT 	REPLACE(FK_NOP, '.', '') NOP,
                        TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,
                        BULAN_PAJAK,
                        TGL_SETORAN TRANSACTION_DATE,
                        JML_POKOK NOMINAL_POKOK,
                        JML_DENDA NOMINAL_SANKSI,
                        0 NOMINAL_ADMINISTRASI,
                        0 NOMINAL_LAINNYA,
                        0 PENGURANG_POKOK,
                        0 PENGURANG_SANSKSI,
                        1 SEQ_KETETAPAN
                FROM VW_SIMPADA_SSPD@LIHATHPPSERVER
                WHERE NAMA_PAJAK_DAERAH='PPJ' AND TAHUN_SETOR=TO_CHAR(SYSDATE,'YYYY')
                    AND  REPLACE(FK_NOP, '.', '') = :NOP AND A.TAHUN = :TAHUN AND A.MASA = :MASA AND 
            ";

            var pembayaranSspdList = _contMonitoringDb.Set<SSPDPbjt>()
                 .FromSqlRaw(sql, new[] {
                        new OracleParameter("NOP", row.Nop),
                        new OracleParameter("TAHUN", row.TahunPajakKetetapan),
                        new OracleParameter("MASA", row.MasaPajakKetetapan),
                }).ToList();
            Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] OP (SSPD) (PHR)");

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
                    Console.WriteLine($"{DateTime.Now} [SAVED] DB_MON_PPJ (SSPD) (PHR): {row.TahunPajakKetetapan}-{row.MasaPajakKetetapan}-{row.Nop}-{row.SeqPajakKetetapan}");
                    Console.ResetColor();
                }
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
