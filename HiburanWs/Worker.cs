using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using static MonPDLib.Helper;

namespace HiburanWs
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
                //var now = DateTime.Now;

                //// Hitung waktu untuk 00:00 esok hari
                //var nextRunTime = now.Date.AddHours(1); // Tambah 1 hari dan set jam 00:00
                //var delay = nextRunTime - now;

                //_logger.LogInformation("Next run scheduled at: {time}", nextRunTime);
                //_logger.LogInformation("Next run scheduled : {lama}", delay.Hours + ":" + delay.Minutes);

                //// Tunggu hingga waktu eksekusi
                //await Task.Delay(delay, stoppingToken);

                // Eksekusi tugas
                try
                {
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                }
            }
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
        D.ID  KATEGORI_ID,
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
JOIN OBJEK_PAJAK_HIBURAN B ON A.NOP = B.NOP
JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
JOIN M_KATEGORI_PAJAK D ON D.ID = A.KATEGORI
LEFT JOIN M_KECAMATAN B ON A.KD_CAMAT = B.KD_CAMAT           
                    ";

                    var result = await _contSbyTax.Set<DbOpHiburan>().FromSqlRaw(sql).ToListAsync();
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
	    CASE NAMA_JENIS_PAJAK
		  WHEN 'FITNESS/PUSAT KEBUGARAN' THEN 53
		  WHEN 'KARAOKE KELUARGA' THEN 46
		  WHEN 'BOWLING' THEN 50
		  WHEN 'INSIDENTIL (KONTES KECANTIKAN DAN SEJENISNYA)' THEN 43
		  WHEN 'PAMERAN KOMPUTER' THEN 40
		  WHEN 'PAMERAN BUSANA' THEN 40
		  WHEN 'PANTI PIJAT/THERAPY/SAUNA/SPA' THEN 55
		  WHEN 'WISATA TIRTA/REKREASI AIR' THEN 54
		  WHEN 'SIRKUS, AKROBAT, SULAP & SEJENISNYA' THEN 43
		  WHEN 'PESTA ANAK & KELUARGA' THEN 51
		  WHEN 'TURNAMEN BASKET' THEN 44
		  WHEN 'INSIDENTIL (KARAOKE KELUARGA)' THEN 46
		  WHEN 'INSIDENTIL (KONTES BINARAGA DAN SEJENISNYA)' THEN 53
		  WHEN 'KONTES BINARAGA & SEJENISNYA' THEN 53
		  WHEN 'TAMAN SATWA/PEMANDIAN ALAM/TAMAN REKREASI' THEN 54
		  WHEN 'PERMAINAN KETANGKASAN' THEN 48
		  WHEN 'INSIDENTIL (PAGELARAN KESENIAN, MUSIK, TARI DAN/ATAU BUSANA)' THEN 43
		  WHEN 'INSIDENTIL (NONTON BARENG)' THEN 38
		  WHEN 'INSIDENTIL (PAMERAN)' THEN 40
		  WHEN 'INSIDENTIL (DISKOTIK, KARAOKE DEWASA, KELAB MALAM DAN SEJENISNYA)' THEN 52
		  WHEN 'INSIDENTIL (PAMERAN BUSANA, KOMPUTER, ELEKTRONIK, OTOMOTIF, DAN PROPERTY)' THEN 40
		  WHEN 'PAMERAN PROPERTY' THEN 40
		  WHEN 'BILLYARD' THEN 47
		  WHEN 'DISKOTIK' THEN 52
		  WHEN 'INSIDENTIL (PAMERAN FRANCHISE)' THEN 40
		  WHEN 'PERTUNJUKAN MUSIK' THEN 43
		  WHEN 'INSIDENTIL (PEMERAN MASAKAN)' THEN 40
		  WHEN 'INSIDENTIL (SEMINAR)' THEN 43
		  WHEN 'PERMAINAN ANAK' THEN 51
		  WHEN 'INSIDENTIL (HIBURAN SEKOLAH)' THEN 43
		  WHEN 'INSIDENTIL (KESENIAN TRADISIONAL)' THEN 43
		  WHEN 'PAMERAN OTOMITIF' THEN 40
		  WHEN 'INSIDENTIL (PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU)' THEN 40
		  WHEN 'INSIDENTIL (PUSAT KEBUGARAN/FITNESS CENTER)' THEN 53
		  WHEN 'INSIDENTIL (PERMAINAN KETANGKASAN)' THEN 48
		  WHEN 'INSIDENTIL (KESENIAN TRADISIONAL - PAGELARAN KESENIAN, MUSIK DAN/ATAU TARI YANG BERSIFAT TRADISIONAL)' THEN 43
		  WHEN 'INSIDENTIL (PANTI PIJAT, REFLEKSI DAN MANDI UAP/SPA)' THEN 55
		  WHEN 'BIOSKOP' THEN 38
		  WHEN 'INSIDENTIL DI DISKOTIK, KLUB MALAM' THEN 52
		  WHEN 'INSIDENTIL (TARI MODERN)' THEN 43
		  WHEN 'PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU' THEN 40
		  WHEN 'PAMERAN CELLULAR' THEN 40
		  WHEN 'INSIDENTIL (KONSER MUSIK) INCLUDE PAJAK' THEN 43
		  WHEN 'INSIDENTIL (PERTUNJUKAN MUSIK)' THEN 43
		  WHEN 'GEDUNG OLAHRAGA' THEN 42
		  WHEN 'PERTUNJUKAN' THEN 43
		  WHEN 'KONTES KECANTIKAN & SEJENISNYA' THEN 43
		  WHEN 'INSIDENTIL (PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBUHAN, SATWA DAN HASIL PRODUKSI BARANG DAN/ATAU JASA LAINNYA)' THEN 40
		  WHEN 'INSIDENTIL (SIRKUS, AKROBAT, SULAP DAN SEJENISNYA)' THEN 43
		  WHEN 'KARAOKE DEWASA' THEN 49
		  WHEN 'BAR/CAFE/KLAB MALAM' THEN 52
		  WHEN 'FUTSAL (OLAHRAGA)' THEN 39
		  WHEN 'KOLAM RENANG' THEN 45
		  WHEN 'INSIDENTIL (PERMAINAN ANAK)' THEN 51
		  WHEN 'PERTANDINGAN OLAHRAGA' THEN 42
		  WHEN 'KOLAM PANCING' THEN 54
		  WHEN 'INSIDENTIL (PERTANDINGAN OLAHRAGA)' THEN 42
		  WHEN 'PAMERAN MAINAN ANAK' THEN 40
		  WHEN 'PAMERAN' THEN 40
		  WHEN 'PACUAN KUDA & KENDARAAN BERMOTOR' THEN 43
		  ELSE 38
		END AS KATEGORI_ID,
		CASE NAMA_JENIS_PAJAK
	  WHEN 'FITNESS/PUSAT KEBUGARAN' THEN 'FITNESS/GYM'
	  WHEN 'KARAOKE KELUARGA' THEN 'KARAOKE KELUARGA'
	  WHEN 'BOWLING' THEN 'BOWLING'
	  WHEN 'INSIDENTIL (KONTES KECANTIKAN DAN SEJENISNYA)' THEN 'PERTUNJUKAN'
	  WHEN 'PAMERAN KOMPUTER' THEN 'PAMERAN'
	  WHEN 'PAMERAN BUSANA' THEN 'PAMERAN'
	  WHEN 'PANTI PIJAT/THERAPY/SAUNA/SPA' THEN 'PANTI PIJAT/THERAPY/SAUNA/SPA'
	  WHEN 'WISATA TIRTA/REKREASI AIR' THEN 'TAMAN WISATA'
	  WHEN 'SIRKUS, AKROBAT, SULAP & SEJENISNYA' THEN 'PERTUNJUKAN'
	  WHEN 'PESTA ANAK & KELUARGA' THEN 'PLAYGROUND'
	  WHEN 'TURNAMEN BASKET' THEN 'BASKET'
	  WHEN 'INSIDENTIL (KARAOKE KELUARGA)' THEN 'KARAOKE KELUARGA'
	  WHEN 'INSIDENTIL (KONTES BINARAGA DAN SEJENISNYA)' THEN 'FITNESS/GYM'
	  WHEN 'KONTES BINARAGA & SEJENISNYA' THEN 'FITNESS/GYM'
	  WHEN 'TAMAN SATWA/PEMANDIAN ALAM/TAMAN REKREASI' THEN 'TAMAN WISATA'
	  WHEN 'PERMAINAN KETANGKASAN' THEN 'PERMAINAN/GAMES'
	  WHEN 'INSIDENTIL (PAGELARAN KESENIAN, MUSIK, TARI DAN/ATAU BUSANA)' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (NONTON BARENG)' THEN 'TONTONAN FILM'
	  WHEN 'INSIDENTIL (PAMERAN)' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (DISKOTIK, KARAOKE DEWASA, KELAB MALAM DAN SEJENISNYA)' THEN 'DISKOTIK/KELAB MALAM/BAR'
	  WHEN 'INSIDENTIL (PAMERAN BUSANA, KOMPUTER, ELEKTRONIK, OTOMOTIF, DAN PROPERTY)' THEN 'PAMERAN'
	  WHEN 'PAMERAN PROPERTY' THEN 'PAMERAN'
	  WHEN 'BILLYARD' THEN 'BILLYARD'
	  WHEN 'DISKOTIK' THEN 'DISKOTIK/KELAB MALAM/BAR'
	  WHEN 'INSIDENTIL (PAMERAN FRANCHISE)' THEN 'PAMERAN'
	  WHEN 'PERTUNJUKAN MUSIK' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (PEMERAN MASAKAN)' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (SEMINAR)' THEN 'PERTUNJUKAN'
	  WHEN 'PERMAINAN ANAK' THEN 'PLAYGROUND'
	  WHEN 'INSIDENTIL (HIBURAN SEKOLAH)' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (KESENIAN TRADISIONAL)' THEN 'PERTUNJUKAN'
	  WHEN 'PAMERAN OTOMITIF' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU)' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (PUSAT KEBUGARAN/FITNESS CENTER)' THEN 'FITNESS/GYM'
	  WHEN 'INSIDENTIL (PERMAINAN KETANGKASAN)' THEN 'PERMAINAN/GAMES'
	  WHEN 'INSIDENTIL (KESENIAN TRADISIONAL - PAGELARAN KESENIAN, MUSIK DAN/ATAU TARI YANG BERSIFAT TRADISIONAL)' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (PANTI PIJAT, REFLEKSI DAN MANDI UAP/SPA)' THEN 'PANTI PIJAT/THERAPY/SAUNA/SPA'
	  WHEN 'BIOSKOP' THEN 'TONTONAN FILM'
	  WHEN 'INSIDENTIL DI DISKOTIK, KLUB MALAM' THEN 'DISKOTIK/KELAB MALAM/BAR'
	  WHEN 'INSIDENTIL (TARI MODERN)' THEN 'PERTUNJUKAN'
	  WHEN 'PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBU' THEN 'PAMERAN'
	  WHEN 'PAMERAN CELLULAR' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (KONSER MUSIK) INCLUDE PAJAK' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (PERTUNJUKAN MUSIK)' THEN 'PERTUNJUKAN'
	  WHEN 'GEDUNG OLAHRAGA' THEN 'GEDUNG OLAHRAGA'
	  WHEN 'PERTUNJUKAN' THEN 'PERTUNJUKAN'
	  WHEN 'KONTES KECANTIKAN & SEJENISNYA' THEN 'PERTUNJUKAN'
	  WHEN 'INSIDENTIL (PAMERAN SENI BUDAYA, SENI UKIR, BARANG SENI, TUMBUHAN, SATWA DAN HASIL PRODUKSI BARANG DAN/ATAU JASA LAINNYA)' THEN 'PAMERAN'
	  WHEN 'INSIDENTIL (SIRKUS, AKROBAT, SULAP DAN SEJENISNYA)' THEN 'PERTUNJUKAN'
	  WHEN 'KARAOKE DEWASA' THEN 'KARAOKE DEWASA'
	  WHEN 'BAR/CAFE/KLAB MALAM' THEN 'DISKOTIK/KELAB MALAM/BAR'
	  WHEN 'FUTSAL (OLAHRAGA)' THEN 'FUTSAL'
	  WHEN 'KOLAM RENANG' THEN 'KOLAM RENANG'
	  WHEN 'INSIDENTIL (PERMAINAN ANAK)' THEN 'PLAYGROUND'
	  WHEN 'PERTANDINGAN OLAHRAGA' THEN 'GEDUNG OLAHRAGA'
	  WHEN 'KOLAM PANCING' THEN 'TAMAN WISATA'
	  WHEN 'INSIDENTIL (PERTANDINGAN OLAHRAGA)' THEN 'GEDUNG OLAHRAGA'
	  WHEN 'PAMERAN MAINAN ANAK' THEN 'PAMERAN'
	  WHEN 'PAMERAN' THEN 'PAMERAN'
	  WHEN 'PACUAN KUDA & KENDARAAN BERMOTOR' THEN 'PERTUNJUKAN'
	  ELSE 'TONTONAN FILM'
	END AS KATEGORI_NAMA,
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
	FROM VW_SIMPADA_OP_all_mon
	WHERE 	NAMA_PAJAK_DAERAH ='HIBURAN' 
			AND KATEGORI_PAJAK <> 'OBJEK TESTING'
			AND FK_NOP IS NOT NULL
			AND REPLACE(FK_NOP, '.', '') LIKE '3578%'

                    ";

                    var result = await _contHpp.Set<DbOpHiburan>().FromSqlRaw(sql).ToListAsync();

                    var distinctNop = result.Select(x => x.Nop).ToList();
                    var dataExisting = _contMonPd.DbOpHiburans.Where(x => distinctNop.Contains(x.Nop)).ToList();


                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == i).ToListAsync();
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

            //////FILL KETETAPAN 
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
                            _contMonPd.DbMonHiburans.Add(new DbMonHiburan()
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
                                KategoriId = op.KategoriId,
                                KategoriNama = op.KategoriNama,
                                TahunBuku = thn,
                                Akun = op.Akun,
                                NamaAkun = op.NamaAkun,
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
                                UpdBy = "JOB"
                            });

                            _contMonPd.SaveChanges();
                            Console.WriteLine($"DB_MON_HIBURAN {thn}-{bln}-{item.NOP}-{item.SEQ}");
                        }
                    }
                }
            }

            //FILL KETETAPAN MONITORING DB
            var _contMonitoringDb = DBClass.GetMonitoringDbContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                var opList = _contMonPd.DbOpHiburans.Where(x => x.TahunBuku == thn).ToList();
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
                             JOIN PHRH_USER.NOP_BARU@LIHATHR B ON A.NOP=B.NOP AND JENISUSAHA='HIBURAN'
                             WHERE STATUS=0
                             UNION ALL
                             select KD_BILL,NPWPD,KODEREKENING,
                                     TAHUNPAJAK,MASAPAJAK,PERIODE_AWAL,PERIODE_AKHIR,0 OMSET,
                                     PROSEN,PAJAK,A.NOP,NPWPD NPWPD2,JATUH_TEMPO,A.CREATEDATE,A.CREATEDATE,'ONLINE','-','-','ONLINE' JENIS_LAPOR 
                             from sptpd_payment@LIHATBONANG A
                             JOIN PHRH_USER.NOP_BARU@LIHATHR B ON A.NOP=B.NOP AND JENISUSAHA='HIBURAN'
                             where STATUS_HAPUS=0
                            ) A
                            WHERE A.TAHUN = :tahun AND A.MASAPAJAK = :bulan
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
                            var rowMonHiburan = _contMonPd.DbMonHiburans.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak && x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                            if (rowMonHiburan != null)
                            {
                                _contMonPd.DbMonHiburans.Remove(rowMonHiburan);
                            }
                            _contMonPd.DbMonHiburans.Add(new DbMonHiburan()
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
                                KategoriId = op.KategoriId,
                                KategoriNama = op.KategoriNama,
                                TahunBuku = thn,
                                Akun = op.Akun,
                                NamaAkun = op.NamaAkun,
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
                                UpdBy = "JOB"
                            });

                            _contMonPd.SaveChanges();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{DateTime.Now} DB_MON_HIBURAN_MONITORINGDB {thn}-{bln}-{item.NOP}-{item.SEQ}");
                            Console.ResetColor();
                        }
                    }
                }
            }

            //PEMBAYARAN
            var _contBima = DBClass.GetBimaContext();
            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                for (int bln = 1; bln <= 12; bln++)
                {
                    var opList = _contMonPd.DbMonHiburans.Where(x => x.TahunPajakKetetapan == thn && x.MasaPajakKetetapan == bln).ToList();

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
                                    WHERE 	A.JENIS_PAJAK = 5 AND 
                                      A.NOP = :NOP AND
                                      A.TAHUN = :TAHUN AND 
                                      A.MASA = :MASA AND 
                                      A.SEQ_KETETAPAN = :SEQ";

                        var pembayaranSspdList1 = await _contBima.Set<SSPD>()
                            .FromSqlRaw(sql, new[] {
                                            new OracleParameter("NOP", op.Nop),
                                            new OracleParameter("TAHUN", thn),
                                            new OracleParameter("MASA", bln),
                                            new OracleParameter("SEQ", op.SeqPajakKetetapan)
                            }).ToListAsync();

                        if (pembayaranSspdList1 != null && pembayaranSspdList1.Count > 0)
                        {
                            DateTime tanggalBayarTerakhir = pembayaranSspdList1.Max(x => (DateTime)x.TRANSACTION_DATE);
                            int maxTahunBayar = pembayaranSspdList1.Max(x => ((DateTime)x.TRANSACTION_DATE).Year);
                            decimal nominalPokokBayar = pembayaranSspdList1.Sum(x => (decimal)x.NOMINAL_POKOK);
                            decimal nominalSanksiBayar = pembayaranSspdList1.Sum(x => (decimal)x.NOMINAL_SANKSI);
                            decimal nominalAdministrasi = pembayaranSspdList1.Sum(x => (decimal)x.NOMINAL_ADMINISTRASI);
                            decimal nominalLainnya = pembayaranSspdList1.Sum(x => (decimal)x.NOMINAL_LAINYA);
                            decimal pengurangPokok = pembayaranSspdList1.Sum(x => (decimal)x.PENGURANG_POKOK);
                            decimal pengurangSanksi = pembayaranSspdList1.Sum(x => (decimal)x.PENGURANG_SANKSI);

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

                                op.TglBayarPokok = TGL_BAYAR_POKOK;
                                op.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
                                op.AkunPokokBayar = AKUN_POKOK_BAYAR;
                                op.JenisPokokBayar = JENIS_POKOK_BAYAR;
                                op.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
                                op.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
                                op.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
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

                                op.TglBayarSanksi = TGL_BAYAR_SANKSI;
                                op.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
                                op.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
                                op.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
                                op.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
                                op.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
                                op.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
                                op.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
                            }
                            _contMonPd.SaveChanges();
                            Console.WriteLine($"{DateTime.Now} DB_MON_HIBURAN (SSPD): {thn}-{bln}-{op.Nop}-{op.SeqPajakKetetapan}");
                        }
                    }
                }
            }

            //PEMBAYARAN PHR
            var _contPhr = DBClass.GetPhrhContext();

            Console.WriteLine($"{DateTime.Now} [QUERY] OP (SSPD) (PHR)");
            var sql1 = @"
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
                FROM PHRH_USER.VW_SIMPADAHPP_SSPD_PHR A
                JOIN PHRH_USER.KODEREKENING_BARU B ON A.FK_AYAT_PAJAK=B.KODE
                WHERE NAMA_PAJAK_DAERAH='HIBURAN' AND TAHUN_SETOR=TO_CHAR(SYSDATE,'YYYY')
            ";
            var sspdList = await _contPhr.Set<SSPDPbjt>().FromSqlRaw(sql1).ToListAsync();
            Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] OP (SSPD) (PHR)");

            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
            {
                for (int bln = 1; bln <= 12; bln++)
                {
                    var opList = _contMonPd.DbMonHiburans.Where(x => x.TahunPajakKetetapan == thn && x.MasaPajakKetetapan == bln).ToList();

                    foreach (var op in opList)
                    {
                        Console.WriteLine($"{DateTime.Now} [PROCESS] OP (SSPD) (PHR) {thn}-{bln}-{op.Nop}-{op.SeqPajakKetetapan}");

                        if (sspdList != null)
                        {
                            var pembayaranSspdList = sspdList.Where(x => x.TAHUN_PAJAK == thn && x.BULAN_PAJAK == bln && x.NOP == op.Nop).ToList();

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

                                    op.TglBayarPokok = TGL_BAYAR_POKOK;
                                    op.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
                                    op.AkunPokokBayar = AKUN_POKOK_BAYAR;
                                    op.JenisPokokBayar = JENIS_POKOK_BAYAR;
                                    op.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
                                    op.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
                                    op.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
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

                                    op.TglBayarSanksi = TGL_BAYAR_SANKSI;
                                    op.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
                                    op.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
                                    op.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
                                    op.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
                                    op.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
                                    op.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
                                    op.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
                                }
                                _contMonPd.SaveChanges();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{DateTime.Now} [SAVED] DB_MON_HIBURAN (SSPD) (PHR): {thn}-{bln}-{op.Nop}-{op.SeqPajakKetetapan}");
                                Console.ResetColor();
                            }
                        }
                    }
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
