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

                var now = DateTime.Now;
                var nextRun = new DateTime(2025, 7, 11, 1, 0, 0); // 11 Juli 2025 jam 02:00

                if (nextRun <= now)
                {
                    _logger.LogInformation("Scheduled time has already passed.");
                    return; // atau break / return tergantung konteksmu
                }

                var delay = nextRun - now;

                _logger.LogInformation("Next run scheduled at: {time}", nextRun);

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

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
                    var sql = @"
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
USER_VER
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
	USER_VER
	FROM VWTABELPERMOHONAN@lihatreklame
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
	NULL USER_VER
	FROM VWTABELPERMOHONANINS@lihatreklame
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
	CAST(FLAG_EX AS VARCHAR(100)) FLAGEXP,
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
	NULL USER_VER
	FROM VWPERMOHONANSIMRLAMA1@lihatreklame
) A

                    ";

                    var result = await _contMonitoringDb.Set<DbOpReklame>().FromSqlRaw(sql).ToListAsync(); //822
                    var source = await _contMonPd.DbOpReklames.ToListAsync();
                    foreach (var item in result)
                    {
                        var sourceRow = source.SingleOrDefault(x => x.NoFormulir == item.NoFormulir);
                        if (sourceRow != null)
                        {
                            
                        }
                        else
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

                            newRow.TahunBuku = 2025;
                            _contMonPd.DbOpReklames.Add(newRow);
                        }
                        Console.WriteLine($"DB_OP_REKLAME: {item.Nop}");
                        _contMonPd.SaveChanges();
                    }
                }
            }


//            // do fill ketetapan
//            var _contMonitoringDb2 = DBClass.GetMonitoringDbContext();
//            var opList = _contMonPd.DbOpReklames.ToList();
//            foreach (var op in opList)
//            {
//                var sql = @"select   A.NOP, a.TAHUN, a.MASAPAJAK, a.SEQ, JENIS_KETETAPAN, NPWPD, AKUN, AKUN_JENIS, AKUN_JENIS_OBJEK, AKUN_JENIS_OBJEK_RINCIAN, 
//        AKUN_JENIS_OBJEK_RINCIAN_SUB, 
//        TGL_KETETAPAN, POKOK, SANKSI_TERLAMBAT_LAPOR, SANKSI_ADMINISTRASI, PROSEN_TARIF_PAJAK, PROSEN_SANKSI_TELAT_BAYAR, TGL_JATUH_TEMPO_BAYAR, 
//        TGL_JATUH_TEMPO_LAPOR, JATUH_TEMPO_LAPOR_MODE, JATUH_TEMPO_BAYAR, JATUH_TEMPO_BAYAR_MODE, KELOMPOK_ID, KELOMPOK_NAMA, VOL_PENGGUNAAN_AIR, 
//        STATUS_BATAL, BATAL_KET, BATAL_DATE, BATAL_BY, BATAL_REF, INS_DATE, INS_BY, PERUNTUKAN, NILAI_PENGURANG, JENIS_PENGURANG, REFF_PENGURANG, NVL(B.NO_KETETAPAN, '-') NO_KETETAPAN
//from objek_pajak_skpd_abt a
//LEFT JOIN (
//	SELECT nop, tahun, masapajak, seq, (A.SURAT_KLASIFIKASI || '/' || A.SURAT_PAJAK || A.SURAT_DOKUMEN || A.SURAT_BIDANG || A.SURAT_AGENDA || '/' || A.SURAT_OPD || '/' || A.SURAT_TAHUN) no_Ketetapan
//	from OBJEK_PAJAK_SKPD_REKLAME_PNTPN a
//) b ON a.nop = b.nop and a.tahun = b.tahun AND a.MASAPAJAK = b.MASAPAJAK AND a.SEQ = b.SEQ
//WHERE a.NOP=:nop AND a.TAHUN=:tahun AND a.MASAPAJAK=:bulan AND a.STATUS_BATAL=0";

