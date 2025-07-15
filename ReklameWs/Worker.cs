using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using static MonPDLib.Helper;

namespace ReklameWs
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

                //var nextRun = now.AddDays(1); // besok jam 00:00
                //var delay = nextRun - now;

                //_logger.LogInformation("Next run scheduled at: {time}", nextRun);

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
                    "ERROR REKLAME WS",
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
                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    Console.WriteLine($"{DateTime.Now} DB_OP_REKLAME_MONITORINGDB");
                    var sql = @"
SELECT 	NVL(NO_FORMULIR, '-') NO_FORMULIR, 
		NO_PERUSAHAAN, 
		NAMA_PERUSAHAAN, 
		ALAMAT_PERUSAHAAN, 
		NO_ALAMAT_PERUSAHAAN, 
		BLOK_ALAMAT_PERUSAHAAN, 
		ALAMATPER, 
		TELP_PERUSAHAAN, 
		CLIENTNAMA, 
		CLIENTALAMAT, 
		JABATAN, 
		KODE_JENIS, 
		NAMA_JENIS, 
		NO_WP, 
		NAMA, 
		ALAMAT, 
		NO_ALAMAT, 
		BLOK_ALAMAT, 
		ALAMATWP, 
		JENIS_PERMOHONAN, 
TGL_PERMOHONAN, 
TGL_MULAI_BERLAKU, 
TGL_AKHIR_BERLAKU, 
NAMA_JALAN, 
NO_JALAN, 
BLOK_JALAN, 
ALAMATREKLAME, 
DETIL_LOKASI, 
KECAMATAN, 
JENIS_PRODUK, 
LETAK_REKLAME, 
STATUS_TANAH, 
FLAG_PERMOHONAN, 
STATUSPROSES, 
FLAG_SIMPATIK, 
KODE_OBYEK, 
PANJANG, 
LEBAR, 
LUAS, 
LUASDISKON, 
SISI, 
KETINGGIAN, 
ISI_REKLAME, 
PERMOHONAN_BARU, 
NO_FORMULIR_LAMA, 
SUDUT_PANDANG, 
NILAIPAJAK, 
NILAIJAMBONG, 
KELAS_JALAN, 
NO_TELP, 
TIMETRANS, 
NPWPD, 
FLAGTUNG, 
STATUSCABUT, 
FLAGEXP, 
KODE_LOKASI, 
NAMA_PENEMPATAN, 
NO_FORMULIR_AWAL, 
KETPERSIL, 
PER_PENANGGUNGJAWAB, 
ALAMATPER_PENANGGUNGJAWAB, 
NPWPD_PENANGGUNGJAWAB, 
POTENSI, 
FLAGMALL, 
FLAGJEDA, 
FLAGBRANDED, 
NLPR, 
USERNAME, 
JENIS_WP, 
TGL_CETAK_PER, 
STATUS_A_WP, 
STATUS_A_PER, 
NMKELURAHAN, 
NOP, 
UNIT_KERJA, 
UNIT_BERKAS, 
STATUS_VER, 
TGL_VER, 
USER_VER,
TGL_OP_TUTUP,
NOR,
2025 TAHUN_BUKU,
KATEGORI_ID,
0 SEQ
FROM (
	SELECT NO_FORMULIR,
	NO_PERUSAHAAN,
	NAMA_PERUSAHAAN,
	ALAMAT_PERUSAHAAN,
	NO_ALAMAT_PERUSAHAAN,
	BLOK_ALAMAT_PERUSAHAAN,
	ALAMATPER,
	TELP_PERUSAHAAN,
	CLIENTNAMA,
	CLIENTALAMAT,
	JABATAN,
	KODE_JENIS,
	NAMA_JENIS,
	NO_WP,
	NAMA,
	ALAMAT,
	NO_ALAMAT,
	BLOK_ALAMAT,
	ALAMATWP,
	JENIS_PERMOHONAN,
	TGL_PERMOHONAN,
	TGL_MULAI_BERLAKU,
	TGL_AKHIR_BERLAKU,
	NAMA_JALAN,
	NO_JALAN,
	BLOK_JALAN,
	ALAMATREKLAME,
	DETIL_LOKASI,
	KECAMATAN,
	JENIS_PRODUK,
	LETAK_REKLAME,
	STATUS_TANAH,
	FLAG_PERMOHONAN,
	STATUSPROSES,
	FLAG_SIMPATIK,
	KODE_OBYEK,
	PANJANG,
	LEBAR,
	LUAS,
	LUASDISKON,
	SISI,
	KETINGGIAN,
	ISI_REKLAME,
	PERMOHONAN_BARU,
	NO_FORMULIR_LAMA,
	SUDUT_PANDANG,
	NILAIPAJAK,
	NILAIJAMBONG,
	KELAS_JALAN,
	NO_TELP,
	TIMETRANS,
	NPWPD,
	FLAGTUNG,
	STATUSCABUT,
	FLAGEXP,
	KODE_LOKASI,
	NAMA_PENEMPATAN,
	NO_FORMULIR_AWAL,
	KETPERSIL,
	PER_PENANGGUNGJAWAB,
	ALAMATPER_PENANGGUNGJAWAB,
	NPWPD_PENANGGUNGJAWAB,
	POTENSI,
	FLAGMALL,
	FLAGJEDA,
	FLAGBRANDED,
	NLPR,
	USERNAME,
	JENIS_WP,
	TGL_CETAK_PER,
	STATUS_A_WP,
	STATUS_A_PER,
	NMKELURAHAN,
	NOP,
	UNIT_KERJA,
	UNIT_BERKAS,
	STATUS_VER,
	TGL_VER,
	USER_VER,
    TGL_AKHIR_BERLAKU AS TGL_OP_TUTUP,
    60 KATEGORI_ID,
	NOR
	FROM VWTABELPERMOHONAN@lihatreklame
	WHERE TGL_PERMOHONAN >= SYSDATE - INTERVAL '4' YEAR
	UNION ALL
	SELECT NO_FORMULIR,
	NO_PERUSAHAAN,
	NAMA_PERUSAHAAN,
	ALAMAT_PERUSAHAAN,
	NO_ALAMAT_PERUSAHAAN,
	BLOK_ALAMAT_PERUSAHAAN,
	ALAMATPER,
	TELP_PERUSAHAAN,
	CLIENTNAMA,
	CLIENTALAMAT,
	JABATAN,
	KODE_JENIS,
	NAMA_JENIS,
	NO_WP,
	NAMA,
	ALAMAT,
	NO_ALAMAT,
	BLOK_ALAMAT,
	ALAMATWP,
	JENIS_PERMOHONAN,
	TGL_PERMOHONAN,
	TGL_MULAI_BERLAKU,
	TGL_AKHIR_BERLAKU,
	NAMA_JALAN,
	NO_JALAN,
	BLOK_JALAN,
	ALAMATREKLAME,
	DETIL_LOKASI,
	KECAMATAN,
	JENIS_PRODUK,
	LETAK_REKLAME,
	STATUS_TANAH,
	FLAG_PERMOHONAN,
	STATUSPROSES,
	FLAG_SIMPATIK,
	NULL KODE_OBYEK,
	PANJANG,
	LEBAR,
	LUAS,
	0 LUASDISKON,
	0 SISI,
	0 KETINGGIAN,
	ISI_REKLAME,
	NULL PERMOHONAN_BARU,
	NULL NO_FORMULIR_LAMA,
	0 SUDUT_PANDANG,
	NILAIPAJAK,
	NILAIJAMBONG,
	KELAS_JALAN,
	NO_TELP,
	TIMETRANS,
	NPWPD,
	NULL FLAGTUNG,
	STATUSCABUT,
	NULL FLAGEXP,
	NULL KODE_LOKASI,
	NULL NAMA_PENEMPATAN,
	NULL NO_FORMULIR_AWAL,
	KET_EVENT KETPERSIL,
	PER_PENANGGUNGJAWAB,
	ALAMATPER_PENANGGUNGJAWAB,
	NPWPD_PENANGGUNGJAWAB,
	POTENSI,
	FLAGMALL,
	NULL FLAGJEDA,
	FLAGBRANDED,
	NULL NLPR,
	USERNAME,
	JENIS_WP,
	NULL TGL_CETAK_PER,
	NULL STATUS_A_WP,
	NULL STATUS_A_PER,
	NULL NMKELURAHAN,
	NULL NOP,
	NULL UNIT_KERJA,
	NULL UNIT_BERKAS,
	0 STATUS_VER,
	NULL TGL_VER,
	NULL USER_VER,
    TGL_AKHIR_BERLAKU AS TGL_OP_TUTUP,
    59 KATEGORI_ID,
	CAST(NULL AS VARCHAR(500)) AS NOR
	FROM VWTABELPERMOHONANINS@lihatreklame
	WHERE TGL_PERMOHONAN >= SYSDATE - INTERVAL '4' YEAR
	UNION ALL
	SELECT NO_FORMULIR,
	0 NO_PERUSAHAAN,
	NAMA_PERUSAHAAN,
	ALAMAT_PERUSAHAAN,
	NO_ALAMAT_PRSH NO_ALAMAT_PERUSAHAAN,
	BLOK_ALAMAT_PRSH BLOK_ALAMAT_PERUSAHAAN,
	ALAMATPER,
	TELP_PERUSAHAAN,
	NULL CLIENTNAMA,
	NULL CLIENTALAMAT,
	NULL JABATAN,
	KODE_JENIS,
	NAMA_JENIS,
	NO_WP,
	NAMA,
	ALAMAT_WP ALAMAT,
	NO_ALAMAT_WP NO_ALAMAT,
	BLOK_ALAMAT_WP BLOK_ALAMAT,
	NULL ALAMATWP,
	NULL JENIS_PERMOHONAN,
	TGL_PERMOHONAN,
	TGL_MULAI_BERLAKU,
	TGL_AKHIR_BERLAKU,
	NAMA_JALAN,
	NO_JALAN,
	BLOK_JALAN,
	ALAMATREKLAME,
	DETIL_LOKASI,
	NULL KECAMATAN,
	NULL JENIS_PRODUK,
	LETAK_REKLAME,
	NULL STATUS_TANAH,
	FLAG_PERMOHONAN,
	NULL STATUSPROSES,
	NULL FLAG_SIMPATIK,
	KODE_OBYEK,
	PANJANG,
	LEBAR,
	LUAS,
	LUASDISKON,
	SISI,
	KETINGGIAN,
	ISI_REKLAME,
	PERMOHONAN_BARU,
	NO_FORMULIR_LAMA,
	SUDUT_PANDANG,
	NILAIPAJAK,
	0 NILAIJAMBONG,
	KELAS_JALAN,
	TELP_WP NO_TELP,
	CAST(NULL  AS DATE) TIMETRANS,
	NPWPD,
	FLAGTUNG,
	NULL STATUSCABUT,
	CAST(FLAG_EX AS VARCHAR(500)) FLAGEXP,
	NULL KODE_LOKASI,
	NAMA_PENEMPATAN,
	NULL NO_FORMULIR_AWAL,
	NULL KETPERSIL ,
	NULL PER_PENANGGUNGJAWAB,
	NULL ALAMATPER_PENANGGUNGJAWAB,
	NULL NPWPD_PENANGGUNGJAWAB,
	NULL POTENSI,
	NULL FLAGMALL,
	FLAGJEDA,
	NULL FLAGBRANDED,
	NULL NLPR,
	USERNAME,
	JENIS_WP,
	CAST(NULL AS DATE) TGL_CETAK_PER,
	CAST(NULL AS NUMBER) STATUS_A_WP,
	CAST(NULL AS NUMBER) STATUS_A_PER,
	NULL NMKELURAHAN,
	NULL NOP,
	NULL UNIT_KERJA,
	NULL UNIT_BERKAS,
	NULL STATUS_VER,
	CAST(NULL AS DATE) TGL_VER,
	NULL USER_VER,
    TGL_AKHIR_BERLAKU AS TGL_OP_TUTUP,
    61 KATEGORI_ID,
	NOR
	FROM VWPERMOHONANSIMRLAMA1@lihatreklame
	WHERE TGL_PERMOHONAN >= SYSDATE - INTERVAL '4' YEAR
) A

                    ";

                    var result = await _contMonitoringDb.Set<DbOpReklame>().FromSqlRaw(sql).ToListAsync();
                    
                    var existingr = await _contMonPd.DbOpReklames.ToListAsync();
                    _contMonPd.DbOpReklames.RemoveRange(existingr);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} DB_OP_REKLAME_MONITORINGDB");
                    Console.ResetColor();

                    foreach (var item in result)
                    {
                        var newRow = new MonPDLib.EF.DbOpReklame();
                        newRow.NoFormulir = item.NoFormulir;
                        newRow.NoPerusahaan = item.NoPerusahaan;
                        newRow.NamaPerusahaan = item.NamaPerusahaan;
                        newRow.AlamatPerusahaan = item.AlamatPerusahaan;
                        newRow.NoAlamatPerusahaan = item.NoAlamatPerusahaan;
                        newRow.BlokAlamatPerusahaan = item.BlokAlamatPerusahaan;
                        newRow.Alamatper = item.Alamatper;
                        newRow.TelpPerusahaan = item.TelpPerusahaan;
                        newRow.Clientnama = item.Clientnama;
                        newRow.Clientalamat = item.Clientalamat;
                        newRow.Jabatan = item.Jabatan;
                        newRow.KodeJenis = item.KodeJenis;
                        newRow.NamaJenis = item.NamaJenis;
                        newRow.NoWp = item.NoWp;
                        newRow.Nama = item.Nama;
                        newRow.Alamat = item.Alamat;
                        newRow.NoAlamat = item.NoAlamat;
                        newRow.BlokAlamat = item.BlokAlamat;
                        newRow.Alamatwp = item.Alamatwp;
                        newRow.JenisPermohonan = item.JenisPermohonan;
                        newRow.TglPermohonan = item.TglPermohonan;
                        newRow.TglMulaiBerlaku = item.TglMulaiBerlaku;
                        newRow.TglAkhirBerlaku = item.TglAkhirBerlaku;
                        newRow.NamaJalan = item.NamaJalan;
                        newRow.NoJalan = item.NoJalan;
                        newRow.BlokJalan = item.BlokJalan;
                        newRow.Alamatreklame = item.Alamatreklame;
                        newRow.DetilLokasi = item.DetilLokasi;
                        newRow.Kecamatan = item.Kecamatan;
                        newRow.JenisProduk = item.JenisProduk;
                        newRow.LetakReklame = item.LetakReklame;
                        newRow.StatusTanah = item.StatusTanah;
                        newRow.FlagPermohonan = item.FlagPermohonan;
                        newRow.Statusproses = item.Statusproses;
                        newRow.FlagSimpatik = item.FlagSimpatik;
                        newRow.KodeObyek = item.KodeObyek;
                        newRow.Panjang = item.Panjang;
                        newRow.Lebar = item.Lebar;
                        newRow.Luas = item.Luas;
                        newRow.Luasdiskon = item.Luasdiskon;
                        newRow.Sisi = item.Sisi;
                        newRow.Ketinggian = item.Ketinggian;
                        newRow.IsiReklame = item.IsiReklame;
                        newRow.PermohonanBaru = item.PermohonanBaru;
                        newRow.NoFormulirLama = item.NoFormulirLama;
                        newRow.SudutPandang = item.SudutPandang;
                        newRow.Nilaipajak = item.Nilaipajak;
                        newRow.Nilaijambong = item.Nilaijambong;
                        newRow.KelasJalan = item.KelasJalan;
                        newRow.NoTelp = item.NoTelp;
                        newRow.Timetrans = item.Timetrans;
                        newRow.Npwpd = item.Npwpd;
                        newRow.Flagtung = item.Flagtung;
                        newRow.Statuscabut = item.Statuscabut;
                        newRow.Nor = item.Nor;
                        newRow.KodeLokasi = item.KodeLokasi;
                        newRow.NamaPenempatan = item.NamaPenempatan;
                        newRow.NoFormulirAwal = item.NoFormulirAwal;
                        newRow.Ketpersil = item.Ketpersil;
                        newRow.PerPenanggungjawab = item.PerPenanggungjawab;
                        newRow.AlamatperPenanggungjawab = item.AlamatperPenanggungjawab;
                        newRow.NpwpdPenanggungjawab = item.NpwpdPenanggungjawab;
                        newRow.Potensi = item.Potensi;
                        newRow.Flagmall = item.Flagmall;
                        newRow.Flagjeda = item.Flagjeda;
                        newRow.Flagbranded = item.Flagbranded;
                        newRow.Nlpr = item.Nlpr;
                        newRow.Username = item.Username;
                        newRow.JenisWp = item.JenisWp;
                        newRow.TglCetakPer = item.TglCetakPer;
                        newRow.StatusAWp = item.StatusAWp;
                        newRow.StatusAPer = item.StatusAPer;
                        newRow.Nmkelurahan = item.Nmkelurahan;
                        newRow.Nop = item.Nop;
                        newRow.UnitKerja = item.UnitKerja;
                        newRow.UnitBerkas = item.UnitBerkas;
                        newRow.StatusVer = item.StatusVer;
                        newRow.TglVer = item.TglVer;
                        newRow.UserVer = item.UserVer;
                        newRow.TahunBuku = item.TglMulaiBerlaku.HasValue ? item.TglMulaiBerlaku.Value.Year : DateTime.Now.Year;
                        newRow.TglOpTutup = item.TglOpTutup;
                        newRow.KategoriId = item.KategoriId;

                        _contMonPd.DbOpReklames.Add(newRow);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{DateTime.Now} [PROCESS] DB_OP_REKLAME_MONITORINGDB {item.NoFormulir}");
                        Console.ResetColor();
                    }

                    _contMonPd.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} [FINISHED] DB_OP_REKLAME_MONITORINGDB");
                    Console.ResetColor();
                }
            }

            // do fill realisasi
            var _contMonitoringDb2 = DBClass.GetMonitoringDbContext();
            var opList = _contMonPd.DbOpReklames.ToList();
            var sql2 = @"
