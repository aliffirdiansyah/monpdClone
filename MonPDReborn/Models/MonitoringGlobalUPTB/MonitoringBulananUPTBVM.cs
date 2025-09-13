using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Web.Mvc;


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
                        
                        var mamin = context.DbOpRestos
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        
                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && mamin.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();

                        
                        var dataTargetRestoPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();

                        
                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetRestoPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiRestoPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var ppj = context.DbOpListriks
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();


                        var realisasiPpjPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && ppj.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();


                        var dataTargetPpjPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetPpjPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiPpjPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var hotel = context.DbOpHotels
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();


                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && hotel.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();


                        var dataTargetHotelPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetHotelPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiHotelPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var parkir = context.DbOpParkirs
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();


                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && parkir.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();


                        var dataTargetParkirPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetParkirPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiParkirPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var hiburan = context.DbOpHiburans
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();


                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && hiburan.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();


                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetHiburanPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiHiburanPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var abt = context.DbOpAbts
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();


                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && abt.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .ToList();


                        var dataTargetAbtPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetAbtPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiAbtPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        var pbb = context.DbOpPbbs
                            .Where(x => x.Uptb == wilayah)
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        // Ambil realisasi per bulan hanya untuk NOP yang ada di wilayah tersebut
                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TahunBuku == tahun
                                        && x.JumlahBayarPokok > 0
                                        && pbb.Contains(x.Nop))
                            .GroupBy(x => x.TglBayar.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        // Ambil target bulanan per UPTB
                        var dataTargetPbbPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun
                                        && x.PajakId == (decimal)jenisPajak
                                        && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();

                        // Gabungkan hasil target dan realisasi menjadi view model bulanan
                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetPbbPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiPbbPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.Realisasi ?? 0;

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1)
                                    .ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
                            });
                        }
                        break;
                    default:
                       
                        var dataTargetSemuaPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();

                        
                        var nopMamin = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopPpj = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var nopPbb = context.DbOpPbbs.Where(x => x.Uptb == wilayah)
                                            .Select(x => x.Nop).Distinct().ToList();


                        // === REALISASI SEMUA JENIS PAJAK ===
                        var realisasiSemuaPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        // Restoran / Mamin
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopMamin.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // PPJ
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopPpj.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Hotel
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopHotel.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Parkir
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopParkir.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Hiburan
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopHiburan.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // ABT
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && nopAbt.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // PBB
                        var realisasiPbb = (
                            from m in context.DbMonPbbs
                            join o in context.DbOpPbbs on m.Nop equals o.Nop
                            where o.Uptb == wilayah
                                  && m.TahunBuku == tahun
                                  && m.TglBayar.HasValue
                                  && m.TglBayar.Value.Year == tahun
                                  && m.JumlahBayarPokok > 0
                            group m by m.TglBayar.Value.Month into g
                            select new ValueTuple<int, decimal>(
                                g.Key,
                                g.Sum(x => x.JumlahBayarPokok) ?? 0
                            )
                        ).ToList();

                        realisasiSemuaPerBulan.AddRange(realisasiPbb);



                        // === GABUNGKAN TARGET DAN REALISASI ===
                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetSemuaPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiSemuaPerBulan
                                .Where(x => x.Bulan == bulanLoop)
                                .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = 0, // 0 = Semua
                                JenisPajak = "SEMUA PAJAK",
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = targetBulan,
                                Realisasi = realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
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
                        var nopMamin = context.DbOpRestos
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopMamin.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetRestoPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        var nopPpj = context.DbOpListriks
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiPpjPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopPpj.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetPpjPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        decimal akumulasiTargetPpj = 0;
                        decimal akumulasiRealisasiPpj = 0;

                        foreach (var item in dataTargetPpjPerBulan.OrderBy(x => x.Bulan))
                        {
                            var totalRealisasi = realisasiPpjPerBulan
                                .Where(x => x.Bulan == item.Bulan)
                                .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = (int)jenisPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                Tahun = tahun,
                                Bulan = item.Bulan,
                                BulanNama = new DateTime(tahun, item.Bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetPpj += item.TotalTarget,
                                Realisasi = akumulasiRealisasiPpj += totalRealisasi,
                                Pencapaian = item.TotalTarget == 0 ? 0 : (totalRealisasi / item.TotalTarget) * 100
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var nopHotel = context.DbOpHotels
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopHotel.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHotelPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        var nopParkir = context.DbOpParkirs
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopParkir.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetParkirPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        var nopHiburan = context.DbOpHiburans
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopHiburan.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetHiburanPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        var nopAbt = context.DbOpAbts
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && nopAbt.Contains(x.Nop))
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetAbtPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        // 🚀 Optimasi: jangan ambil semua NOP, filter langsung di DbMonPbbs
                        var realisasiPbbPerBulan = (
                            from mon in context.DbMonPbbs
                            join op in context.DbOpPbbs
                                on mon.Nop equals op.Nop
                            where mon.TglBayar.HasValue
                                  && mon.TglBayar.Value.Year == tahun
                                  && mon.TahunBuku == tahun
                                  && op.Uptb == ((int)wilayah)
                            group mon by mon.TglBayar.Value.Month into g
                            select new
                            {
                                Bulan = g.Key,
                                Realisasi = g.Sum(x => x.JumlahBayarPokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        var dataTargetPbbPerBulan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun
                                            && x.PajakId == (decimal)jenisPajak
                                            && x.Uptb == (int)wilayah)
                                .GroupBy(x => x.Bulan)
                                .Select(g => new
                                {
                                    Bulan = (int)g.Key,
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
                        var dataTargetSemuaPerBulan = context.DbAkunTargetBulanUptbs
                            .Where(x => x.TahunBuku == tahun && x.Uptb == (int)wilayah)
                            .GroupBy(x => x.Bulan)
                            .Select(g => new
                            {
                                Bulan = (int)g.Key,
                                TotalTarget = g.Sum(x => x.Target)
                            })
                            .ToList();


                        var semuaMamin = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaPpj = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)wilayah).ToString()
                                            && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                                            .Select(x => x.Nop).ToList();

                        var semuaPbb = context.DbOpPbbs.Where(x => x.Uptb == wilayah)
                                            .Select(x => x.Nop).Distinct().ToList();


                        // === REALISASI SEMUA JENIS PAJAK ===
                        var realisasiSemuaPerBulan = new List<(int Bulan, decimal Realisasi)>();

                        // Restoran / Mamin
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonRestos
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaMamin.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // PPJ
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonPpjs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaPpj.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Hotel
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonHotels
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaHotel.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Parkir
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonParkirs
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaParkir.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // Hiburan
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonHiburans
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaHiburan.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // ABT
                        realisasiSemuaPerBulan.AddRange(
                            context.DbMonAbts
                                .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahun && semuaAbt.Contains(x.Nop))
                                .GroupBy(x => x.TglBayarPokok.Value.Month)
                                .Select(g => new ValueTuple<int, decimal>(g.Key, g.Sum(x => x.NominalPokokBayar) ?? 0))
                                .AsQueryable()
                        );

                        // PBB
                        var realisasiPbb = (
                            from m in context.DbMonPbbs
                            join o in context.DbOpPbbs on m.Nop equals o.Nop
                            where o.Uptb == wilayah
                                  && m.TahunBuku == tahun
                                  && m.TglBayar.HasValue
                                  && m.TglBayar.Value.Year == tahun
                                  && m.JumlahBayarPokok > 0
                            group m by m.TglBayar.Value.Month into g
                            select new ValueTuple<int, decimal>(
                                g.Key,
                                g.Sum(x => x.JumlahBayarPokok) ?? 0
                            )
                        ).ToList();

                        realisasiSemuaPerBulan.AddRange(realisasiPbb);

                        decimal akumulasiTargetSemua = 0;
                        decimal akumulasiRealisasiSemua = 0;

                        // === GABUNGKAN TARGET DAN REALISASI ===
                        for (int bulanLoop = 1; bulanLoop <= 12; bulanLoop++)
                        {
                            var targetBulan = dataTargetSemuaPerBulan
                                .FirstOrDefault(x => x.Bulan == bulanLoop)?.TotalTarget ?? 0;

                            var realisasiBulan = realisasiSemuaPerBulan
                                .Where(x => x.Bulan == bulanLoop)
                                .Sum(x => x.Realisasi);

                            ret.Add(new MonitoringBulananViewModels.BulananPajak()
                            {
                                EnumPajak = 0, // 0 = Semua
                                JenisPajak = "SEMUA PAJAK",
                                Tahun = tahun,
                                Bulan = bulanLoop,
                                BulanNama = new DateTime(tahun, bulanLoop, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                AkpTarget = akumulasiTargetSemua += targetBulan,
                                Realisasi = akumulasiRealisasiSemua += realisasiBulan,
                                Pencapaian = targetBulan == 0 ? 0 : (realisasiBulan / targetBulan) * 100
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
