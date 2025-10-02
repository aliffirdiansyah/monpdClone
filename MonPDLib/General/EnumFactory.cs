using System.ComponentModel;

namespace MonPDLib.General
{
    public class EnumFactory
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class ColorAttribute : Attribute
        {
            public string Color { get; }
            public ColorAttribute(string color) => Color = color;
        }

        public enum EStatusSurvey
        {
            [Description("Belum Survey")]
            BelumSurvey = 0,
            [Description("Pendataan")]
            Pendataan = 1,
        }
        public enum EPeruntukan
        {
            [Description("Non Usaha / Usaha Non Pajak / Usaha Lainnya")]
            NonUsaha = 1,
            [Description("Usaha")]
            Usaha = 2
        }
        public enum EJenisHiburan
        {
            [Description("Tontonan Film")]
            TontonanFilm = -1,
            [Description("Fitness / Gym")]
            FitnessGym = 0,
            [Description("Lainnya")]
            Lainnya = 1,
        }
        public enum EJobName
        {
            DBOPABT,
            DBOPHOTEL,
            DBOPRESTORAN,
            DBOPPARKIR,
            DBOPHIBURAN,
            DBOPLISTRIK,
            DBOPPBB,
            DBOPREKLAME,
            DBOPREKLAMESURAT,
            DBOPREKLAMESURVEY,
            DBNPWPD,
            DBOPOPSEN,
            DBOPPBB1,
            DBOPPBB2,
            DBOPPBB3,
            DBOPPBB4,
            DBOPPBB5,
            DBOPREKLAMEINSJML,
            DBOPALATREKAMTS,
            DBOPALATREKAMTBSB,
            DBOPREKLAMEPERPANJANGAN,
            DBOPREKLAMEUPAYA,
            DBMONUPAYAPAD,
            DBMUTASIBANK
        }
        public enum EStatePemeriksaanSkpdKbJabatan
        {
            [Description("Tidak ada pemeriksaan jabatan")]
            TidakAdaPemeriksaanJabatan = -1,

            [Description("SP ditolak")]
            RejectSP = 0,

            [Description("Masa tunggu Approve SP")]
            ApprovalSP = 1,

            [Description("Menunggu Keputusan Kepala Badan SP")]
            ApprovalKeputusanSP = 2,

            [Description("Menunggu pembuatan SPHP")]
            TungguBuatSPHP = 3,

            [Description("SPHP ditolak")]
            RejectSPHP = 4,

            [Description("Masa tunggu Approve SPHP")]
            ApprovalSPHP = 5,

            [Description("Keputusan SPHP disetujui")]
            ApprovalKeputusanSPHP = 6,

            [Description("Menunggu WP menjawab SPHP")]
            TungguWPJawabSphp = 7,

            [Description("Menunggu pembuatan BAP")]
            TungguBuatBAP = 8,

            [Description("Masa tunggu Approve BAP")]
            ApprovalBAP = 9,

            [Description("Menunggu pembuatan LHP")]
            TungguBuatLHP = 10,

            [Description("LHP ditolak")]
            RejectLHP = 11,

            [Description("Masa tunggu Approve LHP")]
            ApprovalLHP = 12,

            [Description("Menunggu Keputusan Kepala Badan LHP")]
            ApprovalKeputusanLHP = 13,

            [Description("Keputusan LHP ditolak")]
            RejectKeputusanLHP = 14,

            [Description("SKPDKB Jabatan")]
            SKPDKBJabatan = 15
        }

        public enum EStatePemeriksaanSkpdKbRencana
        {
            [Description("Tidak ada Rencana Pemeriksaan")]
            TidakAdaRencanaPemeriksaan = -1,

            [Description("SP ditolak")]
            RejectSP = 0,

            [Description("Masa tunggu Approve SP")]
            ApprovalSP = 1,

            [Description("Menunggu Keputusan Kepala Badan SP")]
            ApprovalKeputusanSP = 2,

            [Description("Menunggu pembuatan SPHP")]
            TungguBuatSPHP = 3,

            [Description("SPHP ditolak")]
            RejectSPHP = 4,

            [Description("Masa tunggu Approve SPHP")]
            ApprovalSPHP = 5,

            [Description("Keputusan SPHP disetujui")]
            ApprovalKeputusanSPHP = 6,

            [Description("Menunggu WP menjawab SPHP")]
            TungguWPJawabSphp = 7,

            [Description("Menunggu pembuatan BAP")]
            TungguBuatBAP = 8,

            [Description("Masa tunggu Approve BAP")]
            ApprovalBAP = 9,

            [Description("Menunggu pembuatan LHP")]
            TungguBuatLHP = 10,

            [Description("LHP ditolak")]
            RejectLHP = 11,

            [Description("Masa tunggu Approve LHP")]
            ApprovalLHP = 12,

            [Description("Menunggu Keputusan Kepala Badan LHP")]
            ApprovalKeputusanLHP = 13,

            [Description("Keputusan LHP ditolak")]
            RejectKeputusanLHP = 14,

            [Description("SKPDKB Pemeriksaan")]
            SKPDKBPemeriksaan = 15
        }

        public enum EJobRole
        {
            [Description("Kepatuhan Lapor SPTPD")]
            HimbauanSptpd = 0,

            [Description("Pengawasan Lapor SPTPD")]
            TeguranSptpd = 1,

            [Description("Penungguan SPTPD")]
            PenungguanSptpd = 2,

