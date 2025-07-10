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
                // Skenario 1: Melebihi target
                new KontrolPrediksi
                {
                    JenisPajak = "Pajak Hotel",
                    Target = "Rp 150.000.000",
                    RealisasiBulanLalu = "Rp 145.500.000",
                    RealisasiBulanIni = "Rp 152.300.000",
                    Prediksi = "Rp 151.000.000",
                    Jumlah = "101,5%",
                    Persentase = 101.5
                },

                // Skenario 2: Di bawah target
                new KontrolPrediksi
                {
                    JenisPajak = "Pajak Restoran",
                    Target = "Rp 200.000.000",
                    RealisasiBulanLalu = "Rp 198.000.000",
                    RealisasiBulanIni = "Rp 185.000.000",
                    Prediksi = "Rp 188.000.000",
                    Jumlah = "92,5%",
                    Persentase = 92.5
                },

                // Skenario 3: Hampir mencapai target
                new KontrolPrediksi
                {
                    JenisPajak = "Pajak Hiburan",
                    Target = "Rp 75.000.000",
                    RealisasiBulanLalu = "Rp 70.000.000",
                    RealisasiBulanIni = "Rp 73.800.000",
                    Prediksi = "Rp 74.500.000",
                    Jumlah = "98,4%",
                    Persentase = 98.4
                },
    
                // Skenario 4: Jauh di bawah target
                new KontrolPrediksi
                {
                    JenisPajak = "Pajak Reklame",
                    Target = "Rp 60.000.000",
                    RealisasiBulanLalu = "Rp 55.000.000",
                    RealisasiBulanIni = "Rp 35.000.000",
                    Prediksi = "Rp 40.000.000",
                    Jumlah = "58,3%",
                    Persentase = 58.3
                },

                // Skenario 5: Sesuai target
                new KontrolPrediksi
                {
                    JenisPajak = "Pajak Parkir",
                    Target = "Rp 90.000.000",
                    RealisasiBulanLalu = "Rp 88.000.000",
                    RealisasiBulanIni = "Rp 90.000.000",
                    Prediksi = "Rp 90.000.000",
                    Jumlah = "100,0%",
                    Persentase = 100.0
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
        public string JenisPajak { get; set; } = null!;
        public string Target { get; set; } = null!;
        public string RealisasiBulanLalu { get; set; } = null!;
        public string RealisasiBulanIni { get; set; } = null!;
        public string Prediksi { get; set; } = null!;
        public string Jumlah { get; set; } = null!;
        public double Persentase { get; set; }
    }
}