//                var ketetapanSbyTaxOld = await _contSbyTaxOld.Set<OPSkpdAbt>()
//                    .FromSqlRaw(sql, new[] {
//                                new OracleParameter("nop", op.Nop),
//                                new OracleParameter("tahun", thn),
//                                new OracleParameter("bulan", bln)
//                    }).ToListAsync();
//                var dbAkunPokok = GetDbAkunPokok(thn, idPajak, (int)op.KategoriId);
//                foreach (var item in ketetapanSbyTaxOld)
//                {
//                    string nop = item.NOP;
//                    int tahunPajak = item.TAHUN;
//                    int masaPajak = item.MASAPAJAK;
//                    int seqPajak = item.SEQ;
//                    var rowMonAbt = _contMonPd.DbMonAbts.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak &&
//                                                                            x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

//                    if (rowMonAbt != null)
//                    {
//                        _contMonPd.DbMonAbts.Remove(rowMonAbt);
//                    }
//                    _contMonPd.DbMonAbts.Add(new DbMonAbt()
//                    {
//                        Nop = item.NOP,
//                        Npwpd = op.Npwpd,
//                        NpwpdNama = op.NpwpdNama,
//                        NpwpdAlamat = op.NpwpdAlamat,
//                        PajakId = op.PajakId,
//                        PajakNama = op.PajakNama,
//                        NamaOp = op.NamaOp,
//                        AlamatOp = op.AlamatOp,
//                        AlamatOpKdLurah = op.AlamatOpKdLurah,
//                        AlamatOpKdCamat = op.AlamatOpKdCamat,
//                        TglOpTutup = op.TglOpTutup,
//                        TglMulaiBukaOp = op.TglMulaiBukaOp,
//                        IsTutup = isOPTutup ? 1 : 0,
//                        PeruntukanId = item.PERUNTUKAN,
//                        PeruntukanNama = item.PERUNTUKAN == 1 ? "NIAGA" : item.PERUNTUKAN == 2 ? "NON NIAGA" : item.PERUNTUKAN == 3 ? "BAHAN BAKU AIR" : "",
//                        KategoriId = op.KategoriId,
//                        KategoriNama = op.KategoriNama,
//                        TahunBuku = thn,
//                        Akun = op.Akun,
//                        NamaAkun = op.NamaAkun,
//                        Kelompok = op.Kelompok,
//                        KelompokNama = op.NamaKelompok,
//                        Jenis = op.Jenis,
//                        NamaJenis = op.NamaJenis,
//                        Objek = op.Objek,
//                        NamaObjek = op.NamaObjek,
//                        Rincian = op.Rincian,
//                        NamaRincian = op.NamaRincian,
//                        SubRincian = op.SubRincian,
//                        NamaSubRincian = op.NamaSubRincian,
//                        TahunPajakKetetapan = item.TAHUN,
//                        MasaPajakKetetapan = item.MASAPAJAK,
//                        SeqPajakKetetapan = item.SEQ,
//                        KategoriKetetapan = item.JENIS_KETETAPAN.ToString(),
//                        TglKetetapan = item.TGL_KETETAPAN,
//                        TglJatuhTempoBayar = item.TGL_JATUH_TEMPO_BAYAR,
//                        PokokPajakKetetapan = item.POKOK - item.NILAI_PENGURANG,
//                        PengurangPokokKetetapan = item.NILAI_PENGURANG,
//                        AkunKetetapan = dbAkunPokok.Akun,
//                        KelompokKetetapan = dbAkunPokok.Kelompok,
//                        JenisKetetapan = dbAkunPokok.Jenis,
//                        ObjekKetetapan = dbAkunPokok.Objek,
//                        RincianKetetapan = dbAkunPokok.Rincian,
//                        SubRincianKetetapan = dbAkunPokok.SubRincian,
//                        InsDate = DateTime.Now,
//                        InsBy = "JOB",
//                        UpdDate = DateTime.Now,
//                        UpdBy = "JOB",
//                        NoKetetapan = item.NO_KETETAPAN
//                    });

//                    Console.WriteLine($"DB_MON_REKLAME {thn}-{bln}-{item.NOP}-{item.SEQ}");
//                    _contMonPd.SaveChanges();
//                }
//            }

