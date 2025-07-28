using MonPDLib;
using MonPDReborn.Models.Reklame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class ReklameLiarVM
    {
        // Untuk halaman utama Index.cshtml
        public class Index
        {
            public int SelectedTahun { get; set; }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<RekapBulanan> DataRekap { get; set; } = new();
            public Show()
            {
                DataRekap = Method.GetRekapDataBulanan();
            }
        }

        // Untuk Partial View _Detail.cshtml (modal)
        public class Detail
        {
            public InfoJalan DataDetail { get; set; } = new();

            public Detail(string namaJalan, string kelasJalan, int bulan)
            {
                DataDetail = Method.GetDetailDataReklame(kelasJalan, namaJalan, bulan);
            }
        }

        public static class Method
        {
            // Method baru untuk data dummy bulanan
            public static List<RekapBulanan> GetRekapDataBulanan()
            {
                var ret = new List<RekapBulanan>();
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                ret = context.MvReklameRekapLiars
                    .Select(x => new RekapBulanan()
                    {
                        TahunData = tahun,
                        NamaJalan = x.NamaJalan ?? "",
                        KelasJalan = x.KelasJalan ?? "",
                        Jenis = x.Jenis ?? "",
                        Jan = x.Jan ?? 0,
                        Feb = x.Feb ?? 0,
                        Mar = x.Mar ?? 0,
                        Apr = x.Apr ?? 0,
                        Mei = x.Mei ?? 0,
                        Jun = x.Jun ?? 0,
                        Jul = x.Jul ?? 0,
                        Agu = x.Agt ?? 0,
                        Sep = x.Sep ?? 0,
                        Okt = x.Okt ?? 0,
                        Nov = x.Nov ?? 0,
                        Des = x.Des ?? 0
                    })
                    .ToList();

                return ret;
            }
            public static InfoJalan GetDetailDataReklame(string kelasJalan, string namaJalan, int bulan)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var data = context.DbMonReklameLiars
                    .Where(x => x.KelasJalan == kelasJalan
                             && x.NamaJalan == namaJalan
                             && x.TanggalSkSilang.Month == bulan)
                    .ToList();

                var jumlahReklame = data.Count;

                var detailDataList = data.Select(x => new DetailData()
                {
                    KelasJalan = x.KelasJalan ?? "",
                    NamaJalan = x.NamaJalan ?? "",
                    AlamatReklame = x.AlamatReklame ?? "",
                    Jenis = x.Jenis ?? "",
                    TanggalSilang = x.TanggalSkSilang,
                    TanggalBantib = x.TanggalBantib.Value,
                    TanggalBongkar = x.TanggalBongkar.Value,
                    TanggalSkBongkar = x.TanggalSkBongkar.Value
                }).ToList();

                var ret = new InfoJalan
                {
                    NamaJalan = namaJalan,
                    Bulan = new DateTime(currentYear, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                    Tahun = currentYear.ToString(),
                    JumlahReklame = jumlahReklame,
                    DetailDataList = detailDataList
                };

                return ret;
            }
        }

        // --- MODEL DATA BARU ---
        public class RekapBulanan
        {
            public int TahunData { get; set; }
            public string NamaJalan { get; set; } = null!;
            public string KelasJalan { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal Mei { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Agu { get; set; }
            public decimal Sep { get; set; }
            public decimal Okt { get; set; }
            public decimal Nov { get; set; }
            public decimal Des { get; set; }
            public decimal Total => Jan + Feb + Mar + Apr + Mei + Jun + Jul + Agu + Sep + Okt + Nov + Des;
        }

        public class DetailData
        {
            public string KelasJalan { get; set; } = null!;
            public string NamaJalan { get; set; } = null!;
            public string AlamatReklame { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public DateTime TanggalSilang { get; set; }
            public DateTime TanggalBantib { get; set; }
            public DateTime? TanggalBongkar { get; set; }
            public DateTime? TanggalSkBongkar { get; set; }
        }

        public class InfoJalan
        {
            public string NamaJalan { get; set; } = "";
            public string Bulan { get; set; } = "";
            public string Tahun { get; set; } = "";
            public int JumlahReklame { get; set; }
            public List<DetailData> DetailDataList { get; set; } = new();
        }
    }
}