            [Description("Undangan SPTPD")]
            UndanganSptpd = 3,

            [Description("Pemeriksaan Jabatan")]
            PemeriksaanJabatan = 4,

            [Description("Pemeriksaan Analisa")]
            PemeriksaanAnalisa = 5,

            [Description("Himbauan Pembayaran")]
            HimbauanPembayaran = 6,

            [Description("Teguran Pembayaran")]
            TeguranPembayaran = 7,

            [Description("Penilaian ABT")]
            PenilaianABT = 8,

            [Description("Penagihan Pajak")]
            Penagihan = 9
        }
        public enum EStatusNTPD
        {
            Belum = 0,
            Sudah = 1
        }
        public enum EMetodePembukuan
        {
            [Description("Kas")]
            Kas = 1,
            [Description("Akrual")]
            Akrual = 2,
            [Description("Campuran")]
            Hibrid = 3
        }

        public enum EStatusPenungguan
        {
            [Description("Tidak ada")]
            TidakAda = 0,
            [Description("Proses Permohonan")]
            Permohonan = 1,
            [Description("Selesai Permohonan")]
            SelesaiPermohonan = 2,
            [Description("Masa Tunggu")]
            MasaTunggu = 3,
            [Description("Selesai")]
            Selesai = 4,
            [Description("Di Tolak")]
            Rejected = -1
        }
        public enum EProsesPembukuan
        {
            [Description("Manual")]
            Manual = 1,
            [Description("Elektronik")]
            Elektronik = 2,
            [Description("Campuran")]
            Hibrid = 3
        }
        public enum ELaporanKeuangandisusun
        {
            [Description("Sendiri")]
            Sendiri = 1,
            [Description("Kantor Akuntan Publik")]
            AkuntanPublik = 2,
            [Description("Campuran")]
            Hibrid = 3
        }


        public enum EPBJTAktifitas
        {
            [Description("Himbauan SPTPD")]
            HimbauanSPTPD = 1,
            [Description("Submit SPTPD")]
            SubmitSPTPD = 2,
            [Description("Penerbitan STPD Sanksi Telat Lapor")]
            STPD_TL = 3,
            [Description("Penerbitan STPD Sanksi Telat Bayar")]
            STPD_TB = 4,
            [Description("Submit STPD SPTPD")]
            STPD_SPTPD = 5,
            [Description("Pembayaran")]
            Pembayaran = 6,
            [Description("Teguran SPTPD")]
            TeguranSPTPD = 7,
            [Description("Penungguan PBJT")]
            PenungguanPBJT = 8,
            [Description("Undangan Pemeriksa")]
            UndanganPemeriksa = 9,
            [Description("Himbauan Pembayaran")]
            HimbauanPembayaran = 10,
            [Description("Teguran Pembayaran")]
            TeguranPembayaran = 11,
            [Description("Penagihan")]
            Penagihan = 12,
            [Description("Pemeriksaan")]
            Pemeriksaan = 13
        }
        public enum EKlasifikasiStatusData
        {
            [Description("Survey Pelayanan")]
            Survey = 0,
            [Description("Himbauan SPTPD")]
            HimbauanSPTPD = 1,
            [Description("Pengawasan Teguran SPTPD")]
            TeguranSPTPD = 2,
            [Description("Pengawasan Pemeriksa Teguran 1")]
            Pemeriksa1 = 3,
            [Description("Pengawasan Pemeriksa Teguran 2")]
            Pemeriksa2 = 4

        }

        public enum EStatusKehadiranUndangan
        {
            [Description("Idle")]
            Idle = 0,
            [Description("Hadir")]
            Hadir = 1,
            [Description("Tidak Hadir")]
            TidakHadir = 2
        }

        public enum ESuratSifat
        {
            [Description("Biasa")]
            Biasa = 1,
            [Description("Penting")]
            Penting = 2,
            [Description("Segera")]
            Segera = 3,
            [Description("Sangat Segera")]
            SangatSegera = 4
        }

        public enum EJenisPerwakilanWP
        {
            [Description("Wajib Pajak")]
            WP = 1,
            [Description("Wakil Wajib Pajak")]
            WakilWP = 2,
            [Description("Kuasa Wajib Pajak")]
            KuasaWP = 3
        }

        public enum EJenisSuratPajak
        {
            [Description("Undangan Pemeriksa SPTPD")]
            UndanganPemeriksaSPTPD = 1,
            [Description("Undangan Pemeriksa SKPDKB")]
            UndanganPemeriksaSKPDKB = 2
        }
        public enum ESuratKategori
        {
            [Description("Pemberitahuan")]
            Pemberitahuan = 1,
            [Description("Undangan")]
            Undangan = 2,
            [Description("Permohonan")]
            Permohonan = 3,
            [Description("Perintah")]
            Perintah = 4
        }

        public enum ESuratStatus
        {
            [Description("Belum dibaca")]
            Unread = 1,
            [Description("Sudah dibaca")]
            Read = 2,
            [Description("Terhapus")]
            Deleted = 3,
            [Description("Batal")]
            Cancel = 4
        }

        public enum EJenisUserAPI
        {
            [Description("Back End")]
            BackEnd = 1,
            [Description("Front End")]
            FrontEnd = 2
        }
        public enum EFilterPencarianSSPD
        {
            [Description("Kode Bill")]
            KodeBill = 0,
            [Description("No Ketetapan")]
            NoKetetapan = 1
        }

