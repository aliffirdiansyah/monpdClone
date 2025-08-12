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

                // Ambil nilai status negatif (selain "Sudah Dibayar")
                // **Penting:** Pastikan string "Belum Dibayar" sesuai dengan data Anda
                string statusBelumBayar = "Belum Ada Pembayaran";

                // Isi properti StatistikData dengan hasil kalkulasi
                StatistikData = new PenetapanOPStatistik
                {
                    // Menjumlahkan seluruh 'NilaiPenetapan'
                    TotalNilaiPenetapan = DataPenetapanOPList.Sum(x => x.NilaiPenetapan),

                    // Menghitung jumlah baris/item dalam list
                    JumlahPenetapan = DataPenetapanOPList.Count(),

                    // Menyaring data yang statusnya belum bayar, lalu menjumlahkan 'NilaiPenetapan'
                    TotalBelumTerbayar = DataPenetapanOPList
                                            .Where(x => x.Status == statusBelumBayar)
                                            .Sum(x => x.NilaiPenetapan)
                };
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
                            .Where(x => x.TglKetetapan.HasValue && x.TahunPajakKetetapan == tahun && x.MasaPajakKetetapan == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new PenetapanOP
                            {
                                Nop = g.Key,
                                NoPenetapan = string.Concat(g.First().Nop, "-", g.First().MasaPajakKetetapan, "-", g.First().TahunPajakKetetapan),
                                NamaWP = g.First().Nama,
                                Alamat = g.First().Alamat,
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
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataKetetapanPbb = context.DbMonPbbs
                        //    .Where(x => x.TglKetetapan.HasValue && x.TahunPajakKetetapan == tahun && x.MasaPajakKetetapan == bulan)
                        //    .GroupBy(x => x.Nop)
                        //    .Select(g => new PenetapanOP
                        //    {
                        //        Nop = g.Key,
                        //        NoPenetapan = string.Concat(g.First().Nop, "-", g.First().MasaPajakKetetapan, "-", g.First().TahunPajakKetetapan),
                        //        NamaWP = g.First().NamaOp,
                        //        Alamat = g.First().AlamatOp,
                        //        NilaiPenetapan = g.First().PokokPajakKetetapan ?? 0,
                        //        MasaPajak = new DateTime(
                        //            (int)(g.First().TahunPajakKetetapan),
                        //            (int)(g.First().MasaPajakKetetapan), 1
                        //        ).ToString("MMMM", new CultureInfo("id-ID")),
                        //        Status = g.First().NominalPokokBayar.HasValue
                        //            ? (g.First().NominalPokokBayar.Value > 0 ? "Sudah Dibayar" : "Belum Dibayar")
                        //            : "Belum Ada Pembayaran"
                        //    })
                        //    .ToList();
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
        }
        public class PenetapanOP
        {
            public string Nop { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
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
        }
    }
}
