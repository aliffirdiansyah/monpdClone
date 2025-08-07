

using ClosedXML.Excel;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Native.EntityFramework;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static MonPDLib.General.EnumFactory;

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
            public EnumFactory.EUPTB Wilayah { get; set; }
            public EnumFactory.EPajak JenisPajak { get; set; }
            public Show() { }

            public Show(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                Wilayah = wilayah;
                JenisPajak = jenisPajak;

                RealisasiWilayahList = Method.GetDataRealisasiWilayahList(wilayah, tahun, bulan, jenisPajak);

                if(wilayah == EnumFactory.EUPTB.SEMUA && jenisPajak == EnumFactory.EPajak.Semua)
                {
                    RealisasiJenisList = Method.GetDataRealisasiJenisList(tahun, bulan);
                }

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
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonRestos
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonRestos
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonPpjs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonPpjs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonHotels
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonHotels
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonParkirs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonParkirs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonHiburans
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonHiburans
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpAbts.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonAbts
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopList.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var nopList = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah)
                                .Sum(x => x.Target);

                            var totalRealisasi = context.DbMonAbts
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            ret.Add(new RealisasiWilayah
                            {
                                Wilayah = $"UPTB {(int)wilayah}",
                                Tahun = tahun,
                                Lokasi = $"UPTB {(int)wilayah}",
                                Target = totalTarget,
                                Realisasi = totalRealisasi ?? 0,
                                Tren = 0,
                                Status = ""
                            });
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        
                        break;
                    case EnumFactory.EPajak.PBB:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);
                                var totalRealisasi = context.DbMonPbbs
                                        .Where(x =>
                                            x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && x.Uptb == Convert.ToInt32(uptb)
                                        ).Sum(q => q.JumlahBayarPokok);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = ""
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var dataPbbWilayah = context.DbMonPbbs
                                 .Where(x => x.TglBayar.Value.Year == tahun && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                 .Select(x => new
                                 {
                                     x.Nop,
                                     WilayahPajak = x.Uptb
                                 })
                                 .Distinct()
                                 .AsQueryable();

                            var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var uptb = dataPbbWilayah
                                .Where(x => Convert.ToInt32(x.WilayahPajak) == (int)wilayah)
                                .Select(x => x.Nop)
                                .AsEnumerable();

                            var dataRealisasiWilayah = context.DbMonPbbs
                                .Where(x =>
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Year == tahun &&
                                    x.TglBayar.Value.Month <= bulan &&
                                    uptb.Contains(x.Nop)
                                )
                                .GroupBy(x => new { x.Nop, TglBayarPokok = x.TglBayar })
                                .Select(x => new
                                {
                                    x.Key.Nop,
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.JumlahBayarPokok)
                                })
                                .AsQueryable();

                            var targetPerUptb = dataTargetWilayahPbb
                                .Where(x => x.Uptb == (int)wilayah) // filter sesuai UPTB
                                .ToList();

                            var totalRealisasi = dataRealisasiWilayah
                                .Where(x => x.TglBayarPokok.Value.Month == bulan)
                                .Sum(x => x.Realisasi);

                            foreach (var item in targetPerUptb)
                            {
                                var re = new RealisasiWilayah();
                                re.Wilayah = $"UPTB {(int)item.Uptb}";
                                re.Tahun = tahun;
                                re.Lokasi = $"UPTB {(int)item.Uptb}";
                                re.Target = item.TotalTarget;
                                re.Realisasi = totalRealisasi ?? 0;
                                re.Tren = 0;
                                re.Status = "";

                                ret.Add(re);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.BPHTB:
                        
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        

                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbAllList = context.MWilayahs.Select(x => Convert.ToInt32(x.Uptd)).Distinct().ToList();

                            foreach (var uptb in uptbAllList)
                            {
                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.Uptb == (int)uptb)
                                    .GroupBy(x => new { x.PajakId })
                                    .Select(x => new
                                    {
                                        x.Key.PajakId,
                                        Target = x.Sum(q => q.Target)
                                    }).ToList();

                                var nopListAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListResto = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();

                                var totalRealisasiAbt = context.DbMonAbts
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListAbt.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiResto = context.DbMonRestos
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListResto.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiHotel = context.DbMonHotels
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHotel.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiListrik = context.DbMonPpjs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListListrik.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiParkir = context.DbMonParkirs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListParkir.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiHiburan = context.DbMonHiburans
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHiburan.Contains(x.Nop)
                                        ).Sum(q => q.NominalPokokBayar);

                                var totalRealisasiPbb = context.DbMonPbbs
                                        .Where(x =>
                                            x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && x.Uptb == Convert.ToInt32(uptb)
                                        ).Sum(q => q.JumlahBayarPokok);

                                foreach (var item in totalTarget)
                                {
                                    var re = new RealisasiWilayah();
                                    if (item.PajakId != null) 
                                    { 
                                        switch ((EnumFactory.EPajak)item.PajakId)
                                        {
                                            case EPajak.MakananMinuman:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiAbt ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.TenagaListrik:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiListrik ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaPerhotelan:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiHotel ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaParkir:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiParkir ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaKesenianHiburan:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiHiburan ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.AirTanah:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiAbt ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.Reklame:
                                                break;
                                            case EPajak.PBB:
                                                re.Wilayah = $"UPTB {uptb}";
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.Lokasi = $"UPTB {uptb}";
                                                re.Target = item.Target;
                                                re.Realisasi = totalRealisasiPbb ?? 0;
                                                re.Tren = 0;
                                                re.Status = "";

                                                ret.Add(re);
                                                break;
                                            case EPajak.BPHTB:
                                                break;
                                            case EPajak.OpsenPkb:
                                                break;
                                            case EPajak.OpsenBbnkb:
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                ret = ret
                                    .GroupBy(x => new
                                    {
                                        x.Wilayah,
                                        x.Tahun,
                                        x.Bulan,
                                        x.Lokasi,
                                    }).Select(x => new RealisasiWilayah()
                                    {
                                        Wilayah = x.Key.Wilayah,
                                        Tahun = x.Key.Tahun,
                                        Bulan = x.Key.Bulan,
                                        Lokasi = x.Key.Lokasi,
                                        Target = x.Sum(q => q.Target),
                                        Realisasi = x.Sum(q => q.Realisasi),
                                        Tren = 0,
                                        Status = ""
                                    }).ToList();
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.Uptb == (int)wilayah)
                                    .GroupBy(x => new { x.PajakId })
                                    .Select(x => new
                                    {
                                        x.Key.PajakId,
                                        Target = x.Sum(q => q.Target)
                                    }).ToList();

                            var nopListAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListResto = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();

                            var totalRealisasiAbt = context.DbMonAbts
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListAbt.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiResto = context.DbMonRestos
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListResto.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiHotel = context.DbMonHotels
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListHotel.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiListrik = context.DbMonPpjs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListListrik.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiParkir = context.DbMonParkirs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListParkir.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiHiburan = context.DbMonHiburans
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListHiburan.Contains(x.Nop)
                                    ).Sum(q => q.NominalPokokBayar);

                            var totalRealisasiPbb = context.DbMonPbbs
                                    .Where(x =>
                                        x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TglBayar.Value.Month <= bulan
                                        && x.Uptb == Convert.ToInt32(wilayah)
                                    ).Sum(q => q.JumlahBayarPokok);

                            foreach (var item in totalTarget)
                            {
                                var re = new RealisasiWilayah();
                                if (item.PajakId != null)
                                {
                                    switch ((EnumFactory.EPajak)item.PajakId)
                                    {
                                        case EPajak.MakananMinuman:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiAbt ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.TenagaListrik:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiListrik ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaPerhotelan:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiHotel ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaParkir:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiParkir ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaKesenianHiburan:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiHiburan ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.AirTanah:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiAbt ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.Reklame:
                                            break;
                                        case EPajak.PBB:
                                            re.Wilayah = $"UPTB {wilayah}";
                                            re.Tahun = tahun;
                                            re.Bulan = bulan;
                                            re.Lokasi = $"UPTB {wilayah}";
                                            re.Target = item.Target;
                                            re.Realisasi = totalRealisasiPbb ?? 0;
                                            re.Tren = 0;
                                            re.Status = "";

                                            ret.Add(re);
                                            break;
                                        case EPajak.BPHTB:
                                            break;
                                        case EPajak.OpsenPkb:
                                            break;
                                        case EPajak.OpsenBbnkb:
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            ret = ret
                                .GroupBy(x => new
                                {
                                    x.Wilayah,
                                    x.Tahun,
                                    x.Bulan,
                                    x.Lokasi,
                                }).Select(x => new RealisasiWilayah()
                                {
                                    Wilayah = x.Key.Wilayah,
                                    Tahun = x.Key.Tahun,
                                    Bulan = x.Key.Bulan,
                                    Lokasi = x.Key.Lokasi,
                                    Target = x.Sum(q => q.Target),
                                    Realisasi = x.Sum(q => q.Realisasi),
                                    Tren = 0,
                                    Status = ""
                                }).ToList();
                        }
                    break;
                }

                return ret;
            }
            public static List<RealisasiJenis> GetDataRealisasiJenisList(int tahun, int bulan)
            {
                var result = new List<RealisasiJenis>();

                var context = DBClass.GetContext();

                var targetPajak = context.DbAkunTargetBulanUptbs
                        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan)
                        .GroupBy(x => new { x.PajakId })
                        .Select(x => new
                        {
                            x.Key.PajakId,
                            Target = x.Sum(q => q.Target)
                        }).ToList();

                var jmlWpAbt = context.DbOpAbts
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpResto = context.DbOpRestos
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpHotel = context.DbOpHotels
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpPpj = context.DbOpListriks
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpParkir = context.DbOpParkirs
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpHiburan = context.DbOpHiburans
                     .Where(x => x.TahunBuku == tahun)
                     .Count();

                var jmlWpPbb = context.DbOpPbbs.Count();

                var realisasiPajakAbt = context.DbMonAbts
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakResto = context.DbMonRestos
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakHotel = context.DbMonHotels
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakPpj = context.DbMonPpjs
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakParkir = context.DbMonParkirs
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakHiburan = context.DbMonHiburans
                        .Where(x =>
                            x.TglBayarPokok.HasValue
                            && x.TglBayarPokok.Value.Year == tahun
                            && x.TglBayarPokok.Value.Month <= bulan
                        ).Sum(q => q.NominalPokokBayar) ?? 0;

                var realisasiPajakPbb = context.DbMonPbbs
                        .Where(x =>
                            x.TglBayar.HasValue
                            && x.TglBayar.Value.Year == tahun
                            && x.TglBayar.Value.Month <= bulan
                        ).Sum(q => q.JumlahBayarPokok) ?? 0;

                foreach (var item in targetPajak)
                {
                    if(item.PajakId != null)
                    {
                        var ret = new RealisasiJenis();
                        switch ((EnumFactory.EPajak)item.PajakId)
                        {
                            case EPajak.MakananMinuman:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.MakananMinuman).GetDescription();
                                ret.JmlWP = jmlWpResto;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakResto;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.TenagaListrik:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.TenagaListrik).GetDescription();
                                ret.JmlWP = jmlWpPpj;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakPpj;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.JasaPerhotelan:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.JasaPerhotelan).GetDescription();
                                ret.JmlWP = jmlWpHotel;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakHotel;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.JasaParkir:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.JasaParkir).GetDescription();
                                ret.JmlWP = jmlWpParkir;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakParkir;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.JasaKesenianHiburan:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.JasaKesenianHiburan).GetDescription();
                                ret.JmlWP = jmlWpHiburan;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakHiburan;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.AirTanah:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.AirTanah).GetDescription();
                                ret.JmlWP = jmlWpAbt;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakAbt;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.PBB:
                                ret.Tahun = tahun;
                                ret.Bulan = bulan;
                                ret.JenisPajak = (EnumFactory.EPajak.PBB).GetDescription();
                                ret.JmlWP = jmlWpPbb;
                                ret.Target = item.Target;
                                ret.Realisasi = realisasiPajakPbb;
                                ret.Tren = 0;
                                ret.Status = "-";

                                result.Add(ret);
                                break;
                            case EPajak.Reklame:
                                break;
                            case EPajak.BPHTB:
                                break;
                            case EPajak.OpsenPkb:
                                break;
                            case EPajak.OpsenBbnkb:
                                break;
                            default:
                                break;
                        }
                    }
                }

                return result;
            }
            public static List<DataHarian> GetDataDataHarianList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DataHarian>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahResto = context.DbMonRestos
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);

                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahResto.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahResto.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahResto = context.DbMonRestos
                                .Where(x =>
                                     x.TglBayarPokok.HasValue
                                    && x.TglBayarPokok.Value.Year == tahun
                                    && x.TglBayarPokok.Value.Month <= bulan
                                    && nopList.Contains(x.Nop)
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahResto.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahResto.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahListrik = context.DbMonPpjs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);

                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahListrik.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahListrik.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahListrik = context.DbMonPpjs
                                .Where(x =>
                                     x.TglBayarPokok.HasValue
                                    && x.TglBayarPokok.Value.Year == tahun
                                    && x.TglBayarPokok.Value.Month <= bulan
                                    && nopList.Contains(x.Nop)
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahListrik.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahListrik.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var dataTargetWilayahHotel = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahHotel = context.DbMonHotels
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);

                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahHotel.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahHotel.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahHotel = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahHotel = context.DbMonHotels
                                .Where(x =>
                                     x.TglBayarPokok.HasValue
                                    && x.TglBayarPokok.Value.Year == tahun
                                    && x.TglBayarPokok.Value.Month <= bulan
                                    && nopList.Contains(x.Nop)
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahHotel.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahHotel.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                ret.Add(result);
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var dataTargetWilayahParkir = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahParkir = context.DbMonParkirs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);

                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahParkir.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahParkir.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahParkir = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahParkir = context.DbMonParkirs
                                .Where(x =>
                                     x.TglBayarPokok.HasValue
                                    && x.TglBayarPokok.Value.Year == tahun
                                    && x.TglBayarPokok.Value.Month <= bulan
                                    && nopList.Contains(x.Nop)
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahParkir.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahParkir.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                ret.Add(result);
                            }
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == uptb)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                                var dataTargetWilayahHiburan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahHiburan = context.DbMonHiburans
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopList.Contains(x.Nop)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);

                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahHiburan.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahHiburan.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahHiburan = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahHiburan = context.DbMonHiburans
                                .Where(x =>
                                     x.TglBayarPokok.HasValue
                                    && x.TglBayarPokok.Value.Year == tahun
                                    && x.TglBayarPokok.Value.Month <= bulan
                                    && nopList.Contains(x.Nop)
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahHiburan.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahHiburan.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


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
                         /*.Select(x => new
                         {
                             x.Nop,
                             Wilayah = Regex.Match(x.WilayahPajak ?? "", @"\d+").Value,
                             x.PajakId
                         })*/
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
                                    .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)item.Uptb && w.PajakId == item.PajakId)
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
                            var uptb = dataAbtWilayah.Where(x => Convert.ToInt32(x.WilayahPajak) == (int)wilayah).Select(x => x.Nop).ToList();
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
                                    .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)item.Uptb && w.PajakId == item.PajakId)
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
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                                var dataRealisasiWilayahPbb = context.DbMonPbbs
                                    .Where(x =>
                                        x.TahunBuku == tahun &&
                                        x.TglBayar.HasValue &&
                                        x.TglBayar.Value.Year == tahun &&
                                        x.TglBayar.Value.Month <= bulan &&
                                        x.Uptb == Convert.ToInt32(uptb)
                                    )
                                    .GroupBy(x => new { TglBayarPokok = x.TglBayar.Value.Date, PajakId = 9 })
                                    .Select(x => new
                                    {
                                        Tanggal = x.Key.TglBayarPokok,
                                        x.Key.PajakId,
                                        TotalRealisasi = x.Sum(q => q.JumlahBayarPokok)
                                    })
                                    .ToList();

                                var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                                for (int i = 1; i <= totalTanggal; i++)
                                {
                                    var tanggal = new DateTime(tahun, bulan, i);
                                    var result = new DataHarian();
                                    result.Wilayah = $"UPTB {uptb} ";
                                    result.EnumWilayah = Convert.ToInt32(uptb);
                                    result.Tanggal = tanggal;
                                    result.Tahun = (int)bulan;
                                    result.Bulan = (int)tahun;
                                    result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                    result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                    result.Target = dataTargetWilayahPbb.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahPbb.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var dataTargetWilayahPbb = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    Uptb = g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    PajakId = g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var dataRealisasiWilayahPbb = context.DbMonPbbs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Year == tahun &&
                                    x.TglBayar.Value.Month <= bulan &&
                                    x.Uptb == (int)wilayah
                                )
                                .GroupBy(x => new { TglBayarPokok = x.TglBayar.Value.Date, PajakId = 9 })
                                .Select(x => new
                                {
                                    Tanggal = x.Key.TglBayarPokok,
                                    x.Key.PajakId,
                                    TotalRealisasi = x.Sum(q => q.JumlahBayarPokok)
                                })
                                .ToList();

                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            for (int i = 1; i <= totalTanggal; i++)
                            {
                                var tanggal = new DateTime(tahun, bulan, i);
                                var result = new DataHarian();
                                result.Wilayah = $"UPTB {(int)wilayah} ";
                                result.EnumWilayah = (int)wilayah;
                                result.Tanggal = tanggal;
                                result.Tahun = (int)bulan;
                                result.Bulan = (int)tahun;
                                result.JenisPajak = ((EnumFactory.EPajak.PBB)).GetDescription();
                                result.EnumPajak = (int)(EnumFactory.EPajak.PBB);
                                result.Target = dataTargetWilayahPbb.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                result.Realisasi = dataRealisasiWilayahPbb.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                ret.Add(result);
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
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var totalTanggal = DateTime.DaysInMonth(tahun, bulan);
                            var uptbList = context.MWilayahs.Select(x => Convert.ToInt32(x.Uptd)).Distinct().ToList();
                            foreach (var uptb in uptbList)
                            {
                                var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && Convert.ToInt32(x.Uptb) == Convert.ToInt32(uptb))
                                    .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                    .Select(g => new
                                    {
                                        Uptb = g.Key.Uptb,
                                        g.Key.Tgl,
                                        g.Key.Bulan,
                                        g.Key.TahunBuku,
                                        PajakId = g.Key.PajakId,
                                        TotalTarget = g.Sum(x => x.Target)
                                    })
                                    .AsQueryable();

                                var nopListAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListResto = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();
                                var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList();

                                var totalRealisasiAbt = context.DbMonAbts
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListAbt.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 6 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiResto = context.DbMonRestos
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListResto.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 1 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiHotel = context.DbMonHotels
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHotel.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 3 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiListrik = context.DbMonPpjs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListListrik.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 2 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiParkir = context.DbMonParkirs
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListParkir.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 4 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiHiburan = context.DbMonHiburans
                                        .Where(x =>
                                            x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHiburan.Contains(x.Nop)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 5 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
            })
            .ToList();

                                var totalRealisasiPbb = context.DbMonPbbs
                                        .Where(x =>
                                            x.TglBayar.HasValue
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && x.Uptb == Convert.ToInt32(uptb)
                                        ).GroupBy(x => new { TglBayarPokok = x.TglBayar.Value.Date, PajakId = 9 })
            .Select(x => new
            {
                Tanggal = x.Key.TglBayarPokok,
                x.Key.PajakId,
                TotalRealisasi = x.Sum(q => q.JumlahBayarPokok)
            })
            .ToList();

                                foreach (var item in dataTargetWilayah)
                                {
                                    var tanggal = new DateTime(tahun, bulan, Convert.ToInt32(item.Tgl));
                                    var re = new DataHarian();
                                    if(item.PajakId != null)
                                    {
                                        switch ((EnumFactory.EPajak)item.PajakId)
                                        {
                                            case EPajak.MakananMinuman:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiResto.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.TenagaListrik:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiListrik.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaPerhotelan:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiHotel.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaParkir:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiParkir.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.JasaKesenianHiburan:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiHiburan.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.AirTanah:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiAbt.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.Reklame:
                                                break;
                                            case EPajak.PBB:
                                                re.Wilayah = $"UPTB {(int)uptb} ";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = (int)bulan;
                                                re.Bulan = (int)tahun;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = totalRealisasiPbb.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                                ret.Add(re);
                                                break;
                                            case EPajak.BPHTB:
                                                break;
                                            case EPajak.OpsenPkb:
                                                break;
                                            case EPajak.OpsenBbnkb:
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && Convert.ToInt32(x.Uptb) == Convert.ToInt32(wilayah))
                                    .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                    .Select(g => new
                                    {
                                        Uptb = g.Key.Uptb,
                                        g.Key.Tgl,
                                        g.Key.Bulan,
                                        g.Key.TahunBuku,
                                        PajakId = g.Key.PajakId,
                                        TotalTarget = g.Sum(x => x.Target)
                                    })
                                    .AsQueryable();

                            var nopListAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListResto = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();

                            var totalRealisasiAbt = context.DbMonAbts
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListAbt.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 6 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiResto = context.DbMonRestos
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListResto.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 1 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiHotel = context.DbMonHotels
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListHotel.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 3 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiListrik = context.DbMonPpjs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListListrik.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 2 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiParkir = context.DbMonParkirs
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListParkir.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 4 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiHiburan = context.DbMonHiburans
                                    .Where(x =>
                                        x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun
                                        && x.TglBayarPokok.Value.Month <= bulan
                                        && nopListHiburan.Contains(x.Nop)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 5 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
        })
        .ToList();

                            var totalRealisasiPbb = context.DbMonPbbs
                                    .Where(x =>
                                        x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TglBayar.Value.Month <= bulan
                                        && x.Uptb == Convert.ToInt32(wilayah)
                                    ).GroupBy(x => new { TglBayarPokok = x.TglBayar.Value.Date, PajakId = 9 })
        .Select(x => new
        {
            Tanggal = x.Key.TglBayarPokok,
            x.Key.PajakId,
            TotalRealisasi = x.Sum(q => q.JumlahBayarPokok)
        })
        .ToList();

                            foreach (var item in dataTargetWilayah)
                            {
                                var tanggal = new DateTime(tahun, bulan, Convert.ToInt32(item.Tgl));
                                var re = new DataHarian();
                                if (item.PajakId != null)
                                {
                                    switch ((EnumFactory.EPajak)item.PajakId)
                                    {
                                        case EPajak.MakananMinuman:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiResto.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.TenagaListrik:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiListrik.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaPerhotelan:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiHotel.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaParkir:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiParkir.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.JasaKesenianHiburan:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiHiburan.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.AirTanah:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiAbt.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.Reklame:
                                            break;
                                        case EPajak.PBB:
                                            re.Wilayah = $"UPTB {(int)wilayah} ";
                                            re.EnumWilayah = (int)wilayah;
                                            re.Tanggal = tanggal;
                                            re.Tahun = (int)bulan;
                                            re.Bulan = (int)tahun;
                                            re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                            re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                            re.Target = item.TotalTarget;
                                            re.Realisasi = totalRealisasiPbb.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;

                                            ret.Add(re);
                                            break;
                                        case EPajak.BPHTB:
                                            break;
                                        case EPajak.OpsenPkb:
                                            break;
                                        case EPajak.OpsenBbnkb:
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            ret = ret
                                .GroupBy(x => new
                                {
                                    x.Wilayah,
                                    x.EnumWilayah,
                                    x.Tanggal,
                                    x.Tahun,
                                    x.Bulan,
                                    x.JenisPajak,
                                    x.EnumPajak
                                }).Select(x => new DataHarian()
                                {
                                    Wilayah = x.Key.Wilayah,
                                    EnumWilayah = x.Key.EnumWilayah,
                                    Tanggal = x.Key.Tanggal,
                                    Tahun = x.Key.Tahun,
                                    Bulan = x.Key.Bulan,
                                    JenisPajak = x.Key.JenisPajak,
                                    EnumPajak = x.Key.EnumPajak,
                                    Target = x.Sum(q => q.Target),
                                    Realisasi = x.Sum(q => q.Realisasi)
                                }).ToList();
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
                                x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                                x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                               x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                                x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                                x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                                x.WilayahPajak,
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
                            .Where(w => Convert.ToInt32(w.WilayahPajak) == (int)wilayah && w.PajakId == (int)jenisPajak)
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
                        var dataPbbWilayah = context.DbMonPbbs
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.Uptb.ToString() ?? "", @"\d+").Value,
                                NamaOp = "-",            // PBB biasanya tidak ada, default "-"
                                AlamatOp = "-",          // PBB biasanya tidak ada, default "-"
                                KategoriNama = "-",
                                PajakId = 9m
                            })
                            .ToList();

                        // Ambil data realisasi bayar PBB per NOP, tanggal, pajak
                        var dataRealisasiWilayahPbbQuery = context.DbMonPbbs
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayar.HasValue &&
                                x.TglBayar.Value.Year == tanggal.Year &&
                                x.TglBayar.Value.Month <= tanggal.Month
                            )
                            .GroupBy(x => new { x.Nop, x.TglBayar, PajakId = 9m })
                            .Select(x => new
                            {
                                x.Key.Nop,
                                x.Key.TglBayar,
                                x.Key.PajakId,
                                Realisasi = x.Sum(q => q.JumlahBayarPokok)
                            })
                            .ToList();

                        // Filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbPbb = dataPbbWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // Ambil detail realisasi + join ke master dataPbbWilayah
                        var realisasiDetailPbb = dataRealisasiWilayahPbbQuery
                            .Where(x =>
                                x.TglBayar.Value.Month == tanggal.Month &&
                                x.TglBayar.Value.Day == tanggal.Day &&
                                x.TglBayar.Value.Year == tanggal.Year &&
                                x.PajakId == (int)jenisPajak &&
                                nopsUptbPbb.Contains(x.Nop)
                            )
                            .Select(r =>
                            {
                                var master = dataPbbWilayah.FirstOrDefault(m => m.Nop == r.Nop);
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
                            Detail = realisasiDetailPbb
                        };

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
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        result = context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        result = context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        result = context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        result = context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        result = context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.Reklame:
                        result = context.DbMonReklames
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        break;
                    case EnumFactory.EPajak.PBB:
                        result = context.DbMonPbbs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.JumlahBayarPokok) ?? 0;
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        result = context.DbMonBphtbs
                                .Where(x =>
                                    x.Tahun == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.Pokok) ?? 0;
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        result = context.DbMonOpsenPkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd.Date == DateTime.Now.Date
                                ).Sum(q => q.JmlPokok);
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        result = context.DbMonOpsenBbnkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd.Date == DateTime.Now.Date
                                ).Sum(q => q.JmlPokok);
                        break;
                    default:
                        result += context.DbMonRestos
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonPpjs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonHotels
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonParkirs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonHiburans
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonAbts
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonReklames
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.NominalPokokBayar) ?? 0;
                        result += context.DbMonPbbs
                                .Where(x =>
                                    x.TahunBuku == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.JumlahBayarPokok) ?? 0;
                        result += context.DbMonBphtbs
                                .Where(x =>
                                    x.Tahun == tahun &&
                                    x.TglBayar.HasValue &&
                                    x.TglBayar.Value.Date == DateTime.Now.Date
                                ).Sum(q => q.Pokok) ?? 0;
                        result += context.DbMonOpsenPkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd.Date == DateTime.Now.Date
                                ).Sum(q => q.JmlPokok);
                        result += context.DbMonOpsenBbnkbs
                                .Where(x =>
                                    x.TahunPajakSspd == tahun &&
                                    x.TglSspd.Date == DateTime.Now.Date
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