        public enum EJenisPengurang
        {
            //0=TDK ADA PENGURANG, 1 = PERWALI , 2= SK
            [Description("Tidak ada Pengurang")]
            TidakAda = 0,
            [Description("Perwali")]
            Perwali = 1,
            [Description("SK")]
            SK = 2
        }
        public enum EExpSatuanVa
        {
            [Description("Hari")]
            Hari = 1,
            [Description("Menit")]
            Menit = 2
        }

        public enum EBankRekening
        {
            [Description("Bendahara Penerimaan Pajak Online")]
            BendaharaPenerimaanPajakOnline = 0011259146,

            [Description("Bendahara Penerimaan PBB")]
            BendaharaPenerimaanPBB = 0011240003,

            [Description("Bendahara Penerimaan Pajak BPHTB")]
            BendaharaPenerimaanPajakBPHTB = 0011260004,

            [Description("Bendahara Penerimaan Pajak Penerangan Jalan")]
            BendaharaPenerimaanPajakPeneranganJalan = 0011244904,

            [Description("Bendahara Penerimaan Pajak Air Bawah Tanah")]
            BendaharaPenerimaanPajakAirBawahTanah = 0011244921,

            [Description("Bendahara Penerimaan Pajak Reklame")]
            BendaharaPenerimaanPajakReklame = 0011251641,

            [Description("Bendahara Penerimaan Jaminan Bongkar")]
            BendaharaPenerimaanJaminanBongkar = 0011251633,

            [Description("UPTB 1")]
            UPTB1 = 0011246206,

            [Description("UPTB 2")]
            UPTB2 = 0011248420,

            [Description("UPTB 3")]
            UPTB3 = 0011248438,

            [Description("UPTB 4")]
            UPTB4 = 0011248446,

            [Description("UPTB 5")]
            UPTB5 = 0011248454,

            [Description("OPSEN PKB")]
            OPSENPKB = 0011307256,

            [Description("OPSEN BBNKB")]
            OPSENBBNKB = 0011307264
        }

        //EModeCreateSTPDSanksiTelatBayar
        public enum EModeCreateSTPDSanksiTelatBayar
        {
            [Description("Tanggal Berjalan")]
            Berjalan = 0,
            [Description("Semua dengan Histori")]
            All = 1
        }
        public enum EJenisUser
        {
            [Description("User Bapenda")]
            UserBapenda = 0,
            [Description("NPWPD Induk")]
            NPWPDInduk = 1,
            [Description("NPWPD Cabang")]
            NPWPDCabang = 2,
            [Description("Sistem")]
            Sistem = 3
        }

        public enum EJenisPetugas
        {
            [Description("Internal Bapenda")]
            Internal = 0,
            [Description("Internal Pemkot")]
            InternalPemkot = 1,
            [Description("Eksternal")]
            Eksternal = 2
        }

        public enum EModeKirim
        {
            [Description("SurabyaaTax Inbox")]
            Inbox = 0,
            [Description("Email")]
            Email = 1,
            [Description("WA")]
            WA = 2,
            [Description("Manual")]
            Manual = 3
        }

        public enum EStatusKirim
        {
            [Description("Gagal")]
            Gagal = 0,
            [Description("Sukses")]
            Sukses = 1
        }
        public enum EJenisSKPDKB
        {
            [Description("Pemeriksaan")]
            Pemeriksaan = 1,
            [Description("Jabatan")]
            Jabatan = 2
        }

        public enum EPencarianOPKey
        {
            [Description("Nama Objek Pajak")]
            NamaOP = 1,
            [Description("Alamat Objek Pajak")]
            AlamatOP = 2,
            [Description("NOP")]
            NOP = 3
        }
        public enum EKelompokPajakHiburan
        {
            [Description("Tidak Diketahui")]
            TdkDiketahui = 0,
            [Description("Bioskop")]
            Bioskop = 1,
            [Description("Fitnes / Gym")]
            Fitnes = 2,
            [Description("Wahana Permainan")]
            Wahana = 3,
            [Description("Karaoke")]
            Karaoke = 4,
            [Description("Ketangkasan")]
            Ketangkasan = 5,
            [Description("Hiburan Malam")]
            Malam = 6,
            [Description("Panti Pijat")]
            Pijat = 7
        }
        public enum EPencarianWPKey
        {
            [Description("Nama Wajib Pajak")]
            NamaWP = 1,
            [Description("Alamat Wajib Pajak")]
            AlamatWP = 2,
            [Description("NPWPD")]
            NPWPD = 3
        }
        public enum EJenisBanquet
        {
            [Description("Ball Room")]
            Ballroom = 1,
            [Description("Meeting Room")]
            Meetingroom = 2
        }
        public enum EFlagVABankJatim
        {
            Create = 1,
            Update = 2,
            Delete = 3
        }

        public enum EModePengawasan
        {
            Silent = 0,
            Pengedokan = 1
        }

        public enum ESortByMonitoringAnak
        {
            [Description("Tunggakan-Tertinggi")]
            Tunggakan_Tertinggi = 1,
            [Description("Tunggakan-Terendah")]
            Tunggakan_Terendah = 2,
            [Description("Pembayaran-Tertinggi")]
            Pembayaran_Tertinggi = 3,
            [Description("Pembayaran-Terendah")]
            Pembayaran_Terendah = 4,
            [Description("Belum Lapor-Tertinggi")]
            BelumLapor_Tertinggi = 5,
            [Description("Belum Lapor-Terendah")]
            BelumLapor_Terendah = 6,
            [Description("Terakhir Lapor-Terbaru")]
            TerakhirLapor_Terbaru = 7,
            [Description("Terakhir Lapor-Terlama")]
            TerakhirLapor_Terlama = 8,
        }

