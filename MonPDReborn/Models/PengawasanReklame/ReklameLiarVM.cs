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
            public Show(int tahun)
            {
                DataRekap = Method.GetRekapDataBulanan(tahun);
            }
        }

        // Untuk Partial View _Detail.cshtml (modal)
        public class Detail
        {
            // ... (bisa diisi nanti jika diperlukan)
        }

        public static class Method
        {
            // Method baru untuk data dummy bulanan
            public static List<RekapBulanan> GetRekapDataBulanan(int tahun)
            {
                // Anda bisa menambahkan logika filter berdasarkan 'tahun' di sini
                return new List<RekapBulanan>
                {
                    new RekapBulanan { NamaJalan = "JL. A. YANI", KelasJalan = "1", Jenis = "Baliho", Jan = 8, Feb = 7, Mar = 9, Apr = 6, Mei = 8, Jun = 10, Jul = 12, Agu = 11, Sep = 14, Okt = 15, Nov = 17, Des = 16 },
                    new RekapBulanan { NamaJalan = "JL. SUDIRMAN", KelasJalan = "1", Jenis = "Spanduk", Jan = 6, Feb = 8, Mar = 5, Apr = 9, Mei = 11, Jun = 7, Jul = 5, Agu = 8, Sep = 10, Okt = 7, Nov = 9, Des = 12 },
                    new RekapBulanan { NamaJalan = "JL. GATOT SUBROTO", KelasJalan = "1", Jenis = "Billboard", Jan = 5, Feb = 4, Mar = 6, Apr = 7, Mei = 5, Jun = 4, Jul = 6, Agu = 5, Sep = 8, Okt = 9, Nov = 7, Des = 6 },
                    new RekapBulanan { NamaJalan = "JL. DIPONEGORO", KelasJalan = "2", Jenis = "Neon Box", Jan = 3, Feb = 4, Mar = 3, Apr = 5, Mei = 4, Jun = 6, Jul = 5, Agu = 3, Sep = 4, Okt = 6, Nov = 5, Des = 2 },
                    new RekapBulanan { NamaJalan = "JL. IMAM BONJOL", KelasJalan = "2", Jenis = "Baliho", Jan = 7, Feb = 6, Mar = 8, Apr = 5, Mei = 7, Jun = 6, Jul = 8, Agu = 9, Sep = 7, Okt = 6, Nov = 8, Des = 9 }
                };
            }
        }

        // --- MODEL DATA BARU ---
        public class RekapBulanan
        {
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
    }
}