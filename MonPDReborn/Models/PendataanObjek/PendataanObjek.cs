namespace MonPDReborn.Models.PendataanObjek
{
    public class PendataanObjek
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index() { }
        }

        public class Show
        {
            public List<DataPendataan> DataPendataanList { get; set; } = new();

            public Show() { }

            public Show(string keyword)
            {
                DataPendataanList = Method.GetFilteredData(keyword);
            }
        }

        public class Detail
        {
            public List<DataDetailPendataan> DataDetailList { get; set; } = new();

            public Detail() { }

            public Detail(string jenisPajak)
            {
                DataDetailList = Method.GetDetailByJenisPajak(jenisPajak);
            }
        }

        public class Method
        {
            public static List<DataPendataan> GetFilteredData(string keyword)
            {
                var all = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return all;

                return all.Where(x => x.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            public static List<DataDetailPendataan> GetDetailByJenisPajak(string jenisPajak)
            {
                var all = GetAllDetail();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return all;

                return all
                    .Where(x => x.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DataPendataan> GetAllData()
            {
                return new List<DataPendataan>
                {
                    new() { JenisPajak = "Pajak Hotel", JumlahOp = 5, Potensi = 50000000, TotalRealisasi = 40000000, Selisih = 10000000 },
                    new() { JenisPajak = "Pajak Restoran", JumlahOp = 6, Potensi = 60000000, TotalRealisasi = 45000000, Selisih = 15000000 }
                };
            }

            private static List<DataDetailPendataan> GetAllDetail()
            {
                return new List<DataDetailPendataan>
                {
                    new() { JenisPajak = "Pajak Hotel", NOP = "35.78.170.005.902.00066", ObjekPajak = "Hotel Bintang", Alamat = "Jl. Rajawali No. 47", Omzet = 100000000, PajakBulanan = 10000000, AvgRealisasi = 9000000, Selisih = 1000000 },
                    new() { JenisPajak = "Pajak Hotel", NOP = "35.78.170.005.902.00067", ObjekPajak = "Hotel Nusantara", Alamat = "Jl. Merdeka No. 12", Omzet = 120000000, PajakBulanan = 12000000, AvgRealisasi = 11000000, Selisih = 1000000 },
                    new() { JenisPajak = "Pajak Restoran", NOP = "35.78.100.002.902.00172", ObjekPajak = "Restoran Nikmat", Alamat = "Jl. Bubutan No. 1-7", Omzet = 80000000, PajakBulanan = 8000000, AvgRealisasi = 7000000, Selisih = 1000000 }
                };
            }
        }

        public class DataPendataan
        {
            public string JenisPajak { get; set; } = null!;
            public int JumlahOp { get; set; }
            public int Potensi { get; set; }
            public int TotalRealisasi { get; set; }
            public int Selisih { get; set; }
        }

        public class DataDetailPendataan
        {
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string ObjekPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int Omzet { get; set; }
            public int PajakBulanan { get; set; }
            public int AvgRealisasi { get; set; }
            public int Selisih { get; set; }
        }
    }
}
