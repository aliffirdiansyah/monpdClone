using DevExpress.CodeParser;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using static MonPDReborn.Models.DashboardVM.ViewModel;


namespace MonPDReborn.Models.MonitoringGlobalUPTB
{
    public class MonitoringBulananUPTBVM
    {
        public class Index
        {
            public int SelectedPajak { get; set; }
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public List<SelectListItem> JenisPajakList { get; set; } = new();
            public List<SelectListItem> TahunList { get; set; } = new();
            public Index()
            {
                JenisPajakList.Add(new SelectListItem { Value = "0", Text = "Semua Jenis Pajak" });
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                for (int i = 2025; i >= 2021; i--)
                {
                    TahunList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }
            public Index(EnumFactory.EPajak jenisPajak)
            {
                SelectedPajak = (int)jenisPajak;
                JenisPajakList.Add(new SelectListItem { Value = "0", Text = "Semua Jenis Pajak" });
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
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
            public List<MonitoringBulananViewModels.BulananPajak> BulananPajakList { get; set; } = new();
            public List<MonitoringBulananViewModels.BulananPajak> AkumulasiBulananPajakList { get; set; } = new();
            public MonitoringBulananViewModels.DataRekapBulanan Data { get; set; } = new();
            public Show(EnumFactory.EPajak jenisPajak, int wilayah, int tahun)
            {
                BulananPajakList = Method.GetDataBulananPajak(jenisPajak, wilayah, tahun);
                AkumulasiBulananPajakList = Method.GetBulananPajakAkumulasi(jenisPajak, wilayah, tahun);
                if ((int)jenisPajak != 0)
                {
                    Data.NamaJenisPajak = jenisPajak.GetDescription();
                }
                Data.Target = BulananPajakList.Sum(x => x.AkpTarget);
                Data.Realisasi = BulananPajakList.Sum(x => x.Realisasi);
                Data.Tahun = tahun;
            }
        }

        public class Method
        {
            public static List<MonitoringBulananViewModels.BulananPajak> GetDataBulananPajak(EnumFactory.EPajak jenisPajak, int wilayah, int tahun)
            {
                var ret = new List<MonitoringBulananViewModels.BulananPajak>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTargetRestoPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListResto = context.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListResto.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetRestoPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiRestoPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataTargetPPJPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListListrik = context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                                .Select(x => x.Nop).Distinct().ToList();

                        var realisasiListrikPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListListrik.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetPPJPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiListrikPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataTargetHotelPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHotel = context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                                    .Select(x => x.Nop).Distinct().ToList();

                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListHotel.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetHotelPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHotelPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataTargetParkirPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListParkir = context.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListParkir.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetParkirPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiParkirPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHiburan = context.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListHiburan.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetHiburanPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHiburanPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataTargetAbtPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListAbt = context.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListAbt.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetAbtPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiAbtPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataTargetPbbPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TahunBuku == tahun
                                        && x.JumlahBayarPokok > 0
                                        && x.Uptb == Convert.ToInt32(wilayah))
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        foreach (var item in dataTargetPbbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPbbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    default:
                        var dataTargetPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Uptb == (int)wilayah && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        var realisasiPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        var nopListSemuaAbt = context.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaResto = context.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHotel = context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaListrik = context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaParkir = context.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHiburan = context.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        realisasiPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaResto.Contains(x.Nop)
                                        )
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaListrik.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaHotel.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaAbt.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaParkir.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaHiburan.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonPbbs
                                .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TahunBuku == tahun
                                            && x.JumlahBayarPokok > 0
                                            && x.Uptb == Convert.ToInt32(wilayah))
                                .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayar,
                                    g.Sum(x => x.JumlahBayarPokok) ?? 0
                                ))
                                .ToList()
                        );

                        foreach (var item in dataTargetPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = item.TotalTarget,
                                Realisasi = totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                }