//            // do fill realisasi
//            var _contBima = DBClass.GetBimaContext();
//            for (var thn = tahunAmbil; thn <= tglServer.Year; thn++)
//            {
//                for (int bln = 1; bln <= 12; bln++)
//                {
//                    var opList = _contMonPd.DbMonAbts.Where(x => x.TahunPajakKetetapan == thn && x.MasaPajakKetetapan == bln).ToList();

//                    foreach (var op in opList)
//                    {
//                        var sql = @"SELECT 	ID_SSPD, 
//		                                    KODE_BILL, 
//		                                    NO_KETETAPAN, 
//		                                    JENIS_PEMBAYARAN, 
//		                                    JENIS_PAJAK, 
//		                                    JENIS_KETETAPAN, 
//		                                    JATUH_TEMPO, 
//		                                    NOP, 
//		                                    MASA, 
//		                                    TAHUN, 
//		                                    NOMINAL_POKOK, 
//		                                    NOMINAL_SANKSI, 
//		                                    NOMINAL_ADMINISTRASI, 
//		                                    NOMINAL_LAINYA, 
//		                                    PENGURANG_POKOK, 
//		                                    PENGURANG_SANKSI, 
//		                                    REFF_PENGURANG_POKOK, 
//		                                    REFF_PENGURANG_SANKSI, 
//		                                    AKUN_POKOK, 
//		                                    AKUN_SANKSI, 
//		                                    AKUN_ADMINISTRASI, 
//		                                    AKUN_LAINNYA, 
//		                                    AKUN_PENGURANG_POKOK, 
//		                                    AKUN_PENGURANG_SANKSI, 
//		                                    INVOICE_NUMBER, 
//		                                    TRANSACTION_DATE, 
//		                                    NO_NTPD, 
//		                                    STATUS_NTPD, 
//		                                    REKON_DATE, 
//		                                    REKON_BY, 
//		                                    REKON_REFF, 
//		                                    SEQ_KETETAPAN, 
//		                                    INS_DATE	
//                                    FROM T_SSPD A
//                                    WHERE 	A.JENIS_PAJAK = 6 AND 
//		                                    A.NOP = :NOP AND
//		                                    A.TAHUN = :TAHUN AND 
//		                                    A.MASA = :MASA AND 
//		                                    A.SEQ_KETETAPAN = :SEQ";

//                        var pembayaranSspdList = await _contBima.Set<SSPD>()
//                            .FromSqlRaw(sql, new[] {
//                                            new OracleParameter("NOP", op.Nop),
//                                            new OracleParameter("TAHUN", thn),
//                                            new OracleParameter("MASA", bln),
//                                            new OracleParameter("SEQ", op.SeqPajakKetetapan)
//                            }).ToListAsync();

//                        if (pembayaranSspdList != null && pembayaranSspdList.Count > 0)
//                        {
//                            DateTime tanggalBayarTerakhir = pembayaranSspdList.Max(x => (DateTime)x.TRANSACTION_DATE);
//                            int maxTahunBayar = pembayaranSspdList.Max(x => ((DateTime)x.TRANSACTION_DATE).Year);
//                            decimal nominalPokokBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_POKOK);
//                            decimal nominalSanksiBayar = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_SANKSI);
//                            decimal nominalAdministrasi = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_ADMINISTRASI);
//                            decimal nominalLainnya = pembayaranSspdList.Sum(x => (decimal)x.NOMINAL_LAINYA);
//                            decimal pengurangPokok = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_POKOK);
//                            decimal pengurangSanksi = pembayaranSspdList.Sum(x => (decimal)x.PENGURANG_SANKSI);

//                            string akunBayar = "-";
//                            string kelompokBayar = "-";
//                            string jenisBayar = "-";
//                            string objekBayar = "-";
//                            string rincianBayar = "-";
//                            string subrincianBayar = "-";

//                            var getAkun = GetDbAkun(maxTahunBayar, 6, 56);
//                            if (getAkun != null)
//                            {
//                                akunBayar = getAkun.Akun;
//                                kelompokBayar = getAkun.Kelompok;
//                                jenisBayar = getAkun.Jenis;
//                                objekBayar = getAkun.Objek;
//                                rincianBayar = getAkun.Rincian;
//                                subrincianBayar = getAkun.SubRincian;
//                            }

