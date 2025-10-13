using DevExpress.CodeParser;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static MonPDLib.Helper;

namespace MonPDReborn.Models.KontrolPBB
{
    public class StrategicViewVM
    {
        public class Index
        {
            public ViewModels.Dashboard Data { get; set; } = new ViewModels.Dashboard();
            public Index()
            {
                Data = Methods.GetDashboard();
            }
        }
        public class ShowCapaian
        {
            public List<ViewModels.CapaianKomprehensif> Data { get; set; } = new();
            public ShowCapaian()
            {
                Data = Methods.GetCapaianKomprehensif();
            }
        }
        public class ShowKategori
        {
            public List<ViewModels.KategoriOP> Data { get; set; } = new();
            public ShowKategori()
            {
                Data = Methods.GetKategoriOP();
            }
        }
        public class ShowTunggakan
        {
            public List<ViewModels.CapaianTunggakan> Data { get; set; } = new();
            public ShowTunggakan()
            {
                Data = Methods.GetCapaianTunggakan();
            }
        }
        public class ViewModels
        {
            public class CapaianKomprehensif
            {
                public string Komponen { get; set; } = null!;
                public decimal JmlObjek { get; set; }
                public decimal TargetNow { get; set; }
                public decimal RealisasiNow { get; set; }
                public decimal Selisih { get; set; }
                public decimal Pencapaian { get; set; }
                public decimal Prediksi { get; set; }
                public decimal PersenPrediksi { get; set; }
                public decimal PersenTarget { get; set; }
            }
            public class KategoriOP
            {
                public string Kategori { get; set; } = null!;
                public decimal JmlObjek { get; set; }
                public decimal Penetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Gap => Penetapan - Realisasi;
                public decimal Pencapaian => Penetapan == 0 ? 0 : (Realisasi / Penetapan);
                public decimal Prediksi { get; set; }
                public decimal PersenPrediksi => Penetapan == 0 ? 0 : (Prediksi / Penetapan);
            }
            public class CapaianTunggakan
            {
                public decimal TahunPajak { get; set; }
                public decimal JmlObjek { get; set; }
                public decimal Ketetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Tunggakan => Ketetapan - Realisasi;
                public decimal Gap => Realisasi - Tunggakan;
                public decimal Pencapaian => Tunggakan == 0 ? 0 : (Realisasi / Tunggakan);
                public decimal Prediksi { get; set; }
                public decimal PersenPrediksi => Ketetapan == 0 ? 0 : (Prediksi / Ketetapan);

            }
            public class Dashboard
            {
                public decimal Target { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Capaian { get; set; }
                public decimal Potensi { get; set; }
            }
        }
        public class Methods
        {
            public static List<ViewModels.CapaianKomprehensif> GetCapaianKomprehensif()
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.CapaianKomprehensif>();

                ret = context.DbMonPbbSumpages
                    .Select(x => new ViewModels.CapaianKomprehensif
                    {
                        Komponen = x.Komponen ?? "-",
                        JmlObjek = x.JumlahObjek ?? 0,
                        TargetNow = x.TargetTahunan ?? 0,
                        RealisasiNow = x.TotalRealisasi ?? 0,
                        Prediksi = x.Prediksi ?? 0,
                        Selisih = x.Selisih ?? 0,
                        Pencapaian = x.CapaianPersen ?? 0,
                        PersenPrediksi = x.PersenPrediksi ?? 0,
                        PersenTarget = x.PersenTarget ?? 0

                    })
                    .ToList();

                return ret;
            }
            public static List<ViewModels.KategoriOP> GetKategoriOP()
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.KategoriOP>();

                var tahun = DateTime.Now.Year;
                var hariIni = DateTime.Now.Date;
                var awalTahun = new DateTime(tahun, 1, 1);
                var akhirTahun = new DateTime(tahun, 12, 31);

                var hariBerjalan = (hariIni - awalTahun).Days + 1;  
                var sisaHari = (akhirTahun - hariIni).Days;         

                ret = context.DbMonPbbSummaries
                    .Where(x => x.TahunPajak == tahun)
                    .GroupBy(x => x.Katagori)
                    .Select(g => new ViewModels.KategoriOP
                    {
                        Kategori = g.Key,
                        JmlObjek = g.Count(),
                        Penetapan = g.Sum(x => x.Ketetapan) ?? 0,
                        Realisasi = g.Sum(x => x.Realisasi) ?? 0,
                        Prediksi = hariBerjalan > 0
                            ? ((g.Sum(x => x.Ketetapan) ?? 0) / hariBerjalan) * sisaHari
                            : 0
                    })
                    .ToList();

                return ret;
            }
            public static List<ViewModels.CapaianTunggakan> GetCapaianTunggakan()
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.CapaianTunggakan>();

                var tahun = DateTime.Now.Year;
                var hariIni = DateTime.Now.Date;
                var awalTahun = new DateTime(tahun, 1, 1);
                var akhirTahun = new DateTime(tahun, 12, 31);

                var hariBerjalan = (hariIni - awalTahun).Days + 1;
                var sisaHari = (akhirTahun - hariIni).Days;

                ret = context.DbMonPbbSummaries
                    .GroupBy(x => x.TahunPajak)
                    .Select(g => new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = g.Key ?? 0,
                        JmlObjek = g.Count(),
                        Ketetapan = g.Sum(x => x.Ketetapan) ?? 0,
                        Realisasi = g.Sum(x => x.Realisasi) ?? 0,
                        Prediksi = hariBerjalan > 0
                            ? ((g.Sum(x => x.Ketetapan) ?? 0) / hariBerjalan) * sisaHari
                            : 0
                    })
                    .OrderByDescending(x => x.TahunPajak)
                    .ToList();

                return ret;
            }
            public static ViewModels.Dashboard GetDashboard()
            {
                var context = DBClass.GetContext();
                var ret = new ViewModels.Dashboard();

                ret = new ViewModels.Dashboard
                {
                    Target = context.DbMonPbbSumpages.Sum(x => x.TargetTahunan) ?? 0,
                    Realisasi = context.DbMonPbbSumpages.Sum(x => x.TotalRealisasi) ?? 0,
                    Capaian = context.DbMonPbbSumpages.Average(x => x.CapaianPersen) ?? 0,
                    Potensi = context.DbMonPbbSumpages.Sum(x => x.Prediksi) ?? 0
                };

                return ret;
            }
        }
    }
}
