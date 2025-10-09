using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.PencarianOPVM;

namespace MonPDReborn.Models.KontrolPBB
{
    public class PencarianPBBVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }
            public Index(string keyword)
            {
                Keyword = keyword;
            }
        }
        public class Show
        {
            public List<ViewModels.PencarianPBB> Data { get; set; } = new();
            public Show(string keyword)
            {
                Data = Method.GetDataPencarian(keyword);
            }
        }
        public class Detail
        {
            public ViewModels.DataPBB Data { get; set; }

            public Detail(string nop)
            {
                Data = Method.GetDataPbb(nop).FirstOrDefault() ?? new ViewModels.DataPBB();
            }
        }

        public class ViewModels
        {
            public class PencarianPBB
            {
                public string NOP { get; set; } = null!;
                public string NamaWP { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public string Wilayah { get; set; } = null!;
                public string Kategori { get; set; } = null!;
            }

            public class DataPBB
            {
                public string NOP { get; set; } = null!;
                public DataOP OPData { get; set; } = new();
                public DataWP WPData { get; set; } = new();
                public List<Riwayat> DataRiwayat { get; set; } = new();
                public class DataOP
                {
                    public decimal LTanah { get; set; }
                    public decimal LBangunan { get; set; }
                    public string Jenis { get; set; } = null!;
                    public string Lokasi { get; set; } = null!;
                }
                public class DataWP
                {
                    public string NamaWP { get; set; } = null!;
                    public string TelpWP { get; set; } = null!;
                    public string AlamatWP { get; set; } = null!;
                    public string Status { get;set; } = null!;
                }
                public class Riwayat
                {
                    public decimal TahunPajak { get; set; }
                    public decimal Jumlah { get; set; }
                    public string Status { get; set; } = null!;
                }
            }
        }
        public class Method
        {
            public static List<ViewModels.PencarianPBB> GetDataPencarian(string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    throw new ArgumentException("Keyword harus diisi");
                }
                if (keyword.Length < 3)
                {
                    throw new ArgumentException("Keyword harus diisi minimal 3");
                }
                if (keyword.Length == 24 || keyword.Contains("."))
                {
                    keyword = keyword.Replace(".", "");
                }
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.PencarianPBB>();

                ret = context.DbMonPbbSummaries
                    .Where(x => (x.Nop == keyword) || (x.NamaWp.ToUpper().Contains(keyword.ToUpper())) || (x.Katagori.ToUpper().Contains(keyword.ToUpper())) || (x.Wilayah.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunPajak)
                    .Select(x => new ViewModels.PencarianPBB 
                    {
                        NOP = x.Nop ?? "-",
                        NamaWP = x.NamaWp ?? "-",
                        Alamat = x.AlamatWp ?? "-",
                        Wilayah = x.Wilayah ?? "-",
                        Kategori = x.Katagori ?? "-",
                    })
                    .Distinct()
                    .ToList();

                return ret;
            }
            public static List<ViewModels.DataPBB> GetDataPbb(string nop)
            {
                using var context = DBClass.GetContext();

                // Ambil data utama dulu
                var dataUtama = context.DbMonPbbSummaries
                    .Where(x => x.Nop == nop)
                    .Select(x => new ViewModels.DataPBB
                    {
                        NOP = x.Nop,
                        OPData = new ViewModels.DataPBB.DataOP
                        {
                            LTanah = x.LBumi ?? 0,
                            LBangunan = x.LBangunan ?? 0,
                            Jenis = x.Katagori ?? "-",
                            Lokasi = x.AlamatOp ?? "-"
                        },
                        WPData = new ViewModels.DataPBB.DataWP
                        {
                            NamaWP = x.NamaWp ?? "-",
                            TelpWP = x.Kontak ?? "-",
                            AlamatWP = x.AlamatWp ?? "-",
                            Status = x.StatusLunas ?? "-"
                        },
                        DataRiwayat = new List<ViewModels.DataPBB.Riwayat>()
                    })
                    .Distinct()
                    .ToList();

                // Isi DataRiwayat secara terpisah (di client-side)
                foreach (var item in dataUtama)
                {
                    item.DataRiwayat = context.DbMonPbbSummaries
                        .Where(r => r.Nop == item.NOP)
                        .Select(r => new ViewModels.DataPBB.Riwayat
                        {
                            TahunPajak = r.TahunPajak ?? 0,
                            Jumlah = r.Realisasi ?? 0,
                            Status = r.StatusLunas ?? "-"
                        })
                        .ToList();
                }

                return dataUtama;
            }


        }
    }
}