//                            string akunSanksi = "-";
//                            string kelompokSanksi = "-";
//                            string jenisSanksi = "-";
//                            string objekSanksi = "-";
//                            string rincianSanksi = "-";
//                            string subrincianSanksi = "-";

//                            var getAkunSanksi = GetDbAkunSanksi(maxTahunBayar, 6, 56);
//                            if (getAkunSanksi != null)
//                            {
//                                akunSanksi = getAkunSanksi.Akun;
//                                kelompokSanksi = getAkunSanksi.Kelompok;
//                                jenisSanksi = getAkunSanksi.Jenis;
//                                objekSanksi = getAkunSanksi.Objek;
//                                rincianSanksi = getAkunSanksi.Rincian;
//                                subrincianSanksi = getAkunSanksi.SubRincian;
//                            }

//                            if (nominalPokokBayar > 0)
//                            {
//                                DateTime TGL_BAYAR_POKOK = tanggalBayarTerakhir;
//                                decimal NOMINAL_POKOK_BAYAR = nominalPokokBayar;
//                                string AKUN_POKOK_BAYAR = akunBayar;
//                                string KELOMPOK_POKOK_BAYAR = kelompokBayar;
//                                string JENIS_POKOK_BAYAR = jenisBayar;
//                                string OBJEK_POKOK_BAYAR = objekBayar;
//                                string RINCIAN_POKOK_BAYAR = rincianBayar;
//                                string SUB_RINCIAN_POKOK_BAYAR = subrincianBayar;

//                                op.TglBayarPokok = TGL_BAYAR_POKOK;
//                                op.NominalPokokBayar = NOMINAL_POKOK_BAYAR;
//                                op.AkunPokokBayar = AKUN_POKOK_BAYAR;
//                                op.JenisPokokBayar = JENIS_POKOK_BAYAR;
//                                op.ObjekPokokBayar = OBJEK_POKOK_BAYAR;
//                                op.RincianPokokBayar = RINCIAN_POKOK_BAYAR;
//                                op.SubRincianPokokBayar = SUB_RINCIAN_POKOK_BAYAR;
//                            }

//                            if (nominalSanksiBayar > 0 || nominalLainnya > 0 || nominalAdministrasi > 0)
//                            {
//                                DateTime TGL_BAYAR_SANKSI = tanggalBayarTerakhir;
//                                decimal NOMINAL_SANKSI_BAYAR = (nominalSanksiBayar + nominalLainnya + nominalAdministrasi);
//                                string AKUN_SANKSI_BAYAR = akunSanksi;
//                                string KELOMPOK_SANKSI_BAYAR = kelompokSanksi;
//                                string JENIS_SANKSI_BAYAR = jenisSanksi;
//                                string OBJEK_SANKSI_BAYAR = objekSanksi;
//                                string RINCIAN_SANKSI_BAYAR = rincianSanksi;
//                                string SUB_RINCIAN_SANKSI_BAYAR = subrincianSanksi;

//                                op.TglBayarSanksi = TGL_BAYAR_SANKSI;
//                                op.NominalSanksiBayar = NOMINAL_SANKSI_BAYAR;
//                                op.AkunSanksiBayar = AKUN_SANKSI_BAYAR;
//                                op.KelompokSanksiBayar = KELOMPOK_SANKSI_BAYAR;
//                                op.JenisSanksiBayar = JENIS_SANKSI_BAYAR;
//                                op.ObjekSanksiBayar = OBJEK_SANKSI_BAYAR;
//                                op.RincianSanksiBayar = RINCIAN_SANKSI_BAYAR;
//                                op.SubRincianSanksiBayar = SUB_RINCIAN_SANKSI_BAYAR;
//                            }
//                            Console.WriteLine($"DB_MON_REKLAME (SSPD): {thn}-{bln}-{op.Nop}-{op.SeqPajakKetetapan}");
//                            _contMonPd.SaveChanges();
//                        }
//                    }
//                }
//            }

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
