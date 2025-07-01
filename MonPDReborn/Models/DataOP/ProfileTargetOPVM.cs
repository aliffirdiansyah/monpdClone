namespace MonPDReborn.Models.DataOP
{
    public class ProfileTargetOPVM
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
            public List<TargetSeluruh> DataTargetSeluruhList { get; set; } = new();
            public Show()
            {
                
            }
            public Show(string keyword)
            {
                DataTargetSeluruhList = Method.GetDataRealisasiOpList(keyword);
            }
        }
       /* public class Detail
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
            public static List<TargetSeluruh> GetDataRealisasiOpList(string keyword)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

          /*  public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var allDetail = GetAllDetail();
                return allDetail.Where(x => x.NOP == nop).ToList();
            }*/

            // Internal dummy data
            private static List<TargetSeluruh> GetAllData()
            {
                return new List<TargetSeluruh>
                {
                    new() { JenisPajak = "Hotel", Wilayah = "UPTB 1", Target = 500_000_000, Keterangan = "Target Q1 Hotel Wilayah Timur", Tanggal = new DateTime(2025, 1, 15) },
                    new() { JenisPajak = "Hotel", Wilayah = "UPTB 2", Target = 420_000_000, Keterangan = "Target Q1 Hotel Wilayah Barat", Tanggal = new DateTime(2025, 1, 15) },
                    new() { JenisPajak = "Restoran", Wilayah = "UPTB 3", Target = 600_000_000, Keterangan = "Target Q1 Restoran Wilayah Timur", Tanggal = new DateTime(2025, 1, 20) },
                    new() { JenisPajak = "Restoran", Wilayah = "UPTB 4", Target = 550_000_000, Keterangan = "Target Q1 Restoran Wilayah Barat", Tanggal = new DateTime(2025, 1, 20) },
                    new() { JenisPajak = "Parkir", Wilayah = "UPTB 5", Target = 300_000_000, Keterangan = "Target Q1 Parkir Wilayah Utara", Tanggal = new DateTime(2025, 2, 1) },
                    new() { JenisPajak = "Hiburan", Wilayah = "UPTB 2", Target = 700_000_000, Keterangan = "Target Q1 Hiburan Wilayah Selatan", Tanggal = new DateTime(2025, 2, 5) },
                    new() { JenisPajak = "PBB", Wilayah = "UPTB 1", Target = 2_000_000_000, Keterangan = "Target Tahunan PBB", Tanggal = new DateTime(2025, 1, 1) },
                    new() { JenisPajak = "BPHTB", Wilayah = "UPTB 4", Target = 1_500_000_000, Keterangan = "Target Tahunan BPHTB Wilayah Pusat", Tanggal = new DateTime(2025, 1, 10) },
                };
            }

            public static List<TargetPajakBulanan> GetDummyTargetPajakBulanan()
            {
                return new List<TargetPajakBulanan>
                {
                    new() { Tahun = 2025, Bulan = "Jan", Target = 400_000_000 },
                    new() { Tahun = 2025, Bulan = "Feb", Target = 430_000_000 },
                    new() { Tahun = 2025, Bulan = "Mar", Target = 470_000_000 },
                    new() { Tahun = 2025, Bulan = "Apr", Target = 540_000_000 },
                    new() { Tahun = 2025, Bulan = "Mei", Target = 580_000_000 },
                    new() { Tahun = 2025, Bulan = "Jun", Target = 690_000_000 },
                    new() { Tahun = 2025, Bulan = "Jul", Target = 690_000_000 },
                    new() { Tahun = 2025, Bulan = "Agu", Target = 710_000_000 },
                    new() { Tahun = 2025, Bulan = "Sep", Target = 760_000_000 },
                    new() { Tahun = 2025, Bulan = "Okt", Target = 800_000_000 },
                    new() { Tahun = 2025, Bulan = "Nov", Target = 850_000_000 },
                    new() { Tahun = 2025, Bulan = "Des", Target = 900_000_000 }
                };
            }


            /*  private static List<RealisasiBulanan> GetAllDetail()
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
              }*/
        }


        public class TargetSeluruh
        {
            public string JenisPajak { get; set; } = null!;
            public string Wilayah { get; set; } = null!;
            public decimal Target { get; set; }
            public string Keterangan { get; set; } = null!;
            public DateTime Tanggal { get; set; }
        }

        public class TargetPajakBulanan
        {
            public int Tahun { get; set; }          // Misal: 2025
            public string Bulan { get; set; } = ""; // Misal: "Jan", "Feb", dst.
            public decimal Target { get; set; }     // Misal: 400_000_000
        }

        /*   public class RealisasiBulanan
           {
               public string NOP { get; set; } = null!;
               public string Bulan { get; set; } = null!;
               public decimal Nominal { get; set; }
           }*/
    }
}