        public enum EFilterStatusLunas
        {
            [Description("Belum Lunas")]
            BelumLunas = 0,
            [Description("Lunas")]
            Lunas = 1,
            [Description("Semua")]
            Semua = -1
        }

        public enum EStatusLunas
        {
            [Description("Belum Lunas")]
            BelumLunas = 0,
            [Description("Lunas")]
            Lunas = 1
        }

        public enum EKeputusanPengawasan
        {
            [Description("Undangan Rasionalisasi")]
            Undangan = 0,
            [Description("Usulan Pemeriksaan")]
            Pemeriksaan = 1
        }

        public enum EHadirAbsensi
        {
            [Description("Belum Datang")]
            BelumDatang = -1,
            [Description("Datang Undangan")]
            Datang = 1,
            [Description("Tidak Datang Undangan")]
            TidakDatang = 2,
        }

        public enum EHadirUndanganPengawasan
        {
            [Description("Belum Datang")]
            BelumDatang = -1,
            [Description("Datang Undangan")]
            Datang = 1,
            [Description("Tidak Datang Undangan")]
            TidakDatang = 2,
        }
        public enum EJenisPembayaran
        {
            [Description("Tidak Ada")]
            TidakAda = 0,
            [Description("Virtual Account Bank Jatim")]
            VABJ = 1,
            [Description("QRIS Bank Jatim")]
            QRISBJ = 2,
            [Description("Virtual Account Bank BRI")]
            VABRI = 3,
        }

        public enum EStatusPersetujuanSPHP
        {
            [Description("Belum Dijawab")]
            BelumdiJawab = 0,
            [Description("Setuju")]
            Setuju = 1,
            [Description("Tidak Setuju Sebagaian")]
            TidakSetujuSebagian = 2,
            [Description("Tidak Setuju Semua")]
            TidakSetujuSemua = 3
        }

        public enum ESPHPStatus
        {
            [Description("Invalid")]
            Invalid = -1,
            [Description("Masa Jawab")]
            MasaJawab = 0,
            [Description("Menunggu Permohonan BAP")]
            MenungguPermohonanBAP = 1,
            [Description("Masa Permohonan BAP")]
            MasaPermohonanBAP = 2,
            [Description("Sudah Menjadi BAP")]
            SudahMenjadiBAP = 3
        }

        public enum EBAPApprove
        {
            [Description("Belum Approved")]
            Idle = 0,
            [Description("Approved")]
            Approved = 1
        }
        public enum EBAPStatus
        {
            [Description("Belum Approved")]
            Idle = 0,
            [Description("Sudah Approve Belum LHP")]
            ApprovedNoLHP = 1,
            [Description("Sudah Approve Sudah LHP")]
            ApprovedLHP = 2
        }

        public enum EJawabSPHP
        {
            [Description("Setuju")]
            Setuju = 1,
            [Description("Tidak Setuju Sebagaian")]
            TidakSetujuSebagian = 2,
            [Description("Tidak Setuju Semua")]
            TidakSetujuSemua = 3
        }

        public enum ESurveyKategori
        {
            NoSurvey = -1,
            PermohonanOP = 1,
            SurveyPendataanPBJT = 2
        }

        public enum EJenisKetetapan
        {
            //STPD, SPPT, SKPD, SKPDKB, SKPDKBT, SKPDN, atau SKPDLB            
            [Description("-")]
            UNDEFINED = 0,
            [Description("Sptpd")]
            PBJT_SPTPD = 1,
            [Description("Sptpd")]
            PBJT_STPD_SPTPD = 2,
            [Description("Stpd Telat Lapor")]
            PBJT_STPD_TL = 3,
            [Description("Stpd Telat Bayar")]
            PBJT_STPD_TB = 4,
            [Description("Skpd Abt")]
            ABT_SKPD = 5,
            [Description("SkpdKB Pbjt")]
            PBJT_SKPD_KB = 6,
            [Description("SkpdKB T Pbjt")]
            PBJT_SKPD_KB_T = 7,
            [Description("SkpdN Pbjt")]
            PBJT_SKPD_N = 8,
            [Description("SkpdLB Pbjt")]
            PBJT_SKPD_LB = 9,
            [Description("Stpd Telat Bayar")]
            ABT_SKPD_TB = 10,
            [Description("Skpd Pbjt")]
            PBJT_SKPD = 11,
            [Description("Stpd Telat Bayar")]
            PBJT_SKPD_TB = 12,
            [Description("Stpd Telat Bayar")]
            PBJT_SKPDKB_TB = 13,
        }

        public enum EKategoriWF
        {
            [Description("SK Pelayanan Pajak")]
            Pelayanan = 0,
            [Description("Undangan")]
            Undangan = 1,
            [Description("Penetapan Pajak")]
            Penetapan = 2,
            [Description("Surat Pajak")]
            Surat = 3
        }
        public enum EJenisPengurangan
        {
            [Description("Pengurangan Pokok")]
            Pokok = 0,

            [Description("Pengurangan Sanksi")]
            Sanksi = 1
        }
        public enum EPajak
        {
            [Description("Semua Jenis Pajak")]
            Semua = 0,

