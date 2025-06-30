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
        public class Show
        {
            public List<DataPotensi> DataPotensiList { get; set; } = new();
            public Show()
            {
                
            }
            public Show(string keyword)
            {
                DataPotensiList = Method.GetDataPotensiList(keyword);
            }
        }
        public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();

            public Detail() { }

            public Detail(string nop, string jenisPajak)
            {
                DataRealisasiBulananList = Method.GetDetailByNOP(nop, jenisPajak);
            }
        }

        public class Method
        {
            public static List<DataPotensi> GetDataPotensiList(string keyword)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.NOP != null && d.NOP.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<RealisasiBulanan> GetDetailByNOP(string nop, string jenisPajak)
            {
                var dataPotensi = GetAllData()
                    .FirstOrDefault(d => d.NOP == nop && d.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase));

                if (dataPotensi != null)
                {
                    var match = AllRealisasiBulanan
                        .Where(r => r.NOP == nop)
                        .ToList();

                    if (match.Any()) return match;
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
                     new() {NOP = "32.76.050.123", NamaWP = "PT ABC Sejahtera", Alamat = "Jl. Merdeka", JenisPajak = "Massage", MasaPajak = "Januari", Potensi = 12500000, Realisasi = 10000000, Tahun = 2024},
                     new() {NOP = "32.76.050.124", NamaWP = "CV Mitra Usaha", Alamat = "Jl. Sudirman", JenisPajak = "Hotel", MasaPajak = "Februari", Potensi = 15000000, Realisasi = 12000000, Tahun = 2024},
                     new() {NOP = "32.76.050.125", NamaWP = "PT Maju Mundur", Alamat = "Jl. Gatot Subroto", JenisPajak = "Restoran", MasaPajak = "Maret", Potensi = 10000000, Realisasi = 8000000, Tahun = 2024},
                     new() {NOP = "32.76.050.126", NamaWP = "Toko Sumber Rejeki", Alamat = "Jl. Gajah Mada", JenisPajak = "Gym", MasaPajak = "April", Potensi = 5000000, Realisasi = 4500000, Tahun = 2024},
                     new() {NOP = "32.76.050.127", NamaWP = "PT Sinar Terang", Alamat = "Jl. Imam Bonjol", JenisPajak = "Bioskop", MasaPajak = "Mei", Potensi = 13000000, Realisasi = 13000000, Tahun = 2024},
                     new() {NOP = "32.76.050.128", NamaWP = "Bengkel Jaya Motor", Alamat = "Jl. Pahlawan", JenisPajak = "Parkir", MasaPajak = "Juni", Potensi = 6000000, Realisasi = 5000000, Tahun = 2024},
                     new() {NOP = "32.76.050.129", NamaWP = "Apotek Sehat Sentosa", Alamat = "Jl. Kalimantan", JenisPajak = "PBB", MasaPajak = "Juli", Potensi = 3000000, Realisasi = 3000000, Tahun = 2024},
                     new() {NOP = "32.76.050.130", NamaWP = "Kafe Kopi Nusantara", Alamat = "Jl. Sumatra", JenisPajak = "Restoran", MasaPajak = "Agustus", Potensi = 8500000, Realisasi = 6000000, Tahun = 2024},
                     new() {NOP = "32.76.050.131", NamaWP = "PT Transport Abadi", Alamat = "Jl. Diponegoro", JenisPajak = "Parkir", MasaPajak = "September", Potensi = 9000000, Realisasi = 7500000, Tahun = 2024},
                     new() {NOP = "32.76.050.132", NamaWP = "Mall Surya", Alamat = "Jl. Basuki Rahmat", JenisPajak = "Hotel", MasaPajak = "Oktober", Potensi = 20000000, Realisasi = 18000000, Tahun = 2024}
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



        }

        public class DataPotensi
        {
            public string NOP { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string MasaPajak { get; set; } = null!;
            public int Potensi { get; set; }

            public int Realisasi { get; set; }
            public int Tahun { get; set; }
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int Kapasitas { get; set; }
            public int Perhari { get; set; }
            public int Perbulan { get; set; }
            public int Pertahun { get; set; }
        }



    }
}
