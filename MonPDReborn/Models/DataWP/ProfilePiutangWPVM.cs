using static MonPDReborn.Models.DataWP.ProfileWPVM;

namespace MonPDReborn.Models.DataWP
{
    public class ProfilePiutangWPVM
    {
        public class Index
        {
            public DashboardPiutang Data { get; set; } = new DashboardPiutang();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }
        public class ShowRekap
        {
            public List<RekapPiutangWP> DataRekapPiutangWPList { get; set; } = new();


            public ShowRekap()
            {
                DataRekapPiutangWPList = Method.GetDataRekapPiutangWP();
            }
        }

        public class ShowRiwayat
        {
            public List<RiwayatPiutangWP> DataRiwayatPiutangWPList { get; set; } = new();


            public ShowRiwayat()
            {
                DataRiwayatPiutangWPList = Method.GetDataRiwayatPiutangWP();
            }
        }
        public class Detail
        {
            public List<DetailPiutangWP> DetailPiutangWPList { get; set; } = new();
            public List<SejarahPembayaran> DetailSejarahWPList { get; set; } = new();
            public List<RiwayatUpaya> DetailUpayaWPList { get; set; } = new();


            public Detail() { }

            public Detail(string NPWPD)
            {
                DetailPiutangWPList = Method.GetFilteredDetailPiutangWP(NPWPD);
                DetailSejarahWPList = Method.GetFilteredDetailSejarahWP(NPWPD);
                DetailUpayaWPList = Method.GetFilteredDetailUpayaWP(NPWPD);
            }
        }
        public class Method
        {
            public static DashboardPiutang GetDashboardData()
            {
                return new DashboardPiutang
                {
                    TotalKetetapan = 1000000,
                    TotalPembayaran = 500000,
                    SisaPiutang = 500000,
                    OrangPribadi = 10,
                    BadanUsaha = 5,
                    TargetBulanan = 200000,
                    RealisasiBulanan = 150000,
                    WPRespon = 8,
                    WPTidakRespon = 7
                };
            }

            public static List<RekapPiutangWP> GetDataRekapPiutangWP()
            {
                return GetAllDataRekapPiutangWP();
            }

            private static List<RekapPiutangWP> GetAllDataRekapPiutangWP()
            {
                return new List<RekapPiutangWP>
                {
                    new RekapPiutangWP { NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Piutang = 20000000, Status = "Belum Bayar" },
                    new RekapPiutangWP { NPWPD = "01.01.0002", NamaWP = "Restoran Sederhana", Piutang = 150000000, Status = "Bayar Sebagian" },
                    new RekapPiutangWP { NPWPD = "01.01.0003", NamaWP = "Taman Hiburan Anak", Piutang = 30000000, Status = "Ditagih" }
                };
            }

            public static List<RiwayatPiutangWP> GetDataRiwayatPiutangWP()
            {
                return GetAllDataRiwayatPiutangWP();
            }

