namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringTahunanVM
    {
        public class Index
        {
            public DateTime TanggalCutOff { get; set; } = DateTime.Now;
            public Index()
            {

            }
        }
        public class Show
        {
            public List<MonitoringTahunanViewModels.TahunanPajak> TahunanPajakList { get; set; } = new();
            public Show(DateTime TanggalCutOff)
            {
                TahunanPajakList = Method.GetTahunanPajak(TanggalCutOff);
            }
        }
        public class Method
        {
            public static List<MonitoringTahunanViewModels.TahunanPajak> GetTahunanPajak(DateTime TanggalCutOff)
            {
                return new List<MonitoringTahunanViewModels.TahunanPajak>
                {
                    new MonitoringTahunanViewModels.TahunanPajak
                    {
                        JenisPajak = "Pajak Bumi dan Bangunan",
                        AkpTahun = 10000000,
                        RealisasiHariAccrual = 1000000,
                        RealisasiSDHariAccrual = 5000000,
                        PersenAccrual = 20.32
                    },
                    new MonitoringTahunanViewModels.TahunanPajak
                    {
                        JenisPajak = "Pajak Kendaraan Bermotor",
                        AkpTahun = 13000000,
                        RealisasiHariAccrual = 2000000,
                        RealisasiSDHariAccrual = 8000000,
                        PersenAccrual = 26.55
                    }
                };
            }
        }
        public class MonitoringTahunanViewModels
        {
            public class TahunanPajak
            {
                public string JenisPajak { get; set; } = null!;
                public long AkpTahun { get; set; }
                public long RealisasiHariAccrual { get; set; }
                public long RealisasiSDHariAccrual { get; set; }
                public double PersenAccrual { get; set; }
            }
        }
    }
}
