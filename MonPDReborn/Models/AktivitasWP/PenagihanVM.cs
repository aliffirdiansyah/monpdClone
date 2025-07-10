using static MonPDReborn.Models.DataWP.ProfilePiutangWPVM;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM;

namespace MonPDReborn.Models.AktivitasWP
{
    public class PenagihanVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }
        public class Show
        {
            public string JenisPajak { get; set; } = string.Empty;

            public List<RekapDataWP> DataRekapWP { get; set; } = new();

            public Show() { }

            public Show(string jenisPajak)
            {
                JenisPajak = jenisPajak;

                // ambil data sesuai filter
                DataRekapWP = Method.GetDataRekapWPList(jenisPajak);
            }
        }

        public class Detail
        {
            public List<DetailWP> DetailWPList { get; set; } = new();
            public List<SejarahPembayaran> DetailSejarahWPList { get; set; } = new();
            public List<RiwayatUpaya> DetailUpayaWPList { get; set; } = new();


            public Detail() { }

            public Detail(string NPWPD)
            {
                DetailWPList = Method.GetFilteredDetailWP(NPWPD);
                DetailSejarahWPList = Method.GetFilteredSejarahPembayaran(NPWPD);
                DetailUpayaWPList = Method.GetFilteredRiwayatUpaya(NPWPD);
            }
        }
        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    JmlPenagihan = 1274_000_000,
                    PenagihanAktif = 156,
                    TotalPenerimaan = 1_500_000_000
                };
            }
            public static List<RekapDataWP> GetDataRekapWPList(string JenisPajak)
            {
                var allData = GetRekapDataWP();

                return allData
                    .Where(d =>
                        (string.IsNullOrWhiteSpace(JenisPajak) ||
                         (d.JenisPajak != null && d.JenisPajak.Contains(JenisPajak, StringComparison.OrdinalIgnoreCase))))
                    .ToList();
            }

            private static List<RekapDataWP> GetRekapDataWP()
            {
                return new List<RekapDataWP>
                {
                    new RekapDataWP
                    {
                        NPWPD = "3578090000001",
                        NamaWP = "PT Maju Terus",
                        JenisPajak = "Hotel",
                        NilaiTagihan = 12_500_000m,
                        StatusPengaihan = "Surat Tagihan"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000002",
                        NamaWP = "CV Sukses Selalu",
                        JenisPajak = "Restoran",
                        NilaiTagihan = 7_800_000m,
                        StatusPengaihan = "Surat Paksa"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000003",
                        NamaWP = "Toko Makmur",
                        JenisPajak = "Hiburan",
                        NilaiTagihan = 5_250_000m,
                        StatusPengaihan = "Surat Sita"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000004",
                        NamaWP = "PT Aman Sentosa",
                        JenisPajak = "Parkir",
                        NilaiTagihan = 3_000_000m,
                        StatusPengaihan = "Surat Paksa"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000005",
                        NamaWP = "CV Berkah",
                        JenisPajak = "Hotel",
                        NilaiTagihan = 15_750_000m,
                        StatusPengaihan = "Surat Tagihan"
                    }
                };
            }

            public static List<DetailWP> GetFilteredDetailWP(string npwpd)
            {
                return GetDetailWP()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DetailWP> GetDetailWP()
            {
                return new List<DetailWP>
                {
                    new DetailWP
                    {
                        NPWPD = "3578090000001",
                        NamaWP = "PT Maju Terus",
                        Alamat = "Jl. Merdeka No. 123, Surabaya",
                        Telpon = "031-5551234",
                        TotalPiutang = 12_500_000m,
                        StatusBayar = "Belum Lunas",
                        TingkatRespon = "Cepat",
                        JumlahTindakan = "2",
                        TindakanTerakhir = "Surat Tagihan"
                    },
                    new DetailWP
                    {
                        NPWPD = "3578090000002",
                        NamaWP = "CV Sukses Selalu",
                        Alamat = "Jl. Raya Darmo No. 45, Surabaya",
                        Telpon = "031-6662345",
                        TotalPiutang = 7_800_000m,
                        StatusBayar = "Menunggak",
                        TingkatRespon = "Sedang",
                        JumlahTindakan = "3",
                        TindakanTerakhir = "Surat Paksa"
                    },
                    new DetailWP
                    {
                        NPWPD = "3578090000003",
                        NamaWP = "Toko Makmur",
                        Alamat = "Jl. Pahlawan No. 78, Surabaya",
                        Telpon = "031-7773456",
                        TotalPiutang = 5_250_000m,
                        StatusBayar = "Belum Bayar",
                        TingkatRespon = "Lambat",
                        JumlahTindakan = "1",
                        TindakanTerakhir = "Surat Sita"
                    }
                };
            }

            public static List<SejarahPembayaran> GetFilteredSejarahPembayaran(string npwpd)
            {
                return GetSejarahPembayaran()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<SejarahPembayaran> GetSejarahPembayaran()
            {
                return new List<SejarahPembayaran>
                {
                    new SejarahPembayaran
                    {
                        NPWPD = "3578090000001",
                        NamaWP = "PT Maju Terus",
                        JenisPajak = "Hotel",
                        NamaOP = "Hotel Maju Terus",
                        Piutang = 12_500_000m,
                        Terbayar = 5_000_000m,
                        TanggalSejarah = new DateTime(2025, 6, 15),
                        Keterangan = "Pembayaran sebagian melalui transfer"
                    },
                    new SejarahPembayaran
                    {
                        NPWPD = "3578090000002",
                        NamaWP = "CV Sukses Selalu",
                        JenisPajak = "Restoran",
                        NamaOP = "Restoran Sukses",
                        Piutang = 7_800_000m,
                        Terbayar = 3_500_000m,
                        TanggalSejarah = new DateTime(2025, 5, 20),
                        Keterangan = "Pembayaran dicicil 2 tahap"
                    },
                    new SejarahPembayaran
                    {
                        NPWPD = "3578090000003",
                        NamaWP = "Toko Makmur",
                        JenisPajak = "Hiburan",
                        NamaOP = "Bioskop Makmur",
                        Piutang = 5_250_000m,
                        Terbayar = 0m,
                        TanggalSejarah = new DateTime(2025, 4, 10),
                        Keterangan = "Belum ada pembayaran"
                    }
                };
            }
            public static List<RiwayatUpaya> GetFilteredRiwayatUpaya(string npwpd)
            {
                return GetRiwayatUpaya()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            private static List<RiwayatUpaya> GetRiwayatUpaya()
            {
                return new List<RiwayatUpaya>
                {
                    new RiwayatUpaya
                    {
                        NPWPD = "3578090000001",
                        NamaWP = "PT Maju Terus",
                        TanggalRiwayat = new DateTime(2025, 6, 16),
                        JenisTindakan = "Surat Tagihan",
                        status = "Terkirim",
                        Petugas = "Budi Santoso"
                    },
                    new RiwayatUpaya
                    {
                        NPWPD = "3578090000002",
                        NamaWP = "CV Sukses Selalu",
                        TanggalRiwayat = new DateTime(2025, 5, 22),
                        JenisTindakan = "Surat Paksa",
                        status = "Diproses",
                        Petugas = "Ani Lestari"
                    },
                    new RiwayatUpaya
                    {
                        NPWPD = "3578090000003",
                        NamaWP = "Toko Makmur",
                        TanggalRiwayat = new DateTime(2025, 4, 12),
                        JenisTindakan = "Surat Sita",
                        status = "Belum Tindak Lanjut",
                        Petugas = "Dedi Gunawan"
                    }
                };
            }
        }
      

        public class Dashboard
        { 
            public decimal JmlPenagihan { get; set; }
            public int PenagihanAktif { get; set; }
            public decimal TotalPenerimaan { get; set; }

        }

        public class RekapDataWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public decimal NilaiTagihan { get; set; }
            public string StatusPengaihan { get; set; } = null!;
        }

        public class DetailWP
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
