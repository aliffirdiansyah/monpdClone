namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringHarianVM
    {
        public class Index
        {
            public string Bulan { get; set; } = "Juli";
            public string JenisPajak { get; set; } = "Semua Pajak";
            public DateTime TanggalUpdate { get; set; } = DateTime.Now;
            public decimal Target { get; set; } = 769_810_788_820;
            public decimal Realisasi { get; set; } = 3_821_305_874;

            // Tambahkan daftar data harian di sini
            public List<HarianPajak> ListHarian { get; set; } = new List<HarianPajak>();
        }

        public class Show
        {
        }

        public class HarianPajak
        {
            public int Tanggal { get; set; }            // 1 - 31
            public string Hari { get; set; }            // Senin, Selasa, ...
            public decimal Target { get; set; }         // Target harian
            public decimal Realisasi { get; set; }      // Realisasi harian
        }
    }
}
