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
        public class Show
        {
            public Show() 
            { 
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
        }
        public class Methods
        {

        }
    }
}
