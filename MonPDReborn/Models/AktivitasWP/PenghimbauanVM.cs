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
                    PenghimbauanSelesai = 103,
                    TingkatKepatuhan = 11.7,
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
                    new PenghimbauanWP {NPWPD = "01.01.0001", NamaWP = "Hotel Mawar", Ketetapan = "Rp 20.000.000", Terbayar = "Rp 20.000.000", Status = "Lunas"},
                    new PenghimbauanWP {NPWPD = "01.01.0002", NamaWP = "PT. ABC", Ketetapan = "Rp 15.300.000", Terbayar = "Rp 12.000.000", Status = "Sebagian"},
                    new PenghimbauanWP {NPWPD = "01.01.0003", NamaWP = "Karaoke Galaxy", Ketetapan = "Rp 7.800.000", Terbayar = "Rp 7.800.000", Status = "Tunggak"},

                };
            }
        }

        public class Dashboard
        {
            public int TotalPenghimbauan { get; set; }       
            public double PenghimbauanAktif { get; set; }     
            public double PenghimbauanSelesai { get; set; }   
            public double TingkatKepatuhan { get; set; }   

        }

        public class PenghimbauanWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Ketetapan { get; set; } = null!;
            public string Terbayar { get; set; } = null!;
            public string Status { get; set; } = null!;
        }
    }
}
