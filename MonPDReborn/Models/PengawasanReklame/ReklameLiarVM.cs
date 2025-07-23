using MonPDReborn.Models.Reklame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class ReklameLiarVM
    {
        // Untuk halaman utama Index.cshtml
        public class Index
        {
            public int SelectedTahun { get; set; }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<RekapBulanan> DataRekap { get; set; } = new();
            public Show(int? tahun)
            {
                DataRekap = Method.GetRekapDataBulanan(tahun);
            }
        }

        // Untuk Partial View _Detail.cshtml (modal)
        public class Detail
        {
            public InfoJalan DataJalan { get; set; } = new();
            public List<DetailData> DataDetail { get; set; } = new();

            public Detail(string jalan, string kategori, string status)
            {
                // 'status' di sini adalah nama bulan, misal "Nov"
                // 'kategori' di sini adalah jenis reklame, misal "Baliho"
                DataJalan = Method.GetInfoJalan(jalan, kategori, status);
                DataDetail = Method.GetDetailDataReklame(jalan, kategori, status);
            }
        }

        public static class Method
        {
            // Method baru untuk data dummy bulanan
            public static List<RekapBulanan> GetRekapDataBulanan(int? tahun)
            {
                var allData = new List<RekapBulanan>
                {
                    new RekapBulanan { TahunData = 2024, NamaJalan = "JL. A. YANI", KelasJalan = "1", Jenis = "Baliho", Jan = 8, Feb = 7, Mar = 9, Apr = 6, Mei = 8, Jun = 10, Jul = 12, Agu = 11, Sep = 14, Okt = 15, Nov = 17, Des = 16 },
                    new RekapBulanan { TahunData = 2024, NamaJalan = "JL. SUDIRMAN", KelasJalan = "1", Jenis = "Spanduk", Jan = 6, Feb = 8, Mar = 5, Apr = 9, Mei = 11, Jun = 7, Jul = 5, Agu = 8, Sep = 10, Okt = 7, Nov = 9, Des = 12 },
                    new RekapBulanan { TahunData = 2025, NamaJalan = "JL. GATOT SUBROTO", KelasJalan = "1", Jenis = "Billboard", Jan = 5, Feb = 4, Mar = 6, Apr = 7, Mei = 5, Jun = 4, Jul = 6, Agu = 5, Sep = 8, Okt = 9, Nov = 7, Des = 6 },
                    new RekapBulanan { TahunData = 2025, NamaJalan = "JL. DIPONEGORO", KelasJalan = "2", Jenis = "Neon Box", Jan = 3, Feb = 4, Mar = 3, Apr = 5, Mei = 4, Jun = 6, Jul = 5, Agu = 3, Sep = 4, Okt = 6, Nov = 5, Des = 2 },
                    new RekapBulanan { TahunData = 2025, NamaJalan = "JL. IMAM BONJOL", KelasJalan = "2", Jenis = "Baliho", Jan = 7, Feb = 6, Mar = 8, Apr = 5, Mei = 7, Jun = 6, Jul = 8, Agu = 9, Sep = 7, Okt = 6, Nov = 8, Des = 9 }
                };

                // Jika tidak ada tahun yang dipilih, tampilkan data tahun sekarang sebagai default
                if (!tahun.HasValue)
                {
                    return allData.Where(d => d.TahunData == DateTime.Now.Year).ToList();
                }

                // Jika ada tahun yang dipilih, filter berdasarkan tahun tersebut
                return allData.Where(d => d.TahunData == tahun.Value).ToList();
            }
            public static InfoJalan GetInfoJalan(string namaJalan, string jenis, string bulan)
            {
                // Ambil data rekap untuk mencari jumlah reklame pada bulan yg diklik
                var dataJalan = GetRekapDataBulanan(null)
                    .FirstOrDefault(r => r.NamaJalan == namaJalan && r.Jenis == jenis);

                // Mengambil nilai bulan secara dinamis
                int jumlahReklame = 0;
                if (dataJalan != null)
                {
                    var prop = typeof(RekapBulanan).GetProperty(bulan);
                    if (prop != null)
                    {
                        jumlahReklame = (int)(prop.GetValue(dataJalan) ?? 0);
                    }
                }

                return new InfoJalan
                {
                    NamaJalan = namaJalan,
                    Bulan = bulan,
                    Tahun = DateTime.Now.Year.ToString(), // Ambil tahun sekarang
                    JumlahReklame = jumlahReklame
                };
            }

            public static List<DetailData> GetDetailDataReklame(string jalan, string jenis, string bulan)
            {
                // Logika dummy: mengembalikan data seolah-olah difilter
                return new List<DetailData> {
                    new DetailData { NamaJalan = jalan, KelasJalan = "1", AlamatReklame = $"{jalan} No. 62", Jenis = "Atap", TanggalBongkar = new DateTime(2026, 1, 5) },
                    new DetailData { NamaJalan = jalan, KelasJalan = "1", AlamatReklame = $"{jalan} No. 200", Jenis = "Dinding", TanggalBongkar = new DateTime(2026, 1, 28) },
                    new DetailData { NamaJalan = jalan, KelasJalan = "1", AlamatReklame = $"{jalan} No. 192", Jenis = "Konstruksi Khusus", TanggalBongkar = new DateTime(2026, 4, 4) },
                    new DetailData { NamaJalan = jalan, KelasJalan = "1", AlamatReklame = $"{jalan} No. 179", Jenis = "Atap", TanggalBongkar = new DateTime(2026, 12, 29) }
                };
            }
        }

        // --- MODEL DATA BARU ---
        public class RekapBulanan
        {
            public int TahunData { get; set; }
            public string NamaJalan { get; set; } = null!;
            public string KelasJalan { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public int Jan { get; set; }
            public int Feb { get; set; }
            public int Mar { get; set; }
            public int Apr { get; set; }
            public int Mei { get; set; }
            public int Jun { get; set; }
            public int Jul { get; set; }
            public int Agu { get; set; }
            public int Sep { get; set; }
            public int Okt { get; set; }
            public int Nov { get; set; }
            public int Des { get; set; }
            public int Total => Jan + Feb + Mar + Apr + Mei + Jun + Jul + Agu + Sep + Okt + Nov + Des;
        }

        public class DetailData
        {
            public string KelasJalan { get; set; } = null!;
            public string NamaJalan { get; set; } = null!;
            public string AlamatReklame { get; set; } = null!;
            public string Jenis { get; set; } = null!; // DIUBAH: dari JenisDetail menjadi Jenis
            public DateTime TanggalBongkar { get; set; }
        }

        public class InfoJalan
        {
            public string NamaJalan { get; set; } = "";
            public string Bulan { get; set; } = ""; // Tambahan
            public string Tahun { get; set; } = ""; // Tambahan
            public int JumlahReklame { get; set; } // Tambahan
        }
    }
}