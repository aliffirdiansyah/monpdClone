namespace MonPDReborn.Models.Pencarian
{
    public class PencarianOP
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
        public class Method
        {
            public static List<DataRealisasiOp> GetDataRealisasiOpList(string keyword)
            {
                var allData = new List<DataRealisasiOp>
                {
                    new DataRealisasiOp
                    {
                        No = 1,
                        Wilayah = "01",
                        NOP = "35.78.170.005.902.00066",
                        StatusNOP = "Buka",
                        Nama = "MC. DONALDS",
                        Alamat = "RAJAWALI NO.47",
                        JenisOp = "RESTORAN (RESTORAN)",
                        JenisPenarikan = "TS",
                        Realisasi = new List<RealisasiBulanan>
                        {
                            new() { Bulan = "Jan", Nominal = 186020436 },
                            new() { Bulan = "Feb", Nominal = 152000000 },
                            new() { Bulan = "Mar", Nominal = 173000000 },
                            new() { Bulan = "Apr", Nominal = 165000000 },
                            new() { Bulan = "Mei", Nominal = 178000000 },
                            new() { Bulan = "Jun", Nominal = 181000000 },
                            new() { Bulan = "Jul", Nominal = 190000000 },
                            new() { Bulan = "Agt", Nominal = 200000000 },
                            new() { Bulan = "Sep", Nominal = 210000000 },
                            new() { Bulan = "Okt", Nominal = 220000000 },
                            new() { Bulan = "Nov", Nominal = 230000000 },
                            new() { Bulan = "Des", Nominal = 240000000 }
                        }
                    },
                    new DataRealisasiOp
                    {
                        No = 2,
                        Wilayah = "01",
                        NOP = "35.78.100.002.902.00172",
                        StatusNOP = "Buka",
                        Nama = "MC. DONALDS KIOS",
                        Alamat = "BUBUTAN 1-7 (BG JUNCTION LT.GL DAN LT.LL)",
                        JenisOp = "RESTORAN (RESTORAN)",
                        JenisPenarikan = "TS",
                        Realisasi = new List<RealisasiBulanan>
                        {
                            new() { Bulan = "Jan", Nominal = 30222959 },
                            new() { Bulan = "Feb", Nominal = 25000000 },
                            new() { Bulan = "Mar", Nominal = 27000000 }
                        }
                    },
                    new DataRealisasiOp
                    {
                        No = 3,
                        Wilayah = "01",
                        NOP = "35.78.160.001.902.05140",
                        StatusNOP = "Tutup Permanen",
                        Nama = "MC. DONALDS",
                        Alamat = "MALL PASAR ATUM",
                        JenisOp = "RESTORAN (RESTORAN)",
                        JenisPenarikan = "TIDAK TERPASANG",
                        Realisasi = new List<RealisasiBulanan>() // Kosong
                    },
                    new DataRealisasiOp
                    {
                        No = 4,
                        Wilayah = "01",
                        NOP = "35.78.170.005.902.01044",
                        StatusNOP = "Tutup Permanen",
                        Nama = "MC. DONALDS",
                        Alamat = "JL. TAMAN JAYENGRONO (JMP)",
                        JenisOp = "RESTORAN (RESTORAN)",
                        JenisPenarikan = "TIDAK TERPASANG",
                        Realisasi = new List<RealisasiBulanan>() // Kosong
                    },
                    new DataRealisasiOp
                    {
                        No = 5,
                        Wilayah = "02",
                        NOP = "35.78.050.005.902.00124",
                        StatusNOP = "Buka",
                        Nama = "MC. DONALDS",
                        Alamat = "DR. IR. H. SOEKARNO NO.218",
                        JenisOp = "RESTORAN (RESTORAN)",
                        JenisPenarikan = "TS",
                        Realisasi = new List<RealisasiBulanan>
                        {
                            new() { Bulan = "Jan", Nominal = 134483411 },
                            new() { Bulan = "Feb", Nominal = 140000000 },
                            new() { Bulan = "Mar", Nominal = 150000000 },
                            new() { Bulan = "Des", Nominal = 155000000 }
                        }
                    }
                };

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.Nama != null && d.Nama.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
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
            public List<RealisasiBulanan> Realisasi { get; set; } = new();
        }

        public class RealisasiBulanan
        {
            public string Bulan { get; set; } = null!;
            public decimal? Nominal { get; set; }
        }
    }
}
