using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM;

namespace MonPDReborn.Models.AktivitasWP
{
    public class PenagihanVM
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
            public string JenisPajak { get; set; } = string.Empty;

            public List<RekapDataWP> DataRekapWP { get; set; } = new();

            public Show() { }

            public Show(string jenisPajak)
            {
                JenisPajak = jenisPajak;

                // ambil data sesuai filter
                DataRekapWP = Method.GetDataRekapWPList(jenisPajak);
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
                    JmlPenagihan = 1274_000_000,
                    PenagihanAktif = 156,
                    TotalPenerimaan = 1_500_000_000
                };
            }
            public static List<RekapDataWP> GetDataRekapWPList(string JenisPajak)
            {
                var allData = GetRekapDataWP();

                return allData
                    .Where(d =>
                        (string.IsNullOrWhiteSpace(JenisPajak) ||
                         (d.JenisPajak != null && d.JenisPajak.Contains(JenisPajak, StringComparison.OrdinalIgnoreCase))))
                    .ToList();
            }

            private static List<RekapDataWP> GetRekapDataWP()
            {
                return new List<RekapDataWP>
                {
                    new RekapDataWP
                    {
                        NPWPD = "3578090000001",
                        NamaWP = "PT Maju Terus",
                        JenisPajak = "Hotel",
                        NilaiTagihan = 12_500_000m,
                        StatusPengaihan = "Surat Tagihan"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000002",
                        NamaWP = "CV Sukses Selalu",
                        JenisPajak = "Restoran",
                        NilaiTagihan = 7_800_000m,
                        StatusPengaihan = "Surat Paksa"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000003",
                        NamaWP = "Toko Makmur",
                        JenisPajak = "Hiburan",
                        NilaiTagihan = 5_250_000m,
                        StatusPengaihan = "Surat Sita"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000004",
                        NamaWP = "PT Aman Sentosa",
                        JenisPajak = "Parkir",
                        NilaiTagihan = 3_000_000m,
                        StatusPengaihan = "Surat Paksa"
                    },
                    new RekapDataWP
                    {
                        NPWPD = "3578090000005",
                        NamaWP = "CV Berkah",
                        JenisPajak = "Hotel",
                        NilaiTagihan = 15_750_000m,
                        StatusPengaihan = "Surat Tagihan"
                    }
                };
            }
        }
      

        public class Dashboard
        { 
            public decimal JmlPenagihan { get; set; }
            public int PenagihanAktif { get; set; }
            public decimal TotalPenerimaan { get; set; }

        }

        public class RekapDataWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public decimal NilaiTagihan { get; set; }
            public string StatusPengaihan { get; set; } = null!;
        }
    }
}
