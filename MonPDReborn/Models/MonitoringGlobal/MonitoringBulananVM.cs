using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringBulananVM
    {
        public class Index
        {
            public int SelectedJenisPajakId { get; set; }  // Untuk binding pilihan user
            public List<dynamic> JenisPajakList { get; set; } = new();
            public Index()
            {
                JenisPajakList = GetJenisPajakList();
            }
            private List<dynamic> GetJenisPajakList()
            {
                return new List<dynamic>
                {
                    new { Value = 0, Text = "Semua Pajak" },
                    new { Value = 1, Text = "PBJT - HOTEL" },
                    new { Value = 2, Text = "PBJT - RESTORAN" },
                    new { Value = 3, Text = "PBJT - HIBURAN" },
                    new { Value = 4, Text = "REKLAME" },
                    new { Value = 5, Text = "PBJT - PPJ" },
                    new { Value = 7, Text = "PBJT - PARKIR" },
                    new { Value = 8, Text = "AIR TANAH" },
                    new { Value = 12, Text = "PBB P2" },
                    new { Value = 13, Text = "BPHTB" },
                    new { Value = 20, Text = "OPSEN PKB" },
                    new { Value = 21, Text = "OPSEN BBNKB" },
                };
            }
        }
        public class Show
        {
            public List<MonitoringBulananViewModels.BulananPajak> BulananPajakList { get; set; } = new();
            public Show(DateTime TanggalCutOff)
            {
                BulananPajakList = Method.GetBulananPajak(TanggalCutOff);
            }
        }
        public class Method
        {
            public static List<MonitoringBulananViewModels.BulananPajak> GetBulananPajak(DateTime TanggalCutOff)
            {
                var dataBulanan = new List<MonitoringBulananViewModels.BulananPajak>
                {
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 1, BulanNama = "Januari", AkpTarget = 100000000, Realisasi = 95000000, Pencapaian = 95 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 2, BulanNama = "Februari", AkpTarget = 90000000, Realisasi = 85000000, Pencapaian = 94.4 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 3, BulanNama = "Maret", AkpTarget = 120000000, Realisasi = 100000000, Pencapaian = 83.3 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 4, BulanNama = "April", AkpTarget = 110000000, Realisasi = 78000000, Pencapaian = 70.9 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 5, BulanNama = "Mei", AkpTarget = 95000000, Realisasi = 67000000, Pencapaian = 70.5 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 6, BulanNama = "Juni", AkpTarget = 105000000, Realisasi = 96000000, Pencapaian = 91.4 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 7, BulanNama = "Juli", AkpTarget = 115000000, Realisasi = 108000000, Pencapaian = 93.9 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 8, BulanNama = "Agustus", AkpTarget = 98000000, Realisasi = 76000000, Pencapaian = 77.6 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 9, BulanNama = "September", AkpTarget = 100000000, Realisasi = 69000000, Pencapaian = 69.0 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 10, BulanNama = "Oktober", AkpTarget = 102000000, Realisasi = 97000000, Pencapaian = 95.1 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 11, BulanNama = "November", AkpTarget = 99000000, Realisasi = 95000000, Pencapaian = 96.0 },
                    new MonitoringBulananViewModels.BulananPajak { JenisPajak = "Restoran", Tahun = 2025, Bulan = 12, BulanNama = "Desember", AkpTarget = 125000000, Realisasi = 128000000, Pencapaian = 102.4 },
                };
                return dataBulanan;
            }
        }
        public class MonitoringBulananViewModels
        {
            public class BulananPajak
            {
                public string JenisPajak { get; set; } = null!;
                public int Tahun { get; set; } 
                public int Bulan { get; set; } 
                public string BulanNama { get; set; } = null!;
                public long AkpTarget { get; set; }
                public long Realisasi { get; set; }
                public double Pencapaian { get; set; }
                public double Selisih => Realisasi - AkpTarget;
            }
        }
    }
}
