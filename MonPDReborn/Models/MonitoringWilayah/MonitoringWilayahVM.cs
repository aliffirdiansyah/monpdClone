

using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MonPDReborn.Models.MonitoringWilayah
{
    public class MonitoringWilayahVM
    {
        public class Index
        {
            public int SelectedUPTB { get; set; }
            public int SelectedPajak { get; set; }
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
            public List<SelectListItem> JenisUptbList { get; set; } = new();
            public List<SelectListItem> JenisPajakList { get; set; } = new();
            public List<SelectListItem> BulanList { get; set; } = new();
            public List<SelectListItem> TahunList { get; set; } = new();
            public Index()
            {
                JenisUptbList = Enum.GetValues(typeof(EnumFactory.EUPTB))
                    .Cast<EnumFactory.EUPTB>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();

                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                for (int i = 1; i <= 12; i++)
                {
                    var namaBulan = new DateTime(1, i, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    BulanList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = namaBulan
                    });
                }
                for (int i = 2025; i >= 2021; i--)
                {
                    TahunList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }

        }
        public class Show
        {
            public List<RealisasiWilayah> RealisasiWilayahList { get; set; } = new();
            public List<RealisasiJenis> RealisasiJenisList { get; set; } = new();
            public decimal TotalTarget { get; set; } = 0;
            public decimal TotalRealisasi { get; set; } = 0;
            public decimal PersenTotal { get; set; } = 0;
            public decimal TotalPencapaianHarian { get; set; } = 0;
            public decimal TotalWajibPajak { get; set; } = 0;

            public Show() { }


            public Show(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                RealisasiWilayahList = Method.GetDataRealisasiWilayahList(wilayah, tahun, bulan, jenisPajak);
                RealisasiJenisList = Method.GetDataRealisasiJenisList(wilayah, tahun, bulan, jenisPajak);

                TotalTarget = RealisasiWilayahList.Sum(x => x.Target);
                TotalRealisasi = RealisasiWilayahList.Sum(x => x.Realisasi);
                if (TotalTarget > 0)
                {
                    PersenTotal = Math.Round((TotalRealisasi / TotalTarget) * 100, 2);
                }
                else
                {
                    PersenTotal = 0;
                }
                TotalPencapaianHarian = Method.TotalRealisasiPencapaianHarianPerHariIni(wilayah, tahun, bulan, jenisPajak);
                TotalWajibPajak = RealisasiJenisList.Sum(x => x.JmlWP);
            }
        }

        public class Detail
        {
            public List<DataHarian> DataHarianList { get; set; } = new();

            public Detail() { }

            public Detail(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                DataHarianList = Method.GetDataDataHarianList(wilayah, tahun, bulan, jenisPajak);
            }
        }

        public class DetailModal
        {
            public DataModal DataModalRow { get; set; } = new();
            public DetailModal() { }
            public DetailModal(DateTime tanggal, EnumFactory.EUPTB wilayah, EnumFactory.EPajak jenisPajak)
            {
                DataModalRow = Method.GetDataModalRow(wilayah, tanggal, jenisPajak);
            }
        }
        public class Method
        {
            public static List<RealisasiWilayah> GetDataRealisasiWilayahList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<RealisasiWilayah>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataRestoWilayah = context.DbOpRestos
                             .Where(x => x.TahunBuku == tahun)
                             .Select(x => new
                             {
                                 x.Nop,
                                 x.WilayahPajak
                             })
                             .ToList()
                             .Select(x => new
                             {
                                 x.Nop,
                                 Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                             })
                             .ToList();
                        var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahResto)
                            {

                                var nopUptb = dataRestoWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataRestoWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            var targetPerUptb = dataTargetWilayahResto
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrikWilayah = context.DbOpListriks
                             .Where(x => x.TahunBuku == tahun)
                             .Select(x => new
                             {
                                 x.Nop,
                                 x.WilayahPajak
                             })
                             .ToList()
                             .Select(x => new
                             {
                                 x.Nop,
                                 Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                             })
                             .ToList();
                        var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            foreach (var item in dataTargetWilayahListrik)
                            {

                                var nopUptb = dataListrikWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataListrikWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var targetPerUptb = dataTargetWilayahListrik
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotelWilayah = context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new
                            {
                                x.Nop,
                                x.WilayahPajak
                            })
                            .ToList()
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                            })
                            .ToList();
                        var dataTargetWilayahHotel = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            foreach (var item in dataTargetWilayahHotel)
                            {

                                var nopUptb = dataHotelWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataHotelWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();
                            var targetPerUptb = dataTargetWilayahHotel
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkirWilayah = context.DbOpParkirs
                             .Where(x => x.TahunBuku == tahun)
                             .Select(x => new
                             {
                                 x.Nop,
                                 x.WilayahPajak
                             })
                             .ToList()
                             .Select(x => new
                             {
                                 x.Nop,
                                 Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                             })
                             .ToList();
                        var dataTargetWilayahParkir = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahParkir)
                            {

                                var nopUptb = dataParkirWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataParkirWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();
                            var targetPerUptb = dataTargetWilayahParkir
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburanWilayah = context.DbOpHiburans
                             .Where(x => x.TahunBuku == tahun)
                             .Select(x => new
                             {
                                 x.Nop,
                                 x.WilayahPajak
                             })
                             .ToList()
                             .Select(x => new
                             {
                                 x.Nop,
                                 Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                             })
                             .ToList();
                        var dataTargetWilayahHiburan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            foreach (var item in dataTargetWilayahHiburan)
                            {

                                var nopUptb = dataHiburanWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataHiburanWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var targetPerUptb = dataTargetWilayahHiburan
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbtWilayah = context.DbOpAbts
                             .Where(x => x.TahunBuku == tahun)
                             .Select(x => new
                             {
                                 x.Nop,
                                 x.WilayahPajak
                             })
                             .ToList()
                             .Select(x => new
                             {
                                 x.Nop,
                                 Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                             })
                             .ToList();
                        var dataTargetWilayahAbt = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            foreach (var item in dataTargetWilayahAbt)
                            {

                                var nopUptb = dataAbtWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                                    .Select(w => w.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }


                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var uptb = dataAbtWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var targetPerUptb = dataTargetWilayahAbt
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            foreach (var item in targetPerUptb)
                            {
                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.Realisasi);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {(int)item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataPbbWilayah = context.DbOpPbbs
                        //     .Where(x => x.TahunBuku == tahun)
                        //     .Select(x => new
                        //     {
                        //         x.Nop,
                        //         x.WilayahPajak
                        //     })
                        //     .ToList()
                        //     .Select(x => new
                        //     {
                        //         x.Nop,
                        //         Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                        //     })
                        //     .ToList();
                        //var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                        //        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                        //        .GroupBy(x => new { x.Uptb })
                        //        .Select(g => new
                        //        {
                        //            Uptb = g.Key.Uptb,
                        //            TotalTarget = g.Sum(x => x.Target)
                        //        })
                        //        .ToList();

                        //if (wilayah == EnumFactory.EUPTB.SEMUA)
                        //{
                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();
                        //    foreach (var item in dataTargetWilayahPbb)
                        //    {

                        //        var nopUptb = dataPbbWilayah
                        //            .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb)
                        //            .Select(w => w.Nop)
                        //            .ToList();

                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                        //            .Sum(x => x.Realisasi);

                        //        ret.Add(new RealisasiWilayah
                        //        {
                        //            Wilayah = $"UPTB {(int)item.Uptb}",
                        //            Tahun = tahun,
                        //            Lokasi = $"UPTB {(int)item.Uptb}",
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0,
                        //            Tren = 0,
                        //            Status = ""
                        //        });
                        //    }


                        //}
                        //else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        //{
                        //    var uptb = dataPbbWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan &&
                        //            uptb.Contains(x.Nop)
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();
                        //    var targetPerUptb = dataTargetWilayahPbb
                        //        .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                        //        .ToList();

                        //    foreach (var item in targetPerUptb)
                        //    {
                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == bulan)
                        //            .Sum(x => x.Realisasi);

                        //        ret.Add(new RealisasiWilayah
                        //        {
                        //            Wilayah = $"UPTB {(int)item.Uptb}",
                        //            Tahun = tahun,
                        //            Lokasi = $"UPTB {(int)item.Uptb}",
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0,
                        //            Tren = 0,
                        //            Status = ""
                        //        });
                        //    }
                        //}

                        break;
                    case EnumFactory.EPajak.BPHTB:
                        
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        

                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default: // ALL PAJAK
                             // Ambil target semua jenis pajak, dikelompokkan per UPTB
                        var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan)
                            .GroupBy(x => x.Uptb)
                            .Select(g => new
                            {
                                Uptb = g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();

                        var dataWilayahGabungan = new List<(string Nop, string Wilayah)>();

                        dataWilayahGabungan.AddRange(
                            context.DbOpRestos
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHotels
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpParkirs
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpListriks
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHiburans
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpAbts
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

                        //dataWilayahGabungan.AddRange(
                        //    context.DbOpPbbs
                        //        .Where(x => x.TahunBuku == tahun)
                        //        .Select(x => new ValueTuple<string, string>(
                        //            x.Nop,
                        //            Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                        //        ))
                        //        .ToList()
                        //);

                        var dataRealisasiGabungan = new List<(string Nop, DateTime? TglBayarPokok, decimal NominalPokokBayar)>();

                        // RESTORAN
                        dataRealisasiGabungan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // HOTEL
                        dataRealisasiGabungan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // PARKIR
                        dataRealisasiGabungan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // LISTRIK (PPJ)
                        dataRealisasiGabungan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // HIBURAN
                        dataRealisasiGabungan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // ABT
                        dataRealisasiGabungan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // PBB
                        //dataRealisasiGabungan.AddRange(
                        //    context.DbMonPbbs
                        //        .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                        //        .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                        //        .ToList()
                        //);

                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            foreach (var item in dataTargetWilayah)
                            {
                                var daftarNop = dataWilayahGabungan
                                    .Where(x => Convert.ToInt32(x.Wilayah) == item.Uptb)
                                    .Select(x => x.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiGabungan
                                    .Where(x => daftarNop.Contains(x.Nop) && x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {item.Uptb}",
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    Lokasi = $"UPTB {item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else
                        {
                            foreach (var item in dataTargetWilayah.Where(x => x.Uptb == (int)wilayah))
                            {
                                var daftarNop = dataWilayahGabungan
                                    .Where(x => Convert.ToInt32(x.Wilayah) == item.Uptb)
                                    .Select(x => x.Nop)
                                    .ToList();

                                var totalRealisasi = dataRealisasiGabungan
                                    .Where(x => daftarNop.Contains(x.Nop) && x.TglBayarPokok.Value.Month == bulan)
                                    .Sum(x => x.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {item.Uptb}",
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    Lokasi = $"UPTB {item.Uptb}",
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }

                        break;

                }

                return ret;
            }
            public static List<RealisasiJenis> GetDataRealisasiJenisList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<RealisasiJenis>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataRestoWilayah = context.DbOpRestos
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahResto)
                            {

                                var nopUptb = dataRestoWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataRestoWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahResto.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataRestoWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrikWilayah = context.DbOpListriks
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahListrik)
                            {

                                var nopUptb = dataListrikWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataListrikWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahListrik.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataListrikWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotelWilayah = context.DbOpHotels
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahHotel = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHotel)
                            {

                                var nopUptb = dataHotelWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataHotelWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHotel.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataHotelWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkirWilayah = context.DbOpParkirs
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahParkir = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahParkir)
                            {

                                var nopUptb = dataParkirWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataParkirWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahParkir.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataParkirWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburanWilayah = context.DbOpHiburans
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahHiburan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHiburan)
                            {

                                var nopUptb = dataHiburanWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataHiburanWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHiburan.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataHiburanWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbtWilayah = context.DbOpAbts
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahAbt = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahAbt)
                            {

                                var nopUptb = dataAbtWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataAbtWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahAbt.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataAbtWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                RealisasiJenis result = new RealisasiJenis
                                {
                                    Wilayah = item.Uptb.ToString(),
                                    Tahun = tahun,
                                    Bulan = bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    JmlWP = nopUptb.Count(),
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "Status here"
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataPbbWilayah = context.DbOpPbbs
                        // .Where(x => x.TahunBuku == tahun)
                        // .Select(x => new
                        // {
                        //     x.Nop,
                        //     x.WilayahPajak,
                        //     PajakId = 9m
                        // })
                        // .ToList()
                        // .Select(x => new
                        // {
                        //     x.Nop,
                        //     Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                        //     x.PajakId
                        // })
                        // .ToList();


                        //var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                        //        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                        //        .GroupBy(x => new { x.Uptb, x.PajakId })
                        //        .Select(g => new
                        //        {
                        //            Uptb = g.Key.Uptb,
                        //            PajakId = g.Key.PajakId,
                        //            TotalTarget = g.Sum(x => x.Target)
                        //        })
                        //        .ToList();


                        //if (wilayah == EnumFactory.EUPTB.SEMUA)
                        //{

                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            x.Key.PajakId,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();


                        //    foreach (var item in dataTargetWilayahPbb)
                        //    {

                        //        var nopUptb = dataPbbWilayah
                        //            .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                        //            .Select(w => w.Nop)
                        //            .ToList();


                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                        //            .Sum(x => x.Realisasi);


                        //        RealisasiJenis result = new RealisasiJenis
                        //        {
                        //            Wilayah = item.Uptb.ToString(),
                        //            Tahun = tahun,
                        //            Bulan = bulan,
                        //            JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                        //            JmlWP = nopUptb.Count(),
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0,
                        //            Tren = 0,
                        //            Status = "Status here"
                        //        };


                        //        ret.Add(result);
                        //    }
                        //}
                        //else
                        //{
                        //    var uptb = dataPbbWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan &&
                        //            uptb.Contains(x.Nop)
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            x.Key.PajakId,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();


                        //    foreach (var item in dataTargetWilayahPbb.Where(x => x.Uptb == (decimal)wilayah))
                        //    {

                        //        var nopUptb = dataPbbWilayah
                        //            .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                        //            .Select(w => w.Nop)
                        //            .ToList();


                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                        //            .Sum(x => x.Realisasi);


                        //        RealisasiJenis result = new RealisasiJenis
                        //        {
                        //            Wilayah = item.Uptb.ToString(),
                        //            Tahun = tahun,
                        //            Bulan = bulan,
                        //            JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                        //            JmlWP = nopUptb.Count(),
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0,
                        //            Tren = 0,
                        //            Status = "Status here"
                        //        };


                        //        ret.Add(result);
                        //    }
                        //}

                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default: // ALL PAJAK
                        var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                           .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan)
                           .GroupBy(x => new { x.Uptb, x.PajakId })
                           .Select(g => new
                           {
                               Uptb = g.Key.Uptb,
                               PajakId = g.Key.PajakId,
                               TotalTarget = g.Sum(x => x.Target)
                           })
                           .ToList();

                        var dataWilayahGabungan = new List<(string Nop, string Wilayah, decimal PajakId)>();

                        dataWilayahGabungan.AddRange(
                            context.DbOpRestos
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHotels
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpParkirs
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpListriks
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHiburans
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpAbts
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        //dataWilayahGabungan.AddRange(
                        //    context.DbOpPbbs
                        //        .Where(x => x.TahunBuku == tahun)
                        //        .Select(x => new
                        //        {
                        //            x.Nop,
                        //            Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                        //            PajakId = 9m // PBB
                        //        })
                        //        .ToList()
                        //        .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                        //        .ToList()
                        //);
                        // Gabungkan data realisasi
                        var dataRealisasiGabungan = new List<(string Nop, DateTime? TglBayarPokok, decimal NominalPokokBayar, decimal PajakId)>();

                        dataRealisasiGabungan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        //dataRealisasiGabungan.AddRange(
                        //    context.DbMonPbbs
                        //        .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                        //        .Select(x => new
                        //        {
                        //            x.Nop,
                        //            x.TglBayarPokok,
                        //            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                        //            x.PajakId
                        //        })
                        //        .ToList()
                        //        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        //        .ToList()
                        //);

                        foreach (var item in dataTargetWilayah)
                        {
                            var nopUptb = dataWilayahGabungan
                                .Where(x => Convert.ToInt32(x.Wilayah) == item.Uptb && x.PajakId == item.PajakId)
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var totalRealisasi = dataRealisasiGabungan
                                .Where(x => x.PajakId == item.PajakId && x.TglBayarPokok.Value.Month == bulan && nopUptb.Contains(x.Nop))
                                .Sum(x => x.NominalPokokBayar);

                            var jenisPajakDesc = ((EnumFactory.EPajak)item.PajakId).GetDescription();

                            ret.Add(new RealisasiJenis
                            {
                                Wilayah = item.Uptb.ToString(),
                                EnumWilayah = (int)item.Uptb,
                                Tahun = tahun,
                                Bulan = bulan,
                                JenisPajak = jenisPajakDesc,
                                JmlWP = nopUptb.Count,
                                Target = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Tren = 0, // Silakan isi jika ada tren historis
                                Status = "Normal" // Ganti dengan logika status jika ada
                            });
                        }
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            ret = ret
                                .GroupBy(x => new { x.JenisPajak })
                                .Select(x => new RealisasiJenis
                                {
                                    JenisPajak = x.Key.JenisPajak,
                                    JmlWP = x.Sum(q => q.JmlWP),
                                    Target = x.Sum(q => q.Target),
                                    Realisasi = x.Sum(q => q.Realisasi)
                                })
                                .ToList();
                        }
                        else
                        {
                            ret = ret
                                .Where(x => x.EnumWilayah == (int)wilayah)
                                .GroupBy(x => new { x.JenisPajak })
                                .Select(x => new RealisasiJenis
                                {
                                    JenisPajak = x.Key.JenisPajak,
                                    JmlWP = x.Sum(q => q.JmlWP),
                                    Target = x.Sum(q => q.Target),
                                    Realisasi = x.Sum(q => q.Realisasi)
                                })
                                .ToList();
                        }
                        break;

                }

                return ret;

            }
            public static List<DataHarian> GetDataDataHarianList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DataHarian>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataRestoWilayah = context.DbOpRestos
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahResto)
                            {

                                var nopUptb = dataRestoWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataRestoWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahResto.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataRestoWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrikWilayah = context.DbOpListriks
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahListrik)
                            {

                                var nopUptb = dataListrikWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataListrikWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahListrik.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataListrikWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotelWilayah = context.DbOpHotels
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahHotel = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHotel)
                            {

                                var nopUptb = dataHotelWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataHotelWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHotel.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataHotelWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkirWilayah = context.DbOpParkirs
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahParkir = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahParkir)
                            {

                                var nopUptb = dataParkirWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataParkirWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahParkir.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataParkirWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburanWilayah = context.DbOpHiburans
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahHiburan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHiburan)
                            {

                                var nopUptb = dataHiburanWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataHiburanWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahHiburan.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataHiburanWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbtWilayah = context.DbOpAbts
                         .Where(x => x.TahunBuku == tahun)
                         .Select(x => new
                         {
                             x.Nop,
                             x.WilayahPajak,
                             x.PajakId
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })
                         .ToList();


                        var dataTargetWilayahAbt = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {

                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahAbt)
                            {

                                var nopUptb = dataAbtWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }
                        else
                        {
                            var uptb = dataAbtWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();


                            foreach (var item in dataTargetWilayahAbt.Where(x => x.Uptb == (decimal)wilayah))
                            {

                                var nopUptb = dataAbtWilayah
                                    .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                                    .Select(w => w.Nop)
                                    .ToList();


                                var totalRealisasi = dataRealisasiWilayah
                                    .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                                    .Sum(x => x.Realisasi);


                                DataHarian result = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)item.Uptb}",
                                    EnumWilayah = (int)item.Uptb,
                                    Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                    Tahun = (int)item.Bulan,
                                    Bulan = (int)item.Tahun,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget,
                                    Realisasi = totalRealisasi ?? 0
                                };


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataPbbWilayah = context.DbOpPbbs
                        // .Where(x => x.TahunBuku == tahun)
                        // .Select(x => new
                        // {
                        //     x.Nop,
                        //     x.WilayahPajak,
                        //     PajakId = 9m
                        // })
                        // .ToList()
                        // .Select(x => new
                        // {
                        //     x.Nop,
                        //     Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                        //     x.PajakId
                        // })
                        // .ToList();


                        //var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                        //        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak)
                        //        .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                        //        .Select(g => new
                        //        {
                        //            Uptb = g.Key.Uptb,
                        //            Tgl = g.Key.Tgl,
                        //            Bulan = g.Key.Bulan,
                        //            Tahun = g.Key.TahunBuku,
                        //            PajakId = g.Key.PajakId,
                        //            TotalTarget = g.Sum(x => x.Target)
                        //        })
                        //        .ToList();


                        //if (wilayah == EnumFactory.EUPTB.SEMUA)
                        //{

                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            x.Key.PajakId,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();


                        //    foreach (var item in dataTargetWilayahPbb)
                        //    {

                        //        var nopUptb = dataPbbWilayah
                        //            .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                        //            .Select(w => w.Nop)
                        //            .ToList();


                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                        //            .Sum(x => x.Realisasi);


                        //        DataHarian result = new DataHarian
                        //        {
                        //            Wilayah = $"UPTB {(int)item.Uptb}",
                        //            EnumWilayah = (int)item.Uptb,
                        //            Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                        //            Tahun = (int)item.Bulan,
                        //            Bulan = (int)item.Tahun,
                        //            JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                        //            EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0
                        //        };


                        //        ret.Add(result);
                        //    }
                        //}
                        //else
                        //{
                        //    var uptb = dataPbbWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                        //    var dataRealisasiWilayah = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value.Year == tahun &&
                        //            x.TglBayarPokok.Value.Month <= bulan &&
                        //            uptb.Contains(x.Nop)
                        //        )
                        //        .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                        //        .Select(x => new
                        //        {
                        //            x.Key.Nop,
                        //            x.Key.TglBayarPokok,
                        //            x.Key.PajakId,
                        //            Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //        })
                        //        .ToList();


                        //    foreach (var item in dataTargetWilayahPbb.Where(x => x.Uptb == (decimal)wilayah))
                        //    {

                        //        var nopUptb = dataPbbWilayah
                        //            .Where(w => Convert.ToInt32(w.Wilayah) == (int)item.Uptb && w.PajakId == item.PajakId)
                        //            .Select(w => w.Nop)
                        //            .ToList();


                        //        var totalRealisasi = dataRealisasiWilayah
                        //            .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == item.Tahun && nopUptb.Contains(x.Nop) && x.PajakId == item.PajakId)
                        //            .Sum(x => x.Realisasi);


                        //        DataHarian result = new DataHarian
                        //        {
                        //            Wilayah = $"UPTB {(int)item.Uptb}",
                        //            EnumWilayah = (int)item.Uptb,
                        //            Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                        //            Tahun = (int)item.Bulan,
                        //            Bulan = (int)item.Tahun,
                        //            JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                        //            EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                        //            Target = item.TotalTarget,
                        //            Realisasi = totalRealisasi ?? 0
                        //        };


                        //        ret.Add(result);
                        //    }
                        //}

                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default: // ALL PAJAK
                        var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                           .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && (x.Uptb == (int)wilayah || wilayah == EnumFactory.EUPTB.SEMUA))
                           .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                            .Select(g => new
                            {
                                Uptb = g.Key.Uptb,
                                Tgl = g.Key.Tgl,
                                Bulan = g.Key.Bulan,
                                Tahun = g.Key.TahunBuku,
                                PajakId = g.Key.PajakId,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                           .ToList();

                        var dataWilayahGabungan = new List<(string Nop, string Wilayah, decimal PajakId)>();

                        dataWilayahGabungan.AddRange(
                            context.DbOpRestos
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHotels
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpParkirs
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpListriks
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpHiburans
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        dataWilayahGabungan.AddRange(
                            context.DbOpAbts
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new
                                {
                                    x.Nop,
                                    Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                                .ToList()
                        );

                        //dataWilayahGabungan.AddRange(
                        //    context.DbOpPbbs
                        //        .Where(x => x.TahunBuku == tahun)
                        //        .Select(x => new
                        //        {
                        //            x.Nop,
                        //            Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                        //            PajakId = 9m // PBB
                        //        })
                        //        .ToList()
                        //        .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                        //        .ToList()
                        //);
                        // Gabungkan data realisasi
                        var dataRealisasiGabungan = new List<(string Nop, DateTime? TglBayarPokok, decimal NominalPokokBayar, decimal PajakId)>();

                        dataRealisasiGabungan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        dataRealisasiGabungan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new
                                {
                                    x.Nop,
                                    x.TglBayarPokok,
                                    NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                    x.PajakId
                                })
                                .ToList()
                                .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                                .ToList()
                        );

                        //dataRealisasiGabungan.AddRange(
                        //    context.DbMonPbbs
                        //        .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                        //        .Select(x => new
                        //        {
                        //            x.Nop,
                        //            x.TglBayarPokok,
                        //            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                        //            x.PajakId
                        //        })
                        //        .ToList()
                        //        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        //        .ToList()
                        //);

                        if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            dataTargetWilayah = dataTargetWilayah.Where(x => x.Uptb == (int)wilayah).ToList();
                            dataWilayahGabungan = dataWilayahGabungan.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).ToList();
                        }

                        foreach (var item in dataTargetWilayah)
                        {
                            var nopUptb = dataWilayahGabungan
                                .Where(x => Convert.ToInt32(x.Wilayah) == item.Uptb && x.PajakId == item.PajakId)
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var totalRealisasi = dataRealisasiGabungan
                                .Where(x => x.PajakId == item.PajakId && x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && nopUptb.Contains(x.Nop))
                                .Sum(x => x.NominalPokokBayar);

                            var jenisPajakDesc = ((EnumFactory.EPajak)item.PajakId).GetDescription();

                            ret.Add(new DataHarian
                            {
                                Wilayah = $"UPTB {(int)item.Uptb}",
                                EnumWilayah = (int)item.Uptb,
                                EnumPajak = (int)item.PajakId,
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                Tahun = (int)item.Bulan,
                                Bulan = (int)item.Tahun,
                                JenisPajak = jenisPajakDesc,
                                Target = item.TotalTarget,
                                Realisasi = totalRealisasi
                            });

                        }
                        break;

                }
                return ret;
            }
            public static DataModal GetDataModalRow(EnumFactory.EUPTB wilayah, DateTime tanggal, EnumFactory.EPajak jenisPajak)
            {
                var ret = new DataModal();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataRestoWilayah = context.DbOpRestos
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        var dataRealisasiWilayahQuery = context.DbMonRestos
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        var filterWilayah = wilayah == EnumFactory.EUPTB.SEMUA
                            ? null
                            : (int?)wilayah;

                        // ambil semua NOP di wilayah & pajak yg sesuai
                        var nopsUptb = dataRestoWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // ambil realisasi detail untuk tanggal & pajak & NOP sesuai
                        var realisasiDetail = dataRealisasiWilayahQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptb.Contains(x.Nop)
                            )
                            .ToList();

                        // susun hasil akhir
                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetail.Select(r =>
                            {
                                var op = dataRestoWilayah
                                    .FirstOrDefault(p => p.Nop == r.Nop && p.PajakId == r.PajakId);

                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = op?.NamaOp ?? "-",
                                    AlamatOP = op?.AlamatOp ?? "-",
                                    KategoriNama = op?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            }).ToList()
                        };



                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrikWilayah = context.DbOpListriks
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        // Ambil data realisasi bayar listrik per NOP, tanggal, pajak
                        var dataRealisasiWilayahListrikQuery = context.DbMonPpjs
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        // filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbListrik = dataListrikWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // ambil detail realisasi + join ke master dataListrikWilayah
                        var realisasiDetailListrik = dataRealisasiWilayahListrikQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbListrik.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataListrikWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = master?.NamaOp ?? "-",
                                    AlamatOP = master?.AlamatOp ?? "-",
                                    KategoriNama = master?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            })
                            .ToList();

                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetailListrik
                        };

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotelWilayah = context.DbOpHotels
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        // Ambil data realisasi bayar hotel per NOP, tanggal, pajak
                        var dataRealisasiWilayahHotelQuery = context.DbMonHotels
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        // filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbHotel = dataHotelWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // ambil detail realisasi + join ke master dataHotelWilayah
                        var realisasiDetailHotel = dataRealisasiWilayahHotelQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbHotel.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataHotelWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = master?.NamaOp ?? "-",
                                    AlamatOP = master?.AlamatOp ?? "-",
                                    KategoriNama = master?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            })
                            .ToList();

                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetailHotel
                        };

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        // Ambil data master NOP parkir beserta Wilayah, PajakId, Nama & Alamat
                        var dataParkirWilayah = context.DbOpParkirs
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        // Ambil data realisasi bayar parkir per NOP, tanggal, pajak
                        var dataRealisasiWilayahParkirQuery = context.DbMonParkirs
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        // filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbParkir = dataParkirWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // ambil detail realisasi + join ke master dataParkirWilayah
                        var realisasiDetailParkir = dataRealisasiWilayahParkirQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbParkir.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataParkirWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = master?.NamaOp ?? "-",
                                    AlamatOP = master?.AlamatOp ?? "-",
                                    KategoriNama = master?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            })
                            .ToList();

                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetailParkir
                        };


                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburanWilayah = context.DbOpHiburans
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        // Ambil data realisasi bayar hiburan per NOP, tanggal, pajak
                        var dataRealisasiWilayahHiburanQuery = context.DbMonHiburans
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        // Filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbHiburan = dataHiburanWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // Ambil detail realisasi + join ke master dataHiburanWilayah
                        var realisasiDetailHiburan = dataRealisasiWilayahHiburanQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbHiburan.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataHiburanWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = master?.NamaOp ?? "-",
                                    AlamatOP = master?.AlamatOp ?? "-",
                                    KategoriNama = master?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            })
                            .ToList();

                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetailHiburan
                        };

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbtWilayah = context.DbOpAbts
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                                x.NamaOp,
                                x.AlamatOp,
                                x.KategoriNama,
                                x.PajakId
                            })
                            .ToList();

                        // Ambil data realisasi bayar ABT per NOP, tanggal, pajak
                        var dataRealisasiWilayahAbtQuery = context.DbMonAbts
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        // Filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbAbt = dataAbtWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // Ambil detail realisasi + join ke master dataAbtWilayah
                        var realisasiDetailAbt = dataRealisasiWilayahAbtQuery
                            .Where(x =>
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbAbt.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataAbtWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                                return new DataDetailModal
                                {
                                    NOP = r.Nop,
                                    NamaOP = master?.NamaOp ?? "-",
                                    AlamatOP = master?.AlamatOp ?? "-",
                                    KategoriNama = master?.KategoriNama ?? "-",
                                    Realisasi = r.Realisasi ?? 0
                                };
                            })
                            .ToList();

                        ret = new DataModal
                        {
                            Wilayah = $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Detail = realisasiDetailAbt
                        };

                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataPbbWilayah = context.DbOpPbbs
                        //    .Where(x => x.TahunBuku == tanggal.Year)
                        //    .Select(x => new
                        //    {
                        //        x.Nop,
                        //        Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                        //        NamaOp = "-",            // PBB biasanya tidak ada, default "-"
                        //        AlamatOp = "-",          // PBB biasanya tidak ada, default "-"
                        //        KategoriNama = "-",
                        //        PajakId = 9m
                        //    })
                        //    .ToList();

                        //// Ambil data realisasi bayar PBB per NOP, tanggal, pajak
                        //var dataRealisasiWilayahPbbQuery = context.DbMonPbbs
                        //    .Where(x =>
                        //        x.TahunBuku == tanggal.Year &&
                        //        x.TglBayarPokok.HasValue &&
                        //        x.TglBayarPokok.Value.Year == tanggal.Year &&
                        //        x.TglBayarPokok.Value.Month <= tanggal.Month
                        //    )
                        //    .GroupBy(x => new { x.Nop, x.TglBayarPokok, x.PajakId })
                        //    .Select(x => new
                        //    {
                        //        x.Key.Nop,
                        //        x.Key.TglBayarPokok,
                        //        x.Key.PajakId,
                        //        Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //    })
                        //    .ToList();

                        //// Filter NOP sesuai UPTB & jenis pajak
                        //var nopsUptbPbb = dataPbbWilayah
                        //    .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                        //    .Select(w => w.Nop)
                        //    .ToList();

                        //// Ambil detail realisasi + join ke master dataPbbWilayah
                        //var realisasiDetailPbb = dataRealisasiWilayahPbbQuery
                        //    .Where(x =>
                        //        x.TglBayarPokok.Value.Month == tanggal.Month &&
                        //        x.TglBayarPokok.Value.Day == tanggal.Day &&
                        //        x.TglBayarPokok.Value.Year == tanggal.Year &&
                        //        x.PajakId == (int)jenisPajak &&
                        //        nopsUptbPbb.Contains(x.Nop)
                        //    )
                        //    .Select(r =>
                        //    {
                        //        var master = dataPbbWilayah.FirstOrDefault(m => m.Nop == r.Nop);
                        //        return new DataDetailModal
                        //        {
                        //            NOP = r.Nop,
                        //            NamaOP = master?.NamaOp ?? "-",
                        //            AlamatOP = master?.AlamatOp ?? "-",
                        //            KategoriNama = master?.KategoriNama ?? "-",
                        //            Realisasi = r.Realisasi ?? 0
                        //        };
                        //    })
                        //    .ToList();

                        //ret = new DataModal
                        //{
                        //    Wilayah = $"UPTB {(int)wilayah}",
                        //    EnumWilayah = (int)wilayah,
                        //    Tanggal = tanggal,
                        //    Tahun = tanggal.Year,
                        //    Bulan = tanggal.Month,
                        //    JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                        //    Detail = realisasiDetailPbb
                        //};

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
            public static decimal TotalRealisasiPencapaianHarianPerHariIni(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                decimal result = 0;
                var tgl = DateTime.Now;
                var context = DBClass.GetContext();
                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        result = context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        result = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        result = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        result = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        result = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        result = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.Reklame:
                        result = context.DbMonReklames
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.PBB:
                        //result = context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value == DateTime.Now
                        //        ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        result = context.DbMonBphtbs
                                .Where(x =>
                                    x.Tahun == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value == DateTime.Now
                                ).Sum(q => q.Pokok) ?? 0;
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        result = context.DbMonOpsenPkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd == DateTime.Now
                                ).Sum(q => q.JmlPokok);
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        result = context.DbMonOpsenBbnkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd == DateTime.Now
                                ).Sum(q => q.JmlPokok);
                        break;
                    default:
                        result += context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonReklames
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value == DateTime.Now
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        //result += context.DbMonPbbs
                        //        .Where(x =>
                        //            x.TahunBuku == tahun &&
                        //            x.TglBayarPokok.HasValue &&
                        //            x.TglBayarPokok.Value == DateTime.Now
                        //        ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonBphtbs
                                .Where(x =>
                                    x.Tahun == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value == DateTime.Now
                                ).Sum(q => q.Pokok) ?? 0;
                        result += context.DbMonOpsenPkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd == DateTime.Now
                                ).Sum(q => q.JmlPokok);
                        result += context.DbMonOpsenBbnkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd == DateTime.Now
                                ).Sum(q => q.JmlPokok);

                        break;
                }

                return result;
            }

        }

        public class RealisasiWilayah
        {
            public string Wilayah { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }

            public string Lokasi { get; set; } = null!;
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Pencapaian => Target > 0 ? Math.Round((decimal)(Realisasi / Target) * 100, 2) : 0;
            public decimal Tren { get; set; }
            public string Status { get; set; } = null!;
        }

        public class RealisasiJenis
        {
            public string Wilayah { get; set; } = null!;
            public int EnumWilayah { get; set; }
            public int Tahun { get; set; }
            public int Bulan { get; set; }

            public string JenisPajak { get; set; } = null!;
            public int JmlWP { get; set; }
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public double Tren { get; set; }
            public string Status { get; set; } = null!;
        }

        public class DataHarian
        {
            public string Wilayah { get; set; } = null!;
            public int EnumWilayah { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public DateTime Tanggal { get; set; }
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public string Hari => Tanggal.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));
        }

        public class DataModal
        {
            public string Wilayah { get; set; } = null!;
            public int EnumWilayah { get; set; }
            public string JenisPajak { get; set; } = null!;
            public DateTime Tanggal { get; set; }
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public string Hari => Tanggal.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));
            public List<DataDetailModal> Detail { get; set; } = new();
        }

        public class DataDetailModal
        {
            public string NOP { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string KategoriNama { get; set; } = null!;
            public decimal Realisasi { get; set; }

        }

    }
}
