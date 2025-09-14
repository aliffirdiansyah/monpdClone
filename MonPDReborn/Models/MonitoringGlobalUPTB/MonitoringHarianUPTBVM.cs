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
                    //.Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
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
                    //.Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
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
                            .Where(x => 
                                x.TahunBuku == tahun && 
                                x.WilayahPajak == ((int)wilayah).ToString() && 
                                (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
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
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        break;
                    case EnumFactory.EPajak.PBB:
                        break;
                    default:
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
