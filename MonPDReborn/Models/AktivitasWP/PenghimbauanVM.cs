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
            public Show()
            {
                
            }
            public Show(string keyword)
            {
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
        }

        public class Dashboard
        {
            public int TotalPenghimbauan { get; set; }       
            public double PenghimbauanAktif { get; set; }     
            public double PenghimbauanSelesai { get; set; }   
            public double TingkatKepatuhan { get; set; }   

        }
    }
}
