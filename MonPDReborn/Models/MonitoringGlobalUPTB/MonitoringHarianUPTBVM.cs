using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using static MonPDReborn.Models.DashboardVM.ViewModel;
using static MonPDReborn.Models.MonitoringGlobal.MonitoringHarianVM;

namespace MonPDReborn.Models.MonitoringGlobalUPTB
{
    public class MonitoringHarianUPTBVM
    {
        public class Index
        {
            public int SelectedPajak { get; set; }
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
            public List<SelectListItem> JenisPajakList { get; set; } = new();
            public List<SelectListItem> BulanList { get; set; } = new();
            public List<SelectListItem> TahunList { get; set; } = new();
            public Index()
            {
                SelectedPajak = 0;
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
            public Index(EnumFactory.EPajak jenisPajak, int tahun, int bulan)
            {
                SelectedPajak = (int)jenisPajak;
                SelectedTahun = tahun;
                SelectedBulan = bulan;
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
            public List<MonitoringHarian> DataMonitoringHarianList { get; set; } = new();
            public Dashboard Data { get; set; } = new();

            // Constructor ini akan dipanggil oleh Controller dengan membawa nilai filter
            public Show(int jenisPajakId, int wilayah, int tahun, int bulan)
            {
                // 1. Ambil data list yang sudah difilter
                DataMonitoringHarianList = Method.GetMonitoringHarianList((EnumFactory.EPajak)jenisPajakId, wilayah, tahun, bulan);

                // 2. Hitung data summary dashboard DARI data yang sudah difilter di atas
                Data = Method.GetDashboardData(DataMonitoringHarianList, (EnumFactory.EPajak)jenisPajakId, wilayah, tahun, bulan);
            }
        }

        public class  Method
        {
            public static Dashboard GetDashboardData(List<MonitoringHarian> filteredData, EnumFactory.EPajak jenisPajak, int wilayah, int tahun, int bulan)
            {
                var dashboard = new Dashboard();
                if (filteredData.Any())
                {
                    dashboard.TotalTarget = filteredData.Sum(x => x.TargetHarian);
                    dashboard.TotalRealisasi = filteredData.Sum(x => x.Realisasi);
                    dashboard.Pencapaian = (dashboard.TotalTarget > 0) ? (double)(dashboard.TotalRealisasi / dashboard.TotalTarget) * 100 : 0;
                }
                return dashboard;
            }

            public static List<MonitoringHarian> GetMonitoringHarianList(EnumFactory.EPajak jenisPajak, int wilayah, int tahun, int bulan)
            {
                var ret = new List<MonitoringHarian>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTargetRestoPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListResto = context.DbOpRestos
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListResto.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetRestoPerBulan)
                        {
                            var totalRealisasi = realisasiRestoPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataTargetPPJPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListPPJ = context.DbOpListriks
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiPPJPerBulan = context.DbMonPpjs
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListPPJ.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetPPJPerBulan)
                        {
                            var totalRealisasi = realisasiPPJPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataTargetHotelPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHotel = context.DbOpHotels
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListHotel.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetHotelPerBulan)
                        {
                            var totalRealisasi = realisasiHotelPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataTargetParkirPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListParkir = context.DbOpParkirs
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListParkir.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetParkirPerBulan)
                        {
                            var totalRealisasi = realisasiParkirPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHiburan = context.DbOpHiburans
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListHiburan.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetHiburanPerBulan)
                        {
                            var totalRealisasi = realisasiHiburanPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataTargetAbtPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListAbt = context.DbOpAbts
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month == bulan
                                        && nopListAbt.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetAbtPerBulan)
                        {
                            var totalRealisasi = realisasiAbtPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi ?? 0
                            };

                            ret.Add(result);
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataTargetPbbPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListPbb = context.DbMonPbbs
                                .Where(x => x.TahunBuku == tahun && x.Uptb == wilayah)
                                .Select(x => x.Nop)
                                .Distinct()
                                .AsQueryable();

                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && nopListPbb.Contains(x.Nop))
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayar, PajakId = 9 })
                            .Select(g => new
                            {
                                g.Key.Nop,
                                g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .ToList();

                        foreach (var item in dataTargetPbbPerBulan)
                        {
                            var totalRealisasi = realisasiPbbPerBulan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi
                            };

                            ret.Add(result);
                        }
                        break;
                    default:
                        var dataTargetPerhari = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.Uptb == (int)wilayah && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21)
                                .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Tgl = g.Key.Tgl,
                                    Bulan = g.Key.Bulan,
                                    Tahun = g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        var realisasiPerHari = new List<(string Nop, DateTime Tgl, decimal Realisasi, int pajakId)>();

                        var nopListSemuaAbt = context.DbOpAbts
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaResto = context.DbOpRestos
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHotel = context.DbOpHotels
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaListrik = context.DbOpListriks
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaParkir = context.DbOpParkirs
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHiburan = context.DbOpHiburans
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaPbb = context.DbMonPbbs
                            .Where(x => x.TahunBuku == tahun && x.Uptb == wilayah)
                            .Select(x => x.Nop)
                            .Distinct()
                            .AsQueryable();

                        realisasiPerHari.AddRange(
                            context.DbMonRestos
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaResto.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.MakananMinuman
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonAbts
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaAbt.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.AirTanah
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonHotels
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaHotel.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.JasaPerhotelan
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonPpjs
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaListrik.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.TenagaListrik
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonParkirs
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaParkir.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.JasaParkir
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonHiburans
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan &&
                                nopListSemuaHiburan.Contains(x.Nop)
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0,
                                (int)EnumFactory.EPajak.JasaKesenianHiburan
                            ))
                            .ToList()
                        );

                        realisasiPerHari.AddRange(
                            context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && nopListSemuaPbb.Contains(x.Nop))
                            .GroupBy(x => new { x.Nop, TglBayar = x.TglBayar })
                            .Select(x => new ValueTuple<string, DateTime, decimal, int>(
                                x.Key.Nop,
                                x.Key.TglBayar.Value,
                                x.Sum(q => q.JumlahBayarPokok) ?? 0,
                                (int)EnumFactory.EPajak.PBB
                            ))
                            .ToList()
                        );

                        foreach (var item in dataTargetPerhari)
                        {
                            var totalRealisasi = realisasiPerHari
                                .Where(x => x.Tgl.Month == item.Bulan && x.Tgl.Day == item.Tgl && x.Tgl.Year == tahun && x.pajakId == item.PajakId)
                                .Sum(x => x.Realisasi);

                            MonitoringHarian result = new MonitoringHarian
                            {
                                Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                                JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                TargetHarian = item.TotalTarget,
                                Realisasi = totalRealisasi
                            };

                            ret.Add(result);
                        }
                        break;
                }

                return ret;
            }
        }

        public class Dashboard
        {
            public decimal TotalTarget { get; set; }
            public decimal TotalRealisasi { get; set; }
            public double Pencapaian { get; set; }
        }

        public class MonitoringHarian
        {
            public DateTime Tanggal { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string namaHari => Tanggal.ToString("dddd", new CultureInfo("id-ID"));
            public decimal TargetHarian { get; set; }
            public decimal Realisasi { get; set; }
            public double Pencapaian => TargetHarian > 0 ? Math.Round((double)(Realisasi / TargetHarian) * 100, 2) : 0.0;

            public string Status
            {
                get
                {
                    if (Pencapaian < 50)
                        return "Belum Tercapai";
                    else if (Pencapaian <= 80)
                        return "Dibawah Target";
                    else
                        return "Tercapai";
                }
            }
        }
    }
}
