using static MonPDReborn.Models.DataWP.ProfilePembayaranWPVM;

namespace MonPDReborn.Models.AktivitasWP
{
    public class PenghimbauanVM
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
            public List<PenghimbauanWP> DataPenghimbauanWPList { get; set; } = new();


            public Show()
            {
                DataPenghimbauanWPList = Method.GetFilteredData();
            }
        }
        public class Detail
        {
            public Detail()
            {
                
            }
            public Detail(string nop)
            {
            }
        }
        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalPenghimbauan = 1274,
                    PenghimbauanAktif = 156,
                };
            }

            public static List<PenghimbauanWP> GetFilteredData()
            {
                return GetAllDataPenghimbauanWP();
            }

            private static List<PenghimbauanWP> GetAllDataPenghimbauanWP()
            {
                return new List<PenghimbauanWP>
                {
                    new PenghimbauanWP {NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Ketetapan = 15_000_000, Terbayar = 15_000_000, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20), Status = "Lunas"},
                    new PenghimbauanWP {NPWPD = "01.01.0002", NamaWP = "PT. ABC", Ketetapan = 15_000_000, Terbayar = 7_000_000, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20), Status = "Sebagian"},
                    new PenghimbauanWP {NPWPD = "01.01.0003", NamaWP = "Karaoke Galaxy", Ketetapan = 15_000_000, Terbayar = 0, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20), Status = "Tunggak"},

                };
            }
        }

        public class Dashboard
        {
            public int TotalPenghimbauan { get; set; }       
            public decimal PenghimbauanAktif { get; set; }
            public decimal PresentaseTingkat
            {
                get
                {
                    if (TotalPenghimbauan == 0) return 0;
                    return Math.Round((decimal)PenghimbauanAktif / TotalPenghimbauan * 100, 2);
                }
            }

        }

        public class PenghimbauanWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public decimal Ketetapan { get; set; }
            public decimal Terbayar { get; set; }
            public string Status { get; set; } = null!;
            public DateTime TglKetetapan { get; set; }
            public DateTime TglBayar { get; set; }
        }
    }
}
