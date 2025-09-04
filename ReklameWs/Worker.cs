using Dapper;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Formats.Tar;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace ReklameWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 7;
        private static EnumFactory.EPajak PAJAK_ENUM = EnumFactory.EPajak.Reklame;
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
                    nextRun = now.AddHours(2); // next jam 00:00
                    delay = nextRun - now;
                }


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
                    "ERROR REKLAME_SURAT WS",
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
                OpProcess();
            }

            // do fill realisasi
            RealisasiProcess();

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                UpdateKoreksi(i);
            }

            MailHelper.SendMail(
            false,
            "DONE REKLAME WS",
            $@"REKLAME WS FINISHED",
            null
            );
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
                    var tanggal = DateTime.Now.Date;
                    if (tahunBuku < DateTime.Now.Year)
                    {
                        tanggal = new DateTime(tahunBuku, 12, 31);
                    }

                    var source = context.DbOpReklames.FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahunBuku);
                    if (source != null)
                    {
                        source.Nama = namaop;
                        source.TahunBuku = tahunBuku;

                        context.DbOpReklames.Update(source);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbOpReklame();

                        newRow.NoFormulir = nop;
                        newRow.NoPerusahaan = 0;
                        newRow.NamaPerusahaan = "-";
                        newRow.AlamatPerusahaan = "-";
                        newRow.NoAlamatPerusahaan = "-";
                        newRow.BlokAlamatPerusahaan = "-";
                        newRow.Alamatper = "-";
                        newRow.TelpPerusahaan = "-";
                        newRow.Clientnama = "-";
                        newRow.Clientalamat = "-";
                        newRow.Jabatan = "-";
                        newRow.KodeJenis = "-";
                        newRow.NamaJenis = "-";
                        newRow.NoWp = nop;
                        newRow.Nama = namaop;
                        newRow.Alamat = "-";
                        newRow.NoAlamat = "-";
                        newRow.BlokAlamat = "-";
                        newRow.Alamatwp = "-";
                        newRow.JenisPermohonan = "-";
                        newRow.TglPermohonan = tanggal;
                        newRow.TglMulaiBerlaku = tanggal;
                        newRow.TglAkhirBerlaku = tanggal;
                        newRow.NamaJalan = "-";
                        newRow.NoJalan = "-";
                        newRow.BlokJalan = "-";
                        newRow.Alamatreklame = "-";
                        newRow.DetilLokasi = "-";
                        newRow.Kecamatan = "-";
                        newRow.JenisProduk = "-";
                        newRow.LetakReklame = "-";
                        newRow.StatusTanah = "-";
                        newRow.FlagPermohonan = "-";
                        newRow.Statusproses = "-";
                        newRow.FlagSimpatik = "-";
                        newRow.KodeObyek = "-";
                        newRow.Panjang = 0;
                        newRow.Lebar = 0;
                        newRow.Luas = 0;
                        newRow.Luasdiskon = 0;
                        newRow.Sisi = 0;
                        newRow.Ketinggian = 0;
                        newRow.IsiReklame = "-";
                        newRow.PermohonanBaru = "-";
                        newRow.NoFormulirLama = "-";
                        newRow.SudutPandang = 0;
                        newRow.Nilaipajak = 0;
                        newRow.Nilaijambong = 0;
                        newRow.KelasJalan = "-";
                        newRow.NoTelp = "-";
                        newRow.Timetrans = null;
                        newRow.Npwpd = "-";
                        newRow.Flagtung = "-";
                        newRow.Statuscabut = "-";
                        newRow.Nor = "-";
                        newRow.KodeLokasi = "-";
                        newRow.NamaPenempatan = "-";
                        newRow.NoFormulirAwal = "-";
                        newRow.Ketpersil = "-";
                        newRow.PerPenanggungjawab = "-";
                        newRow.AlamatperPenanggungjawab = "-";
                        newRow.NpwpdPenanggungjawab = "-";
                        newRow.Potensi = "-";
                        newRow.Flagmall = "-";
                        newRow.Flagjeda = "-";
                        newRow.Flagbranded = "-";
                        newRow.Nlpr = "-";
                        newRow.Username = "-";
                        newRow.JenisWp = "-";
                        newRow.TglCetakPer = null;
                        newRow.StatusAWp = 0;
                        newRow.StatusAPer = 0;
                        newRow.Nmkelurahan = "-";
                        newRow.Nop = nop;
                        newRow.UnitKerja = "-";
                        newRow.UnitBerkas = "-";
                        newRow.StatusVer = 0;
                        newRow.TglVer = null;
                        newRow.UserVer = "-";
                        newRow.TahunBuku = tahunBuku;
                        newRow.Seq = 0;
                        newRow.TglOpTutup = null;
                        newRow.KategoriId = 0;
                        newRow.TglMulaiBukaOp = null;
                        newRow.KategoriNama = "-";


                        context.DbOpReklames.Add(newRow);
                        context.SaveChanges();
                    }


                    source = context.DbOpReklames.FirstOrDefault(x => x.NoFormulir == nop && x.TahunBuku == tahunBuku);
                    if (source == null)
                    {
                        throw new Exception("Gagal membuat data OP untuk koreksi scontro");
                    }
                    var sourceMon = context.DbMonReklames.Where(x => x.NoFormulir == nop && x.TahunBuku == tahunBuku).FirstOrDefault();
                    if (sourceMon != null)
                    {
                        sourceMon.NominalPokokBayar = selisih;
                        context.DbMonReklames.Update(sourceMon);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newRow = new DbMonReklame();

                        newRow.NoFormulir = nop;
                        newRow.NoPerusahaan = 0;
                        newRow.NamaPerusahaan = "-";
                        newRow.AlamatPerusahaan = "-";
                        newRow.NoAlamatPerusahaan = "-";
                        newRow.BlokAlamatPerusahaan = "-";
                        newRow.Alamatper = "-";
                        newRow.TelpPerusahaan = "-";
                        newRow.Clientnama = "-";
                        newRow.Clientalamat = "-";
                        newRow.Jabatan = "-";
                        newRow.KodeJenis = "-";
                        newRow.NmJenis = "-";
                        newRow.NoWp = nop;
                        newRow.Nama = namaop;
                        newRow.Alamat = "-";
                        newRow.NoAlamat = "-";
                        newRow.BlokAlamat = "-";
                        newRow.Alamatwp = "-";
                        newRow.JenisPermohonan = "-";
                        newRow.TglPermohonan = tanggal;
                        newRow.TglMulaiBerlaku = tanggal;
                        newRow.TglAkhirBerlaku = tanggal;
                        newRow.NamaJalan = "-";
                        newRow.NoJalan = "-";
                        newRow.BlokJalan = "-";
                        newRow.Alamatreklame = "-";
                        newRow.DetilLokasi = "-";
                        newRow.Kecamatan = "-";
                        newRow.JenisProduk = "-";
                        newRow.LetakReklame = "-";
                        newRow.StatusTanah = "-";
                        newRow.FlagPermohonan = "-";
                        newRow.Statusproses = "-";
                        newRow.FlagSimpatik = "-";
                        newRow.KodeObyek = "-";
                        newRow.Panjang = 0;
                        newRow.Lebar = 0;
                        newRow.Luas = 0;
                        newRow.Luasdiskon = 0;
                        newRow.Sisi = 0;
                        newRow.Ketinggian = 0;
                        newRow.IsiReklame = "-";
                        newRow.PermohonanBaru = "-";
                        newRow.NoFormulirLama = "-";
                        newRow.SudutPandang = 0;
                        newRow.Nilaipajak = 0;
                        newRow.Nilaijambong = 0;
                        newRow.KelasJalan = "-";
                        newRow.NoTelp = "-";
                        newRow.Timetrans = null;
                        newRow.Npwpd = "-";
                        newRow.Flagtung = "-";
                        newRow.Statuscabut = "-";
                        newRow.Nor = "-";
                        newRow.KodeLokasi = "-";
                        newRow.NamaPenempatan = "-";
                        newRow.NoFormulirAwal = "-";
                        newRow.Ketpersil = "-";
                        newRow.PerPenanggungjawab = "-";
                        newRow.AlamatperPenanggungjawab = "-";
                        newRow.NpwpdPenanggungjawab = "-";
                        newRow.Potensi = "-";
                        newRow.Flagmall = "-";
                        newRow.Flagjeda = "-";
                        newRow.Flagbranded = "-";
                        newRow.Nlpr = "-";
                        newRow.Username = "-";
                        newRow.JenisWp = "-";
                        newRow.TglCetakPer = null;
                        newRow.StatusAWp = 0;
                        newRow.StatusAPer = 0;
                        newRow.Nmkelurahan = "-";
                        newRow.Nop = nop;
                        newRow.UnitKerja = "-";
                        newRow.UnitBerkas = "-";
                        newRow.StatusVer = 0;
                        newRow.TglVer = null;
                        newRow.UserVer = "-";
                        newRow.TahunBuku = tahunBuku;
                        newRow.IdKetetapan = "-";
                        newRow.Tglpenetapan = null;
                        newRow.TahunPajak = "-";
                        newRow.BulanPajak = "-";
                        newRow.PajakPokok = 0;
                        newRow.JnsKetetapan = "-";
                        newRow.TglJtempoSkpd = null;
                        newRow.Akun = "-";
                        newRow.NamaAkun = "-";
                        newRow.Kelompok = "-";
                        newRow.NamaKelompok = "-";
                        newRow.Jenis = "-";
                        newRow.NamaJenis = "-";
                        newRow.Objek = "-";
                        newRow.NamaObjek = "-";
                        newRow.Rincian = "-";
                        newRow.NamaRincian = "-";
                        newRow.SubRincian = "-";
                        newRow.NamaSubRincian = "-";
                        newRow.TahunPajakKetetapan = 0;
                        newRow.MasaPajakKetetapan = 0;
                        newRow.SeqPajakKetetapan = 0;
                        newRow.KategoriKetetapan = "-";
                        newRow.TglKetetapan = null;
                        newRow.TglJatuhTempoBayar = null;
                        newRow.IsLunasKetetapan = 0;
                        newRow.TglLunasKetetapan = null;
                        newRow.PokokPajakKetetapan = 0;
                        newRow.PengurangPokokKetetapan = 0;
                        newRow.AkunKetetapan = "-";
                        newRow.KelompokKetetapan = "-";
                        newRow.JenisKetetapan = "-";
                        newRow.ObjekKetetapan = "-";
                        newRow.RincianKetetapan = "-";
                        newRow.SubRincianKetetapan = "-";
                        newRow.TglBayarPokok = tanggal;
                        newRow.NominalPokokBayar = selisih;
                        newRow.AkunPokokBayar = "-";
                        newRow.KelompokPokokBayar = "-";
                        newRow.JenisPokokBayar = "-";
                        newRow.ObjekPokokBayar = "-";
                        newRow.RincianPokokBayar = "-";
                        newRow.SubRincianPokokBayar = "-";
                        newRow.TglBayarSanksi = null;
                        newRow.NominalSanksiBayar = 0;
                        newRow.AkunSanksiBayar = "-";
                        newRow.KelompokSanksiBayar = "-";
                        newRow.JenisSanksiBayar = "-";
                        newRow.ObjekSanksiBayar = "-";
                        newRow.RincianSanksiBayar = "-";
                        newRow.SubRincianSanksiBayar = "-";
                        newRow.TglBayarSanksiKenaikan = null;
                        newRow.NominalJambongBayar = 0;
                        newRow.AkunJambongBayar = "-";
                        newRow.KelompokJambongBayar = "-";
                        newRow.JenisJambongBayar = "-";
                        newRow.ObjekJambongBayar = "-";
                        newRow.RincianJambongBayar = "-";
                        newRow.SubRincianJambongBayar = "-";
                        newRow.InsDate = DateTime.MinValue;
                        newRow.InsBy = "-";
                        newRow.UpdDate = DateTime.MinValue;
                        newRow.UpdBy = "-";
                        newRow.NoKetetapan = "-";
                        newRow.Seq = -1;

                        context.DbMonReklames.Add(newRow);
                        context.SaveChanges();
                    }
                }

                Console.WriteLine($"[FINISHED] UpdateKoreksi {tahunBuku}");
            }
        }

        private void OpProcess()
        {
            var _contMonPd = DBClass.GetContext();
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

                var result = _contMonitoringDb.Set<DbOpReklame>().FromSqlRaw(sql).ToList();

                var existingr = _contMonPd.DbOpReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToList();
                _contMonPd.DbOpReklames.RemoveRange(existingr);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} DELETE DB_OP_REKLAME_MONITORINGDB (pending commit)");
                Console.ResetColor();

                int process = 0;
                int totalDataa = result.Count;

                var allNewRows = new List<MonPDLib.EF.DbOpReklame>();

                foreach (var item in result)
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
                        TahunBuku = DateTime.Now.Year,
                        TglOpTutup = item.TglOpTutup,
                        KategoriId = item.KategoriId,
                        Seq = item.Seq
                    };

                    allNewRows.Add(newRow);
                    process++;

                    double percent = (process / (double)totalDataa) * 100;
                    Console.Write($"\rProgress Reklame OP: {percent:F2}% ({process}/{totalDataa})");
                }


                _contMonPd.DbOpReklames.AddRange(allNewRows);
                // Sekali save di akhir

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{DateTime.Now} [SAVING] DB_OP_REKLAME_MONITORINGDB");
                Console.ResetColor();

                _contMonPd.SaveChanges();
                _contMonPd.ChangeTracker.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{DateTime.Now} [FINISHED] DB_OP_REKLAME_MONITORINGDB");
                Console.ResetColor();
            }
        }
        private void RealisasiProcess()
        {
            var _contMonPd = DBClass.GetContext();
            using (var _contMonitoringDb2 = DBClass.GetMonitoringDbContext())
            {
                var opList = _contMonPd.DbOpReklames.ToList();
                var sql2 = @"
WITH REKAP_REALISASI AS (
    SELECT 
        NO_FORMULIR,
        SUM(JAMBONG) AS JAMBONG,
        SUM(SANKSI) AS SANKSI,
        EXTRACT(YEAR FROM TGL_BAYAR) AS TAHUN,
        MAX(TGL_BAYAR) AS TGL_REALISASI,
        SUM(PAJAKLB) AS PAJAK_LB
    FROM VW_REALISASI_NRC@lihatreklame
    GROUP BY NO_FORMULIR, EXTRACT(YEAR FROM TGL_BAYAR)
),
REKAP_KETETAPAN AS (
    SELECT 
        K.NO_FORMULIR,
        K.NO_SKPD AS ID_KETETAPAN,
        K.TGLSKPD AS TGLPENETAPAN,
        SUM(K.PAJAKLB) AS PAJAK_POKOK,
        K.TGL_JTEMPO_SKPD,
        EXTRACT(YEAR FROM K.TGLSKPD) AS TAHUN_PAJAK_KETETAPAN,
        EXTRACT(MONTH FROM K.TGLSKPD) AS MASA_PAJAK_KETETAPAN,
        K.JENISSURAT AS KATEGORI_KETETAPAN,
        K.TGLSKPD AS TGL_KETETAPAN,
        K.STATUS_BERKAS AS IS_LUNAS_KETETAPAN,
        MAX(K.TGL_BAYAR) AS TGL_LUNAS_KETETAPAN,
        SUM(K.PAJAK) AS POKOK_PAJAK_KETETAPAN,
        K.NO_SKPD AS NO_KETETAPAN
    FROM VW_KETETAPAN_NRC@lihatreklame K
    GROUP BY 
        K.NO_FORMULIR,
        K.NO_SKPD,
        K.TGLSKPD,
        K.TGL_JTEMPO_SKPD,
        K.JENISSURAT,
        K.STATUS_BERKAS
)
SELECT 
    K.NO_FORMULIR,
    K.ID_KETETAPAN,
    K.TGLPENETAPAN,
    EXTRACT(YEAR FROM R.TGL_REALISASI) AS TAHUN_PAJAK,
    EXTRACT(MONTH FROM R.TGL_REALISASI) AS BULAN_PAJAK,
    K.PAJAK_POKOK,
    1 AS JNS_KETETAPAN,
    K.TGL_JTEMPO_SKPD,
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
    K.TAHUN_PAJAK_KETETAPAN,
    K.MASA_PAJAK_KETETAPAN,
    1 SEQ_PAJAK_KETETAPAN,
    K.KATEGORI_KETETAPAN,
    K.TGL_KETETAPAN,
    K.IS_LUNAS_KETETAPAN,
    K.TGL_LUNAS_KETETAPAN,
    K.POKOK_PAJAK_KETETAPAN,
    0 PENGURANG_POKOK_KETETAPAN,
	'-' AKUN_KETETAPAN,
	'-' KELOMPOK_KETETAPAN,
	'-' JENIS_KETETAPAN,
	'-' OBJEK_KETETAPAN,
	'-' RINCIAN_KETETAPAN,
	'-' SUB_RINCIAN_KETETAPAN,
    R.TGL_REALISASI AS TGL_BAYAR_POKOK,
    R.PAJAK_LB AS NOMINAL_POKOK_BAYAR,
    R.TGL_REALISASI TGL_BAYAR_SANKSI,
    R.SANKSI AS NOMINAL_SANKSI_BAYAR,
    '-' AKUN_SANKSI_BAYAR,
	'-' KELOMPOK_SANKSI_BAYAR,
	'-' JENIS_SANKSI_BAYAR,
	'-' OBJEK_SANKSI_BAYAR,
	'-' RINCIAN_SANKSI_BAYAR,
	'-' SUB_RINCIAN_SANKSI_BAYAR,
	R.TGL_REALISASI TGL_BAYAR_SANKSI_KENAIKAN,
    R.JAMBONG AS NOMINAL_JAMBONG_BAYAR,
	'-' AKUN_JAMBONG_BAYAR,
	'-' KELOMPOK_JAMBONG_BAYAR,
	'-' JENIS_JAMBONG_BAYAR,
	'-' OBJEK_JAMBONG_BAYAR,
	'-' RINCIAN_JAMBONG_BAYAR,
	'-' SUB_RINCIAN_JAMBONG_BAYAR,
    SYSDATE AS INS_DATE,
    'JOB' AS INS_BY,
    SYSDATE AS UPD_DATE,
    'JOB' AS UPD_BY,
    K.NO_KETETAPAN,
    ROWNUM AS SEQ
FROM REKAP_KETETAPAN K
LEFT JOIN REKAP_REALISASI R 
    ON K.NO_FORMULIR = R.NO_FORMULIR
WHERE R.TAHUN = EXTRACT(YEAR FROM SYSDATE)
";

                var pembayaranSspdList = _contMonitoringDb2
                .Set<OpSkpdSspdReklame>()
                .FromSqlRaw(sql2)
                .ToList();

                // --- 2. Ambil data Op Reklame ---
                var opList2 = _contMonPd.DbOpReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToList();

                // --- 3. Kosongkan tabel Monitoring ---
                var existing = _contMonPd.DbMonReklames.Where(x => x.TahunBuku == DateTime.Now.Year).ToList();
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
                    }
                    else
                    {
                        newRow.NoFormulir = item.NO_FORMULIR;
                        newRow.NoPerusahaan = 0;
                        newRow.NamaPerusahaan = "-";
                        newRow.AlamatPerusahaan = "-";
                        newRow.NoAlamatPerusahaan = "-";
                        newRow.BlokAlamatPerusahaan = "-";
                        newRow.Alamatper = "-";
                        newRow.TelpPerusahaan = "-";
                        newRow.Clientnama = "-";
                        newRow.Clientalamat = "-";
                        newRow.Jabatan = "-";
                        newRow.KodeJenis = "-";
                        newRow.NmJenis = "-";
                        newRow.NoWp = "-";
                        newRow.Nama = "-";
                        newRow.Alamat = "-";
                        newRow.NoAlamat = "-";
                        newRow.BlokAlamat = "-";
                        newRow.Alamatwp = "-";
                        newRow.JenisPermohonan = "-";
                        newRow.TglPermohonan = null;
                        newRow.TglMulaiBerlaku = null;
                        newRow.TglAkhirBerlaku = null;
                        newRow.NamaJalan = "-";
                        newRow.NoJalan = "-";
                        newRow.BlokJalan = "-";
                        newRow.Alamatreklame = "-";
                        newRow.DetilLokasi = "-";
                        newRow.Kecamatan = "-";
                        newRow.JenisProduk = "-";
                        newRow.LetakReklame = "-";
                        newRow.StatusTanah = "-";
                        newRow.FlagPermohonan = "-";
                        newRow.Statusproses = "-";
                        newRow.FlagSimpatik = "-";
                        newRow.KodeObyek = "-";
                        newRow.Panjang = 0;
                        newRow.Lebar = 0;
                        newRow.Luas = 0;
                        newRow.Luasdiskon = 0;
                        newRow.Sisi = 0;
                        newRow.Ketinggian = 0;
                        newRow.IsiReklame = "-";
                        newRow.PermohonanBaru = "-";
                        newRow.NoFormulirLama = "-";
                        newRow.SudutPandang = 0;
                        newRow.Nilaipajak = 0;
                        newRow.Nilaijambong = 0;
                        newRow.KelasJalan = "-";
                        newRow.NoTelp = "-";
                        newRow.Timetrans = null;
                        newRow.Npwpd = "-";
                        newRow.Flagtung = "-";
                        newRow.Statuscabut = "-";
                        newRow.Nor = "-";
                        newRow.KodeLokasi = "-";
                        newRow.NamaPenempatan = "-";
                        newRow.NoFormulirAwal = "-";
                        newRow.Ketpersil = "-";
                        newRow.PerPenanggungjawab = "-";
                        newRow.AlamatperPenanggungjawab = "-";
                        newRow.NpwpdPenanggungjawab = "-";
                        newRow.Potensi = "-";
                        newRow.Flagmall = "-";
                        newRow.Flagjeda = "-";
                        newRow.Flagbranded = "-";
                        newRow.Nlpr = "-";
                        newRow.Username = "-";
                        newRow.JenisWp = "-";
                        newRow.TglCetakPer = null;
                        newRow.StatusAWp = 0;
                        newRow.StatusAPer = 0;
                        newRow.Nmkelurahan = "-";
                        newRow.Nop = "-";
                        newRow.UnitKerja = "-";
                        newRow.UnitBerkas = "-";
                        newRow.StatusVer = 0;
                        newRow.TglVer = null;
                        newRow.UserVer = "-";
                        newRow.TahunBuku = item.TAHUN_PAJAK ?? DateTime.Now.Year;
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
                    }
                    processed++;
                    double percent = (processed / (double)totalData) * 100;
                    Console.Write($"\rProgress Reklame MON_SSPD: {percent:F2}% ({processed}/{totalData})");
                }

                // Masukkan semua data sekaligus
                _contMonPd.DbMonReklames.AddRange(newRows);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{DateTime.Now} [SAVING] DB_MON_REKLAME_MONITORINGDB");
                Console.ResetColor();

                // Sekali commit di akhir
                _contMonPd.SaveChanges();
                _contMonPd.ChangeTracker.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{DateTime.Now} [SUCCESS] DB_MON_REKLAME_MONITORINGDB");
                Console.ResetColor();
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
