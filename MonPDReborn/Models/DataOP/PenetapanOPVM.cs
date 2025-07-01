namespace MonPDReborn.Models.DataOP
{
    public class PenetapanOPVM
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
            public List<PenetapanOP> DataPenetapanOPList { get; set; } = new();
            public Show()
            {
                
            }
            public Show(string keyword)
            {
                DataPenetapanOPList = Method.GetDataPenetapanOPList(keyword);
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
            public static List<PenetapanOP> GetDataPenetapanOPList(string keyword)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.NamaWP != null && d.NamaWP.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

          /*  public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var allDetail = GetAllDetail();
                return allDetail.Where(x => x.NOP == nop).ToList();
            }*/

            // Internal dummy data
            private static List<PenetapanOP> GetAllData()
            {
                return new List<PenetapanOP>
                {
                    new() { No = 2, NoPenetapan = "SPPT-2025-00003", NamaWP = "Siti Aminah", Alamat = "Jl. Merdeka No. 10", NilaiPenetapan = 320_000_000, MasaPajak = "2025", Status = "Belum Lunas" },
                    new() { No = 3, NoPenetapan = "SPPT-2025-00004", NamaWP = "Ahmad Fauzi", Alamat = "Jl. Kenanga No. 8", NilaiPenetapan = 275_000_000, MasaPajak = "01/01/2025 - 31/12/2025", Status = "Lunas" },
                    new() { No = 4, NoPenetapan = "SPPT-2025-00005", NamaWP = "Lina Marlina", Alamat = "Jl. Gajah Mada No. 12", NilaiPenetapan = 500_000_000, MasaPajak = "2025", Status = "Belum Lunas" },
                    new() { No = 5, NoPenetapan = "SPPT-2025-00006", NamaWP = "Hendra Wijaya", Alamat = "Jl. Cendana No. 3", NilaiPenetapan = 620_000_000, MasaPajak = "2025", Status = "Lunas" },
                    new() { No = 6, NoPenetapan = "SPPT-2025-00007", NamaWP = "Ratna Sari", Alamat = "Jl. Diponegoro No. 77", NilaiPenetapan = 390_000_000, MasaPajak = "2025", Status = "Belum Lunas" },
                    new() { No = 7, NoPenetapan = "SPPT-2025-00008", NamaWP = "Teguh Prasetyo", Alamat = "Jl. Hasanuddin No. 5", NilaiPenetapan = 455_000_000, MasaPajak = "2025", Status = "Lunas" },
                    new() { No = 8, NoPenetapan = "SPPT-2025-00009", NamaWP = "Sri Wahyuni", Alamat = "Jl. Kartini No. 21", NilaiPenetapan = 305_000_000, MasaPajak = "2025", Status = "Belum Lunas" },
                    new() { No = 9, NoPenetapan = "SPPT-2025-00010", NamaWP = "Agus Salim", Alamat = "Jl. Sudirman No. 100", NilaiPenetapan = 710_000_000, MasaPajak = "2025", Status = "Lunas" },
                    new() { No = 10, NoPenetapan = "SPPT-2025-00011", NamaWP = "Nina Karina", Alamat = "Jl. Imam Bonjol No. 15", NilaiPenetapan = 480_000_000, MasaPajak = "2025", Status = "Belum Lunas" },
                }; 
            }

           /* private static List<RealisasiBulanan> GetAllDetail()
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
        public class PenetapanOP
        {
            public int No { get; set; }
            public string NoPenetapan { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public decimal NilaiPenetapan { get; set; }
            public string MasaPajak { get; set; } = null!;
            public string Status { get; set; } = null!;
        }

      /*  public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }*/
    }
}
