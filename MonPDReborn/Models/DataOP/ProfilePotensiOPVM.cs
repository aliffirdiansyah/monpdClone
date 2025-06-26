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
     /*   public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
            public Detail()
            {
                
            }
            public Detail(string nop)
            {
                DataRealisasiBulananList = Method.GetDetailByNOP(nop);
            }
        }*/
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

        /*    public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var allDetail = GetAllDetail();
                return allDetail.Where(x => x.NOP == nop).ToList();
            }*/

            // Internal dummy data
            private static List<DataPotensi> GetAllData()
            {
                return new List<DataPotensi>
                {
                     new() {NOP = "32.76.050.123", NamaWP = "PT ABC Sejahtera", Alamat = "Jl. Merdeka", JenisPajak = "PBB", MasaPajak = "Januari", Potensi = 12500000, Realisasi = 10000000, Tahun = 2024},
                     new() {NOP = "32.76.050.124", NamaWP = "CV Mitra Usaha", Alamat = "Jl. Sudirman", JenisPajak = "Hotel", MasaPajak = "Februari", Potensi = 15000000, Realisasi = 12000000, Tahun = 2024},
                     new() {NOP = "32.76.050.125", NamaWP = "PT Maju Mundur", Alamat = "Jl. Gatot Subroto", JenisPajak = "Restoran", MasaPajak = "Maret", Potensi = 10000000, Realisasi = 8000000, Tahun = 2024},
                     new() {NOP = "32.76.050.126", NamaWP = "Toko Sumber Rejeki", Alamat = "Jl. Gajah Mada", JenisPajak = "Reklame", MasaPajak = "April", Potensi = 5000000, Realisasi = 4500000, Tahun = 2024},
                     new() {NOP = "32.76.050.127", NamaWP = "PT Sinar Terang", Alamat = "Jl. Imam Bonjol", JenisPajak = "PPJ", MasaPajak = "Mei", Potensi = 13000000, Realisasi = 13000000, Tahun = 2024},
                     new() {NOP = "32.76.050.128", NamaWP = "Bengkel Jaya Motor", Alamat = "Jl. Pahlawan", JenisPajak = "Parkir", MasaPajak = "Juni", Potensi = 6000000, Realisasi = 5000000, Tahun = 2024},
                     new() {NOP = "32.76.050.129", NamaWP = "Apotek Sehat Sentosa", Alamat = "Jl. Kalimantan", JenisPajak = "PBB", MasaPajak = "Juli", Potensi = 3000000, Realisasi = 3000000, Tahun = 2024},
                     new() {NOP = "32.76.050.130", NamaWP = "Kafe Kopi Nusantara", Alamat = "Jl. Sumatra", JenisPajak = "Restoran", MasaPajak = "Agustus", Potensi = 8500000, Realisasi = 6000000, Tahun = 2024},
                     new() {NOP = "32.76.050.131", NamaWP = "PT Transport Abadi", Alamat = "Jl. Diponegoro", JenisPajak = "Parkir", MasaPajak = "September", Potensi = 9000000, Realisasi = 7500000, Tahun = 2024},
                     new() {NOP = "32.76.050.132", NamaWP = "Mall Surya", Alamat = "Jl. Basuki Rahmat", JenisPajak = "Hotel", MasaPajak = "Oktober", Potensi = 20000000, Realisasi = 18000000, Tahun = 2024}
                };
            }

            public static List<DataIndex> GetDataIndexData()
            {
                return new List<DataIndex>
                {
                    new() {Potensi = 54500000, RealisasiTotal = 45000000, Capaian = 82, TotalOP = 50, RealisasiOP = 45, capaianOP = 98}
                };
            }
            
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

        public class DataIndex
        {
            public int Potensi { get; set; }
            public int RealisasiTotal { get; set; }
            public int Capaian { get; set; }
            public int TotalOP { get; set; }
            public int RealisasiOP { get; set; }
            public int capaianOP { get; set; }
        }
    }
}
