namespace MonPDReborn.Models.DataOP
{
    public class PProfilOPVM
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
            public List<DataRealisasiOp> DataRealisasiOpList { get; set; } = new();
            public Show()
            {
                
            }
            public Show(string keyword)
            {
                DataRealisasiOpList = Method.GetDataRealisasiOpList(keyword);
            }
        }
        public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
            public Detail()
            {
                
            }
            public Detail(string nop)
            {
                DataRealisasiBulananList = Method.GetDetailByNOP(nop);
            }
        }
        public class Method
        {
            public static List<DataRealisasiOp> GetDataRealisasiOpList(string keyword)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.Nama != null && d.Nama.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var allDetail = GetAllDetail();
                return allDetail.Where(x => x.NOP == nop).ToList();
            }

            // Internal dummy data
            private static List<DataRealisasiOp> GetAllData()
            {
                return new List<DataRealisasiOp>
                {
                    new() { No = 1, Wilayah = "01", NOP = "35.78.170.005.902.00066", StatusNOP = "Buka", Nama = "MC. DONALDS", Alamat = "RAJAWALI NO.47", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" },
                    new() { No = 2, Wilayah = "01", NOP = "35.78.100.002.902.00172", StatusNOP = "Buka", Nama = "MC. DONALDS KIOS", Alamat = "BUBUTAN 1-7 (BG JUNCTION LT.GL DAN LT.LL)", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" },
                    new() { No = 3, Wilayah = "01", NOP = "35.78.160.001.902.05140", StatusNOP = "Tutup Permanen", Nama = "MC. DONALDS", Alamat = "MALL PASAR ATUM", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TIDAK TERPASANG" },
                    new() { No = 4, Wilayah = "01", NOP = "35.78.170.005.902.01044", StatusNOP = "Tutup Permanen", Nama = "MC. DONALDS", Alamat = "JL. TAMAN JAYENGRONO (JMP)", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TIDAK TERPASANG" },
                    new() { No = 5, Wilayah = "02", NOP = "35.78.050.005.902.00124", StatusNOP = "Buka", Nama = "MC. DONALDS", Alamat = "DR. IR. H. SOEKARNO NO.218", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" }
                };
            }

            private static List<RealisasiBulanan> GetAllDetail()
            {
                return new List<RealisasiBulanan>
                {
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jan", Nominal = 186020436 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Feb", Nominal = 152000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Mar", Nominal = 173000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Apr", Nominal = 165000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Mei", Nominal = 178000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jun", Nominal = 181000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jul", Nominal = 190000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Agt", Nominal = 200000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Sep", Nominal = 210000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Okt", Nominal = 220000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Nov", Nominal = 230000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Des", Nominal = 240000000 },

                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Jan", Nominal = 30222959 },
                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Feb", Nominal = 25000000 },
                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Mar", Nominal = 27000000 },

                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Jan", Nominal = 134483411 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Feb", Nominal = 140000000 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Mar", Nominal = 150000000 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Des", Nominal = 155000000 }
                };
            }
        }
        public class DataRealisasiOp
        {
            public int No { get; set; }
            public string Wilayah { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string StatusNOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOp { get; set; } = null!;
            public string JenisPenarikan { get; set; } = null!;
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }
    }
}
