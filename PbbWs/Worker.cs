using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace PbbWs
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

                //if (stoppingToken.IsCancellationRequested)
                //    break;

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
                    "ERROR PBB WS",
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
            int idPajak = 4;
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
                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        SELECT 	A.T_PROP_KD||A.T_DATI2_KD||A.T_KEC_KD||A.T_KEL_KD||A.D_NOP_BLK||A.D_NOP_URUT||A.D_NOP_JNS NOP,
                                57 KATEGORI_ID,
                                'PBB' KATEGORI_NAMA,
                                D_OP_JLN || ' NO. ' || D_OP_JLNO || ' RT/RW ' || TRIM(D_OP_RT) || '/' || TRIM(D_OP_RW) ALAMAT_OP,
                                A.D_OP_JLNO ALAMAT_OP_NO,
                                A.D_OP_JLNO ALAMAT_OP_JLN,
                                A.D_OP_RT ALAMAT_OP_RT,
                                A.D_OP_RW ALAMAT_OP_RW,
                                A.KD_CAMAT ALAMAT_KD_CAMAT,
                                A.KD_LURAH ALAMAT_KD_LURAH,
                                A.D_TNH_LUAS,
                                A.D_WP_JLN ALAMAT_WP,
                                A.D_WP_JLNO ALAMAT_WP_NO,
                                A.D_WP_KEL ALAMAT_WP_KEL,
                                A.D_WP_KOTA ALAMAT_WP_KOTA,
                                b.subjek_pajak_id WP_KTP,
                                d_wp_nama wp_nama,
                                NVL(NPWP ,'-') wp_npwp,
                                d_wp_rt wp_rt,
                                d_wp_rw wp_rw,
                                A.STATUSAKTIF STATUS,
                                A.STATUSAKTIF IS_TUTUP,
                                D_TNH_LUAS LUAS_TANAH,
                                'SURABAYA ' || NVL(D.UPTD, 0) WILAYAH_PAJAK,
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
                        FROM DATAOP@LIHATGATOTKACA A
                        left join DAT_OBJEK_PBB b on T_PROP_KD=b.KD_PROPINSI
                             AND T_DATI2_KD=b.KD_DATI2
                             AND T_KEC_KD=b.KD_KECAMATAN
                             AND T_KEL_KD=b.KD_KELURAHAN
                             AND D_NOP_BLK=b.KD_BLOK
                             AND D_NOP_URUT=b.NO_URUT
                             AND D_NOP_JNS=b.KD_JNS_OP
                        left join dat_subjek_pbb c on b.subjek_pajak_id=c.subjek_pajak_id
                        LEFT JOIN M_WILAYAH D ON A.KD_CAMAT=D.KD_KEC AND A.KD_LURAH=D.KD_KEL
                    ";

                    var result = await _contMonitoringDb.Set<DbOpPbb>().FromSqlRaw(sql).ToListAsync();

                    var source = await _contMonPd.DbOpPbbs.ToListAsync();
                    foreach (var item in result)
                    {
                        var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                        if (sourceRow != null)
                        {
                            var dbakun = GetDbAkun(2025, idPajak, (int)item.KategoriId);
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
                            var newRow = new MonPDLib.EF.DbOpPbb();
                            newRow.Nop = item.Nop;
                            newRow.KategoriId = item.KategoriId;
                            newRow.KategoriNama = item.KategoriNama;
                            newRow.AlamatOp = item.AlamatOp;
                            newRow.AlamatOpNo = item.AlamatOpNo;
                            newRow.AlamatOpRt = item.AlamatOpRt;
                            newRow.AlamatOpRw = item.AlamatOpRw;
                            newRow.AlamatKdCamat = item.AlamatKdCamat;
                            newRow.AlamatKdLurah = item.AlamatKdLurah;
                            newRow.LuasTanah = item.LuasTanah;
                            newRow.AlamatWp = item.AlamatWp;
                            newRow.AlamatWpNo = item.AlamatWpNo;
                            newRow.AlamatWpKel = item.AlamatWpKel;
                            newRow.AlamatWpKota = item.AlamatWpKota;
                            newRow.WpKtp = item.WpKtp;
                            newRow.WpNama = item.WpNama;
                            newRow.WpNpwp = item.WpNpwp;
                            newRow.WpRt = item.WpRt;
                            newRow.WpRw = item.WpRw;
                            newRow.Status = item.Status;
                            newRow.InsDate = item.InsDate;
                            newRow.InsBy = item.InsBy;
                            newRow.TahunBuku = item.TahunBuku;
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
                            newRow.WilayahPajak = item.WilayahPajak;
                            newRow.IsTutup = item.IsTutup;
                            newRow.Kelompok = item.Kelompok;
                            newRow.NamaKelompok = item.NamaKelompok;


                            newRow.TahunBuku = tahunAmbil;
                            var dbakun = GetDbAkun(tahunAmbil, idPajak, (int)item.KategoriId);
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
                            _contMonPd.DbOpPbbs.Add(newRow);
                        }

                        _contMonPd.SaveChanges();
                        Console.WriteLine($"{DateTime.Now} DB_OP_PBB {item.Nop}");
                    }
                }
            }

            //FILL KETETAPAN 
            var _contMonitoringDb2 = DBClass.GetMonitoringDbContext();
            var opList = _contMonPd.DbOpPbbs.ToList();
            Console.WriteLine($"{DateTime.Now} [QUERY] KETETAPAN MONITORING DB");
            var sql2 = @"
                            SELECT 	FK_OP AS NOP,
	                                D_PJK_TJTT AS TGL_JATUH_TEMPO_BAYAR,
	                                D_CREA_DATE AS TGL_KETETAPAN,
	                                EXTRACT(YEAR FROM D_CREA_DATE) AS TAHUN,
	                                EXTRACT(MONTH FROM D_CREA_DATE) AS MASAPAJAK,
	                                1 SEQ,
                                    1 JENIS_KETETAPAN,
	                                0 NILAI_PENGURANG,
	                                D_PJK_TAX POKOK
                            FROM (
	                            SELECT 	ID_KETETAPAN, 
			                            FK_OP, 
			                            FK_NOP, 
			                            D_PJK_THN, 
			                            D_PJK_BLN, 
			                            D_PJK_MGU, 
			                            T_KWIL_KD, 
			                            T_KPPBB_KD, 
			                            T_TUNGGAL_KD, 
			                            T_PERSEPSI_KD, 
			                            T_BANKTP_KD, 
			                            D_PJK_TAX, 
			                            D_PJK_TGAK, 
			                            D_KRG_LBH, 
			                            D_PJK_ADJ, 
			                            D_PJK_JBYR, 
			                            D_PJK_TJTT, 
			                            D_PJK_LST, 
			                            D_PJK_LSB, 
			                            D_PJK_KLT, 
			                            D_PJK_KLB, 
			                            D_PJK_NLT, 
			                            D_PJK_NLB, 
			                            D_PJK_BOOK, 
			                            D_PJK_TGBYR, 
			                            D_PJK_LUNAS, 
			                            D_PJK_NBYR, 
			                            D_PJK_JAD, 
			                            D_CREA_DATE, 
			                            D_CREA_USER, 
			                            D_MODI_DATE, 
			                            D_MODI_USER, 
			                            D_REC_VERSION, 
			                            NOP, 
			                            REF AS REF, 
			                            DENDA_DISETUJUI, 
			                            IPINSERT, 
			                            KATEGORI, 
			                            KD_CAMAT, 
			                            KD_LURAH, 
			                            KATEGORI_OP, 
			                            LUAS_BUMI, 
			                            LUAS_BANGUNAN, 
			                            LUAS_BUMIB, 
			                            LUAS_BANGUNANB
	                            FROM (
		                            SELECT A.T_PROP_KD||A.T_DATI2_KD||A.T_KEC_KD||A.T_KEL_KD||A.D_NOP_BLK||A.D_NOP_URUT||A.D_NOP_JNS||A.D_PJK_THN ID_KETETAPAN,
		                                   A.T_PROP_KD||A.T_DATI2_KD||A.T_KEC_KD||A.T_KEL_KD||A.D_NOP_BLK||A.D_NOP_URUT||A.D_NOP_JNS  FK_OP,
		                                   A.T_PROP_KD|| '.' || A.T_DATI2_KD|| '.' || A.T_KEC_KD|| '.' || A.T_KEL_KD|| '.' || A.D_NOP_BLK|| '.' || A.D_NOP_URUT|| '.' || A.D_NOP_JNS  FK_NOP,
		                                   A.D_PJK_THN, A.D_PJK_BLN, D_PJK_MGU, T_KWIL_KD, T_KPPBB_KD, T_TUNGGAL_KD, T_PERSEPSI_KD, T_BANKTP_KD, D_PJK_TAX, D_PJK_TGAK, D_KRG_LBH, D_PJK_ADJ,
		                                   A.D_PJK_JBYR, A.D_PJK_TJTT, A.D_PJK_LST, D_PJK_LSB, D_PJK_KLT, D_PJK_KLB, D_PJK_NLT, D_PJK_NLB, D_PJK_BOOK, D_PJK_TGBYR, D_PJK_LUNAS, D_PJK_NBYR,
		                                   A.D_PJK_JAD, A.D_CREA_DATE, A.D_CREA_USER, A.D_MODI_DATE, A.D_MODI_USER, A.D_REC_VERSION, A.NOP, REF, DENDA_DISETUJUI, IPINSERT,NVL(B.KATEGORI,'BELUM DIKETAHUI') KATEGORI,KD_CAMAT,KD_LURAH,
		                                   CASE
		                                            WHEN LUAS_BANGUNANB>0 THEN 'BB'
		                                            WHEN LUAS_BANGUNAN=0 THEN 'TK'
		                                            ELSE 'TB'
		                                            END    KATEGORI_op,LUAS_BUMI, LUAS_BANGUNAN,LUAS_BUMIB,LUAS_BANGUNANB,0
		                            FROM DATABAYAR@LIHATGATOTKACA A
		                            LEFT JOIN POTENSIBYR@NRC B ON  T_PROP_KD=SPPT_PROP AND T_DATI2_KD=SPPT_KOTA AND T_KEC_KD=SPPT_KEC AND T_KEL_KD=SPPT_KEL AND D_NOP_BLK=SPPT_URUTBLK AND D_NOP_URUT=SPPT_URUTOP AND D_NOP_JNS=SPPT_TANDA
		                            LEFT JOIN    DATAOP@LIHATGATOTKACA B ON A.T_KEC_KD=B.T_KEC_KD AND A.T_KEL_KD=B.T_KEL_KD AND A.D_NOP_BLK=B.D_NOP_BLK AND A.D_NOP_URUT=B.D_NOP_URUT AND A.D_NOP_JNS=B.D_NOP_JNS
		                            LEFT JOIN    sppt_new@LIHATGATOTKACA c ON A.T_KEC_KD=c.sppt_kec AND A.T_KEL_KD=c.sppt_kel AND A.D_NOP_BLK=c.sppt_urutblk AND A.D_NOP_URUT=c.sppt_urutop AND A.D_NOP_JNS=c.sppt_tanda
		                            WHERE TO_CHAR(A.D_CREA_DATE,'YYYY')= TO_CHAR(SYSDATE,'YYYY')
	                            ) A
                            ) A
                        ";

            var ketetapanSbyTaxOld = await _contMonitoringDb2.Set<OPSkpdPbb>()
                .FromSqlRaw(sql2)
                .ToListAsync();
            Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] KETETAPAN MONITORING DB");
            foreach (var op in opList)
            {
                var dbAkunPokok = GetDbAkunPokok(2025, idPajak, (int)op.KategoriId);
                foreach (var item in ketetapanSbyTaxOld.Where(x => x.NOP == op.Nop))
                {
                    string nop = item.NOP;
                    int tahunPajak = item.TAHUN;
                    int masaPajak = item.MASAPAJAK;
                    int seqPajak = item.SEQ;
                    var rowMonPbb = _contMonPd.DbMonPbbs.SingleOrDefault(x => x.Nop == nop && x.TahunPajakKetetapan == tahunPajak && x.MasaPajakKetetapan == masaPajak && x.SeqPajakKetetapan == seqPajak);

                    if (rowMonPbb != null)
                    {
                        _contMonPd.DbMonPbbs.Remove(rowMonPbb);
                    }
                    _contMonPd.DbMonPbbs.Add(new DbMonPbb()
                    {
                        Npwpd = op.WpNpwp ?? "-",
                        NpwpdNama = op.WpNama ?? "-",
                        NpwpdAlamat = op.AlamatWp ?? "-",
                        PajakId = 9,
                        PajakNama = "PBB",
                        NamaOp = op.WpNama ?? "-",
                        AlamatOpKdLurah = op.AlamatKdLurah ?? "-",
                        AlamatOpKdCamat = op.AlamatKdCamat ?? "-",
                        TglMulaiBukaOp = DateTime.Now,
                        IsTutup = Convert.ToDecimal(op.IsTutup),
                        Dikelola = "-",
                        PungutTarif = "-",
                        Nop = item.NOP,
                        AlamatOp = op.AlamatOp ?? "-",
                        KategoriId = op.KategoriId,
                        KategoriNama = op.KategoriNama,
                        TahunBuku = DateTime.Now.Year,
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
                    Console.WriteLine($"{DateTime.Now} DB_MON_PARKIR_MONITORINGDB -{item.NOP}");
                    Console.ResetColor();
                }
            }

            //PEMBAYARAN PHR

            var _contMonitoringDb3 = DBClass.GetMonitoringDbContext();

            Console.WriteLine($"{DateTime.Now} [QUERY] OP (SSPD) (PHR)");
            var sql1 = @"
                SELECT 	REPLACE(NOP, '.', '') NOP,
		                EXTRACT(YEAR FROM D_PJK_TGBYR) AS TAHUN_PAJAK,
	                    EXTRACT(MONTH FROM D_PJK_TGBYR) AS BULAN_PAJAK,
	                    D_CREA_DATE TRANSACTION_DATE,
	                    POKOK NOMINAL_POKOK,
	                    SANKSI NOMINAL_SANKSI,
	                    0 NOMINAL_ADMINISTRASI,
                        0 NOMINAL_LAINNYA,
                        0 PENGURANG_POKOK,
                        0 PENGURANG_SANSKSI,
                        1 SEQ_KETETAPAN
                FROM (
	                SELECT   ID_SSPD,D_PJK_TGBYR,A.D_CREA_DATE,AKUN,A.T_PROP_KD || '.' || A.T_DATI2_KD || '.' || A.T_KEC_KD || '.' || A.T_KEL_KD || '.' || A.D_NOP_BLK || '.' || A.D_NOP_URUT || '.' || A.D_NOP_JNS NOP,
	                         D_WP_NAMA NAMA,D_OP_JLN || ' ' || D_OP_JLNO ALAMAT,-1 MASA,D_PJK_THN TAHUN,POKOK,SANKSI,B.KD_CAMAT || '-' || B.KD_LURAH NO_KETETAPAN,TEMPAT_BAYAR,REFF,SYSDATE REKON_DATE,'JOB' REKON_BY
	                FROM(
	                            SELECT      A.T_PROP_KD || '.' || A.T_DATI2_KD || '.' || A.T_KEC_KD || '.' || A.T_KEL_KD || '.' || A.D_NOP_BLK || '.' || A.D_NOP_URUT || '.' || A.D_NOP_JNS || '.' || A.d_pjk_thn ID_SSPD,
	                            A.T_PROP_KD , A.T_DATI2_KD , A.T_KEC_KD , A.T_KEL_KD , A.D_NOP_BLK ,A.D_NOP_URUT , A.D_NOP_JNS ,
	                            D_PJK_TGBYR,D_CREA_DATE,'4.1.1.15.01' AKUN,-1,D_PJK_THN,D_PJK_PBB POKOK,D_PJK_JMBYR-D_PJK_PBB SANKSI,'-' NO_KETETAPAN,'MOBLING/DINAS/' || D_CREA_USER TEMPAT_BAYAR,'-' REFF
	                            FROM        CATBAYAR@LIHATGATOTKACA A 
	                            WHERE       to_char(D_PJK_TGBYR,'MMYYYY') = TO_CHAR(SYSDATE,'MMYYYY')
	                            UNION ALL
	                            SELECT      NTPPD,A.KD_PROPINSI , A.KD_DATI2 , A.KD_KECAMATAN , A.KD_KELURAHAN , A.KD_BLOK , A.NO_URUT , A.KD_JNS_OP ,
	                                        TGL_PEMBAYARAN_SPPT,TGL_REKAM_BYR_SPPT,'4.1.1.15.01' AKUN,-1,THN_PAJAK_SPPT,JML_SPPT_YG_DIBAYAR-DENDA_SPPT,DENDA_SPPT,'-',
	                                        CASE 
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='00' THEN 'BANK JATIM'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='08' THEN 'BANK MANDIRI'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='09' THEN 'BANK BNI'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='11401' THEN 'BANK JATIM QRIS'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='114' THEN 'BANK JATIM AGREGAT'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='11402' THEN 'BANK JATIM VA'
	                                        WHEN TRIM(KD_BANK_TUNGGAL)='11588' THEN 'BANK BRI'
	                                        ELSE '-'
	                                        END TEMPAT_BAYAR
	                                        ,'-' REFF
	                            FROM        PEMBAYARAN_SPPT@LIHATGATOTKACA A
	                            WHERE       to_char(TGL_PEMBAYARAN_SPPT,'MMYYYY') = TO_CHAR(SYSDATE,'MMYYYY') AND NVL(REV_FLAG,0) !=1
	                    ) A
	                JOIN    DATAOP@LIHATGATOTKACA B ON A.T_KEC_KD=B.T_KEC_KD AND A.T_KEL_KD=B.T_KEL_KD AND A.D_NOP_BLK=B.D_NOP_BLK AND A.D_NOP_URUT=B.D_NOP_URUT AND A.D_NOP_JNS=B.D_NOP_JNS
                ) A
            ";
            var sspdList = await _contMonitoringDb3.Set<SSPDPbjt>().FromSqlRaw(sql1).ToListAsync();
            Console.WriteLine($"{DateTime.Now} [QUERY_FINISHED] OP (SSPD) (PHR)");

            var opListMon = _contMonPd.DbMonPbbs.ToList();

            foreach (var op in opListMon)
            {
                Console.WriteLine($"{DateTime.Now} [PROCESS] OP (SSPD) (PHR) -{op.Nop}-");

                if (sspdList != null)
                {
                    var pembayaranSspdList = sspdList.Where(x => x.TAHUN_PAJAK == op.TahunPajakKetetapan && x.BULAN_PAJAK == op.MasaPajakKetetapan && x.NOP == op.Nop).ToList();

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
                        Console.WriteLine($"{DateTime.Now} [SAVED] OP (SSPD) (PHR) -{op.Nop}-");
                        Console.ResetColor();
                    }
                }
            }

            MailHelper.SendMail(
            false,
            "DONE PBB WS",
            $@"PBB WS FINISHED",
            null
            );
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPPBB.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPPBB.ToString().ToUpper();
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
