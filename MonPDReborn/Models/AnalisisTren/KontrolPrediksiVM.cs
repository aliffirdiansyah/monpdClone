using static MonPDReborn.Models.AktivitasWP.PenghimbauanVM;

namespace MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM
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
        public List<KontrolPrediksi> DataKontrolPrediksiList { get; set; } = new();


        public Show()
        {
            DataKontrolPrediksiList = Method.GetFilteredData();
        }
    }

    public class Method
    {
        public static Dashboard GetDashboardData()
        {
            return new Dashboard
            {
                TotalTarget = 500000000,
                TotalRealisasi = 435750000.50,
                RataRataPencapaian = 87.15 
            };
        }

        public static List<KontrolPrediksi> GetFilteredData()
        {
            return GetAllDataKontrolPrediksi();
        }

        private static List<KontrolPrediksi> GetAllDataKontrolPrediksi()
        {
            return new List<KontrolPrediksi>
            {
                new KontrolPrediksi
                {
                    tgl = new DateTime(2025, 7, 15),
                    JenisPajak = "PBJT Makanan & Minuman",
                    Target = 1_000_000_000m,
                    RealisasiBulanLalu = 400_000_000m,
                    RealisasiBulanIni = 350_000_000m,
                    RealisasiHari = 25_000_000m
                },
                new KontrolPrediksi
                {
                    tgl = new DateTime(2025, 7, 15),
                    JenisPajak = "PBJT Parkir",
                    Target = 500_000_000m,
                    RealisasiBulanLalu = 200_000_000m,
                    RealisasiBulanIni = 150_000_000m,
                    RealisasiHari = 10_000_000m
                },
                new KontrolPrediksi
                {
                    tgl = new DateTime(2025, 7, 15),
                    JenisPajak = "PBJT Hiburan",
                    Target = 300_000_000m,
                    RealisasiBulanLalu = 100_000_000m,
                    RealisasiBulanIni = 120_000_000m,
                    RealisasiHari = 5_000_000m
                },
                new KontrolPrediksi
                {
                    tgl = new DateTime(2025, 7, 15),
                    JenisPajak = "PBB",
                    Target = 2_000_000_000m,
                    RealisasiBulanLalu = 1_200_000_000m,
                    RealisasiBulanIni = 600_000_000m,
                    RealisasiHari = 50_000_000m
                }
            };
        }
    }

    public class Dashboard
    {
        public int TotalTarget { get; set; }
        public double TotalRealisasi { get; set; }
        public double RataRataPencapaian { get; set; }

    }

    public class KontrolPrediksi
    {
        public DateTime tgl { get; set; }
        public string JenisPajak { get; set; } = null!;
        public decimal Target { get; set; }
        public decimal RealisasiBulanLalu { get; set; }
        public decimal RealisasiBulanIni { get; set; }
        public decimal RealisasiHari { get; set; }
        public decimal Jumlah => RealisasiBulanIni + RealisasiBulanLalu + RealisasiHari;
        public decimal Persentase => Target > 0
            ? Math.Round((Jumlah / Target) * 100, 2)
            : 0;
    }
}