            [Description("PBJT atas Makanan dan/atau Minuman")]
            MakananMinuman = 1,

            [Description("PBJT atas Tenaga Listrik")]
            TenagaListrik = 2,

            [Description("PBJT atas Jasa Perhotelan")]
            JasaPerhotelan = 3,

            [Description("PBJT atas Jasa Parkir")]
            JasaParkir = 4,

            [Description("PBJT atas Jasa Kesenian dan Hiburan")]
            JasaKesenianHiburan = 5,

            [Description("Pajak Air Tanah")]
            AirTanah = 6,
            [Description("Pajak Reklame")]
            Reklame = 7,
            [Description("Pajak Bumi Bangunan")]
            PBB = 9,
            [Description("BPHTB")]
            BPHTB = 12,
            [Description("Opsen PKB")]
            OpsenPkb = 20,
            [Description("Opsen BBNKB")]
            OpsenBbnkb = 21
        }

        public enum EJenisFile
        {
            PDF = 1,
            Image = 2,
            Tidakdiketahui = 3,
            HTML = 4
        }
        public enum EPajakBlok
        {
            JasaPerhotelan = 901,
            MakananMinuman = 902,
            JasaKesenianHiburan = 903,
            TenagaListrik = 905,
            JasaParkir = 907,
            AirTanah = 908,
        }

        public enum EVendorParkirCCTV
        {
            [Description("Jasnita")]
            Jasnita = 1,
            [Description("Telkom")]
            Telkom = 2
        }
        public enum EJenisKendParkirCCTV
        {
            [Description("Unknown"), Color("#888888")]
            Unknown = 0,
            [Description("Motor"), Color("#51d28c")]
            Motor = 1,
            [Description("Mobil"), Color("#038edc")]
            Mobil = 2,
            [Description("Truck"), Color("#f4c20d")]
            Truck = 3,
            [Description("Bus"), Color("#e91e63")]
            Bus = 4
        }
        public enum EStatusCCTV
        {
            [Description("NON AKTIF")]
            NonAktif = 0,
            [Description("AKTIF")]
            Aktif = 1
        }
        public enum CctvParkirDirection
        {
            [Description("Unknown")]
            Unknown = 0,
            [Description("Incoming")]
            Incoming = 1,
            [Description("Outgoing")]
            Outgoing = 2
        }
        public enum EUPTB
        {
            [Description("Semua UPTB")]
            SEMUA = 0,

            [Description("UPTB 1")]
            UPTB1 = 1,

            [Description("UPTB 2")]
            UPTB2 = 2,

            [Description("UPTB 3")]
            UPTB3 = 3,

            [Description("UPTB 4")]
            UPTB4 = 4,

            [Description("UPTB 5")]
            UPTB5 = 5,
        }
        public enum EBidang
        {
            Sekretariat = 14,
            PBB_BPHTB = 11,
            PAD = 12,
            BukanPajak = 13,
            UPTB1 = 1,
            UPTB2 = 2,
            UPTB3 = 3,
            UPTB4 = 4,
            UPTB5 = 5,
        }

        public enum EWorkflow
        {
            // PELAYANAN 1 - 50
            [Description("Permohonan NPWPD Pribadi")]
            PENDAFTARAN_NPWPD_PRIBADI = 1,
            [Description("Permohonan NPWPD Badan")]
            PENDAFTARAN_NPWPD_BADAN = 2,
            [Description("Permohonan Objek Pajak")]
            PENDAFTARAN_NOP = 3,
            [Description("Permohonan Tutup Objek Pajak")]
            PENUTUPAN_NOP = 4,
            [Description("Permohonan NPWPD Pribadi Petugas")]
            PENDAFTARAN_NPWPD_PRIBADI_PETUGAS = 5,
            [Description("Permohonan NPWPD Badan Petugas")]
            PENDAFTARAN_NPWPD_BADAN_PETUGAS = 6,
            [Description("Permohonan Objek Pajak Petugas")]
            PENDAFTARAN_NOP_PETUGAS = 7,

            // Ketetapan 51-70
            [Description("Penetapan SPTPD PBJT")]
            PBJT_SPTPD = 51,
            [Description("Penetapan SPTPD PBJT Insidentil")]
            PBJT_SPTPD_INSIDENTIL = 52,
            [Description("STPD Sanksi Telat Bayar PBJT")]
            PBJT_STPD_STB = 53,
            [Description("STPD Telat Lapor SPTPD")]
            PBJT_STPD_TL = 54,
            [Description("Penetapan SKPD ABT")]
            ABT_SKPD = 55,
            [Description("STPD Sanksi Telat Bayar ABT")]
            ABT_STPD_STB = 56,

            // Surat Pajak 71-90
            [Description("Himbauan SPTPD PBJT")]
            PBJT_HIMBAUAN_SPTPD = 71,
            [Description("Teguran SPTPD PBJT")]
            PBJT_TEGURAN_SPTPD = 72,
            [Description("Penungguan SPTPD PBJT")]
            PBJT_PENUNGGUAN_SPTPD = 73,
            [Description("Undangan SPTPD PBJT")]
            PBJT_UNDANGAN_SPTPD = 74,
            [Description("SP Pemeriksaan SKPD PBJT")]
            PBJT_SP_PEMERIKSAAN_SKPD = 75,
            [Description("Pemeriksaan SPHP SKPD PBJT")]
            PBJT_SPHP_PEMERIKSAAN_SKPD = 76,
            [Description("Pemeriksaan SKPD PBJT")]
            PBJT_PEMERIKSAAN_SKPD = 77,
            [Description("Penetapan SKPDKB PBJT")]
            PBJT_SKPDKB_JABATAN = 78,


