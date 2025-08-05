using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringBulananVM
    {
        public class Index
        {
            public int SelectedPajak { get; set; }
            public List<SelectListItem> JenisPajakList { get; set; } = new();
            public Index()
            {
                JenisPajakList.Add(new SelectListItem { Value = "0", Text = "Semua Jenis Pajak" });
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }
        }
        public class Show
        {
            public List<MonitoringBulananViewModels.BulananPajak> BulananPajakList { get; set; } = new();
            public List<MonitoringBulananViewModels.BulananPajak> AkumulasiBulananPajakList { get; set; } = new();
            public MonitoringBulananViewModels.DataRekapBulanan Data { get; set; } = new();
            public Show(EnumFactory.EPajak jenisPajak, int tahun)
            {
                BulananPajakList = Method.GetBulananPajak(jenisPajak, tahun);
                AkumulasiBulananPajakList = Method.GetBulananPajakAkumulasi(jenisPajak, tahun);
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
            public static List<MonitoringBulananViewModels.BulananPajak> GetBulananPajak(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<MonitoringBulananViewModels.BulananPajak>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetRestoPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetRestoPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiRestoPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiListrikPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetListrikPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetListrikPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiListrikPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHotelPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetHotelPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHotelPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetParkirPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetParkirPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiParkirPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetHiburanPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiHiburanPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetAbtPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetAbtPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiAbtPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                    case EnumFactory.EPajak.Reklame:
                        var realisasiReklamePerBulan = context.DbMonReklames
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetReklamePerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetReklamePerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiReklamePerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun)
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetPbbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetPbbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPbbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                    case EnumFactory.EPajak.BPHTB:
                        var realisasiBphtbPerBulan = context.DbMonBphtbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun)
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.Pokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetBphtbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetBphtbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiBphtbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                    case EnumFactory.EPajak.OpsenPkb:
                        var realisasiOpsenPkbPerBulan = context.DbMonOpsenPkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglSspd,
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetOpsenPkbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetOpsenPkbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiOpsenPkbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var realisasiOpsenBbnkbPerBulan = context.DbMonOpsenBbnkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglSspd,
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetOpsenBbnkbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        foreach (var item in dataTargetOpsenBbnkbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiOpsenBbnkbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
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
                        var dataTargetPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        var realisasiPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        realisasiPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonReklames
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonPbbs
                                .Where(x => x.TglBayar.HasValue && x.TglBayar.Value.Year == tahun)
                                .GroupBy(x => x.TglBayar.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.JumlahBayarPokok) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonBphtbs
                                .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun)
                                .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayar,
                                    g.Sum(x => x.Pokok) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonOpsenPkbs
                                .Where(x => x.TglSspd.Year == tahun)
                                .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglSspd,
                                    g.Sum(x => x.JmlPokok)
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonOpsenBbnkbs
                                .Where(x => x.TglSspd.Year == tahun)
                                .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglSspd,
                                    g.Sum(x => x.JmlPokok)
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
            public static List<MonitoringBulananViewModels.BulananPajak> GetBulananPajakAkumulasi(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<MonitoringBulananViewModels.BulananPajak>();
                var context = DBClass.GetContext();



                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetRestoPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetResto += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiResto += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var realisasiListrikPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetListrikPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        decimal akumulasiTargetListrik = 0;
                        decimal akumulasiRealisasiListrik = 0;
                        foreach (var item in dataTargetListrikPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiListrikPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);


                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetListrik += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiListrik += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHotelPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetHotel += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiHotel += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetParkirPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetParkir += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiParkir += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetHiburan += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiHiburan += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetAbtPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetAbt += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiAbt += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        var realisasiReklamePerBulan = context.DbMonReklames
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = (int)x.TglBayarPokok.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayarPokok,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetReklamePerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        decimal akumulasiTargetReklame = 0;
                        decimal akumulasiRealisasiReklame = 0;
                        foreach (var item in dataTargetReklamePerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiReklamePerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);


                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetReklame += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiReklame += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.PBB:
                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun)
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetPbbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetPbb += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiPbb += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var realisasiBphtbPerBulan = context.DbMonBphtbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun)
                            .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglBayar,
                                Realisasi = g.Sum(x => x.Pokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetBphtbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        decimal akumulasiTargetBphtb = 0;
                        decimal akumulasiRealisasiBphtb = 0;
                        foreach (var item in dataTargetBphtbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiBphtbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);


                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetBphtb += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiBphtb += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        var realisasiOpsenPkbPerBulan = context.DbMonOpsenPkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglSspd,
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetOpsenPkbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        decimal akumulasiTargetOpsenPkb = 0;
                        decimal akumulasiRealisasiOpsenPkb = 0;
                        foreach (var item in dataTargetOpsenPkbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiOpsenPkbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);


                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetOpsenPkb += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiOpsenPkb += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var realisasiOpsenBbnkbPerBulan = context.DbMonOpsenBbnkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                            .Select(g => new
                            {
                                Bulan = g.Key.TglSspd,
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetOpsenBbnkbPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();


                        decimal akumulasiTargetOpsenBbnkb = 0;
                        decimal akumulasiRealisasiOpsenBbnkb = 0;
                        foreach (var item in dataTargetOpsenBbnkbPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiOpsenBbnkbPerBulan
                                    .Where(x => x.Bulan == item.Bulan)
                                    .Sum(x => x.Realisasi);


                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetOpsenBbnkb += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasiOpsenBbnkb += totalRealisasi, // Akumulasi Realisasi
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }

                        break;
                    default:
                        var dataTargetPerBulan = context.DbAkunTargetBulans
                                .Where(x => x.TahunBuku == tahun)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();
                        var realisasiPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        realisasiPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonReklames
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun)
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonPbbs
                                .Where(x => x.TglBayar.HasValue && x.TglBayar.Value.Year == tahun)
                                .GroupBy(x => x.TglBayar.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key,
                                    g.Sum(x => x.NominalPokokBayar) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonBphtbs
                                .Where(x => x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun)
                                .GroupBy(x => new { TglBayar = (int)x.TglBayar.Value.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglBayar,
                                    g.Sum(x => x.Pokok) ?? 0
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonOpsenPkbs
                                .Where(x => x.TglSspd.Year == tahun)
                                .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglSspd,
                                    g.Sum(x => x.JmlPokok)
                                ))
                                .ToList()
                        );
                        realisasiPerBulan.AddRange(
                            context.DbMonOpsenBbnkbs
                                .Where(x => x.TglSspd.Year == tahun)
                                .GroupBy(x => new { TglSspd = (int)x.TglSspd.Month })
                                .Select(g => new ValueTuple<int, decimal>(
                                    g.Key.TglSspd,
                                    g.Sum(x => x.JmlPokok)
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
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTarget += item.TotalTarget, // Akumulasi Target
                                Realisasi = akumulasiRealisasi += totalRealisasi, // Akumulasi Realisasi
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
