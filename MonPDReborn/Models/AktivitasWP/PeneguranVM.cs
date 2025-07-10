namespace MonPDReborn.Models.AktivitasWP
{
    public class PeneguranVM
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
            public List<PeneguranWP> DataPeneguranWPList { get; set; } = new();


            public Show()
            {
                DataPeneguranWPList = Method.GetFilteredData();
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

            public static List<PeneguranWP> GetFilteredData()
            {
                return GetAllDataPeneguranWP();
            }

            private static List<PeneguranWP> GetAllDataPeneguranWP()
            {
                return new List<PeneguranWP>
                {
                    new PeneguranWP {NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Ketetapan = 15_000_000, Terbayar = 15_000_000, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20), TglHimbauan = new DateTime(2018, 5, 10),Status = "Lunas"},
                    new PeneguranWP {NPWPD = "01.01.0002", NamaWP = "PT. ABC", Ketetapan = 15_000_000, Terbayar = 7_000_000, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20),TglHimbauan = new DateTime(2018, 5, 10), Status = "Sebagian"},
                    new PeneguranWP {NPWPD = "01.01.0003", NamaWP = "Karaoke Galaxy", Ketetapan = 15_000_000, Terbayar = 0, TglKetetapan = new DateTime(2018, 5, 12), TglBayar = new DateTime(2018, 5, 20), TglHimbauan = new DateTime(2018, 5, 10), Status = "Tunggak"},

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

        public class PeneguranWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public decimal Ketetapan { get; set; }
            public decimal Terbayar { get; set; }
            public string Status { get; set; } = null!;
            public DateTime TglKetetapan { get; set; }
            public DateTime TglBayar { get; set; }
            public DateTime TglHimbauan { get; set; }
        }
    }
}