SELECT 	A.NO_FORMULIR,
		NO_SKPD ID_KETETAPAN,
		TGLSKPD TGLPENETAPAN,
		EXTRACT(YEAR FROM TGLSKPD) AS TAHUN_PAJAK,
		EXTRACT(MONTH FROM TGLSKPD) AS BULAN_PAJAK,
        A.PAJAKLB PAJAK_POKOK,
		1 JNS_KETETAPAN,
		TGL_JTEMPO_SKPD,
		'-' AKUN,
		'-' NAMA_AKUN,
		'-' KELOMPOK,
		'-' NAMA_KELOMPOK,
		'-' JENIS,
		'-' NAMA_JENIS,
		'-' OBJEK,
		'-' NAMA_OBJEK,
		'-' RINCIAN,
		'-' NAMA_RINCIAN,
		'-' SUB_RINCIAN,
		'-' NAMA_SUB_RINCIAN,
		EXTRACT(YEAR FROM TGLSKPD) AS TAHUN_PAJAK_KETETAPAN,
		EXTRACT(MONTH FROM TGLSKPD) AS MASA_PAJAK_KETETAPAN,
		1 SEQ_PAJAK_KETETAPAN,
		JENISSURAT KATEGORI_KETETAPAN,
		TGLSKPD TGL_KETETAPAN,
		TGL_JTEMPO_SKPD TGL_JATUH_TEMPO_BAYAR,
		STATUS_BERKAS IS_LUNAS_KETETAPAN,
		A.TGL_BAYAR TGL_LUNAS_KETETAPAN,
		PAJAK POKOK_PAJAK_KETETAPAN,
		0 PENGURANG_POKOK_KETETAPAN,
		'-' AKUN_KETETAPAN,
		'-' KELOMPOK_KETETAPAN,
		'-' JENIS_KETETAPAN,
		'-' OBJEK_KETETAPAN,
		'-' RINCIAN_KETETAPAN,
		'-' SUB_RINCIAN_KETETAPAN,
		B.TGL_BAYAR TGL_BAYAR_POKOK,
		B.REALISASI NOMINAL_POKOK_BAYAR,
		B.TGL_BAYAR TGL_BAYAR_SANKSI,
		B.SANKSI NOMINAL_SANKSI_BAYAR,
		'-' AKUN_SANKSI_BAYAR,
		'-' KELOMPOK_SANKSI_BAYAR,
		'-' JENIS_SANKSI_BAYAR,
		'-' OBJEK_SANKSI_BAYAR,
		'-' RINCIAN_SANKSI_BAYAR,
		'-' SUB_RINCIAN_SANKSI_BAYAR,
		B.TGL_BAYAR TGL_BAYAR_SANKSI_KENAIKAN,
		B.JAMBONG NOMINAL_JAMBONG_BAYAR,
		'-' AKUN_JAMBONG_BAYAR,
		'-' KELOMPOK_JAMBONG_BAYAR,
		'-' JENIS_JAMBONG_BAYAR,
		'-' OBJEK_JAMBONG_BAYAR,
		'-' RINCIAN_JAMBONG_BAYAR,
		'-' SUB_RINCIAN_JAMBONG_BAYAR,
		sysdate INS_dATE, 
		'JOB' INS_BY,
		sysdate UPD_DATE, 
		'JOB' UPD_BY,
		NO_SKPD NO_KETETAPAN
