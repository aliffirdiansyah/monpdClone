using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace MonPDReborn.Models.KontrolPBB
{
    public class StrategicViewVM
    {
        public class Index
        {
            public Index()
            {

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
                public decimal Pencapaian => Penetapan == 0 ? 0 : (Realisasi / Penetapan) * 100;
                public decimal Prediksi { get; set; }
            }
            public class  CapaianTunggakan
            {
                public decimal TahunPajak { get; set; }
                public decimal JmlObjek { get; set; }
                public decimal Tunggakan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Gap => Tunggakan - Realisasi;
                public decimal Pencapaian => Tunggakan == 0 ? 0 : (Realisasi / Tunggakan) * 100;
                public decimal Prediksi { get; set; }
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

                ret = context.DbMonPbbSummaries
                    .Where(x => x.TahunPajak == tahun)
                    .GroupBy(x => x.Katagori)
                    .Select(g => new ViewModels.KategoriOP
                    {
                        Kategori = g.Key,
                        JmlObjek = g.Count(), 
                        Penetapan = g.Sum(x => x.Ketetapan) ?? 0, 
                        Realisasi = g.Sum(x => x.Realisasi) ?? 0
                    })
                    .ToList();
                return ret;
            }
            public static List<ViewModels.CapaianTunggakan> GetCapaianTunggakan()
            {
                var list = new List<ViewModels.CapaianTunggakan>
                {
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2020,
                        JmlObjek = 5000,
                        Tunggakan = 2000000000,
                        Realisasi = 1500000000,
                        Prediksi = 2200000000
                    },
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2021,
                        JmlObjek = 3000,
                        Tunggakan = 1000000000,
                        Realisasi = 800000000,
                        Prediksi = 1200000000
                    },
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2022,
                        JmlObjek = 2000,
                        Tunggakan = 500000000,
                        Realisasi = 400000000,
                        Prediksi = 600000000
                    },
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2023,
                        JmlObjek = 2000,
                        Tunggakan = 500000000,
                        Realisasi = 400000000,
                        Prediksi = 600000000
                    },
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2024,
                        JmlObjek = 2000,
                        Tunggakan = 500000000,
                        Realisasi = 400000000,
                        Prediksi = 600000000
                    },
                    new ViewModels.CapaianTunggakan
                    {
                        TahunPajak = 2025,
                        JmlObjek = 2000,
                        Tunggakan = 500000000,
                        Realisasi = 400000000,
                        Prediksi = 600000000
                    }
                };
                return list;
            }
        }
    }
}
