using MonPDLib;

namespace MonPDReborn.Models
{
    public class SeriesBulananPajakVM
    {
        public class Index
        {
            public Index()
            {
            }
        }
        public class Show
        {
            public List<Realisasi> DataRealisasiNow { get; set; } = new();
            public List<Realisasi> DataRealisasiMines { get; set; } = new();
            public Show()
            { 
                DataRealisasiNow = Method.GetDataRealisasiNow();
                DataRealisasiMines = Method.GetDataRealisasiMines();
            }
        }
        public class Detail
        {
            public List<Akumulasi> DataAkumulasiNow { get; set; } = new();
            public List<Akumulasi> DataAkumulasiMines { get; set; } = new();
            public Detail()
            { 
                DataAkumulasiNow = Method.GetDataAkumulasiNow();
                DataAkumulasiMines = Method.GetDataAkumulasiMines();
            }
        }
        public class Method
        {
            public static List<Realisasi> GetDataRealisasiNow()
            {
                var context = DBClass.GetContext();
                var ret = new List<Realisasi>();

                var tahun = DateTime.Now.Year;
                var tahunMin = tahun - 1;

                ret = context.SumSeriesBulans
                    .Where(x => x.Tahun == tahun)
                    .Select(x => new Realisasi
                    {
                        Tahun = (decimal)x.Tahun,
                        JenisPajak = x.JenisPajak,
                        Target = x.Target ?? 0,
                        Jan = x.Jan ?? 0,
                        Feb = x.Feb ?? 0,
                        Mar = x.Mar ?? 0,
                        Apr = x.Apr ?? 0,
                        Mei = x.Mei ?? 0,
                        Jun = x.Jun ?? 0,
                        Jul = x.Jul ?? 0,
                        Aug = x.Agu ?? 0,
                        Sep = x.Sep ?? 0,
                        Okt = x.Okt ?? 0,
                        Nov = x.Nov ?? 0,
                        Des = x.Des ?? 0,
                        Jml = x.Jumlah ?? 0,
                        Selisih = (x.Selisih * (-1)) ?? 0,
                        Capaian = x.Capaian ?? 0
                    }).ToList();

                return ret;
            }
            public static List<Realisasi> GetDataRealisasiMines()
            {
                var context = DBClass.GetContext();
                var ret = new List<Realisasi>();

                var tahun = DateTime.Now.Year;
                var tahunMin = tahun - 1;

                ret = context.SumSeriesBulans
                    .Where(x => x.Tahun == tahunMin)
                    .Select(x => new Realisasi
                    {
                        Tahun = (decimal)x.Tahun,
                        JenisPajak = x.JenisPajak,
                        Target = x.Target ?? 0,
                        Jan = x.Jan ?? 0,
                        Feb = x.Feb ?? 0,
                        Mar = x.Mar ?? 0,
                        Apr = x.Apr ?? 0,
                        Mei = x.Mei ?? 0,
                        Jun = x.Jun ?? 0,
                        Jul = x.Jul ?? 0,
                        Aug = x.Agu ?? 0,
                        Sep = x.Sep ?? 0,
                        Okt = x.Okt ?? 0,
                        Nov = x.Nov ?? 0,
                        Des = x.Des ?? 0,
                        Jml = x.Jumlah ?? 0,
                        Selisih = x.Selisih ?? 0,
                        Capaian = x.Capaian ?? 0
                    }).ToList();

                return ret;
            }
            public static List<Akumulasi> GetDataAkumulasiNow()
            {
                var context = DBClass.GetContext();
                var ret = new List<Akumulasi>();

                var tahun = DateTime.Now.Year;
                var tahunMin = tahun - 1;
                ret = context.SumSeriesBulans
                    .Where(x => x.Tahun == tahun)
                    .Select(x => new Akumulasi
                    {
                        Tahun = (decimal)x.Tahun,
                        JenisPajak = x.JenisPajak,
                        Target = x.Target ?? 0,
                        Jan = x.Jan ?? 0,
                        Feb = (x.Jan ?? 0) + (x.Feb ?? 0),
                        Mar = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0),
                        Apr = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0),
                        Mei = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0),
                        Jun = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0),
                        Jul = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0),
                        Aug = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0),
                        Sep = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0),
                        Okt = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0),
                        Nov = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0) + (x.Nov ?? 0),
                        Des = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0) + (x.Nov ?? 0) + (x.Des ?? 0),

                        Jml = x.Jumlah ?? 0,
                        Selisih = (x.Selisih * (-1)) ?? 0,
                        Capaian = x.Capaian ?? 0
                    }).ToList();

                return ret;
            }
            public static List<Akumulasi> GetDataAkumulasiMines()
            {
                var context = DBClass.GetContext();
                var ret = new List<Akumulasi>();

                var tahun = DateTime.Now.Year;
                var tahunMin = tahun - 1;
                ret = context.SumSeriesBulans
                    .Where(x => x.Tahun == tahunMin)
                    .Select(x => new Akumulasi
                    {
                        Tahun = (decimal)x.Tahun,
                        JenisPajak = x.JenisPajak,
                        Target = x.Target ?? 0,
                        Jan = x.Jan ?? 0,
                        Feb = (x.Jan ?? 0) + (x.Feb ?? 0),
                        Mar = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0),
                        Apr = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0),
                        Mei = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0),
                        Jun = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0),
                        Jul = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0),
                        Aug = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0),
                        Sep = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0),
                        Okt = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0),
                        Nov = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0) + (x.Nov ?? 0),
                        Des = (x.Jan ?? 0) + (x.Feb ?? 0) + (x.Mar ?? 0) + (x.Apr ?? 0) + (x.Mei ?? 0) + (x.Jun ?? 0) + (x.Jul ?? 0) + (x.Agu ?? 0) + (x.Sep ?? 0) + (x.Okt ?? 0) + (x.Nov ?? 0) + (x.Des ?? 0),

                        Jml = x.Jumlah ?? 0,
                        Selisih = (x.Selisih * (-1)) ?? 0,
                        Capaian = x.Capaian ?? 0
                    }).ToList();

                return ret;
            }
        }
        public class  Realisasi
        {
            public decimal Tahun { get; set; }
            public string JenisPajak { get; set; } = null!;
            public decimal Target { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal Mei { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Aug { get; set; }
            public decimal Sep { get; set; }
            public decimal Okt { get; set; }
            public decimal Nov { get; set; }
            public decimal Des { get; set; }
            public decimal Jml { get; set; }
            public decimal Selisih { get; set; }
            public decimal Capaian { get; set; }
        }
        public class  Akumulasi
        {
            public decimal Tahun { get; set; }
            public string JenisPajak { get; set; } = null!;
            public decimal Target { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal Mei { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Aug { get; set; }
            public decimal Sep { get; set; }
            public decimal Okt { get; set; }
            public decimal Nov { get; set; }
            public decimal Des { get; set; }
            public decimal Jml { get; set; }
            public decimal Selisih { get; set; }
            public decimal Capaian { get; set; }
        }
    }
}