            [Description("Pemeriksaan SKPDKB PBJT")]
            PBJT_PEMERIKSAAN_SKPDKB = 80,
            [Description("SP Pemeriksaan SKPDKB PBJT")]
            PBJT_SP_PEMERIKSAAN_SKPDKB = 81,
            [Description("SP Permohonan SPHP SKPDKB PBJT")]
            PBJT_MOHON_SPHP_SKPDKB = 82,
            [Description("Penetapan SKPDKB PBJT Pemeriksaan")]
            PBJT_PENETAPAN_SKPDKB_PEMERIKSAAN = 83,
            [Description("Penetapan SKPDKB PBJT Pemeriksaan")]
            PBJT_PENETAPAN_SKPDKB_STB_PEMERIKSAAN = 84,
            [Description("Penetapan SKPDKB PBJT Jabatan")]
            PBJT_PENETAPAN_SKPDKB_STB_JABATAN = 85,

            //RENCANA
            [Description("Rencana SKPDKB dari Pemeriksaan")]
            PBJT_RENC_SKPDKB_PERIKSA = 86,
            [Description("Rencana SP Mohon SPHP")]
            PBJT_RENC_SP_MOHON_SPHP = 87,
            [Description("Rencana dari Laporan Hasil Pemeriksaan")]
            PBJT_RENC_LHP = 88,

            // PENAGIHAN
            [Description("Himbauan Pembayaran")]
            PBJT_HIMBAUAN_BAYAR = 90,
            [Description("Teguran Pembayaran")]
            PBJT_TEGURAN_BAYAR = 91,

            //KEPATUHAN
            [Description("Kepatuhan")]
            PBJT_KEPATUHAN = 92,

            //PENGAWASAN
            [Description("Pengawasan")]
            PBJT_PENGAWASAN = 93
        }


        public enum EHari
        {
            Minggu = 1,
            Senin = 2,
            Selasa = 3,
            Rabu = 4,
            Kamis = 5,
            Jumat = 6,
            Sabtu = 7
        }

        public enum EBulan
        {
            Januari = 1,
            Februari = 2,
            Maret = 3,
            April = 4,
            Mei = 5,
            Juni = 6,
            Juli = 7,
            Agustus = 8,
            September = 9,
            Oktober = 10,
            November = 11,
            Desember = 12
        }

        public enum EStatusKirimPermohonan
        {
            Draft = 0,
            Submitted = 1,
            Canceled = -1,
            Done = 2

        }


        public enum ESTPDTelatLaporStatus
        {
            [Description("Submited")]
            Submitted = 1
        }

        public enum ESKPDStatus
        {
            [Description("Deleted")]
            Deleted = -1,
            [Description("Draft")]
            Draft = 0,
            [Description("Submited")]
            Terlapor = 1
        }
        public enum ESPTPDStatus
        {
            [Description("Deleted")]
            Deleted = -1,
            [Description("Draft")]
            Draft = 0,
            [Description("Submited")]
            Terlapor = 1
        }

        public enum EPembayaranStatus
        {
            [Description("Belum Terbayar")]
            Deleted = 0,
            [Description("Belum Lunas")]
            Draft = 1,
            [Description("Lunas")]
            Terlapor = 2
        }


        public enum ESPTPDState
        {
            [Description("Draft")]
            Draft = 0,
            [Description("Terlapor - Belum Terbayar")]
            Terlapor = 1,
            [Description("Terbayar")]
            Terbayar = 2,
            [Description("Belum Lapor")]
            BelumBuat = 3,
            [Description("Belum ada kegiatan")]
            TidakAdaKegiatan = 4,
            [Description("Terlapor - Nihil")]
            TerlaporNihil = -2,
            [Description("Batal")]
            Batal = -1
        }

        public enum ESPTPDAktifitasKegiatan
        {
            [Description("Lapor")]
            Lapor = 1,
            [Description("Belum Lapor")]
            BelumLapor = 2,
            [Description("Belum Ada Kegiatan")]
            TidakAdaKegiatan = -1,
            [Description("Lapor Belum Dibuka")]
            PelaporanBelumDibuka = -2,
        }

        public enum EStatusKetetapan
        {
            [Description("Draft")]
            BelumBayar = 0,
            [Description("Terbayar")]
            Terbayar = 2,
            [Description("Batal")]
            Batal = 1
        }
        public enum EStatusKirimPermohonanFilter
        {
            Draft = 0,
            Submitted = 1,
            Canceled = -1,
            All = 2
        }

        public enum EModePenjualan
        {
            Offline = 0,
            Online = 1,
            Hibrid = 2,
        }
        public enum EModePembayaran
        {
            Tunai = 0,
            Elektronik = 1,
            Hibrid = 2,
        }
        public enum EInduk
        {
            Tidak = 0,
            Ya
        }
        public enum EStatusKepemilikanBangunan
        {
            [Description("Milik Sendiri")]
            MilikSendiri,

            [Description("Sewa")]
            Sewa
        }

