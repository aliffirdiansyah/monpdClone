

using MonPDLib.General;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.MonitoringWilayah
{
    public class MonitoringWilayahVM
    {
        public class Index
        {
            public int SelectedPajak {  get; set; }
            public int SelectedBulan {  get; set; }
            public List<SelectListItem> JenisPajakList { get; set; } = new();
            public List<SelectListItem> BulanList { get; set; } = new();
            public Index()
            {
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                for (int i = 1; i <= 12; i++)
                {
                    var namaBulan = new DateTime(1, i, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    BulanList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = namaBulan
                    });
                }
            }

        }
        public class Show
        {
            public List<RealisasiWilayah> RealisasiWilayahList { get; set; } = new();
            public List<RealisasiJenis> RealisasiJenisList { get; set; } = new();

            public Show() { }


            public Show(string wilayah, int tahun, int bulan, string jenisPajak)
            {
                RealisasiWilayahList = Method.GetDataRealisasiWilayahList(wilayah, tahun, bulan, jenisPajak);
                RealisasiJenisList = Method.GetDataRealisasiJenisList(wilayah, tahun, bulan, jenisPajak);
            }
        }

        public class Detail
        {
            public List<DataHarian> DataHarianList { get; set; } = new();

            public Detail() { }

            public Detail(string wilayah, int tahun, int bulan, string jenisPajak)
            {
                DataHarianList = Method.GetDataDataHarianList(wilayah, tahun, bulan, jenisPajak);
            }
        }
        public class Method
        {
            public static List<RealisasiWilayah> GetDataRealisasiWilayahList(string wilayah, int tahun, int bulan, string jenisPajak)
            {
                var allData = GetAllData();
                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return allData;

                return allData.Where(r =>
                    (string.IsNullOrEmpty(wilayah) || wilayah == "Semua Wilayah" || r.Wilayah == wilayah) &&
                    (tahun <= 0 || r.Tanggal.Year == tahun) &&
                    (bulan <= 0 || r.Tanggal.Month == bulan)
                ).ToList();
            }

            private static List<RealisasiWilayah> GetAllData()
            {
                return new List<RealisasiWilayah>
                {
                    new RealisasiWilayah{ Wilayah = "UPTB 1", Tanggal = new DateTime(2025, 7, 8), Lokasi = "Kota Utara", Realisasi = 30800000000, Target = 35200000000, Status = "UP", Tren = 4.2},
                    new RealisasiWilayah{ Wilayah = "UPTB 2", Tanggal = new DateTime(2025, 7, 8), Lokasi = "Kota Selatan", Realisasi = 22100000000, Target = 28500000000, Status = "UP", Tren = 4.2},
                    new RealisasiWilayah{ Wilayah = "UPTB 3", Tanggal = new DateTime(2025, 7, 8), Lokasi = "Kota Timur", Realisasi = 16500000000, Target = 22800000000, Status = "UP", Tren = 4.2},
                    new RealisasiWilayah{ Wilayah = "UPTB 4", Tanggal = new DateTime(2025, 7, 8), Lokasi = "Kota Barat", Realisasi = 15800000000, Target = 19700000000, Status = "UP", Tren = 4.2},
                    new RealisasiWilayah{ Wilayah = "UPTB 5", Tanggal = new DateTime(2025, 7, 8), Lokasi = "Kota Pusat", Realisasi = 13000000000, Target = 19500000000, Status = "UP", Tren = 4.2},
                };
            }

            public static List<RealisasiJenis> GetDataRealisasiJenisList(string wilayah, int tahun, int bulan, string jenisPajak)
            {
                var allData = GetAllDataRealisasiJenis();

                return allData.Where(r =>
                    (string.IsNullOrEmpty(wilayah) || wilayah == "Semua Wilayah" || r.Wilayah == wilayah) &&
                    (tahun <= 0 || r.Tanggal.Year == tahun) &&
                    (bulan <= 0 || r.Tanggal.Month == bulan) &&
                    (string.IsNullOrEmpty(jenisPajak) || jenisPajak == "Semua Jenis Pajak" || r.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            private static List<RealisasiJenis> GetAllDataRealisasiJenis()
            {
                return new List<RealisasiJenis>
                {
                    new RealisasiJenis{ JenisPajak = "Pajak Hotel",    Tanggal = new DateTime(2025, 7, 8), JmlWP = 1350, Realisasi = 30800000000, Target = 35200000000, Status = "UP", Tren = 4.2},
                    new RealisasiJenis{ JenisPajak = "Pajak Restoran", Tanggal = new DateTime(2025, 7, 8), JmlWP = 1350, Realisasi = 22100000000, Target = 28500000000, Status = "UP", Tren = 4.2},
                    new RealisasiJenis{ JenisPajak = "Pajak Hiburan",  Tanggal = new DateTime(2025, 7, 8), JmlWP = 1350, Realisasi = 16500000000, Target = 22800000000, Status = "UP", Tren = 4.2},
                    new RealisasiJenis{ JenisPajak = "Pajak Parkir",   Tanggal = new DateTime(2025, 7, 8), JmlWP = 1350, Realisasi = 15800000000, Target = 19700000000, Status = "UP", Tren = 4.2},
                    new RealisasiJenis{ JenisPajak = "Pajak Reklame",  Tanggal = new DateTime(2025, 7, 8), JmlWP = 1350, Realisasi = 13000000000, Target = 19500000000, Status = "UP", Tren = 4.2},
                };
            }

            public static List<DataHarian> GetDataDataHarianList(string wilayah, int tahun, int bulan, string jenisPajak)
            {
                var allData = GetAllDataHarian();

                return allData.Where(r =>
                    (string.IsNullOrEmpty(wilayah) || wilayah == "Semua Wilayah" || r.Wilayah == wilayah) &&
                    (tahun <= 0 || r.Tanggal.Year == tahun) &&
                    (bulan <= 0 || r.Tanggal.Month == bulan) &&
                    (string.IsNullOrEmpty(jenisPajak) || jenisPajak == "Semua Jenis Pajak" || r.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            private static List<DataHarian> GetAllDataHarian()
            {
                return new List<DataHarian>
                {
                    new DataHarian { Wilayah = "UPTB 1", JenisPajak = "Pajak Hotel",     Tanggal = new DateTime(2025, 7, 8), Target = 1_000_000_000m, Realisasi = 850_000_000m },
                    new DataHarian { Wilayah = "UPTB 2", JenisPajak = "Pajak Restoran", Tanggal = new DateTime(2025, 7, 8), Target = 800_000_000m,   Realisasi = 700_000_000m },
                    new DataHarian { Wilayah = "UPTB 3", JenisPajak = "Pajak Hiburan",  Tanggal = new DateTime(2025, 7, 8), Target = 600_000_000m,   Realisasi = 450_000_000m },
                    new DataHarian { Wilayah = "UPTB 4", JenisPajak = "Pajak Parkir",   Tanggal = new DateTime(2025, 7, 8), Target = 400_000_000m,   Realisasi = 380_000_000m },
                    new DataHarian { Wilayah = "UPTB 5", JenisPajak = "Pajak Reklame",  Tanggal = new DateTime(2025, 7, 8), Target = 300_000_000m,   Realisasi = 250_000_000m },                
                };
            }
        }

        public class RealisasiWilayah
        {
            public string Wilayah { get; set; } = null!;
            public DateTime Tanggal { get; set; }

            public int Tahun => Tanggal.Year;
            public int Bulan => Tanggal.Month;

            public string Lokasi { get; set; } = null!;
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public double Tren { get; set; }
            public string Status { get; set; } = null!;
        }

        public class RealisasiJenis
        {
            public string Wilayah { get; set; } = null!;
            public DateTime Tanggal { get; set; }

            public int Tahun => Tanggal.Year;
            public int Bulan => Tanggal.Month;

            public string JenisPajak { get; set; } = null!;
            public int JmlWP { get; set; }
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public double Tren { get; set; }
            public string Status { get; set; } = null!;
        }

        public class DataHarian
        {
            public string Wilayah { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public DateTime Tanggal { get; set; }

            public int Tahun => Tanggal.Year;
            public int Bulan => Tanggal.Month;

            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public string Hari => Tanggal.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));
        }

    }
}
