using static MonPDReborn.Models.DataWP.ProfileWPVM;

namespace MonPDReborn.Models.DataWP
{
    public class ProfilePembayaranWPVM
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
            public List<PembayaranWP> DataPembayaranWPList { get; set; } = new();


            public Show()
            {
                DataPembayaranWPList = Method.GetFilteredData();
            }
        }
        public class Detail
        {
            public List<ProfilWP> DataWPList { get; set; } = new();
            public List<DetailPembayaranWP> DetailPembayaranWPList { get; set; } = new();

            public Detail() { }

            public Detail(string NPWPD)
            {
                DataWPList = Method.GetFilteredDataProfilWP(NPWPD);
                DetailPembayaranWPList = Method.GetDetailData(NPWPD);
            }

        }
        public class Method
        {
            public static List<PembayaranWP> GetFilteredData()
            {
                return GetAllDataPembayaranWP();
            }

            private static List<PembayaranWP> GetAllDataPembayaranWP()
            {
                return new List<PembayaranWP>
                {
                    new PembayaranWP {NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Ketetapan = "Rp 20.000.000", Terbayar = "Rp 20.000.000", Status = "Lunas"},
                    new PembayaranWP {NPWPD = "01.01.0002", NamaWP = "PT. ABC", Ketetapan = "Rp 15.300.000", Terbayar = "Rp 12.000.000", Status = "Sebagian"},
                    new PembayaranWP {NPWPD = "01.01.0003", NamaWP = "Karaoke Galaxy", Ketetapan = "Rp 7.800.000", Terbayar = "Rp 7.800.000", Status = "Tunggak"},
                    
                };
            }

            public static List<DetailPembayaranWP> GetDetailData(string npwpd)
            {
                return GetAllDetailPembayaranWP()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList(); ;
            }

            private static List<DetailPembayaranWP> GetAllDetailPembayaranWP()
            {
                return new List<DetailPembayaranWP>
                {
                    new DetailPembayaranWP {NPWPD = "01.01.0001", JenisPajak = "Pajak Hotel", TahunBuku = "Hotel Mawar", Ketetapan = "Rp 20.000.000", Terbayar = "Rp 20.000.000", Tunggakan = "Rp 20.000.000", Status = "Lunas", Kepatuhan = "00%"},
                    new DetailPembayaranWP {NPWPD = "01.01.0002" , JenisPajak = "Pajak Restoran", TahunBuku = "PT. ABC", Ketetapan = "Rp 15.300.000", Terbayar = "Rp 12.000.000", Tunggakan = "Rp 20.000.000", Status = "Sebagian", Kepatuhan = "00%"},
                    new DetailPembayaranWP {NPWPD = "01.01.0003", JenisPajak = "Pajak Hiburan", TahunBuku = "Karaoke Galaxy", Ketetapan = "Rp 7.800.000", Terbayar = "Rp 7.800.000",Tunggakan = "Rp 20.000.000", Status = "Tunggak", Kepatuhan = "00%"},

                };
            }

            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalWP = 1274,
                    WPBulan = 156,
                    RataWPBulan = 103,
                    KetetapanWP = 11.7,
                    TerbayarWP = 8.2,
                    TargetKepatuhan = 85
                };
            }

            public static List<ProfilWP> GetFilteredDataProfilWP(string npwpd)
            {
                return GetAllDataProfilWP()
                    .Where(x => x.NPWPD.Equals(npwpd, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            private static List<ProfilWP> GetAllDataProfilWP()
            {
                return new List<ProfilWP>
                {
                    new ProfilWP {NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", JenisSubjek = "Badan Usaha", NIB = "1234567890", Status = "Aktif", TanggalDaftar = new DateTime(2018, 5, 12), AlamatDom = "Jl. Mawar No. 1, Jakarta", AlamatUsaha = "Jl. Mawar Raya No. 2, Jakarta", Kontak = "08123456789", Email = "info@hotelmawar.com"},
                    new ProfilWP {NPWPD = "01.01.0002", NamaWP = "PT. ABC", JenisSubjek = "Badan Usaha", NIB = "9876543210", Status = "Aktif", TanggalDaftar = new DateTime(2020, 1, 20), AlamatDom = "Jl. Sederhana No. 5, Bandung", AlamatUsaha = "Jl. Sederhana Raya No. 10, Bandung", Kontak = "08129876543", Email = "contact@restoransederhana.co.id"},
                    new ProfilWP {NPWPD = "01.01.0003", NamaWP = "Karaoke Galaxy", JenisSubjek = "Orang Pribadi", NIB = "1122334455", Status = "Non Aktif", TanggalDaftar = new DateTime(2015, 7, 15), AlamatDom = "Jl. Kenanga No. 8, Surabaya", AlamatUsaha = "Jl. Kenanga Raya No. 12, Surabaya", Kontak = "08122223333", Email = "owner@tamankidsplay.id"}
                };

            }
        }

        public class PembayaranWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Ketetapan { get; set; } = null!;
            public string Terbayar { get; set; } = null!;
            public string Status { get; set; } = null!;
        }

        public class DetailPembayaranWP
        {
            public string NPWPD { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string TahunBuku { get; set; } = null!;
            public string Ketetapan { get; set; } = null!;
            public string Terbayar { get; set; } = null!;
            public string Tunggakan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public string Kepatuhan { get; set; } = null!;
        }

        public class ProfilWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string NIB { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TanggalDaftar { get; set; }
            public string AlamatDom { get; set; } = null!;
            public string AlamatUsaha { get; set; } = null!;
            public string Kontak { get; set; } = null!;
            public string Email { get; set; } = null!;

            public List<DetailPembayaranWP> DetailPembayaranWPList { get; set; }

        }

        public class Dashboard
        {
            // Wajib Pajak Baru
            public int TotalWP { get; set; }       // Total tahun ini
            public double WPBulan { get; set; }       // Bulan ini
            public double RataWPBulan { get; set; }   // Rata-rata per bulan

            public double PresentaseWP
            {
                get
                {
                    if (RataWPBulan == 0) return 0; // Hindari pembagi 0
                    return ((double)(WPBulan - RataWPBulan) / RataWPBulan) * 100;
                }
            }

            // Wajib Pajak Aktif
            public Double KetetapanWP { get; set; }
            public Double TerbayarWP { get; set; }
            public int TargetKepatuhan { get; set; }

            public double PersentaseKepatuhan
            {
                get
                {
                    if (KetetapanWP == 0) return 0; // Hindari pembagi 0
                    return ((double)TerbayarWP / KetetapanWP) * 100;
                }
            }

            public double Tunggakan
            {
                get
                {
                    return ((double)KetetapanWP - TerbayarWP);
                }
            }

            public double WPMenunggak
            {
                get
                {
                    return ((double)TotalWP - WPBulan);
                }
            }

        }
    }
}
