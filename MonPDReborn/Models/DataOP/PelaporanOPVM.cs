namespace MonPDReborn.Models.DataOP
{
    public class PelaporanOPVM
    {
        // Tidak ada filter
        public class Index
        {
        }

        // Tampilkan semua data hasil pelaporan
        public class Show
        {
            public List<HasilPelaporan> DaftarHasil { get; set; } = new();

            public Show()
            {
                DaftarHasil = Method.GetAllData();
            }

            public Show(string? keyword)
            {
                var allData = Method.GetAllData();
                DaftarHasil = string.IsNullOrWhiteSpace(keyword)
                    ? allData
                    : allData.Where(d => d.Nama.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        // Detail per NOP
        public class Detail
        {
            public List<RealisasiBulanan> DaftarRealisasi { get; set; } = new();

            public Detail(string nop)
            {
                DaftarRealisasi = Method.GetDetailByNOP(nop);
            }
        }

        // Model HasilPelaporan
        public class HasilPelaporan
        {
            public int No { get; set; }
            public string NOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string Tahun { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public string Wilayah { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int MasaBelumLapor { get; set; }
            public double Kepatuhan { get; set; }
            public string Alamat { get; set; } = null!;
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public string Tahun { get; set; } = null!;
            public decimal Nominal { get; set; }
        }

        // Static Method
        public static class Method
        {
            public static List<HasilPelaporan> GetAllData()
            {
                return new List<HasilPelaporan>
                {
                    new() { No = 1, Wilayah = "01", NOP = "35.78.001.001.902.00001", Nama = "Hotel Indah", JenisPajak = "Pajak Hotel", Tahun = "2024", Bulan = "Januari", Status = "Aktif", Alamat = "Jl. Raya Darmo", MasaBelumLapor = 0, Kepatuhan = 100 },
                    new() { No = 2, Wilayah = "01", NOP = "35.78.001.001.902.00002", Nama = "Restoran Sederhana", JenisPajak = "Pajak Restoran", Tahun = "2024", Bulan = "Februari", Status = "Aktif", Alamat = "Jl. Mayjen Sungkono", MasaBelumLapor = 1, Kepatuhan = 91.7 },
                    new() { No = 3, Wilayah = "02", NOP = "35.78.001.001.902.00003", Nama = "Hotel Surya", JenisPajak = "Pajak Hotel", Tahun = "2023", Bulan = "Maret", Status = "Tutup", Alamat = "Jl. Basuki Rahmat", MasaBelumLapor = 6, Kepatuhan = 50 },
                    new() { No = 4, Wilayah = "02", NOP = "35.78.001.001.902.00004", Nama = "Cafe Laris", JenisPajak = "Pajak Restoran", Tahun = "2024", Bulan = "Januari", Status = "Aktif", Alamat = "Jl. Kusuma Bangsa", MasaBelumLapor = 0, Kepatuhan = 100 },
                };
            }

            public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                return GetAllDetail().Where(x => x.NOP == nop).ToList();
            }

            private static List<RealisasiBulanan> GetAllDetail()
            {
                return new List<RealisasiBulanan>
                {
                    new() { NOP = "35.78.001.001.902.00001", Bulan = "Januari", Tahun = "2024", Nominal = 125000000 },
                    new() { NOP = "35.78.001.001.902.00002", Bulan = "Februari", Tahun = "2024", Nominal = 85000000 },
                    new() { NOP = "35.78.001.001.902.00003", Bulan = "Maret", Tahun = "2023", Nominal = 45000000 },
                    new() { NOP = "35.78.001.001.902.00004", Bulan = "Januari", Tahun = "2024", Nominal = 78000000 },
                };
            }
        }
    }
}
