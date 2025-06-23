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
