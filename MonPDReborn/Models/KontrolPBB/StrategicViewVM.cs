using Microsoft.AspNetCore.Mvc;

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
            public static List<ViewModels.CapaianKomprehensif> Data { get; set; } = new();
            public ShowCapaian()
            {
                Data = Methods.GetCapaianKomprehensif();
            }
        }
        public class ShowKategori
        {
            public static List<ViewModels.KategoriOP> Data { get; set; } = new();
            public ShowKategori()
            {
                Data = Methods.GetKategoriOP();
            }
        }
        public class ShowTunggakan
        {
            public static List<ViewModels.CapaianTunggakan> Data { get; set; } = new();
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
                public decimal Gap => TargetNow - RealisasiNow;
                public decimal PencapaianNow => TargetNow == 0 ? 0 : (RealisasiNow / TargetNow) * 100;
                public decimal Prediksi { get; set; }
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
                var list = new List<ViewModels.CapaianKomprehensif>
                {
                    new ViewModels.CapaianKomprehensif
                    {
                        Komponen = "Penetapan Tahun Ini",
                        JmlObjek = 150000,
                        TargetNow = 12000000000,
                        RealisasiNow = 11500000000,
                        Prediksi = 12500000000
                    },
                    new ViewModels.CapaianKomprehensif
                    {
                        Komponen = "Rencana Penagihan Tunggakan",
                        JmlObjek = 5000,
                        TargetNow = 3000000000,
                        RealisasiNow = 2800000000,
                        Prediksi = 3200000000
                    },
                };
                return list;
            }
            public static List<ViewModels.KategoriOP> GetKategoriOP()
            {
                var list = new List<ViewModels.KategoriOP>
                {
                    new ViewModels.KategoriOP
                    {
                        Kategori = "Tanah Kosong",
                        JmlObjek = 80000,
                        Penetapan = 6000000000,
                        Realisasi = 5800000000,
                        Prediksi = 6200000000
                    },
                    new ViewModels.KategoriOP
                    {
                        Kategori = "Tanah + Bangunan",
                        JmlObjek = 40000,
                        Penetapan = 4000000000,
                        Realisasi = 3500000000,
                        Prediksi = 4200000000
                    },
                    new ViewModels.KategoriOP
                    {
                        Kategori = "Bangunan",
                        JmlObjek = 30000,
                        Penetapan = 3000000000,
                        Realisasi = 2700000000,
                        Prediksi = 3100000000
                    }
                };
                return list;
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
