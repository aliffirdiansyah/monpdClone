using MonPDLib;

namespace MonPDReborn.Models.KontrolPBB
{
    public class AnalitikViewVM
    {
        public class Index
        {
            public Index()
            {
            }
        }
        public class ShowWilayah
        {
            public List<ViewModels.DetailWilayah> Data { get; set; } = new();
            public ShowWilayah(int wilayah, int kec) 
            { 
                Data = Methods.GetDetailWilayah(wilayah, kec);
            }
        }
        public class ShowSegmentasi
        {
            public List<ViewModels.Segmentasi> Data { get; set; } = new();
            public ShowSegmentasi(int wilayah, int kec)
            {
                Data = Methods.GetDataSegmentasi(wilayah, kec);
            }
        }
        public class ViewModels
        {
            public class DetailWilayah
            {
                public decimal Wilayah { get; set; }
                public string NOP { get; set; } = null!;
                public string NamaWP { get; set; } = null!;
                public decimal Penetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Pencapaian => Penetapan == 0 ? 0 : (Realisasi / Penetapan) * 100;
                public string Status { get; set; } = null!;
            }
            public class Segmentasi
            {
                public string KategoriWP { get; set; } = null!;
                public decimal JmlObjek { get; set; }
                public decimal Target { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Pencapaian => Target == 0 ? 0 : (Realisasi / Target) * 100;
                public decimal Kontribusi => Realisasi == 0 ? 0 : (Realisasi / Realisasi) * 100;
            }
            public class DashoboardKinerjaPetugas
            {
                public string Petugas { get; set; } = null!;
                public decimal Capaian { get; set; }
            }
        }
        public class uptbView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class kecamatanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class Methods
        {
            public static List<ViewModels.DetailWilayah> GetDetailWilayah(int wilayah, int kec)
            {
                var list = new List<ViewModels.DetailWilayah>()
                {
                    new ViewModels.DetailWilayah
                    {
                        Wilayah = 5,
                        NOP = "32.30.20.355.235",
                        NamaWP = "CV. Simpel",
                        Penetapan = 250000000,
                        Realisasi = 150000000,
                    },
                    new ViewModels.DetailWilayah
                    {
                        Wilayah = 2,
                        NOP = "32.30.20.355.102",
                        NamaWP = "CV. Sampel",
                        Penetapan = 250000000,
                        Realisasi = 150000000,
                    }
                };
                return list;
            }
            public static List<ViewModels.Segmentasi> GetDataSegmentasi(int wilayah, int kec)
            {
                var list = new List<ViewModels.Segmentasi>()
                {
                    new ViewModels.Segmentasi()
                    {
                        KategoriWP = "WP Besar",
                        JmlObjek = 200,
                        Target = 30000000000,
                        Realisasi = 25000000000,
                    },
                    new ViewModels.Segmentasi()
                    {
                        KategoriWP = "WP Sedang",
                        JmlObjek = 500,
                        Target = 15000000000,
                        Realisasi = 10000000000,
                    },
                    new ViewModels.Segmentasi()
                    {
                        KategoriWP = "WP Kecil",
                        JmlObjek = 800,
                        Target = 5000000000,
                        Realisasi = 3000000000,
                    }
                };
                return list;
            }
        }
    }
}
