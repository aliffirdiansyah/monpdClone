using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class PenetapanOPVM
    {
        public class Index
        {
            public int SelectedPajak { get; set; }
            public List<SelectListItem> JenisPajakList { get; set; }

            public int SelectedBulan { get; set; }
            public List<SelectListItem> BulanList { get; set; }

            public int SelectedTahun { get; set; }
            public List<SelectListItem> TahunList { get; set; }

            public Index()
            {
                // Jenis Pajak
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x == EnumFactory.EPajak.PBB || x == EnumFactory.EPajak.AirTanah || x == EnumFactory.EPajak.Reklame)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();

                // Bulan 1-12
                BulanList = new List<SelectListItem>();
                for (int i = 1; i <= 12; i++)
                {
                    BulanList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(1, i, 1).ToString("MMMM", new System.Globalization.CultureInfo("id-ID"))
                    });
                }

                // Tahun dari 2023 hingga sekarang
                TahunList = new List<SelectListItem>();
                int tahunSekarang = DateTime.Now.Year;
                for (int i = 2023; i <= tahunSekarang; i++)
                {
                    TahunList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
                SelectedBulan = DateTime.Now.Month;
                SelectedTahun = tahunSekarang;
            }
        }
        public class Show
        {
            public List<PenetapanOP> DataPenetapanOPList { get; set; } = new();
            public PenetapanOPStatistik StatistikData { get; set; } = new();

            public Show()
            {

            }
            public Show(EnumFactory.EPajak JenisPajak, int tahun, int bulan)
            {
                if ((int)JenisPajak == 0)
                {
                    throw new ArgumentException("Harap Pilih Jenis Pajak!");
                }
                DataPenetapanOPList = Method.GetDataPenetapanOPList(JenisPajak, tahun, bulan);
                StatistikData = Method.GetDataPenetapanOPStatistik(JenisPajak, tahun, bulan);

            }
        }
        public class Method
        {
            public static List<PenetapanOP> GetDataPenetapanOPList(EnumFactory.EPajak JenisPajak, int tahun, int bulan)
            {
                var ret = new List<PenetapanOP>();

                var context = DBClass.GetContext();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataKetetapanAbt = context.DbMonAbts
                            .Where(x => x.TglKetetapan.HasValue && x.TahunPajakKetetapan == tahun && x.MasaPajakKetetapan == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new PenetapanOP
                            {
                                Nop = g.Key,
                                NoPenetapan = g.First().NoKetetapan ?? string.Concat(g.First().Nop, "-", g.First().MasaPajakKetetapan, "-", g.First().TahunPajakKetetapan),
                                NamaWP = g.First().NamaOp,
                                Alamat = g.First().AlamatOp,
                                NilaiPenetapan = g.First().PokokPajakKetetapan ?? 0,
                                MasaPajak = new DateTime(
                                    (int)(g.First().TahunPajakKetetapan),
                                    (int)(g.First().MasaPajakKetetapan), 1
                                ).ToString("MMMM", new CultureInfo("id-ID")),
                                Status = g.First().NominalPokokBayar.HasValue
                                    ? (g.First().NominalPokokBayar.Value > 0 ? "Sudah Dibayar" : "Belum Dibayar")
                                    : "Belum Ada Pembayaran"
                            })
                            .ToList();

                        ret.AddRange(dataKetetapanAbt);
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataKetetapanReklame = context.DbMonReklames
                         .Where(x => x.TahunPajak == tahun.ToString() && x.MasaPajakKetetapan == bulan)
                         .Select(x => new
                         {
                             x.NoFormulir,
                             x.FlagPermohonan,
                             x.NoKetetapan,
                             x.NamaPerusahaan,
                             x.Alamatreklame,
                             x.PokokPajakKetetapan,
                             x.TahunPajak
                         })
                         .GroupBy(x => x.NoFormulir)
                         .Select(g => g.OrderByDescending(x => x.FlagPermohonan).FirstOrDefault())
                         .AsEnumerable()  // baru pindahkan ke client
                         .Select(g => new PenetapanOP
                         {
                             Nop = g.NoFormulir,
                             NoPenetapan = $"{g.NoFormulir}-{g.NoKetetapan}-{g.TahunPajak}",
                             NamaWP = g.NamaPerusahaan,
                             Alamat = string.IsNullOrEmpty(g.Alamatreklame)
                                 ? ""
                                 : g.Alamatreklame + " NO. " + g.Alamatreklame,
                             NilaiPenetapan = g.PokokPajakKetetapan ?? 0,
                             MasaPajak = $"Tahun Buku {g.TahunPajak}",
                             Status = g.PokokPajakKetetapan.HasValue
                                 ? (g.PokokPajakKetetapan.Value > 0 ? "Sudah Dibayar" : "Belum Dibayar")
                                 : "Belum Ada Pembayaran"
                         })
                         .ToList();




                        ret.AddRange(dataKetetapanReklame);
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataKetetapanPbb = context.DbMonPbbs
                            .Where(x => x.TahunPajak == tahun)
                            .GroupBy(x => x.Nop)
                            .Select(g => g
                                .OrderByDescending(x => x.KategoriId) // ambil kategoriId terbesar
                                .FirstOrDefault()
                            )
                            .AsEnumerable()
                            .Select(g => new PenetapanOP
                            {
                                Nop = g.Nop,
                                NoPenetapan = string.Concat(
                                    g.Nop, "-",
                                    g.TglBayar?.ToString("yyyyMMdd") ?? "", "-",
                                    g.TahunPajak
                                ),
                                NamaWP = g.WpNama,
                                Alamat = string.Concat(
                                    g.AlamatOp ?? "",
                                    string.IsNullOrEmpty(g.AlamatOpNo) ? "" : $" NO. {g.AlamatOpNo}"
                                ),
                                NilaiPenetapan = g.PokokPajak ?? 0,
                                MasaPajak = $"Tahun Buku {g.TahunPajak}",
                                Status = g.PokokPajak.HasValue
                                    ? (g.PokokPajak.Value > 0 ? "Sudah Dibayar" : "Belum Dibayar")
                                    : "Belum Ada Pembayaran"
                            })
                            .ToList();

                        ret.AddRange(dataKetetapanPbb);
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }

                return ret;
            }

            public static PenetapanOPStatistik GetDataPenetapanOPStatistik(EnumFactory.EPajak JenisPajak, int tahun, int bulan)
            {
                var context = DBClass.GetContext();

                var ret = new PenetapanOPStatistik();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.AirTanah:
                        var abt = context.DbMonAbts
                            .Where(x => x.TglKetetapan.HasValue
                                        && x.TahunPajakKetetapan == tahun
                                        && x.MasaPajakKetetapan == bulan);

                        ret.TotalNilaiPenetapan = abt.Sum(x => (decimal?)x.PokokPajakKetetapan) ?? 0;
                        ret.JumlahPenetapan = abt.Select(x => x.Nop).Distinct().Count();
                        ret.TotalBayar = abt.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0;
                        ret.TotalBelumTerbayar = ret.TotalNilaiPenetapan - ret.TotalBayar;
                        break;

                    case EnumFactory.EPajak.Reklame:
                        var reklame = context.DbMonReklames
                            .Where(x => x.TahunPajak == tahun.ToString() && x.MasaPajakKetetapan == bulan);

                        ret.TotalNilaiPenetapan = reklame.Sum(x => (decimal?)x.PokokPajakKetetapan) ?? 0;
                        ret.JumlahPenetapan = reklame.Select(x => x.Nop).Distinct().Count();
                        ret.TotalBayar = reklame.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0;
                        ret.TotalBelumTerbayar = ret.TotalNilaiPenetapan - ret.TotalBayar;
                        break;

                    case EnumFactory.EPajak.PBB:
                        var pbb = context.DbMonPbbs
                            .Where(x => x.TahunPajak == tahun);

                        ret.TotalNilaiPenetapan = pbb.Sum(x => (decimal?)x.PokokPajak) ?? 0;
                        ret.JumlahPenetapan = pbb.Select(x => x.Nop).Distinct().Count();
                        ret.TotalBayar = pbb.Sum(x => (decimal?)x.JumlahBayarPokok) ?? 0;
                        ret.TotalBelumTerbayar = ret.TotalNilaiPenetapan - ret.TotalBayar;
                        break;

                    default:
                        ret.TotalNilaiPenetapan = 0;
                        ret.JumlahPenetapan = 0;
                        ret.TotalBayar = 0;
                        ret.TotalBelumTerbayar = 0;
                        break;
                }

                return ret;
            }


        }

        public class PenetapanOP
        {
            public string Nop { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public string Nor { get; set; }
            public string NoPenetapan { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public decimal NilaiPenetapan { get; set; } = 0;
            public string MasaPajak { get; set; } = null!;
            public string Status { get; set; } = null!;
        }

        public class PenetapanOPStatistik
        {
            public decimal TotalNilaiPenetapan { get; set; }
            public int JumlahPenetapan { get; set; }
            public decimal TotalBelumTerbayar { get; set; }
            public decimal TotalBayar { get; set; }
        }
    }
}
