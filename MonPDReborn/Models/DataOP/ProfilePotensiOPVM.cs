namespace MonPDReborn.Models.DataOP
{
    public class ProfilePotensiOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }
        }

        public class ShowRekap
        {
            public List<RekapPotensi> DataRekapPotensi { get; set; } = new();
            public ShowRekap() { }
            public ShowRekap(string jenisPajak)
            {

                DataRekapPotensi = Method.GetRekapPotensiList(jenisPajak);
            }
        }

        public class ShowDetail
        {
            public List<DetailPotensi> DataDetailPotensi { get; set; } = new();
            public string JenisPajak { get; set; } = string.Empty;

            public ShowDetail() { }
            public ShowDetail(string jenisPajak)
            {
                JenisPajak = jenisPajak;
                DataDetailPotensi = Method.GetDetailPotensiList(jenisPajak);
            }
        }

        public class ShowData
        {
            public List<DataPotensi> DataPotensiList { get; set; } = new();
            public string JenisPajak { get; set; } = string.Empty;
            public string Kategori { get; set; } = string.Empty;
            public ShowData() { }
            public ShowData(string jenisPajak, string kategori)
            {
                JenisPajak = jenisPajak;
                Kategori = kategori;
                DataPotensiList = Method.GetDataPotensiList(jenisPajak, kategori);
            }
        }
        public class Detail
        {
            public string NOP { get; set; } = string.Empty;
            public string JenisPajak { get; set; } = string.Empty;
            public string? Kategori { get; set; }
            
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();

            public Detail() { }

            public Detail(string nop, string jenisPajak, string? kategori = null)
            {
                NOP = nop;
                JenisPajak = jenisPajak;
                Kategori = kategori;

                DataRealisasiBulananList = Method.GetDetailByNOP(nop, jenisPajak, kategori);
            }
        }


        public class Method
        {
            
            public static List<DataPotensi> GetDataPotensiList(string jenisPajak, string kategori)
            {
                var allData = GetAllData();

                return allData
                    .Where(d =>
                        (string.IsNullOrWhiteSpace(jenisPajak) || 
                         (!string.IsNullOrEmpty(d.JenisPajak) && d.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase)))
                        &&
                        (string.IsNullOrWhiteSpace(kategori) || 
                         (!string.IsNullOrEmpty(d.Kategori) && d.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase)))
                    )
                    .ToList();
            }


            public static List<RealisasiBulanan> GetDetailByNOP(string nop, string jenisPajak, string? kategori = null)
            {
                var dataPotensi = GetAllData()
                    .FirstOrDefault(d => d.NOP == nop && d.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase));

                if (dataPotensi != null)
                {
                    var match = AllRealisasiBulanan
                        .Where(r => r.NOP == nop);

                    // jika kategori ada, filter juga berdasarkan kategori
                    if (!string.IsNullOrWhiteSpace(kategori))
                    {
                        match = match.Where(r => r.Kategori != null && r.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase));
                    }

                    var result = match.ToList();

                    if (result.Any()) return result;
                }

                // fallback
                return new List<RealisasiBulanan>
                {
                    new()
                    {
                        NOP = nop,
                        NamaWP = "Data tidak ditemukan",
                        Alamat = "-",
                        Kapasitas = 0,
                        Perhari = 0,
                        Perbulan = 0,
                        Pertahun = 0
                    }
                };
                        }




            // Internal dummy data
            private static List<DataPotensi> GetAllData()
            {
                return new List<DataPotensi>
                {
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.123", NamaWP = "PT ABC Sejahtera", Alamat = "Jl. Merdeka", Kategori = "Massage", MasaPajak = "Januari", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hotel", NOP = "32.76.050.124", NamaWP = "CV Mitra Usaha", Alamat = "Jl. Sudirman", Kategori = "Bintang 5", MasaPajak = "Februari", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Restoran", NOP = "32.76.050.125", NamaWP = "PT Maju Mundur", Alamat = "Jl. Gatot Subroto", Kategori = "Restoran", MasaPajak = "Maret", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.126", NamaWP = "Toko Sumber Rejeki", Alamat = "Jl. Gajah Mada", Kategori = "Gym", MasaPajak = "April", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.127", NamaWP = "PT Sinar Terang", Alamat = "Jl. Imam Bonjol", Kategori = "Bioskop", MasaPajak = "Mei", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Parkir", NOP = "32.76.050.128", NamaWP = "Bengkel Jaya Motor", Alamat = "Jl. Pahlawan", Kategori = "Parkir", MasaPajak = "Juni", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "PBB", NOP = "32.76.050.129", NamaWP = "Apotek Sehat Sentosa", Alamat = "Jl. Kalimantan", Kategori = "PBB", MasaPajak = "Juli", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Restoran", NOP = "32.76.050.130", NamaWP = "Kafe Kopi Nusantara", Alamat = "Jl. Sumatra", Kategori = "Restoran", MasaPajak = "Agustus", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Parkir", NOP = "32.76.050.131", NamaWP = "PT Transport Abadi", Alamat = "Jl. Diponegoro", Kategori = "Parkir", MasaPajak = "September", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hotel", NOP = "32.76.050.132", NamaWP = "Mall Surya", Alamat = "Jl. Basuki Rahmat", Kategori = "Bintang 4", MasaPajak = "Oktober", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000}
                };
            }

            private static List<RealisasiBulanan> AllRealisasiBulanan = new List<RealisasiBulanan>
            {
                new()
                {
                    NOP = "32.76.050.200",
                    NamaWP = "URBAN ATHLETES",
                    Alamat = "JL. RAYA TIAD A THE CENTRAL MALL - GUNAWANGSA TIADJI, SURABAYA",
                    Kapasitas = 150,
                    Perhari = 120,
                    Perbulan = 3600,
                    Pertahun = 43200
                },
                new()
                {
                    NOP = "32.76.050.127",
                    NamaWP = "PT Sinar Terang",
                    Alamat = "Jl. Imam Bonjol",
                    Kapasitas = 100,
                    Perhari = 90,
                    Perbulan = 2700,
                    Pertahun = 32400
                },
                new()
                {
                    NOP = "32.76.050.128",
                    NamaWP = "Bengkel Jaya Motor",
                    Alamat = "Jl. Pahlawan",
                    Kapasitas = 80,
                    Perhari = 70,
                    Perbulan = 2100,
                    Pertahun = 25200
                },
                // tambahkan sebanyak yang kamu perlukan
            };

            public static List<RekapPotensi> GetRekapPotensiList(string jenisPajak)
            {
                var allData = GetRekapPotensi();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<RekapPotensi> GetRekapPotensi()
            {
                return new List<RekapPotensi>
                {
                     new RekapPotensi
                    {
                        JenisPajak = "Hotel",
                        Target1 = 10_000_000,
                        Realisasi1 = 8_500_000,
                        Target2 = 12_000_000,
                        Realisasi2 = 11_000_000,
                        Target3 = 15_000_000,
                        Realisasi3 = 14_500_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Restoran",
                        Target1 = 8_000_000,
                        Realisasi1 = 6_750_000,
                        Target2 = 9_000_000,
                        Realisasi2 = 8_250_000,
                        Target3 = 10_000_000,
                        Realisasi3 = 9_200_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Hiburan",
                        Target1 = 5_000_000,
                        Realisasi1 = 4_000_000,
                        Target2 = 6_000_000,
                        Realisasi2 = 5_200_000,
                        Target3 = 7_000_000,
                        Realisasi3 = 6_800_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Parkir",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_500_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 16_500_000
                    }
                };
            }

            public static List<DetailPotensi> GetDetailPotensiList(string jenisPajak)
            {
                var allData = GetDetailPotensi();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DetailPotensi> GetDetailPotensi()
            {
                return new List<DetailPotensi>
                {
                    new DetailPotensi
                    {
                        JenisPajak = "Hotel",
                        Kategori = "Bintang 5",
                        Target1 = 6_000_000,
                        Realisasi1 = 5_000_000,
                        Target2 = 7_000_000,
                        Realisasi2 = 6_200_000,
                        Target3 = 8_000_000,
                        Realisasi3 = 7_800_000,
                        TotalPotensi = 18_000_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hotel",
                        Kategori = "Bintang 3",
                        Target1 = 4_000_000,
                        Realisasi1 = 3_500_000,
                        Target2 = 5_000_000,
                        Realisasi2 = 4_800_000,
                        Target3 = 6_000_000,
                        Realisasi3 = 5_900_000,
                        TotalPotensi = 12_000_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Restoran",
                        Kategori = "Franchise",
                        Target1 = 5_000_000,
                        Realisasi1 = 4_200_000,
                        Target2 = 5_500_000,
                        Realisasi2 = 5_000_000,
                        Target3 = 6_000_000,
                        Realisasi3 = 5_700_000,
                        TotalPotensi = 16_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Restoran",
                        Kategori = "Mandiri",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_550_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_250_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_800_000,
                        TotalPotensi = 10_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hiburan",
                        Kategori = "Bioskop",
                        Target1 = 2_000_000,
                        Realisasi1 = 1_750_000,
                        Target2 = 2_500_000,
                        Realisasi2 = 2_200_000,
                        Target3 = 3_000_000,
                        Realisasi3 = 2_850_000,
                        TotalPotensi = 7_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hiburan",
                        Kategori = "Arena",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_300_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 10_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Parkir",
                        Kategori = "Parkir",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_300_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 10_500_000
                    }
                };
            }
        }

        public class DataPotensi
        {
            public string NOP { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string MasaPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int Kapasitas { get; set; }
            public int Perhari { get; set; }
            public int Perbulan { get; set; }
            public int Pertahun { get; set; }
        }

        public class RekapPotensi
        {
            public string JenisPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class DetailPotensi
        {
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }


    }
}
