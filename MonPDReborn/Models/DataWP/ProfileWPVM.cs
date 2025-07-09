using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using static MonPDReborn.Models.AktivitasOP.PendataanObjekPajakVM;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;

namespace MonPDReborn.Models.DataWP
{
    public class ProfileWPVM
    {
        public class Index
        {
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }
        public class Show
        {
            public List<RekapWP> DataRekapWPList { get; set; } = new();


            public Show()
            {
                DataRekapWPList = Method.GetFilteredData();
            }
        }
        public class Detail
        {
            public List<ProfilWP> DataWPList { get; set; } = new();

            public Detail() { }

            public Detail(string NPWPD)
            {
                DataWPList = Method.GetFilteredDataProfilWP(NPWPD);
            }
        }

        public class Method
        {
            public static List<RekapWP> GetFilteredData()
            {
                return GetAllDataRekapWP();
            }

            private static List<RekapWP> GetAllDataRekapWP()
            {
                return new List<RekapWP>
                {
                    new RekapWP {NPWPD = "01.01.0001", Nama = "Hotel Mawar", JenisSubjek = "Badan Usaha", Kontak = "08123456789", Status = "Aktif"},
                    new RekapWP {NPWPD = "01.01.0002", Nama = "Restoran Sederhana", JenisSubjek = "Badan Usaha", Kontak = "08129876543", Status = "Aktif" },
                    new RekapWP {NPWPD = "01.01.0003", Nama = "Taman Hiburan Anak", JenisSubjek = "Orang Pribadi", Kontak = "08122223333", Status = "Non Aktif"}
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
                    new ProfilWP {NPWPD = "01.01.0001", Nama = "Hotel Mawar", JenisSubjek = "Badan Usaha", NIB = "1234567890", Status = "Aktif", TanggalDaftar = new DateTime(2018, 5, 12), AlamatDom = "Jl. Mawar No. 1, Jakarta", AlamatUsaha = "Jl. Mawar Raya No. 2, Jakarta", Kontak = "08123456789", Email = "info@hotelmawar.com"},
                    new ProfilWP {NPWPD = "01.01.0002", Nama = "Restoran Sederhana", JenisSubjek = "Badan Usaha", NIB = "9876543210", Status = "Aktif", TanggalDaftar = new DateTime(2020, 1, 20), AlamatDom = "Jl. Sederhana No. 5, Bandung", AlamatUsaha = "Jl. Sederhana Raya No. 10, Bandung", Kontak = "08129876543", Email = "contact@restoransederhana.co.id"},
                    new ProfilWP {NPWPD = "01.01.0003", Nama = "Taman Hiburan Anak", JenisSubjek = "Orang Pribadi", NIB = "1122334455", Status = "Non Aktif", TanggalDaftar = new DateTime(2015, 7, 15), AlamatDom = "Jl. Kenanga No. 8, Surabaya", AlamatUsaha = "Jl. Kenanga Raya No. 12, Surabaya", Kontak = "08122223333", Email = "owner@tamankidsplay.id"}
                };

            }

            
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    OrangPribadi = 8234,
                    BadanUsaha = 4613,
                    WPTahun = 1234,
                    WPBulan = 156,
                    RataWPBulan = 103,
                    WPAktif = 11523
                };
            }


        }

        public class RekapWP
        {
            public string NPWPD { get; set; } = null!;
            public string Nama {  get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string Kontak {  get; set; } = null!;
            public string Status { get; set; } = null!;
        }

        public class ProfilWP
        {
            public string NPWPD { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string NIB { get;set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TanggalDaftar { get; set; }
            public string AlamatDom {  get; set; } = null!;
            public string AlamatUsaha { get; set; } = null!;
            public string Kontak { get; set; } = null!;
            public string Email { get; set; } = null!;

        }

        public class Dashboard
        {
            // Data dasar
            public int TotalWP => OrangPribadi + BadanUsaha;
            public int OrangPribadi { get; set; }
            public int BadanUsaha { get; set; }

            // Persentase otomatis
            public double PersentaseOrangPribadi
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (double)OrangPribadi / TotalWP * 100;
                }
            }

            public double PersentaseBadanUsaha
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (double)BadanUsaha / TotalWP * 100;
                }
            }

            // Wajib Pajak Baru
            public int WPTahun { get; set; }       // Total tahun ini
            public int WPBulan { get; set; }       // Bulan ini
            public int RataWPBulan { get; set; }   // Rata-rata per bulan

            public double Presentase
            {
                get
                {
                    if (RataWPBulan == 0) return 0; // Hindari pembagi 0
                    return ((double)(WPBulan - RataWPBulan) / RataWPBulan) * 100;
                }
            }

            // Wajib Pajak Aktif
            public int WPAktif { get; set; }
            public int WPNonAktif
            {
                get
                {
                    return TotalWP - WPAktif;
                }
            }

            public double PersentaseAktif
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return ((double)WPAktif / TotalWP) * 100;
                }
            }
        }


    }
}