                return ret.OrderBy(x => x.Bulan).ToList();
            }
            public static List<MonitoringBulananViewModels.BulananPajak> GetBulananPajakAkumulasi(EnumFactory.EPajak jenisPajak, int wilayah, int tahun)
            {
                var ret = new List<MonitoringBulananViewModels.BulananPajak>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTargetRestoPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListResto = context.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListResto.Contains(x.Nop)
                                    )
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetResto = 0;
                        decimal akumulasiRealisasiResto = 0;

                        foreach (var item in dataTargetRestoPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiRestoPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetResto += item.TotalTarget,
                                Realisasi = akumulasiRealisasiResto += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataTargetPPJPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListListrik = context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                                .Select(x => x.Nop).Distinct().ToList();

                        var realisasiListrikPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListListrik.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetPPJ = 0;
                        decimal akumulasiRealisasiPPJ = 0;

                        foreach (var item in dataTargetPPJPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiListrikPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetPPJ += item.TotalTarget,
                                Realisasi = akumulasiRealisasiPPJ += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataTargetHotelPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHotel = context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                                    .Select(x => x.Nop).Distinct().ToList();

                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListHotel.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetHotel = 0;
                        decimal akumulasiRealisasiHotel = 0;

                        foreach (var item in dataTargetHotelPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHotelPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetHotel += item.TotalTarget,
                                Realisasi = akumulasiRealisasiHotel += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataTargetParkirPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListParkir = context.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListParkir.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetParkir = 0;
                        decimal akumulasiRealisasiParkir = 0;

                        foreach (var item in dataTargetParkirPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiParkirPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetParkir += item.TotalTarget,
                                Realisasi = akumulasiRealisasiParkir += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListHiburan = context.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListHiburan.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetHiburan = 0;
                        decimal akumulasiRealisasiHiburan = 0;

                        foreach (var item in dataTargetHiburanPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHiburanPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetHiburan += item.TotalTarget,
                                Realisasi = akumulasiRealisasiHiburan += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataTargetAbtPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var nopListAbt = context.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopListAbt.Contains(x.Nop))
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetAbt = 0;
                        decimal akumulasiRealisasiAbt = 0;

                        foreach (var item in dataTargetAbtPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiAbtPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetAbt += item.TotalTarget,
                                Realisasi = akumulasiRealisasiAbt += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataTargetPbbPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                });

                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TahunBuku == tahun
                                        && x.JumlahBayarPokok > 0
                                        && x.Uptb == Convert.ToInt32(wilayah))
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        decimal akumulasiTargetPbb = 0;
                        decimal akumulasiRealisasiPbb = 0;

                        foreach (var item in dataTargetPbbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPbbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetPbb += item.TotalTarget,
                                Realisasi = akumulasiRealisasiPbb += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    default:
                        var dataTargetPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Uptb == (int)wilayah && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        var realisasiPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        var nopListSemuaAbt = context.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaResto = context.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHotel = context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaListrik = context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaParkir = context.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();
                        var nopListSemuaHiburan = context.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop).Distinct().ToList();

                        realisasiPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaResto.Contains(x.Nop)
                                        )
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaListrik.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaHotel.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaAbt.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaParkir.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && nopListSemuaHiburan.Contains(x.Nop))
                                .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayarPokok,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );

                        realisasiPerBulan.AddRange(
                            context.DbMonPbbs
                                .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TahunBuku == tahun
                                            && x.JumlahBayarPokok > 0
                                            && x.Uptb == Convert.ToInt32(wilayah))
                                .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayar,
                                    g.Sum(x => x.JumlahBayarPokok) ?? 0
                                ))
                                .ToList()
                        );

                        decimal akumulasiTarget = 0;
                        decimal akumulasiRealisasi = 0;

                        foreach (var item in dataTargetPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTarget += item.TotalTarget,
                                Realisasi = akumulasiRealisasi += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                }

                return ret.OrderBy(x => x.Bulan).ToList();
            }
        }
        public class MonitoringBulananViewModels
        {
            public class BulananPajak
            {
                public int EnumPajak { get; set; }
                public string JenisPajak { get; set; } = null!;
                public int Tahun { get; set; }
                public int Bulan { get; set; }
                public string BulanNama { get; set; } = null!;
                public decimal AkpTarget { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Pencapaian { get; set; }
                public decimal Selisih => Realisasi - AkpTarget;
            }

            public class DataRekapBulanan
            {
                public int Tahun { get; set; }
                public string NamaJenisPajak { get; set; } = "Semua Pajak";

                public decimal Target { get; set; }
                public decimal Realisasi { get; set; }

                public double Persentase => Target > 0
                    ? Math.Round((double)(Realisasi / Target * 100), 2)
                    : 0.0;

                public string PersentaseDisplay => $"{Persentase.ToString("0.##")}%";

                public string RealisasiDisplay => $"Rp. {Realisasi:N0}";
                public string TargetDisplay => $"Rp. {Target:N0}";
            }
        }
    }
}
