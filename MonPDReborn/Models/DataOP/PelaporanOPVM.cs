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

        // Detail per NOP, auto generate Januari–Desember
        public class Detail
        {
            public List<StatusPelaporanBulanan> DaftarRealisasi { get; set; } = new();

            public int TotalNilai { get; set; } // ← Tambahan untuk total nominal

            public Detail(string nop)
            {
                var tahunSekarang = DateTime.Now.Year.ToString();
                var data = Method.GetDetailByNOP(nop);

                DaftarRealisasi = Enumerable.Range(1, 12).Select((bulanKe, index) =>
                {
                    var dataBulanIni = data.FirstOrDefault(x => x.BulanKe == bulanKe && x.Tahun == tahunSekarang);
                    return new StatusPelaporanBulanan
                    {
                        Id = index + 1,
                        Bulan = new DateTime(1, bulanKe, 1).ToString("MMMM"),
                        Status = dataBulanIni?.Status ?? "Belum Lapor",
                        TanggalLapor = dataBulanIni?.TanggalLapor?.ToString("dd MMMM yyyy") ?? "-",
                        Nilai = dataBulanIni?.Nilai ?? 0
                    };
                }).ToList();

                // Hitung total nilai dari data yang ada
                TotalNilai = DaftarRealisasi.Sum(d => d.Nilai);
            }
        }

        // Model Hasil Pelaporan (Index/Show)
        public class HasilPelaporan
        {
            public int No { get; set; }
            public string NOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string Wilayah { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int MasaBelumLapor { get; set; }
            public int PajakSeharusnya { get; set; }
            public int PajakTerlapor { get; set; }
            public string Alamat { get; set; } = null!;
        }

        // Model realisasi pelaporan dari dummy (disederhanakan)
        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public int BulanKe { get; set; } // 1 = Januari, ..., 12 = Desember
            public string Tahun { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime? TanggalLapor { get; set; }
            public int Nilai { get; set; } // <--- tambahkan properti ini
            public int TotaNilai { get; set; }

        }

        // Model tampilan detail bulanan auto-generate
        public class StatusPelaporanBulanan
        {
            public int Id { get; set; }
            public string Bulan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int Nilai { get; set; }
            public int TotalNilai { get; set; }
            public string? TanggalLapor { get; set; }
        }

        // Static Method
        public static class Method
        {
            public static List<HasilPelaporan> GetAllData()
            {
                return new List<HasilPelaporan>
                {
                    new() { No = 1, Wilayah = "01", NOP = "35.78.001.001.902.00001", Nama = "Hotel Indah", JenisPajak = "Pajak Hotel", Status = "Patuh", Alamat = "Jl. Raya Darmo", MasaBelumLapor = 0, PajakSeharusnya = 12, PajakTerlapor = 5},
                    new() { No = 2, Wilayah = "01", NOP = "35.78.001.001.902.00002", Nama = "Restoran Sederhana", JenisPajak = "Pajak Restoran", Status = "Sebagian", Alamat = "Jl. Mayjen Sungkono", MasaBelumLapor = 1, PajakSeharusnya = 12, PajakTerlapor = 8},
                    new() { No = 3, Wilayah = "02", NOP = "35.78.001.001.902.00003", Nama = "Hotel Surya", JenisPajak = "Pajak Hotel", Status = "Belum Lapor", Alamat = "Jl. Basuki Rahmat", MasaBelumLapor = 6, PajakSeharusnya = 12, PajakTerlapor = 6},
                    new() { No = 4, Wilayah = "02", NOP = "35.78.001.001.902.00004", Nama = "Cafe Laris", JenisPajak = "Pajak Restoran", Status = "Patuh", Alamat = "Jl. Kusuma Bangsa", MasaBelumLapor = 0, PajakSeharusnya = 12, PajakTerlapor = 12},
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
                    new() { NOP = "35.78.001.001.902.00001", BulanKe = 1, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 1, 15), Nilai = 45000000 },
                    new() { NOP = "35.78.001.001.902.00001", BulanKe = 3, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 3, 18), Nilai = 45000000 },
                    new() { NOP = "35.78.001.001.902.00001", BulanKe = 6, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 6, 10), Nilai = 45000000 },
                    new() { NOP = "35.78.001.001.902.00004", BulanKe = 1, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 1, 20), Nilai = 45000000 },
                };
            }
        }
    }
}
