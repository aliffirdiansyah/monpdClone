using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using static MonPDReborn.Models.AktivitasOP.PendataanObjekPajakVM;

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
            public Detail()
            {
                
            }
            public Detail(string nop)
            {
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