        public enum EKelompokPajak
        {
            [Description("Pajak Barang dan Jasa Tertentu")]
            PBJT = 0,
            [Description("Air Tanah")]
            AT = 1,
            [Description("Reklame")]
            Reklame = 2,
            [Description("Pajak Bumi dan Bangunan")]
            Pbb = 3,
            [Description("Bea Perolehan Hak atas Tanah dan Bangunan")]
            BPHTB = 4
        }

        public enum EJenisBangunan
        {
            [Description("Permanen")]
            Permanen = 0,
            [Description("Tidak Permanen")]
            TdkPermanen = 1
        }
        public enum EBranch
        {
            Induk = 0,
            Cabang = 1
        }

        public enum ERodaKendaraan
        {
            [Description("SEPEDA")]
            SEPEDA_ATAU_SEJENISNYA = 6,
            [Description("SEPEDA MOTOR ATAU SEJENISNYA (R2)")]
            SEPEDA_MOTOR_ATAU_SEJENISNYA = 7,
            [Description("SEDAN, MINIBUS ATAU SEJENIS (R4)")]
            SEDAN_MINIBUS_ATAU_SEJENISNYA = 8,
            [Description("TRUK MINI ATAU SEJENISNYA (JBB < 3500 KG ")]
            RODA_EMPAT_TRUK = 9,
            //[Description("KERETA TEMPEL, KERETA GANDENGAN, ATAU SEJENISNYA.")]
            //KERETA_TEMPEL_KERETA_GANDENGAN_ATAU_SEJENISNYA = 10,
            [Description("BUS /TRUK ATAU SEJENISNYA (R6) JBB > 3500 KG ")]
            RODA_ENAM = 10,

            //Dua = 2,
            //Tiga = 3,
            //Empat = 4,
            //LebihEmpat = 5
        }

        public enum ESumberInputan
        {
            Mobile = 0, Web = 1, Kantor = 2, WorkerService = 3
        }

        public enum EJenisKegiatanListrik
        {
            Penjualan = 0, Penyerahan = 1, Konsumsi = 2
        }

        public enum ESumberListrik
        {
            [Description("Sendiri")]
            Sendiri = 0,
            [Description("Sumber Lain")]
            SumberLain = 1
        }

        public enum EPeruntukanListrik
        {
            [Description("Umum")]
            Umum = 0,
            [Description("Industri")]
            Industri = 1,
            [Description("Pertambangan Bumi Gas Alam")]
            PertambanganBumiGasAlam = 2
        }


        public enum ESortType
        {
            Asc = 0, Desc = 1
        }

        public enum EStatusPemeriksaanOP
        {
            [Description("Batal")]
            Batal = -1,
            [Description("Masih Dalam Proses pemeriksaan")]
            Proses = 0,
            [Description("Sudah Ditetapkan")]
            SK = 1,
            [Description("Menungu Jawaban WP")]
            MenungguJawabanWP = 2,
            [Description("Menunggu Penetapan")]
            MenungguPenetapanPajak = 3,
            [Description("Approval Penetapan")]
            ApprovalPenetapan = 4
        }

        public enum ESuratPerintah
        {
            [Description("Pemeriksan")]
            Pemeriksa = 1
        }

        public enum EKegiatanPemeriksaan
        {
            [Description("Kirim Undangan")]
            KirimUndangan = 1,
            [Description("Pemeriksaan Untuk Jabatan")]
            PemeriksaanJabatan = 2
        }

        public enum EPemeriksaanAtas
        {
            [Description("Kurang Bayar")]
            KurangBayar = 1,
            [Description("Tidak Lapor SPTPD")]
            TidakLaporSPTPD = 2
        }

        public enum EBank
        {

            BankJatim = 114,
            BankMandiri = 8,
            BNI = 9,
            BCA = 14,
            BRI = 2
        }

        public enum EBatal
        {
            Tidak = 0,
            Ya = 1
        }
        public enum ETutup
        {
            Tidak = 0,
            Ya = 1
        }

        public enum EFilterBatal
        {
            Semua = 2,
            Batal = 1,
            TidakBatal = 0
        }

        public enum EStatusSPTPDTask
        {
            [Description("Undefined")]
            UNDEFINED = 0,
            [Description("Tidak Ada kewajiban Lapor")]
            NonEfektif = 1,
            [Description("Masa Efektif")]
            MasaEfektif = 2,
            [Description("Himbauan untuk lapor SPTPD")]
            Himbauan = 3,
            [Description("Lapor SPTPD")]
            SudahLaporSptpd = 4,
            [Description("Lapor SPTPD dan Bayar")]
            Patuh = 5,
            [Description("Pengawasan")]
            Pengawasan = 6,
            [Description("Lapor STPD")]
            LaporSptpdStpd = 7,
            [Description("Lapor Sptpd Stpd dan Bayar")]
            LaporSptpdStpdBayar = 8,
            [Description("Pemeriksaan")]
            Pemeriksaan = 9,
            [Description("SKPD")]
            SKPD = 10,
            [Description("SKPD dan Bayar")]
            SkpdBayar = 11,
            [Description("Himbauan Bayar")]
            HimbauanBayar = 12,
            [Description("Teguran Bayar")]
            TeguranBayar = 13,
            [Description("Pemeriksaan Histori")]
            PemeriksaanHistori = 14,
            [Description("Kadaluarsa")]
            Kadaluarsa = 15
        }

        public enum EModeHitungHari
        {
            [Description("Hari Kalender Normal")]
            Kalender = 1,
            [Description("Hari Kerja")]
            Kerja = 2
        }