FROM VW_KETETAPAN_NRC@lihatreklame A
LEFT JOIN (
	SELECT NO_FORMULIR, TGL_BAYAR, SUM(PAJAKLB) REALISASI, SUM(SANKSI) SANKSI, SUM(JAMBONG) JAMBONG
	FROM VW_REALISASI_NRC@lihatreklame
    WHERE TGL_BAYAR >= SYSDATE - INTERVAL '4' YEAR
	GROUP BY NO_FORMULIR, TGL_BAYAR
) B ON A.NO_FORMULIR = B.NO_FORMULIR AND A.TGL_BAYAR = B.TGL_BAYAR
WHERE A.TGLSKPD >= SYSDATE - INTERVAL '4' YEAR
";

            var pembayaranSspdList = await _contMonitoringDb2.Set<OpSkpdSspdReklame>()
                .FromSqlRaw(sql2)
                .ToListAsync();

            var opList2 = await _contMonPd.DbOpReklames.ToListAsync();

            var existing = await _contMonPd.DbMonReklames.ToListAsync();
            _contMonPd.DbMonReklames.RemoveRange(existing);

            foreach (var item in pembayaranSspdList)
            {
                var newRow = new DbMonReklame();

                var op = opList2.Where(x => x.NoFormulir == item.NO_FORMULIR).FirstOrDefault();
                if(op != null)
                {
                    newRow.NoFormulir = op.NoFormulir ;
                    newRow.NoPerusahaan = op.NoPerusahaan ;
                    newRow.NamaPerusahaan = op.NamaPerusahaan ;
                    newRow.AlamatPerusahaan = op.AlamatPerusahaan ;
                    newRow.NoAlamatPerusahaan = op.NoAlamatPerusahaan ;
                    newRow.BlokAlamatPerusahaan = op.BlokAlamatPerusahaan ;
                    newRow.Alamatper = op.Alamatper ;
                    newRow.TelpPerusahaan = op.TelpPerusahaan ;
                    newRow.Clientnama = op.Clientnama ;
                    newRow.Clientalamat = op.Clientalamat ;
                    newRow.Jabatan = op.Jabatan ;
                    newRow.KodeJenis = op.KodeJenis ;
                    newRow.NmJenis = op.NamaJenis ;
                    newRow.NoWp = op.NoWp ;
                    newRow.Nama = op.Nama ;
                    newRow.Alamat = op.Alamat ;
                    newRow.NoAlamat = op.NoAlamat ;
                    newRow.BlokAlamat = op.BlokAlamat ;
                    newRow.Alamatwp = op.Alamatwp ;
                    newRow.JenisPermohonan = op.JenisPermohonan ;
                    newRow.TglPermohonan = op.TglPermohonan ;
                    newRow.TglMulaiBerlaku = op.TglMulaiBerlaku ;
                    newRow.TglAkhirBerlaku = op.TglAkhirBerlaku ;
                    newRow.NamaJalan = op.NamaJalan ;
                    newRow.NoJalan = op.NoJalan ;
                    newRow.BlokJalan = op.BlokJalan ;
                    newRow.Alamatreklame = op.Alamatreklame ;
                    newRow.DetilLokasi = op.DetilLokasi ;
                    newRow.Kecamatan = op.Kecamatan ;
                    newRow.JenisProduk = op.JenisProduk ;
                    newRow.LetakReklame = op.LetakReklame ;
                    newRow.StatusTanah = op.StatusTanah ;
                    newRow.FlagPermohonan = op.FlagPermohonan ;
                    newRow.Statusproses = op.Statusproses ;
                    newRow.FlagSimpatik = op.FlagSimpatik ;
                    newRow.KodeObyek = op.KodeObyek ;
                    newRow.Panjang = op.Panjang ;
                    newRow.Lebar = op.Lebar ;
                    newRow.Luas = op.Luas ;
                    newRow.Luasdiskon = op.Luasdiskon ;
                    newRow.Sisi = op.Sisi ;
                    newRow.Ketinggian = op.Ketinggian ;
                    newRow.IsiReklame = op.IsiReklame ;
                    newRow.PermohonanBaru = op.PermohonanBaru ;
                    newRow.NoFormulirLama = op.NoFormulirLama ;
                    newRow.SudutPandang = op.SudutPandang ;
                    newRow.Nilaipajak = op.Nilaipajak ;
                    newRow.Nilaijambong = op.Nilaijambong ;
                    newRow.KelasJalan = op.KelasJalan ;
                    newRow.NoTelp = op.NoTelp ;
                    newRow.Timetrans = op.Timetrans ;
                    newRow.Npwpd = op.Npwpd ;
                    newRow.Flagtung = op.Flagtung ;
                    newRow.Statuscabut = op.Statuscabut ;
                    newRow.Nor = op.Nor ;
                    newRow.KodeLokasi = op.KodeLokasi ;
                    newRow.NamaPenempatan = op.NamaPenempatan ;
                    newRow.NoFormulirAwal = op.NoFormulirAwal ;
                    newRow.Ketpersil = op.Ketpersil ;
                    newRow.PerPenanggungjawab = op.PerPenanggungjawab ;
                    newRow.AlamatperPenanggungjawab = op.AlamatperPenanggungjawab ;
                    newRow.NpwpdPenanggungjawab = op.NpwpdPenanggungjawab ;
                    newRow.Potensi = op.Potensi ;
                    newRow.Flagmall = op.Flagmall ;
                    newRow.Flagjeda = op.Flagjeda ;
                    newRow.Flagbranded = op.Flagbranded ;
                    newRow.Nlpr = op.Nlpr ;
                    newRow.Username = op.Username ;
                    newRow.JenisWp = op.JenisWp ;
                    newRow.TglCetakPer = op.TglCetakPer ;
                    newRow.StatusAWp = op.StatusAWp ;
                    newRow.StatusAPer = op.StatusAPer ;
                    newRow.Nmkelurahan = op.Nmkelurahan ;
                    newRow.Nop = op.Nop ;
                    newRow.UnitKerja = op.UnitKerja ;
                    newRow.UnitBerkas = op.UnitBerkas ;
                    newRow.StatusVer = op.StatusVer ;
                    newRow.TglVer = op.TglVer ;
                    newRow.UserVer = op.UserVer ;
                    newRow.TahunBuku = op.TahunBuku;
                    newRow.IdKetetapan = item.ID_KETETAPAN;
                    newRow.Tglpenetapan = item.TGLPENETAPAN;
                    newRow.TahunPajak = item.TAHUN_PAJAK.ToString();
                    newRow.BulanPajak = item.BULAN_PAJAK.ToString();
                    newRow.PajakPokok = item.PAJAK_POKOK;
                    newRow.JnsKetetapan = item.JENIS_KETETAPAN;
                    newRow.TglJtempoSkpd = item.TGL_JTEMPO_SKPD;
                    newRow.Akun = item.AKUN ?? "-";
                    newRow.NamaAkun = item.NAMA_AKUN ?? "-";
                    newRow.Kelompok = item.KELOMPOK ?? "-";
                    newRow.NamaKelompok = item.NAMA_KELOMPOK ?? "-";
                    newRow.Jenis = item.JENIS ?? "-";
                    newRow.NamaJenis = item.NAMA_JENIS ?? "-";
                    newRow.Objek = item.OBJEK ?? "-";
                    newRow.NamaObjek = item.NAMA_OBJEK ?? "-";
                    newRow.Rincian = item.RINCIAN ?? "-";
                    newRow.NamaRincian = item.NAMA_RINCIAN ?? "-";
                    newRow.SubRincian = item.SUB_RINCIAN ?? "-";
                    newRow.NamaSubRincian = item.NAMA_SUB_RINCIAN ?? "-";
                    newRow.TahunPajakKetetapan = item.TAHUN_PAJAK_KETETAPAN ?? 0;
                    newRow.MasaPajakKetetapan = item.MASA_PAJAK_KETETAPAN ?? 0;
                    newRow.SeqPajakKetetapan = item.SEQ_PAJAK_KETETAPAN ?? 0;
                    newRow.KategoriKetetapan = item.KATEGORI_KETETAPAN;
                    newRow.TglKetetapan = item.TGL_KETETAPAN;
                    newRow.TglJatuhTempoBayar = item.TGL_JATUH_TEMPO_BAYAR;
                    newRow.IsLunasKetetapan = item.IS_LUNAS_KETETAPAN;
                    newRow.TglLunasKetetapan = item.TGL_LUNAS_KETETAPAN;
                    newRow.PokokPajakKetetapan = item.POKOK_PAJAK_KETETAPAN;
                    newRow.PengurangPokokKetetapan = item.PENGURANG_POKOK_KETETAPAN;
                    newRow.AkunKetetapan = item.AKUN_KETETAPAN;
                    newRow.KelompokKetetapan = item.KELOMPOK_KETETAPAN;
                    newRow.JenisKetetapan = item.JENIS_KETETAPAN;
                    newRow.ObjekKetetapan = item.OBJEK_KETETAPAN;
                    newRow.RincianKetetapan = item.RINCIAN_KETETAPAN;
                    newRow.SubRincianKetetapan = item.SUB_RINCIAN_KETETAPAN;
                    newRow.TglBayarPokok = item.TGL_BAYAR_POKOK;
                    newRow.NominalPokokBayar = item.NOMINAL_POKOK_BAYAR;
                    newRow.AkunPokokBayar = item.AKUN;
                    newRow.KelompokPokokBayar = item.KELOMPOK;
                    newRow.JenisPokokBayar = item.JENIS;
                    newRow.ObjekPokokBayar = item.OBJEK;
                    newRow.RincianPokokBayar = item.RINCIAN;
                    newRow.SubRincianPokokBayar = item.SUB_RINCIAN;
                    newRow.TglBayarSanksi = item.TGL_BAYAR_SANKSI;
                    newRow.NominalSanksiBayar = item.NOMINAL_SANKSI_BAYAR;
                    newRow.AkunSanksiBayar = item.AKUN_SANKSI_BAYAR;
                    newRow.KelompokSanksiBayar = item.KELOMPOK_SANKSI_BAYAR;
                    newRow.JenisSanksiBayar = item.JENIS_SANKSI_BAYAR;
                    newRow.ObjekSanksiBayar = item.OBJEK_SANKSI_BAYAR;
                    newRow.RincianSanksiBayar = item.RINCIAN_SANKSI_BAYAR;
                    newRow.SubRincianSanksiBayar = item.SUB_RINCIAN_SANKSI_BAYAR;
                    newRow.TglBayarSanksiKenaikan = item.TGL_BAYAR_SANKSI_KENAIKAN;
                    newRow.NominalJambongBayar = item.NOMINAL_JAMBONG_BAYAR;
                    newRow.AkunJambongBayar = item.AKUN_JAMBONG_BAYAR;
                    newRow.KelompokJambongBayar = item.KELOMPOK_JAMBONG_BAYAR;
                    newRow.JenisJambongBayar = item.JENIS_JAMBONG_BAYAR;
                    newRow.ObjekJambongBayar = item.OBJEK_JAMBONG_BAYAR;
                    newRow.RincianJambongBayar = item.RINCIAN_JAMBONG_BAYAR;
                    newRow.SubRincianJambongBayar = item.SUB_RINCIAN_JAMBONG_BAYAR;
                    newRow.InsDate = DateTime.Now;
                    newRow.InsBy = item.INS_BY ?? "JOB";
                    newRow.UpdDate = DateTime.Now;
                    newRow.UpdBy = item.UPD_BY ?? "JOB";
                    newRow.NoKetetapan = item.NO_KETETAPAN ?? "-";


                    _contMonPd.DbMonReklames.Add(newRow);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{DateTime.Now} [PROCESS] DB_MON_REKLAME_MONITORINGDB {item.NO_FORMULIR}");
                    Console.ResetColor();
                }
            }

            _contMonPd.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} [SUCCESS] DB_MON_REKLAME_MONITORINGDB");
            Console.ResetColor();

            MailHelper.SendMail(
            false,
            "DONE REKLAME WS",
            $@"REKLAME WS FINISHED",
            null
            );
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPREKLAME.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPREKLAME.ToString().ToUpper();
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
