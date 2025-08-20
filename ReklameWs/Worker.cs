using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace ReklameWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 7;
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
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

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
KATEGORI_NAMA,
TGL_MULAI_BUKA_OP,
ROWNUM SEQ
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
    TGL_MULAI_BERLAKU AS TGL_MULAI_BUKA_OP,
    60 KATEGORI_ID,
    'PERMANEN' KATEGORI_NAMA,
	NOR
	FROM VWTABELPERMOHONAN@lihatreklame
	WHERE EXTRACT(YEAR FROM TGL_PERMOHONAN) = EXTRACT(YEAR FROM SYSDATE)
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
TGL_MULAI_BERLAKU AS TGL_MULAI_BUKA_OP,
    59 KATEGORI_ID,
    'INSIDENTIL' KATEGORI_NAMA,
	CAST(NULL AS VARCHAR(500)) AS NOR
	FROM VWTABELPERMOHONANINS@lihatreklame
	WHERE EXTRACT(YEAR FROM TGL_PERMOHONAN) = EXTRACT(YEAR FROM SYSDATE)
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
TGL_MULAI_BERLAKU AS TGL_MULAI_BUKA_OP,
    61 KATEGORI_ID,
    'TERBATAS' KATEGORI_NAMA,
	NOR
	FROM VWPERMOHONANSIMRLAMA1@lihatreklame
	WHERE EXTRACT(YEAR FROM TGL_PERMOHONAN) = EXTRACT(YEAR FROM SYSDATE)
) A

                    ";

                    var result = await _contMonitoringDb.Set<DbOpReklame>().FromSqlRaw(sql).ToListAsync();

                    var existingr = await _contMonPd.DbOpReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToListAsync();
                    _contMonPd.DbOpReklames.RemoveRange(existingr);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} DELETE DB_OP_REKLAME_MONITORINGDB (pending commit)");
                    Console.ResetColor();

                    int process = 0;
                    int totalDataa = result.Count;
                    int batchSize = 500; // jumlah data per batch

                    var allNewRows = new List<MonPDLib.EF.DbOpReklame>();


                    for (int i = 0; i < totalDataa; i += batchSize)
                    {
                        var chunk = result.Skip(i).Take(batchSize).ToList();

                        foreach (var item in chunk)
                        {
                            var newRow = new MonPDLib.EF.DbOpReklame
                            {
                                NoFormulir = item.NoFormulir,
                                NoPerusahaan = item.NoPerusahaan,
                                NamaPerusahaan = item.NamaPerusahaan,
                                AlamatPerusahaan = item.AlamatPerusahaan,
                                NoAlamatPerusahaan = item.NoAlamatPerusahaan,
                                BlokAlamatPerusahaan = item.BlokAlamatPerusahaan,
                                Alamatper = item.Alamatper,
                                TelpPerusahaan = item.TelpPerusahaan,
                                Clientnama = item.Clientnama,
                                Clientalamat = item.Clientalamat,
                                Jabatan = item.Jabatan,
                                KodeJenis = item.KodeJenis,
                                NamaJenis = item.NamaJenis,
                                NoWp = item.NoWp,
                                Nama = item.Nama,
                                Alamat = item.Alamat,
                                NoAlamat = item.NoAlamat,
                                BlokAlamat = item.BlokAlamat,
                                Alamatwp = item.Alamatwp,
                                JenisPermohonan = item.JenisPermohonan,
                                TglPermohonan = item.TglPermohonan,
                                TglMulaiBerlaku = item.TglMulaiBerlaku,
                                TglAkhirBerlaku = item.TglAkhirBerlaku,
                                NamaJalan = item.NamaJalan,
                                NoJalan = item.NoJalan,
                                BlokJalan = item.BlokJalan,
                                Alamatreklame = item.Alamatreklame,
                                DetilLokasi = item.DetilLokasi,
                                Kecamatan = item.Kecamatan,
                                JenisProduk = item.JenisProduk,
                                LetakReklame = item.LetakReklame,
                                StatusTanah = item.StatusTanah,
                                FlagPermohonan = item.FlagPermohonan,
                                Statusproses = item.Statusproses,
                                FlagSimpatik = item.FlagSimpatik,
                                KodeObyek = item.KodeObyek,
                                Panjang = item.Panjang,
                                Lebar = item.Lebar,
                                Luas = item.Luas,
                                Luasdiskon = item.Luasdiskon,
                                Sisi = item.Sisi,
                                Ketinggian = item.Ketinggian,
                                IsiReklame = item.IsiReklame,
                                PermohonanBaru = item.PermohonanBaru,
                                NoFormulirLama = item.NoFormulirLama,
                                SudutPandang = item.SudutPandang,
                                Nilaipajak = item.Nilaipajak,
                                Nilaijambong = item.Nilaijambong,
                                KelasJalan = item.KelasJalan,
                                NoTelp = item.NoTelp,
                                Timetrans = item.Timetrans,
                                Npwpd = item.Npwpd,
                                Flagtung = item.Flagtung,
                                Statuscabut = item.Statuscabut,
                                Nor = item.Nor,
                                KodeLokasi = item.KodeLokasi,
                                NamaPenempatan = item.NamaPenempatan,
                                NoFormulirAwal = item.NoFormulirAwal,
                                Ketpersil = item.Ketpersil,
                                PerPenanggungjawab = item.PerPenanggungjawab,
                                AlamatperPenanggungjawab = item.AlamatperPenanggungjawab,
                                NpwpdPenanggungjawab = item.NpwpdPenanggungjawab,
                                Potensi = item.Potensi,
                                Flagmall = item.Flagmall,
                                Flagjeda = item.Flagjeda,
                                Flagbranded = item.Flagbranded,
                                Nlpr = item.Nlpr,
                                Username = item.Username,
                                JenisWp = item.JenisWp,
                                TglCetakPer = item.TglCetakPer,
                                StatusAWp = item.StatusAWp,
                                StatusAPer = item.StatusAPer,
                                Nmkelurahan = item.Nmkelurahan,
                                Nop = item.Nop,
                                UnitKerja = item.UnitKerja,
                                UnitBerkas = item.UnitBerkas,
                                StatusVer = item.StatusVer,
                                TglVer = item.TglVer,
                                UserVer = item.UserVer,
                                TahunBuku = item.TglMulaiBerlaku.HasValue ? item.TglMulaiBerlaku.Value.Year : DateTime.Now.Year,
                                TglOpTutup = item.TglOpTutup,
                                KategoriId = item.KategoriId,
                                Seq = item.Seq
                            };

                            allNewRows.Add(newRow);
                            process++;
                        }

                        double percent = (process / (double)totalDataa) * 100;
                        Console.WriteLine($"\rProgress Reklame OP: {percent:F2}% ({process}/{totalDataa})");
                    }


                    _contMonPd.DbOpReklames.AddRange(allNewRows);
                    // Sekali save di akhir
                    await _contMonPd.SaveChangesAsync();
                    _contMonPd.ChangeTracker.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} [FINISHED] DB_OP_REKLAME_MONITORINGDB");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} [FINISHED] DB_OP_REKLAME_MONITORINGDB");
                    Console.ResetColor();
                }
            }

            // do fill realisasi
            using (var _contMonitoringDb2 = DBClass.GetMonitoringDbContext())
            {
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
		NO_SKPD NO_KETETAPAN,
        ROWNUM SEQ
FROM VW_KETETAPAN_NRC@lihatreklame A
LEFT JOIN (
	SELECT NO_FORMULIR, TGL_BAYAR, SUM(PAJAKLB) REALISASI, SUM(SANKSI) SANKSI, SUM(JAMBONG) JAMBONG
	FROM VW_REALISASI_NRC@lihatreklame
    WHERE EXTRACT(YEAR FROM TGL_BAYAR) = EXTRACT(YEAR FROM SYSDATE)
	GROUP BY NO_FORMULIR, TGL_BAYAR
) B ON A.NO_FORMULIR = B.NO_FORMULIR AND A.TGL_BAYAR = B.TGL_BAYAR
WHERE EXTRACT(YEAR FROM A.TGLSKPD) = EXTRACT(YEAR FROM SYSDATE)
";

                var pembayaranSspdList = await _contMonitoringDb2
                .Set<OpSkpdSspdReklame>()
                .FromSqlRaw(sql2)
                .ToListAsync();

                // --- 2. Ambil data Op Reklame ---
                var opList2 = await _contMonPd.DbOpReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToListAsync();

                // --- 3. Kosongkan tabel Monitoring ---
                var existing = await _contMonPd.DbMonReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToListAsync();
                _contMonPd.DbMonReklames.RemoveRange(existing);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} DELETE DB_MON_REKLAME_MONITORINGDB (pending commit)");
                Console.ResetColor();

                // --- 4. Insert ulang data ---
                int processed = 0;
                int totalData = pembayaranSspdList.Count;

                var newRows = new List<DbMonReklame>();

                foreach (var item in pembayaranSspdList)
                {
                    var newRow = new DbMonReklame();

                    var op = opList2.FirstOrDefault(x => x.NoFormulir == item.NO_FORMULIR);
                    if (op != null)
                    {
                        // --- mapping semua field OP Reklame (seperti kode Anda) ---
                        newRow.NoFormulir = op.NoFormulir;
                        newRow.NoPerusahaan = op.NoPerusahaan;
                        newRow.NamaPerusahaan = op.NamaPerusahaan;
                        newRow.AlamatPerusahaan = op.AlamatPerusahaan;
                        newRow.NoAlamatPerusahaan = op.NoAlamatPerusahaan;
                        newRow.BlokAlamatPerusahaan = op.BlokAlamatPerusahaan;
                        newRow.Alamatper = op.Alamatper;
                        newRow.TelpPerusahaan = op.TelpPerusahaan;
                        newRow.Clientnama = op.Clientnama;
                        newRow.Clientalamat = op.Clientalamat;
                        newRow.Jabatan = op.Jabatan;
                        newRow.KodeJenis = op.KodeJenis;
                        newRow.NmJenis = op.NamaJenis;
                        newRow.NoWp = op.NoWp;
                        newRow.Nama = op.Nama;
                        newRow.Alamat = op.Alamat;
                        newRow.NoAlamat = op.NoAlamat;
                        newRow.BlokAlamat = op.BlokAlamat;
                        newRow.Alamatwp = op.Alamatwp;
                        newRow.JenisPermohonan = op.JenisPermohonan;
                        newRow.TglPermohonan = op.TglPermohonan;
                        newRow.TglMulaiBerlaku = op.TglMulaiBerlaku;
                        newRow.TglAkhirBerlaku = op.TglAkhirBerlaku;
                        newRow.NamaJalan = op.NamaJalan;
                        newRow.NoJalan = op.NoJalan;
                        newRow.BlokJalan = op.BlokJalan;
                        newRow.Alamatreklame = op.Alamatreklame;
                        newRow.DetilLokasi = op.DetilLokasi;
                        newRow.Kecamatan = op.Kecamatan;
                        newRow.JenisProduk = op.JenisProduk;
                        newRow.LetakReklame = op.LetakReklame;
                        newRow.StatusTanah = op.StatusTanah;
                        newRow.FlagPermohonan = op.FlagPermohonan;
                        newRow.Statusproses = op.Statusproses;
                        newRow.FlagSimpatik = op.FlagSimpatik;
                        newRow.KodeObyek = op.KodeObyek;
                        newRow.Panjang = op.Panjang;
                        newRow.Lebar = op.Lebar;
                        newRow.Luas = op.Luas;
                        newRow.Luasdiskon = op.Luasdiskon;
                        newRow.Sisi = op.Sisi;
                        newRow.Ketinggian = op.Ketinggian;
                        newRow.IsiReklame = op.IsiReklame;
                        newRow.PermohonanBaru = op.PermohonanBaru;
                        newRow.NoFormulirLama = op.NoFormulirLama;
                        newRow.SudutPandang = op.SudutPandang;
                        newRow.Nilaipajak = op.Nilaipajak;
                        newRow.Nilaijambong = op.Nilaijambong;
                        newRow.KelasJalan = op.KelasJalan;
                        newRow.NoTelp = op.NoTelp;
                        newRow.Timetrans = op.Timetrans;
                        newRow.Npwpd = op.Npwpd;
                        newRow.Flagtung = op.Flagtung;
                        newRow.Statuscabut = op.Statuscabut;
                        newRow.Nor = op.Nor;
                        newRow.KodeLokasi = op.KodeLokasi;
                        newRow.NamaPenempatan = op.NamaPenempatan;
                        newRow.NoFormulirAwal = op.NoFormulirAwal;
                        newRow.Ketpersil = op.Ketpersil;
                        newRow.PerPenanggungjawab = op.PerPenanggungjawab;
                        newRow.AlamatperPenanggungjawab = op.AlamatperPenanggungjawab;
                        newRow.NpwpdPenanggungjawab = op.NpwpdPenanggungjawab;
                        newRow.Potensi = op.Potensi;
                        newRow.Flagmall = op.Flagmall;
                        newRow.Flagjeda = op.Flagjeda;
                        newRow.Flagbranded = op.Flagbranded;
                        newRow.Nlpr = op.Nlpr;
                        newRow.Username = op.Username;
                        newRow.JenisWp = op.JenisWp;
                        newRow.TglCetakPer = op.TglCetakPer;
                        newRow.StatusAWp = op.StatusAWp;
                        newRow.StatusAPer = op.StatusAPer;
                        newRow.Nmkelurahan = op.Nmkelurahan;
                        newRow.Nop = op.Nop;
                        newRow.UnitKerja = op.UnitKerja;
                        newRow.UnitBerkas = op.UnitBerkas;
                        newRow.StatusVer = op.StatusVer;
                        newRow.TglVer = op.TglVer;
                        newRow.UserVer = op.UserVer;
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
                        newRow.Seq = item.SEQ;

                        newRows.Add(newRow);
                        processed++;
                        double percent = (processed / (double)totalData) * 100;
                        Console.Write($"\rProgress Reklame MON_SSPD: {percent:F2}% ({processed}/{totalData})");

                    }
                }

                // Masukkan semua data sekaligus
                _contMonPd.DbMonReklames.AddRange(newRows);

                // Sekali commit di akhir
                await _contMonPd.SaveChangesAsync();
                _contMonPd.ChangeTracker.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{DateTime.Now} [SUCCESS] DB_MON_REKLAME_MONITORINGDB");
                Console.ResetColor();
            }

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
