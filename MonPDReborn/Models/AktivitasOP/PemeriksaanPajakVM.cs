using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemeriksaanPajakVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;

            public int TotalOpDiperiksa { get; set; }
            public decimal RataRataKurangBayar { get; set; }
            public decimal TotalKurangBayar { get; set; }
            public Index() { }
        }

        // ======= SHOW ==========
        public class Show
        {
            public List<DataPemeriksaan> DataPemeriksaanList { get; set; } = new();

            public Show() { }

            public Show(string keyword)
            {
                DataPemeriksaanList = Method.GetFilteredData(keyword);
            }
        }

        // ======= DETAIL ==========
        public class Detail
        {
            public List<DataDetailPemeriksaan> DataDetailList { get; set; } = new();

            public Detail() { }

            public Detail(string jenisPajak)
            {
                DataDetailList = Method.GetDetailByJenisPajak(jenisPajak);
            }
        }

        // ======= METHOD ==========
        public class Method
        {
            public static List<DataPemeriksaan> GetFilteredData(string keyword)
            {
                var all = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return all;

                return all.Where(x => x.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            public static List<DataDetailPemeriksaan> GetDetailByJenisPajak(string jenisPajak)
            {
                var all = GetAllDetail();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return all;

                return all
                    .Where(x => x.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DataPemeriksaan> GetAllData()
            {
                var ret = new List<DataPemeriksaan>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                var pemeriksaanRestoMines2 = context.TPemeriksaans.Where(x => x.TahunPajak == currentYear - 2).ToList();
                var pemeriksaanRestoMines1 = context.TPemeriksaans.Where(x => x.TahunPajak == currentYear - 1).ToList();
                var pemeriksaanRestoNow = context.TPemeriksaans.Where(x => x.TahunPajak == currentYear).ToList();

                return new List<DataPemeriksaan>
                {
                    new()
                    {
                        JenisPajak = "Pajak Hotel",
                        JumlahOP2023 = 10,
                        JumlahOP2024 = 12,
                        JumlahOP2025 = 15,
                        Pokok2023 = 12000000,
                        Sanksi2023 = 2000000,
                        Pokok2024 = 15000000,
                        Sanksi2024 = 2500000,
                        Pokok2025 = 18000000,
                        Sanksi2025 = 3000000
                    }
                };
            }

            private static List<DataDetailPemeriksaan> GetAllDetail()
            {
                return new List<DataDetailPemeriksaan>
            {
                new()
                {
                    JenisPajak = "Pajak Hotel", // ✅ cocok dengan "Pajak Hotel"
                    NOP = "35.78.170.005.902.00066",
                    WajibPajak = "Hotel Bintang",
                    Alamat = "Jl. Rajawali No. 47",
                    UPTB = "UPTB Timur",
                    NoSP = "SP-001/2025",
                    TglST = new DateTime(2025, 1, 10),
                    Tahun = 2023,
                    JumlahKB = 15000000,
                    Keterangan = "Kurang Bayar 2023",
                    LHP = "LHP-2025-01",
                    TglLHP = new DateTime(2025, 1, 20),
                    TglBayar = new DateTime(2025, 2, 5),
                    Tim = "Tim A"
                },
                new()
                {
                    JenisPajak = "Pajak Hotel", // ✅ baris kedua juga hotel
                    NOP = "35.78.170.005.902.00067",
                    WajibPajak = "Hotel Nusantara",
                    Alamat = "Jl. Merdeka No. 12",
                    UPTB = "UPTB Selatan",
                    NoSP = "SP-003/2025",
                    TglST = new DateTime(2025, 3, 1),
                    Tahun = 2024,
                    JumlahKB = 18000000,
                    Keterangan = "Kurang Bayar 2024",
                    LHP = "LHP-2025-03",
                    TglLHP = new DateTime(2025, 3, 15),
                    TglBayar = new DateTime(2025, 3, 20),
                    Tim = "Tim C"
                },
                new()
                {
                    JenisPajak = "Pajak Restoran", // ✅ cocok dengan "Pajak Restoran"
                    NOP = "35.78.100.002.902.00172",
                    WajibPajak = "Restoran Nikmat",
                    Alamat = "Jl. Bubutan No. 1-7",
                    UPTB = "UPTB Pusat",
                    NoSP = "SP-002/2025",
                    TglST = new DateTime(2025, 2, 15),
                    Tahun = 2024,
                    JumlahKB = 12000000,
                    Keterangan = "Kurang Bayar 2024",
                    LHP = "LHP-2025-02",
                    TglLHP = new DateTime(2025, 2, 25),
                    TglBayar = new DateTime(2025, 3, 10),
                    Tim = "Tim B"
                }
            };
            }
        }

        // ======= ENTITY UTAMA ==========
        public class DataPemeriksaan
        {
            public string JenisPajak { get; set; } = null!;
            public int JumlahOP2023 { get; set; }
            public int JumlahOP2024 { get; set; }
            public int JumlahOP2025 { get; set; }

            public decimal Pokok2023 { get; set; }
            public decimal Sanksi2023 { get; set; }
            public decimal Total2023 => Pokok2023 + Sanksi2023;

            public decimal Pokok2024 { get; set; }
            public decimal Sanksi2024 { get; set; }
            public decimal Total2024 => Pokok2024 + Sanksi2024;

            public decimal Pokok2025 { get; set; }
            public decimal Sanksi2025 { get; set; }
            public decimal Total2025 => Pokok2025 + Sanksi2025;
        }

        // ======= ENTITY DETAIL ==========
        public class DataDetailPemeriksaan
        {
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string WajibPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string UPTB { get; set; } = null!;
            public string NoSP { get; set; } = null!;
            public DateTime TglST { get; set; }
            public int Tahun { get; set; }
            public decimal JumlahKB { get; set; }
            public string Keterangan { get; set; } = null!;
            public string LHP { get; set; } = null!;
            public DateTime TglLHP { get; set; }
            public DateTime TglBayar { get; set; }
            public string Tim { get; set; } = null!;
        }
    }
}