        public enum EAktif
        {
            [Description("Tidak Aktif")]
            TidakAktif = 0,
            [Description("Aktif")]
            Aktif = 1
        }

        public enum EBlock
        {
            UnBlock = 0,
            Block = 1
        }

        public enum ETerminated
        {
            Tidak = 0,
            Ya = 1
        }

        public enum EModeApproveKeputusanSK
        {
            Manual = 0,
            Auto = 1
        }

        public enum EPermitStatusCreateSK
        {
            Allow = 0,
            FinalWFBlocked = 1,
            UserLoginInActive = 2,
            PegawaiInactive = 3,
            Expired = 4,
            NoAkses = 5,
        }

        public enum EPeruntukanABT
        {
            [Description("Niaga")]
            Niaga = 1,
            [Description("Non Niaga")]
            NonNiaga = 2,
            [Description("Bahan Baku Air")]
            BahanBakuAir = 3
        }
        public enum EPalangParkir
        {
            [Description("Tidak Diketahui")]
            TdkDiketahui = 0,
            [Description("Berpalang")]
            Berpalang = 1,
            [Description("Tidak Berpalang")]
            TdkBerpalang = 2
        }
        public enum EPungutTarifParkir
        {
            [Description("Tidak Memungut Tarif")]
            TdkBertarif = 1,
            [Description("BerTarif")]
            Bertarif = 2
        }
        public enum EKelolaParkir
        {
            [Description("Sendiri")]
            Sendiri = 1,
            [Description("Pihak Ketiga")]
            PihakKetiga = 2
        }
        public enum EFilterAktif
        {
            Semua = 2,
            Aktif = 1,
            TidakAktif = 0
        }

        public enum EJenisPegawai
        {
            NonPNS = 0,
            PNS = 1
        }


        public enum ETipeFile
        {
            PDF = 0,//application/pdf
            Image = 1,//image/png
            Excel = 2,//application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            App = 3,//application/x-msdownload  - dll / exe
            Zip = 4,//application/x-zip-compressed  - 
            Word = 5,//application/vnd.openxmlformats-officedocument.wordprocessingml.document
            Unknown = 6
        }



        public enum ESifatDokumen
        {
            Biasa = 0,
            Penting = 1,
            Rahasia = 2
        }
        public enum EAuth
        {
            NotAuth = 0,
            Auth = 1,
        }
        public enum EActivityState
        {
            Idle = 0,
            InReview = 1,
            FeedBack = 2,
            Done = 3
        }


        public enum EActivityStateFilter
        {
            Idle = 0,
            InReview = 1,
            FeedBack = 2,
            Done = 3,
            All = 4
        }

        public enum EDokumenSendNotifState
        {
            TidakDikirim,
            Dikirim

        }

        public enum EDokumenActivityApprove
        {
            Recomended = 1,
            NotRecomended = 2,
            Reject = -1
        }
        public enum EDokumenActivityState
        {
            Idle = 0,
            Recomended = 1,
            NotRecomended = 2,
            Reject = -1
        }

        public enum EStatusPermohonanNOP
        {
            Draft = 0,
            Submitted = 1,
            Canceled = -1,
        }

        public enum EStatusPermohonanApproval
        {
            Rejected = -1,
            Draft = 0,
            Submitted = 1,
            InReview = 2,
            WaitingFinal = 3,
            DoneAccepted = 4,
            DoneRejected = 5
        }
        public enum EApproval
        {
            Tolak = -1,
            Setuju = 1
        }


        public enum EPengawasanState
        {
            Rejected = -1,
            InReview = 1,
            WaitingSP = 2,
            Selesai = 3,
            Tutup = 4
        }

        public enum EPemeriksaanState
        {
            Rejected = -1,
            InReview = 1,
            WaitingSP = 2,
            Selesai = 3
        }

        public enum EDokumenState
        {
            [Description("Rejected")]
            Rejected = -1,
            [Description("Draft")]
            Draft = 0,
            [Description("Dalam Proses")]
            InReview = 1,
            [Description("Menunggu SK")]
            WaitingSK = 2,
            [Description("Ditolak SK")]
            FinishReject = 3,
            [Description("Diterima SK")]
            FinishAccepted = 4
        }

        public enum EDokumenApproveSK
        {
            FinishAccepted = 1,
            FinishReject = 2,
        }
        public enum EMailResultFilter
        {
            Fail = 0,
            Success = 1,
            All = 3
        }
        public enum EStatusPenanggungJawabNpwpd
        {
            NonAktif = 0,
            Aktif = 1,
        }
        public enum EJenisWajibPajak
        {
            Perorangan = 0,
            Badan = 1,
            Internal = 2,
            Pemerintah = 3,
            Temporary = 5
        }
        public enum EFilterJenisWajibPajak
        {
            All = -1,
            Perorangan = 0,
            Badan = 1,
        }
        public enum EStatusNPWPDSK
        {
            Diterima = 1,
            Ditolak = 2
        }
        public enum EStatusNpwpd
        {
            NonAktif = 0,
            Aktif = 1,
            Block = 2
        }
        public enum EFilterStatusNpwpd
        {
            All = -1,
            NonAktif = 0,
            Aktif = 1
        }
        public enum EFilterColumnNpwpd
        {
            Nama = 0,
            Alamat = 1,
            Kota = 2,
            Propinsi = 3,
            Email = 4,

        }
    }
}
