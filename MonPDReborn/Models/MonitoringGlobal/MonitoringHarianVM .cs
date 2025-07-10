namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringHarianVM
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
            public List<MonitoringHarian> DataMonitoringHarianList { get; set; } = new();


            public Show()
            {
                DataMonitoringHarianList = Method.GetFilteredData();
            }
        }

        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalTarget = 500000000,
                    TotalRealisasi = 435750000.50,
                    RataRataPencapaian = 87.15
                };
            }

            public static List<MonitoringHarian> GetFilteredData()
            {
                return GetAllDataMonitoringHarian();
            }

            private static List<MonitoringHarian> GetAllDataMonitoringHarian()
            {
                return new List<MonitoringHarian>
                {
                    // Data untuk hari ini
                    new MonitoringHarian
                    {
                        Tanggal = "10 Juli 2025",
                        JenisPajak = "Pajak Hotel",
                        TargetHarian = "Rp 30.000.000",
                        Realisasi = "Rp 31.500.000",
                        Pencapaian = "105,0%",
                        Status = "Tercapai"
                    },
                    new MonitoringHarian
                    {
                        Tanggal = "10 Juli 2025",
                        JenisPajak = "Pajak Restoran",
                        TargetHarian = "Rp 45.000.000",
                        Realisasi = "Rp 41.200.000",
                        Pencapaian = "91,6%",
                        Status = "Di Bawah Target"
                    },
                    new MonitoringHarian
                    {
                        Tanggal = "10 Juli 2025",
                        JenisPajak = "Pajak Parkir",
                        TargetHarian = "Rp 15.000.000",
                        Realisasi = "Rp 15.000.000",
                        Pencapaian = "100,0%",
                        Status = "Tercapai"
                    },

                    // Data untuk hari sebelumnya
                    new MonitoringHarian
                    {
                        Tanggal = "09 Juli 2025",
                        JenisPajak = "Pajak Hotel",
                        TargetHarian = "Rp 30.000.000",
                        Realisasi = "Rp 28.900.000",
                        Pencapaian = "96,3%",
                        Status = "Hampir Tercapai"
                    },
                    new MonitoringHarian
                    {
                        Tanggal = "09 Juli 2025",
                        JenisPajak = "Pajak Restoran",
                        TargetHarian = "Rp 45.000.000",
                        Realisasi = "Rp 47.800.000",
                        Pencapaian = "106,2%",
                        Status = "Tercapai"
                    },
                    new MonitoringHarian
                    {
                        Tanggal = "09 Juli 2025",
                        JenisPajak = "Pajak Hiburan",
                        TargetHarian = "Rp 20.000.000",
                        Realisasi = "Rp 17.500.000",
                        Pencapaian = "87,5%",
                        Status = "Di Bawah Target"
                    }
                };
            }
        }

        public class Dashboard
        {
            public int TotalTarget { get; set; }
            public double TotalRealisasi { get; set; }
            public double RataRataPencapaian { get; set; }

        }

        public class MonitoringHarian
        {
            public string Tanggal { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string TargetHarian { get; set; } = null!;
            public string Realisasi { get; set; } = null!;
            public string Pencapaian { get; set; } = null!;
            public string Status { get; set; } = null!;

        }
    }
}
