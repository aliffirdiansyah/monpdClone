using MonPDLib;
using System.Globalization;

namespace MonPDReborn.Models.Reklame
{
    public class ReklameTemuanVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModel.Rekap> Data { get; set; } = new();
            public Show(int tahun)
            {
                Data = Method.GetDataRekap(tahun);
            }
        }
        public class Detail
        {
            public List<ViewModel.Upaya> Data { get; set; } = new();
            public Detail(int tahun, int bulan, string ket)
            {
                Data = Method.GetDataUpaya(tahun, bulan, ket);
            }
        }
        public class ViewModel
        {
            public class Rekap
            {
                public decimal Tahun { get; set; }
                public decimal Bulan { get; set; }
                public string BulanNama { get; set; } = null!;
                public decimal Survey { get; set; }
                public decimal Pemberitahuan { get; set; }
                public decimal Silang { get; set; }
                public decimal Bongkar { get; set; }
                public decimal Bantip { get; set; }
                public decimal JmlKetetapan { get; set; }
                public decimal NominalKetetapan { get; set; }
            }
            public class Upaya
            {
                public string Nor { get; set; } = null!;
                public string NoFormulir { get; set; } = null!;
                public string Keterangan { get; set; } = null!;
                public DateTime? TglSkpd { get; set; }
                public decimal Ketetapan { get; set; }
                public decimal TotalBayar { get; set; }
            }
        }
        public class Method
        {
            public static List<ViewModel.Rekap> GetDataRekap(int tahun)
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModel.Rekap>();

                ret = context.VwRekapUpayas
                    .Where(x => x.Tahun == tahun)
                    .Select( x => new ViewModel.Rekap
                    {
                        Tahun = x.Tahun ?? 0,
                        Bulan = x.Bulan ?? 0,
                        BulanNama = new DateTime(1, (int)x.Bulan.Value, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Survey = x.Survey ?? 0,
                        Pemberitahuan = x.Pemberitahuan ?? 0,
                        Silang = x.Silang ?? 0,
                        Bongkar = x.Bongkar ?? 0,
                        Bantip = x.Bantip ?? 0,
                        JmlKetetapan = x.JmlKetetapan ?? 0,
                        NominalKetetapan = x.NominalKetetapan ?? 0
                    })
                    .ToList();

                return ret;
            }
            public static List<ViewModel.Upaya> GetDataUpaya(int tahun, int bulan, string ket)
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModel.Upaya>();

                if (ket == "PENDATAAN SURVEY")
                {
                    ret = context.VwMonReklameUpayas
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan)
                        .Select(x => new ViewModel.Upaya
                        {
                            Nor = x.Nor,
                            NoFormulir = x.NoFormulir ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            TglSkpd = x.TglSkpd,
                            Ketetapan = x.Ketetapan ?? 0,
                            TotalBayar = x.TotalBayar ?? 0
                        })
                        .AsEnumerable()
                        .DistinctBy(x => x.Nor)
                        .ToList();
                }
                else if(ket != "PENDATAAN SURVEY")
                {
                    ret = context.VwMonReklameUpayas
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.Upaya == ket)
                        .Select(x => new ViewModel.Upaya
                        {
                            Nor = x.Nor,
                            NoFormulir = x.NoFormulir ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            TglSkpd = x.TglSkpd,
                            Ketetapan = x.Ketetapan ?? 0,
                            TotalBayar = x.TotalBayar ?? 0
                        })
                        .AsEnumerable()
                        .DistinctBy(x => x.Nor)
                        .ToList();
                }

                return ret;
            }
        }
    }
}