            public static List<RiwayatPiutangWP> GetAllDataRiwayatPiutangWP()
            {
                return new List<RiwayatPiutangWP>
                {
                    new RiwayatPiutangWP { NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Teguran1 = "2023-01-15", Teguran2 = "2023-02-20", SuratPaksa = "Proses", Status = "Proses Sita" },
                    new RiwayatPiutangWP { NPWPD = "01.01.0002", NamaWP = "Restoran Sederhana", Teguran1 = "2023-01-20", Teguran2 = "2023-02-25", SuratPaksa = "Proses", Status = "Negosiasi" },
                    new RiwayatPiutangWP { NPWPD = "01.01.0003", NamaWP = "Taman Hiburan Anak", Teguran1 = "2023-01-25", Teguran2 = "Proses", SuratPaksa = "", Status = "Menunggu" }
                };
            }
            public static List<DetailPiutangWP> GetFilteredDetailPiutangWP(string npwpd)
            {
                return GetDetailPiutangWP()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<DetailPiutangWP> GetDetailPiutangWP()
            {
                return new List<DetailPiutangWP>
                {
                    new DetailPiutangWP
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        Alamat = "Jl. Melati No. 1",
                        Telpon = "021-1234567",
                        TotalPiutang = 20000000,
                        StatusBayar = "Belum Bayar",
                        TingkatRespon = "Rendah",
                        JumlahTindakan = "2",
                        TindakanTerakhir = "Surat Peringatan"
                    },

                    new DetailPiutangWP
                    {
                        NPWPD = "01.01.0002",
                        NamaWP = "Restoran Sederhana",
                        Alamat = "Jl. Mawar No. 5",
                        Telpon = "021-7654321",
                        TotalPiutang = 150000000,
                        StatusBayar = "Bayar Sebagian",
                        TingkatRespon = "Sedang",
                        JumlahTindakan = "3",
                        TindakanTerakhir = "Negosiasi Pembayaran"
                    },

                    new DetailPiutangWP
                    {
                        NPWPD = "01.01.0003",
                        NamaWP = "Taman Hiburan Anak",
                        Alamat = "Jl. Kenanga No. 10",
                        Telpon = "021-9876543",
                        TotalPiutang = 30000000,
                        StatusBayar = "Ditagih",
                        TingkatRespon = "Tinggi",
                        JumlahTindakan = "1",
                        TindakanTerakhir = "Surat Tagihan"
                    }
                };
            }
            public static List<SejarahPembayaran> GetFilteredDetailSejarahWP(string npwpd)
            {
                return GetSejarahPembayaran()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<SejarahPembayaran> GetSejarahPembayaran()
            {
                return new List<SejarahPembayaran>
                {
                    new SejarahPembayaran
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        JenisPajak = "Hotel",
                        NamaOP = "Bangunan Utama",
                        Piutang = 20000000m,
                        Terbayar = 5000000m,
                        TanggalSejarah = DateTime.Now.AddDays(-60),
                        Keterangan = "Belum Bayar"
                    },
                    new SejarahPembayaran
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        JenisPajak = "Hotel",
                        NamaOP = "Bangunan Utama",
                        Piutang = 15000000m,
                        Terbayar = 3000000m,
                        TanggalSejarah = DateTime.Now.AddDays(-30),
                        Keterangan = "Sudah Bayar"
                    },
                    new SejarahPembayaran
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        JenisPajak = "Hotel",
                        NamaOP = "Bangunan Utama",
                        Piutang = 12000000m,
                        Terbayar = 0m,
                        TanggalSejarah = DateTime.Now.AddDays(-7),
                        Keterangan = "Belum Bayar"
                    }
                };
            }

            public static List<RiwayatUpaya> GetFilteredDetailUpayaWP(string npwpd)
            {
                return GetRiwayatUpaya()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<RiwayatUpaya> GetRiwayatUpaya()
            {
                return new List<RiwayatUpaya>
                {
                     new RiwayatUpaya
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        TanggalRiwayat = DateTime.Now.AddDays(-30),
                        JenisTindakan = "Surat Teguran 1",
                        status = "Terkirim",
                        Petugas = "Budi"
                    },
                    new RiwayatUpaya
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        TanggalRiwayat = DateTime.Now.AddDays(-20),
                        JenisTindakan = "Surat Teguran 2",
                        status = "Dilaksanakan",
                        Petugas = "Siti"
                    },
                    new RiwayatUpaya
                    {
                        NPWPD = "01.01.0001",
                        NamaWP = "Hotel Mawar",
                        TanggalRiwayat = DateTime.Now.AddDays(-10),
                        JenisTindakan = "Surat Peringatan",
                        status = "Dalam Proses",
                        Petugas = "Andi"
                    }
                };
            }
        }

        public class DashboardPiutang
        {
            public decimal TotalKetetapan { get; set; }
            public decimal TotalPembayaran { get; set; }
            public decimal SisaPiutang { get; set; }
            public int OrangPribadi { get; set; }
            public int BadanUsaha { get; set; }
            public int TotalWP => OrangPribadi + BadanUsaha;
            public decimal TargetBulanan { get; set; }
            public decimal RealisasiBulanan { get; set; }
            public int WPRespon { get; set; }
            public int WPTidakRespon { get; set; }
            public decimal PersentaseRespon => TotalWP > 0 ? (decimal)WPRespon / TotalWP * 100 : 0m;
        }

        public class RekapPiutangWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public decimal Piutang { get; set; }
            public string Status { get; set; } = null!;
        }

        public class RiwayatPiutangWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Teguran1 { get; set; } = null!;
            public string Teguran2 { get; set; } = null!;
            public string SuratPaksa { get; set; } = null!;
            public string Status { get; set; } = null!;
        }

        public class DetailPiutangWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string Telpon { get; set; } = null!;
            public decimal TotalPiutang { get; set; }
            public string StatusBayar { get; set; } = null!;
            public string TingkatRespon { get; set; } = null!;
            public string JumlahTindakan { get; set; } = null!;
            public string TindakanTerakhir { get; set; } = null!;

        }

        public class SejarahPembayaran
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public decimal Piutang { get; set; }
            public decimal Terbayar { get; set; }
            public DateTime TanggalSejarah { get; set; }
            public string Keterangan { get; set; } = null!;

        }

        public class RiwayatUpaya
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public DateTime TanggalRiwayat { get; set; }
            public string JenisTindakan { get; set; } = null!;
            public string status { get; set; } = null!;
            public string Petugas { get; set; } = null!;
        }
    }
}
