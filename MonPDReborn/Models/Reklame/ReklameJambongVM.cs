using MonPDLib;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MonPDReborn.Models.Reklame
{
    public class ReklameJambongVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModels.Jambong> Data { get; set; } = new();
            public Show()
            {
                Data = Methods.GetDataJambong();
            }
        }
        public class Detail
        {
            public List<ViewModels.DetailJambong> Data { get; set; } = new();
            public Detail(string jenis, int tahun, int bulan)
            {
                Data = Methods.GetDataDetail(jenis, tahun, bulan);
            }
        }
        public class ViewModels
        {
            public class Jambong
            {
                public string BulanNama { get; set; } = null!;
                public int Bulan { get; set; }
                public int Tahun { get; set; }
                public string JenisInsidentil { get; set; } = null!;
                public decimal Insidentil { get; set; }
                public decimal JambongInsidentil { get; set; }
                public string JenisPermanen { get; set; } = null!;
                public decimal Permanen { get; set; }
                public decimal JambongPermanen { get; set; }
                public string JenisTerbatas { get; set; } = null!;
                public decimal Terbatas { get; set; }
                public decimal JambongTerbatas { get; set; }
            }
            public class DetailJambong
            {
                public string NoFormulir { get; set; } = null!;
                public string Nama { get; set; } = null!;
                public string NamaPerusahaan { get; set; } = null!;
                public string IsiReklame { get; set; } = null!;
                public string AlamatReklame { get; set; } = null!;
                public decimal NilaiJambong { get; set; }
                public string NamaSatu => $"{Nama} - ({NamaPerusahaan})";
            }
        }
        public class Methods
        {
            public static List<ViewModels.Jambong> GetDataJambong()
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.Jambong>();

                var year = DateTime.Now.Year;
                var data = context.DbMonJambongs.AsQueryable();

                for (int i = 1; i <= 12; i++)
                {
                    var currentDate = new DateTime(year, i, 1);
                    ret.Add(new ViewModels.Jambong()
                    {
                        BulanNama = new DateTime(year, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Bulan = i,
                        Tahun = year,

                        JenisInsidentil = "INSIDENTIL",
                        Insidentil = data.Where(x => x.FlagPermohonan == "INSIDENTIL" && x.Tahun == year && x.Bulan == i).Count(),
                        JambongInsidentil = data.Where(x => x.FlagPermohonan == "INSIDENTIL" && x.Tahun == year && x.Bulan == i).Sum(x => x.Jambong ?? 0),

                        JenisPermanen = "PERMANEN",
                        Permanen = data.Where(x => x.FlagPermohonan == "PERMANEN" && x.Tahun == year && x.Bulan == i).Count(),
                        JambongPermanen = data.Where(x => x.FlagPermohonan == "PERMANEN" && x.Tahun == year && x.Bulan == i).Sum(x => x.Jambong ?? 0),

                        JenisTerbatas = "TERBATAS",
                        Terbatas = data.Where(x => x.FlagPermohonan == "TERBATAS" && x.Tahun == year && x.Bulan == i).Count(),
                        JambongTerbatas = data.Where(x => x.FlagPermohonan == "TERBATAS" && x.Tahun == year && x.Bulan == i).Sum(x => x.Jambong ?? 0)
                    });
                }

                return ret;
            }
            public static List<ViewModels.DetailJambong> GetDataDetail(string jenis, int tahun, int bulan)
            {
                using var context = DBClass.GetContext();
                var ret = new List<ViewModels.DetailJambong>();

                // Ambil data dari database sesuai filter
                ret = context.DbMonJambongs
                    .Where(x => x.FlagPermohonan == jenis && x.Tahun == tahun && x.Bulan == bulan)
                    .Select(x => new ViewModels.DetailJambong
                    {
                        NoFormulir = x.NoFormulir ?? "-",
                        Nama = x.Nama ?? "-",
                        NamaPerusahaan = x.NamaPerusahaan ?? "-",
                        AlamatReklame = x.Alamatreklame ?? "-",
                        IsiReklame = x.IsiReklame ?? "-",
                        NilaiJambong = x.Jambong ?? 0
                    })
                    .ToList();

                return ret;
            }

        }
    }
}
