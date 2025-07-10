namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringHarianVM
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
            public List<MonitoringHarian> DataMonitoringHarianList { get; set; } = new();


            public Show()
            {
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
                    Pencapaian = 87.15
                };
            }

            public static List<MonitoringHarian> GetFilteredData(int tahun, int bulan, string jenisPajak)
            {
                // Ambil semua data sebagai dasar
                var allData = GetAllDataMonitoringHarian();

                // Konversi ke IEnumerable agar bisa menggunakan Where (filter)
                IEnumerable<MonitoringHarian> filteredData = allData;

                // 2. Terapkan filter TAHUN
                if (tahun > 0)
                {
                    // Filter berdasarkan string karena tipe data Tanggal adalah string
                    filteredData = filteredData.Where(d => d.Tanggal.Contains(tahun.ToString()));
                }

                // 3. Terapkan filter BULAN
                if (bulan > 0)
                {
                    // Ubah angka bulan (misal: 7) menjadi nama bulan dalam Bahasa Indonesia ("Juli")
                    var namaBulan = new DateTime(2000, bulan, 1).ToString("MMMM", new System.Globalization.CultureInfo("id-ID"));
                    filteredData = filteredData.Where(d => d.Tanggal.Contains(namaBulan));
                }

                // 4. Terapkan filter JENIS PAJAK
                if (!string.IsNullOrEmpty(jenisPajak) && jenisPajak != "Semua Jenis Pajak")
                {
                    filteredData = filteredData.Where(d => d.JenisPajak == jenisPajak);
                }

                return filteredData.ToList();
            }


            public static List<MonitoringHarian> GetAllDataMonitoringHarian()
            {
                return new List<MonitoringHarian>
                {
                    // ================== DATA JULI 2025 ==================
                    new MonitoringHarian { Tanggal = "10 Juli 2025", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 30.000.000", Realisasi = "Rp 31.500.000", Pencapaian = "105,0%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "10 Juli 2025", JenisPajak = "Pajak Restoran", TargetHarian = "Rp 45.000.000", Realisasi = "Rp 41.200.000", Pencapaian = "91,6%", Status = "Di Bawah Target" },
                    new MonitoringHarian { Tanggal = "10 Juli 2025", JenisPajak = "Pajak Parkir", TargetHarian = "Rp 15.000.000", Realisasi = "Rp 15.000.000", Pencapaian = "100,0%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "09 Juli 2025", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 30.000.000", Realisasi = "Rp 28.900.000", Pencapaian = "96,3%", Status = "Hampir Tercapai" },
                    new MonitoringHarian { Tanggal = "09 Juli 2025", JenisPajak = "Pajak Restoran", TargetHarian = "Rp 45.000.000", Realisasi = "Rp 47.800.000", Pencapaian = "106,2%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "09 Juli 2025", JenisPajak = "Pajak Hiburan", TargetHarian = "Rp 20.000.000", Realisasi = "Rp 17.500.000", Pencapaian = "87,5%", Status = "Di Bawah Target" },

                    // ================== DATA JUNI 2025 ==================
                    new MonitoringHarian { Tanggal = "15 Juni 2025", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 30.000.000", Realisasi = "Rp 33.000.000", Pencapaian = "110,0%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "15 Juni 2025", JenisPajak = "Pajak Restoran", TargetHarian = "Rp 45.000.000", Realisasi = "Rp 44.500.000", Pencapaian = "98,9%", Status = "Hampir Tercapai" },
                    new MonitoringHarian { Tanggal = "15 Juni 2025", JenisPajak = "Pajak Reklame", TargetHarian = "Rp 10.000.000", Realisasi = "Rp 8.000.000", Pencapaian = "80,0%", Status = "Di Bawah Target" },
                    new MonitoringHarian { Tanggal = "14 Juni 2025", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 30.000.000", Realisasi = "Rp 29.100.000", Pencapaian = "97,0%", Status = "Hampir Tercapai" },

                    // ================== DATA MEI 2025 ==================
                    new MonitoringHarian { Tanggal = "20 Mei 2025", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 30.000.000", Realisasi = "Rp 25.000.000", Pencapaian = "83,3%", Status = "Di Bawah Target" },
                    new MonitoringHarian { Tanggal = "20 Mei 2025", JenisPajak = "Pajak Restoran", TargetHarian = "Rp 45.000.000", Realisasi = "Rp 46.000.000", Pencapaian = "102,2%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "20 Mei 2025", JenisPajak = "Pajak Parkir", TargetHarian = "Rp 15.000.000", Realisasi = "Rp 16.000.000", Pencapaian = "106,7%", Status = "Tercapai" },
    
                    // ================== DATA APRIL 2025 ==================
                    new MonitoringHarian { Tanggal = "05 April 2025", JenisPajak = "Pajak Hiburan", TargetHarian = "Rp 20.000.000", Realisasi = "Rp 22.000.000", Pencapaian = "110,0%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "05 April 2025", JenisPajak = "Pajak Reklame", TargetHarian = "Rp 10.000.000", Realisasi = "Rp 9.900.000", Pencapaian = "99,0%", Status = "Hampir Tercapai" },
    
                    // ================== DATA TAHUN LALU (2024) ==================
                    new MonitoringHarian { Tanggal = "10 Juli 2024", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 28.000.000", Realisasi = "Rp 29.000.000", Pencapaian = "103,6%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "10 Juli 2024", JenisPajak = "Pajak Restoran", TargetHarian = "Rp 42.000.000", Realisasi = "Rp 42.000.000", Pencapaian = "100,0%", Status = "Tercapai" },
                    new MonitoringHarian { Tanggal = "15 Juni 2024", JenisPajak = "Pajak Hotel", TargetHarian = "Rp 28.000.000", Realisasi = "Rp 25.000.000", Pencapaian = "89,3%", Status = "Di Bawah Target" },
                    new MonitoringHarian { Tanggal = "20 Mei 2024", JenisPajak = "Pajak Parkir", TargetHarian = "Rp 14.000.000", Realisasi = "Rp 13.500.000", Pencapaian = "96,4%", Status = "Hampir Tercapai" }
                };
            }
        }

        public class Dashboard
        {
            public int TotalTarget { get; set; }
            public double TotalRealisasi { get; set; }
            public double Pencapaian { get; set; }

        }

        public class MonitoringHarian
        {
            public string Tanggal { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string TargetHarian { get; set; } = null!;
            public string Realisasi { get; set; } = null!;
            public string Pencapaian { get; set; } = null!;
            public string Status { get; set; } = null!;

        }
    }
}
