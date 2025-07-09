

using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using MonPDLib;
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

            public Show() { }


            public Show(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                RealisasiWilayahList = Method.GetDataRealisasiWilayahList(wilayah, tahun, bulan, jenisPajak);
                RealisasiJenisList = Method.GetDataRealisasiJenisList(wilayah, tahun, bulan, jenisPajak);
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
                        var dataPbbWilayah = context.DbOpPbbs
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
                        var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
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
                            var dataRealisasiWilayah = context.DbMonPbbs
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
                            foreach (var item in dataTargetWilayahPbb)
                            {

                                var nopUptb = dataPbbWilayah
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
                            var uptb = dataPbbWilayah.Where(x => Convert.ToInt32(x.Wilayah) == (int)wilayah).Select(x => x.Nop).ToList();
                            var dataRealisasiWilayah = context.DbMonPbbs
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
                            var targetPerUptb = dataTargetWilayahPbb
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

                        dataWilayahGabungan.AddRange(
                            context.DbOpPbbs
                                .Where(x => x.TahunBuku == tahun)
                                .Select(x => new ValueTuple<string, string>(
                                    x.Nop,
                                    Regex.Match(x.WilayahPajak ?? "", @"\d+").Value
                                ))
                                .ToList()
                        );

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
                        dataRealisasiGabungan.AddRange(
                            context.DbMonPbbs
                                .Where(x => x.TahunBuku == tahun && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= bulan)
                                .Select(x => new ValueTuple<string, DateTime?, decimal>(x.Nop, x.TglBayarPokok, x.NominalPokokBayar ?? 0))
                                .ToList()
                        );

                        // Hitung realisasi berdasarkan UPTB
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

                        break;

                }

                return ret;
            }

            public static List<RealisasiJenis> GetDataRealisasiJenisList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<RealisasiJenis>();

                return ret;

            }

            public static List<DataHarian> GetDataDataHarianList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DataHarian>();

                return ret;
            }

            //private static List<DataHarian> GetAllDataHarian()
            //{
            //    return new List<DataHarian>
            //    {
            //        new DataHarian { Wilayah = "UPTB 1", JenisPajak = "Pajak Hotel",     Tanggal = new DateTime(2025, 7, 8), Target = 1_000_000_000m, Realisasi = 850_000_000m },
            //        new DataHarian { Wilayah = "UPTB 2", JenisPajak = "Pajak Restoran", Tanggal = new DateTime(2025, 7, 8), Target = 800_000_000m,   Realisasi = 700_000_000m },
            //        new DataHarian { Wilayah = "UPTB 3", JenisPajak = "Pajak Hiburan",  Tanggal = new DateTime(2025, 7, 8), Target = 600_000_000m,   Realisasi = 450_000_000m },
            //        new DataHarian { Wilayah = "UPTB 4", JenisPajak = "Pajak Parkir",   Tanggal = new DateTime(2025, 7, 8), Target = 400_000_000m,   Realisasi = 380_000_000m },
            //        new DataHarian { Wilayah = "UPTB 5", JenisPajak = "Pajak Reklame",  Tanggal = new DateTime(2025, 7, 8), Target = 300_000_000m,   Realisasi = 250_000_000m },                
            //    };
            //}
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
            public DateTime Tanggal { get; set; }

            public int Tahun => Tanggal.Year;
            public int Bulan => Tanggal.Month;

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
            public string JenisPajak { get; set; } = null!;
            public DateTime Tanggal { get; set; }

            public int Tahun => Tanggal.Year;
            public int Bulan => Tanggal.Month;

            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => Target > 0 ? Math.Round((double)(Realisasi / Target) * 100, 2) : 0.0;
            public string Hari => Tanggal.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));
        }

    }
}
