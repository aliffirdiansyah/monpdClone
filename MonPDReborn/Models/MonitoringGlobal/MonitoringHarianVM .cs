using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using static MonPDReborn.Models.DashboardVM.ViewModel;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringHarianVM
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
            // Properti untuk menampung data yang akan ditampilkan
            public List<MonitoringHarian> DataMonitoringHarianList { get; set; } = new();
            public Dashboard Data { get; set; } = new();

            // Constructor ini akan dipanggil oleh Controller dengan membawa nilai filter
            public Show(int jenisPajakId, int tahun, int bulan)
            {
                // 1. Ambil data list yang sudah difilter
                DataMonitoringHarianList = Method.GetMonitoringHarianList((EnumFactory.EPajak)jenisPajakId, tahun, bulan);

                // 2. Hitung data summary dashboard DARI data yang sudah difilter di atas
                Data = Method.GetDashboardData(DataMonitoringHarianList, (EnumFactory.EPajak)jenisPajakId, tahun, bulan);
            }
        }

        public class Method
        {
            // Method untuk menghitung data Dashboard secara dinamis
            public static Dashboard GetDashboardData(List<MonitoringHarian> filteredData, EnumFactory.EPajak jenisPajak, int tahun, int bulan)
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

            public static List<MonitoringHarian> GetMonitoringHarianList(EnumFactory.EPajak jenisPajak, int tahun, int bulan)
            {
                var ret = new List<MonitoringHarian>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTargetResto = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiResto = context.DbMonRestos
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetResto)
                        {
                            var totalRealisasi = dataRealisasiResto
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetListrik = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiListrik = context.DbMonPpjs
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetListrik)
                        {
                            var totalRealisasi = dataRealisasiListrik
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetHotel = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiHotel = context.DbMonHotels
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetHotel)
                        {
                            var totalRealisasi = dataRealisasiHotel
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetParkir = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiParkir = context.DbMonParkirs
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetParkir)
                        {
                            var totalRealisasi = dataRealisasiParkir
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetHiburan = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiHiburan = context.DbMonHiburans
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetHiburan)
                        {
                            var totalRealisasi = dataRealisasiHiburan
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetAbt = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiAbt = context.DbMonAbts
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
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

                        foreach (var item in dataTargetAbt)
                        {
                            var totalRealisasi = dataRealisasiAbt
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                    case EnumFactory.EPajak.Reklame:
                        var dataTargetReklame = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiReklame = context.DbMonReklames
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, PajakId = 7 })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayarPokok,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList();

                        foreach (var item in dataTargetReklame)
                        {
                            var totalRealisasi = dataRealisasiReklame
                                .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
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
                        //var dataTargetPbb = context.DbAkunTargetBulans
                        //    .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
                        //    .GroupBy(x => new { x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                        //    .Select(g => new
                        //    {
                        //        Tgl = g.Key.Tgl,
                        //        Bulan = g.Key.Bulan,
                        //        Tahun = g.Key.TahunBuku,
                        //        PajakId = g.Key.PajakId,
                        //        TotalTarget = g.Sum(x => x.Target)
                        //    })
                        //    .ToList();

                        //var dataRealisasiPbb = context.DbMonPbbs
                        //    .Where(x =>
                        //        x.TahunBuku == tahun &&
                        //        x.TglBayarPokok.HasValue &&
                        //        x.TglBayarPokok.Value.Year == tahun &&
                        //        x.TglBayarPokok.Value.Month == bulan
                        //    )
                        //    .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok, x.PajakId })
                        //    .Select(x => new
                        //    {
                        //        x.Key.Nop,
                        //        x.Key.TglBayarPokok,
                        //        x.Key.PajakId,
                        //        Realisasi = x.Sum(q => q.NominalPokokBayar)
                        //    })
                        //    .ToList();

                        //foreach (var item in dataTargetPbb)
                        //{
                        //    var totalRealisasi = dataRealisasiPbb
                        //        .Where(x => x.TglBayarPokok.Value.Month == item.Bulan && x.TglBayarPokok.Value.Day == item.Tgl && x.TglBayarPokok.Value.Year == tahun && x.PajakId == item.PajakId)
                        //        .Sum(x => x.Realisasi);

                        //    MonitoringHarian result = new MonitoringHarian
                        //    {
                        //        Tanggal = new DateTime((int)item.Tahun, (int)item.Bulan, (int)item.Tgl),
                        //        JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                        //        TargetHarian = item.TotalTarget,
                        //        Realisasi = totalRealisasi ?? 0
                        //    };

                        //    ret.Add(result);
                        //}
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var dataTargetBphtb = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiBphtb = context.DbMonBphtbs
                            .Where(x =>
                                x.Tahun == tahun &&
                                x.TglBayar.HasValue &&
                                x.TglBayar.Value.Year == tahun &&
                                x.TglBayar.Value.Month == bulan
                            )
                            .GroupBy(x => new { Nop = x.SpptNop, TglBayar = x.TglBayar, PajakId = 12 })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayar,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.Pokok)
                            })
                            .ToList();

                        foreach (var item in dataTargetBphtb)
                        {
                            var totalRealisasi = dataRealisasiBphtb
                                .Where(x => x.TglBayar.Value.Month == item.Bulan && x.TglBayar.Value.Day == item.Tgl && x.TglBayar.Value.Year == tahun && x.PajakId == item.PajakId)
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
                    case EnumFactory.EPajak.OpsenPkb:
                        var dataTargetOpsenPkb = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs
                            .Where(x =>
                                x.TahunPajakSspd == tahun &&
                                x.TglSspd.Year == tahun &&
                                x.TglSspd.Month == bulan
                            )
                            .GroupBy(x => new { Nop = x.IdSspd, TglSspd = x.TglSspd, PajakId = 20 })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglSspd,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.JmlPokok)
                            })
                            .ToList();

                        foreach (var item in dataTargetOpsenPkb)
                        {
                            var totalRealisasi = dataRealisasiOpsenPkb
                                .Where(x => x.TglSspd.Month == item.Bulan && x.TglSspd.Day == item.Tgl && x.TglSspd.Year == tahun && x.PajakId == item.PajakId)
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
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var dataTargetOpsenBbnkb = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan && x.PajakId == (decimal)jenisPajak)
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

                        var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs
                            .Where(x =>
                                x.TahunPajakSspd == tahun &&
                                x.TglSspd.Year == tahun &&
                                x.TglSspd.Month == bulan
                            )
                            .GroupBy(x => new { Nop = x.IdSspd, TglSspd = x.TglSspd, PajakId = 20 })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglSspd,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.JmlPokok)
                            })
                            .ToList();

                        foreach (var item in dataTargetOpsenBbnkb)
                        {
                            var totalRealisasi = dataRealisasiOpsenBbnkb
                                .Where(x => x.TglSspd.Month == item.Bulan && x.TglSspd.Day == item.Tgl && x.TglSspd.Year == tahun && x.PajakId == item.PajakId)
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
                        var dataTargetPerHari = context.DbAkunTargetBulans
                            .Where(x => x.TahunBuku == tahun && x.Bulan == bulan)
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

                        var realisasiPerHari = new List<(string Nop, DateTime Tgl, decimal Realisasi)>();

                        realisasiPerHari.AddRange(
                            context.DbMonRestos
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonPpjs
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonHotels
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonHotels
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonHiburans
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonAbts
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonReklames
                            .Where(x =>
                                x.TahunBuku == tahun &&
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tahun &&
                                x.TglBayarPokok.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.Nop,
                                x.Key.TglBayarPokok.Value,
                                x.Sum(q => q.NominalPokokBayar) ?? 0
                            ))
                            .ToList()
                        );
                        //realisasiPerHari.AddRange(
                        //    context.DbMonPbbs
                        //    .Where(x =>
                        //        x.TahunBuku == tahun &&
                        //        x.TglBayarPokok.HasValue &&
                        //        x.TglBayarPokok.Value.Year == tahun &&
                        //        x.TglBayarPokok.Value.Month == bulan
                        //    )
                        //    .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayarPokok })
                        //    .Select(x => new ValueTuple<string, DateTime, decimal>(
                        //        x.Key.Nop,
                        //        x.Key.TglBayarPokok.Value,
                        //        x.Sum(q => q.NominalPokokBayar) ?? 0
                        //    ))
                        //    .ToList()
                        //);
                        realisasiPerHari.AddRange(
                            context.DbMonBphtbs
                            .Where(x =>
                                x.Tahun == tahun &&
                                x.TglBayar.HasValue &&
                                x.TglBayar.Value.Year == tahun &&
                                x.TglBayar.Value.Month == bulan
                            )
                            .GroupBy(x => new { x.SpptNop, TglBayar = x.TglBayar })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(
                                x.Key.SpptNop,
                                x.Key.TglBayar.Value,
                                x.Sum(q => q.Pokok) ?? 0
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonOpsenPkbs
                            .Where(x =>
                                x.TahunPajakSspd == tahun &&
                                x.TglSspd.Year == tahun &&
                                x.TglSspd.Month == bulan
                            )
                            .GroupBy(x => new { Nop = x.IdSspd, TglSspd = x.TglSspd })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(

                                x.Key.Nop,
                                x.Key.TglSspd,
                                x.Sum(q => q.JmlPokok)
                            ))
                            .ToList()
                        );
                        realisasiPerHari.AddRange(
                            context.DbMonOpsenBbnkbs
                            .Where(x =>
                                x.TahunPajakSspd == tahun &&
                                x.TglSspd.Year == tahun &&
                                x.TglSspd.Month == bulan
                            )
                            .GroupBy(x => new { Nop = x.IdSspd, TglSspd = x.TglSspd })
                            .Select(x => new ValueTuple<string, DateTime, decimal>(

                                x.Key.Nop,
                                x.Key.TglSspd,
                                x.Sum(q => q.JmlPokok)
                            ))
                            .ToList()
                        );

                        foreach (var item in dataTargetPerHari)
                        {
                            var totalRealisasi = realisasiPerHari
                                .Where(x => x.Tgl.Month == item.Bulan && x.Tgl.Day == item.Tgl && x.Tgl.Year == tahun)
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
