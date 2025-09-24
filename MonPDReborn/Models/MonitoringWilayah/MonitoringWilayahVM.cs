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
            public bool Uptb { get; set; } = false;
            public Index()
            {
                SelectedBulan = DateTime.Now.Month;
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
            public Index(int wilayah, int jenisPajak)
            {
                Uptb = true;
                SelectedBulan = 12;
                SelectedUPTB = wilayah;
                SelectedPajak = jenisPajak;
                if (wilayah == 0)
                {
                    JenisUptbList = Enum.GetValues(typeof(EnumFactory.EUPTB))
                    .Cast<EnumFactory.EUPTB>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                }
                else
                {
                    JenisUptbList = Enum.GetValues(typeof(EnumFactory.EUPTB))
                    .Cast<EnumFactory.EUPTB>()
                    .Where(x => x == (EnumFactory.EUPTB)wilayah)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                }


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

                if (jenisPajak == EnumFactory.EPajak.Semua)
                {
                    RealisasiJenisList = Method.GetDataRealisasiJenisList(tahun, bulan, (EnumFactory.EUPTB)wilayah);
                    TotalWajibPajak = RealisasiJenisList.Sum(x => x.JmlWP);
                }
                else
                {
                    TotalWajibPajak = RealisasiWilayahList.Sum(x => x.JmlWP);
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
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpResto = context.DbOpRestos
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                             .Count();
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
                                    Status = "",
                                    JmlWP = jmlWpResto
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpResto = context.DbOpRestos
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();
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
                                Status = "",
                                JmlWP = jmlWpResto
                            });
                        }

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpPpj = context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                            .Count();
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
                                    Status = "",
                                    JmlWP = jmlWpPpj
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpPpj = context.DbOpListriks
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();
                            var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && x.Sumber == 55)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var totalTarget = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == (int)wilayah && x.SubRincian == "2")
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
                                Status = "",
                                JmlWP = jmlWpPpj
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpHotel = context.DbOpHotels
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                             .Count();
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
                                    Status = "",
                                    JmlWP = jmlWpHotel
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpHotel = context.DbOpHotels
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();
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
                                Status = "",
                                JmlWP = jmlWpHotel
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpParkir = context.DbOpParkirs
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                             .Count();
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
                                    Status = "",
                                    JmlWP = jmlWpParkir
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpParkir = context.DbOpParkirs
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();

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
                                Status = "",
                                JmlWP = jmlWpParkir
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpHiburan = context.DbOpHiburans
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                             .Count();

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
                                    Status = "",
                                    JmlWP = jmlWpHiburan
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpHiburan = context.DbOpHiburans
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();
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
                                Status = "",
                                JmlWP = jmlWpHiburan
                            });
                        }

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpAbt = context.DbOpAbts
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                             .Count();
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
                                    Status = "",
                                    JmlWP = jmlWpAbt
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpAbt = context.DbOpAbts
                             .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == Convert.ToString((int)wilayah))
                             .Count();
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
                                Status = "",
                                JmlWP = jmlWpAbt
                            });
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:

                        break;
                    case EnumFactory.EPajak.PBB:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpPbb = context.DbOpPbbs.Count();
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var totalTarget = context.DbAkunTargetBulanUptbs
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && x.Uptb == Convert.ToInt32(uptb))
                                    .Sum(x => x.Target);

                                var nopListPbb = context.DbMonPbbs
                                    .Where(x => x.TahunBuku == tahun && x.Uptb == Convert.ToInt32(uptb))
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .AsQueryable();

                                var totalRealisasi = context.DbMonPbbs
                                        .Where(x =>
                                            x.TglBayar.HasValue
                                            && x.TahunBuku == tahun
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && nopListPbb.Contains(x.Nop)
                                        ).Sum(q => q.JumlahBayarPokok);

                                ret.Add(new RealisasiWilayah
                                {
                                    Wilayah = $"UPTB {uptb}",
                                    Tahun = tahun,
                                    Lokasi = $"UPTB {uptb}",
                                    Target = totalTarget,
                                    Realisasi = totalRealisasi ?? 0,
                                    Tren = 0,
                                    Status = "",
                                    JmlWP = jmlWpPbb
                                });
                            }
                        }
                        else if (wilayah != EnumFactory.EUPTB.SEMUA)
                        {
                            var jmlWpPbb = context.DbOpPbbs.Where(x => x.Uptb == (decimal)wilayah).Count();
                            var dataPbbWilayah = context.DbMonPbbs
                                 .Where(x => x.TglBayar.Value.Year == tahun && Convert.ToInt32(x.Uptb) == (int)wilayah)
                                 .Select(x => new
                                 {
                                     x.Nop,
                                     WilayahPajak = x.Uptb,
                                     TahunBuku = x.TahunBuku
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
                                .Where(x => x.TahunBuku == tahun && Convert.ToInt32(x.WilayahPajak) == (int)wilayah)
                                .Select(x => x.Nop)
                                .AsEnumerable();

                            var dataRealisasiWilayah = context.DbMonPbbs
                                .Where(x =>
                                    x.TglBayar.HasValue &&
                                    x.TahunBuku == tahun &&
                                    x.TglBayar.Value.Year == tahun &&
                                    x.TglBayar.Value.Month <= bulan
                                    && x.JumlahBayarPokok > 0 &&
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
                                .Where(x => x.TglBayarPokok.Value.Month <= bulan)
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
                                re.JmlWP = jmlWpPbb;

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
                                var nopListPbb = context.DbMonPbbs
                                        .Where(x => x.TahunBuku == tahun && x.Uptb == Convert.ToInt32(uptb))
                                        .Select(x => x.Nop)
                                        .Distinct()
                                        .AsQueryable();

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
                                            && x.TahunBuku == tahun
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && nopListPbb.Contains(x.Nop)
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
                                                re.Realisasi = totalRealisasiResto ?? 0;
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
                                    .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.Uptb == (int)wilayah && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21 && (x.PajakId != 2 || x.SubRincian == "2"))
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
                            var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && x.Sumber == 55)
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                    .Select(x => x.Nop).Distinct().ToList();
                            var nopListPbb = context.DbMonPbbs
                                    .Where(x => x.TahunBuku == tahun && x.Uptb == Convert.ToInt32(wilayah))
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .AsQueryable();

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
                                        && x.TahunBuku == tahun
                                        && x.TglBayar.Value.Year == tahun
                                        && x.TglBayar.Value.Month <= bulan
                                        && nopListPbb.Contains(x.Nop)
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
                                            re.Realisasi = totalRealisasiResto ?? 0;
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
            public static List<RealisasiJenis> GetDataRealisasiJenisList(int tahun, int bulan, EnumFactory.EUPTB wilayah)
            {
                var result = new List<RealisasiJenis>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                if (wilayah == EUPTB.SEMUA)
                {
                    var targetPajak = context.DbAkunTargetBulanUptbs
                        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan)
                        .GroupBy(x => new { x.PajakId })
                        .Select(x => new
                        {
                            x.Key.PajakId,
                            Target = x.Sum(q => q.Target)
                        }).ToList();

                    var jmlWpAbt = context.DbOpAbts
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpResto = context.DbOpRestos
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpHotel = context.DbOpHotels
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpPpj = context.DbOpListriks
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpParkir = context.DbOpParkirs
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpHiburan = context.DbOpHiburans
                         .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .AsQueryable();

                    var jmlWpPbb = context.DbOpPbbs.AsQueryable();

                    var realisasiPajakAbt = context.DbMonAbts
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpAbt.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakResto = context.DbMonRestos
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpResto.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakHotel = context.DbMonHotels
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpHotel.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakPpj = context.DbMonPpjs
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpPpj.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakParkir = context.DbMonParkirs
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpParkir.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakHiburan = context.DbMonHiburans
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && jmlWpHiburan.Select(y => y.Nop).ToList().Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakPbb = context.DbMonPbbs
                            .Where(x =>
                                x.TglBayar.HasValue
                                && x.TahunBuku == tahun
                                && x.TglBayar.Value.Year == tahun
                                && x.TglBayar.Value.Month <= bulan
                                && x.JumlahBayarPokok > 0
                            ).Sum(q => q.JumlahBayarPokok) ?? 0;

                    foreach (var item in targetPajak)
                    {
                        if (item.PajakId != null)
                        {
                            var ret = new RealisasiJenis();
                            switch ((EnumFactory.EPajak)item.PajakId)
                            {
                                case EPajak.MakananMinuman:
                                    ret.Tahun = tahun;
                                    ret.Bulan = bulan;
                                    ret.JenisPajak = (EnumFactory.EPajak.MakananMinuman).GetDescription();
                                    ret.JmlWP = jmlWpResto.Count();
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
                                    ret.JmlWP = jmlWpPpj.Count();
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
                                    ret.JmlWP = jmlWpHotel.Count();
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
                                    ret.JmlWP = jmlWpParkir.Count();
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
                                    ret.JmlWP = jmlWpHiburan.Count();
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
                                    ret.JmlWP = jmlWpAbt.Count();
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
                                    ret.JmlWP = jmlWpPbb.Count();
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
                else
                {
                    int uptb = (int)wilayah;

                    var targetPajak = context.DbAkunTargetBulanUptbs
                        .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.Uptb == uptb && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21 && (x.PajakId != 2 || x.SubRincian == "2"))
                        .GroupBy(x => new { x.PajakId })
                        .Select(x => new
                        {
                            x.Key.PajakId,
                            Target = x.Sum(q => q.Target)
                        }).ToList();

                    var jmlWpAbt = context.DbOpAbts
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpResto = context.DbOpRestos
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpHotel = context.DbOpHotels
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpPpj = context.DbOpListriks
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpParkir = context.DbOpParkirs
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpHiburan = context.DbOpHiburans
                         .Where(x => x.TahunBuku == tahun && x.WilayahPajak == uptb.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                         .Count();

                    var jmlWpPbb = context.DbOpPbbs.Where(x => x.Uptb == uptb).Count();

                    var nopABT = context.DbOpAbts.Where(x => x.WilayahPajak == uptb.ToString())
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopResto = context.DbOpRestos.Where(x => x.WilayahPajak == uptb.ToString())
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopHotel = context.DbOpHotels.Where(x => x.WilayahPajak == uptb.ToString())
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopPpj = context.DbOpListriks.Where(x => x.WilayahPajak == uptb.ToString() && x.Sumber == 55)
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == uptb.ToString())
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == uptb.ToString())
                        .Select(x => x.Nop)
                        .AsQueryable();

                    var nopListPbb = context.DbMonPbbs
                        .Where(x => x.TahunBuku == tahun && x.Uptb == Convert.ToInt32(uptb))
                        .Select(x => x.Nop)
                        .Distinct()
                        .AsQueryable();

                    var realisasiPajakAbt = context.DbMonAbts
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopABT.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakResto = context.DbMonRestos
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopResto.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakHotel = context.DbMonHotels
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopHotel.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakPpj = context.DbMonPpjs
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopPpj.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakParkir = context.DbMonParkirs
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopParkir.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakHiburan = context.DbMonHiburans
                            .Where(x =>
                                x.TglBayarPokok.HasValue
                                && x.TglBayarPokok.Value.Year == tahun
                                && x.TglBayarPokok.Value.Month <= bulan
                                && nopHiburan.Contains(x.Nop)
                            ).Sum(q => q.NominalPokokBayar) ?? 0;

                    var realisasiPajakPbb = context.DbMonPbbs
                            .Where(x =>
                                x.TglBayar.HasValue
                                && x.TglBayar.Value.Year == tahun
                                && x.TglBayar.Value.Month <= bulan
                                && nopListPbb.Contains(x.Nop)
                            ).Sum(q => q.JumlahBayarPokok) ?? 0;

                    foreach (var item in targetPajak)
                    {
                        if (item.PajakId != null)
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
            }
            public static List<DataHarian> GetDataDataHarianList(EnumFactory.EUPTB wilayah, int tahun, int bulan, EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DataHarian>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        if (wilayah == EnumFactory.EUPTB.SEMUA)
                        {
                            var uptbList = context.MWilayahs.Select(x => x.Uptd).Distinct().ToList();

                            foreach (var uptb in uptbList)
                            {
                                var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == uptb && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                    result.JenisPajak = ((jenisPajak)).GetDescription();
                                    result.EnumPajak = (int)(jenisPajak);
                                    result.Target = dataTargetWilayahResto.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahResto.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                result.JenisPajak = ((jenisPajak)).GetDescription();
                                result.EnumPajak = (int)(jenisPajak);
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
                                var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == uptb && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                    result.JenisPajak = ((jenisPajak)).GetDescription();
                                    result.EnumPajak = (int)(jenisPajak);
                                    result.Target = dataTargetWilayahListrik.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahListrik.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.Sumber == 55)
                                    .Select(x => x.Nop)
                                    .Distinct()
                                    .ToList();

                            var dataTargetWilayahListrik = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.Bulan <= bulan && x.PajakId == (decimal)jenisPajak && Convert.ToInt32(x.Uptb) == (int)wilayah && x.SubRincian == "2")
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
                                result.JenisPajak = ((jenisPajak)).GetDescription();
                                result.EnumPajak = (int)(jenisPajak);
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
                                var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == uptb && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                    result.JenisPajak = ((jenisPajak)).GetDescription();
                                    result.EnumPajak = (int)(jenisPajak);
                                    result.Target = dataTargetWilayahHotel.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahHotel.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                result.JenisPajak = ((jenisPajak)).GetDescription();
                                result.EnumPajak = (int)(jenisPajak);
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
                                var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == uptb && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                    result.JenisPajak = ((jenisPajak)).GetDescription();
                                    result.EnumPajak = (int)(jenisPajak);
                                    result.Target = dataTargetWilayahParkir.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahParkir.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                result.JenisPajak = ((jenisPajak)).GetDescription();
                                result.EnumPajak = (int)(jenisPajak);
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
                                var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == uptb && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                    result.JenisPajak = ((jenisPajak)).GetDescription();
                                    result.EnumPajak = (int)(jenisPajak);
                                    result.Target = dataTargetWilayahHiburan.Where(x => x.Tgl == tanggal.Day && x.Bulan == tanggal.Month && x.TahunBuku == tanggal.Year).Sum(q => q.TotalTarget);
                                    result.Realisasi = dataRealisasiWilayahHiburan.Where(x => x.Tanggal == tanggal).Sum(q => q.TotalRealisasi) ?? 0;


                                    ret.Add(result);
                                }
                            }
                        }
                        else
                        {
                            var nopList = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
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
                                result.JenisPajak = ((jenisPajak)).GetDescription();
                                result.EnumPajak = (int)(jenisPajak);
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
                                    JenisPajak = (jenisPajak).GetDescription(),
                                    EnumPajak = (int)jenisPajak,
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
                                    JenisPajak = (jenisPajak).GetDescription(),
                                    EnumPajak = (int)jenisPajak,
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
                                    result.JenisPajak = (jenisPajak).GetDescription();
                                    result.EnumPajak = (int)jenisPajak;
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
                                result.JenisPajak = (jenisPajak).GetDescription();
                                result.EnumPajak = (int)jenisPajak;
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
                                    .ToList(); // ✅ eksekusi query sekarang

                                // Pastikan semua NOP list tidak null
                                var nopListAbt = context.DbOpAbts.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();
                                var nopListResto = context.DbOpRestos.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();
                                var nopListHotel = context.DbOpHotels.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();
                                var nopListListrik = context.DbOpListriks.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();
                                var nopListParkir = context.DbOpParkirs.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();
                                var nopListHiburan = context.DbOpHiburans.Where(x => x.WilayahPajak == ((int)uptb).ToString())
                                        .Select(x => x.Nop).Distinct().ToList() ?? new List<string>();

                                // Grouping realisasi + DefaultIfEmpty untuk mencegah error jika kosong
                                var totalRealisasiAbt = context.DbMonAbts
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListAbt.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 6 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiResto = context.DbMonRestos
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListResto.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 1 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiHotel = context.DbMonHotels
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHotel.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 3 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiListrik = context.DbMonPpjs
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListListrik.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 2 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiParkir = context.DbMonParkirs
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListParkir.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 4 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiHiburan = context.DbMonHiburans
                                        .Where(x => x.TglBayarPokok.HasValue
                                            && x.TglBayarPokok.Value.Year == tahun
                                            && x.TglBayarPokok.Value.Month <= bulan
                                            && nopListHiburan.Contains(x.Nop))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Date, PajakId = 5 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.NominalPokokBayar)
                                        })
                                        .ToList();

                                var totalRealisasiPbb = context.DbMonPbbs
                                        .Where(x => x.TglBayar.HasValue
                                            && x.TahunBuku == tahun
                                            && x.TglBayar.Value.Year == tahun
                                            && x.TglBayar.Value.Month <= bulan
                                            && x.JumlahBayarPokok > 0
                                            && x.Uptb == Convert.ToInt32(uptb))
                                        .GroupBy(x => new { TglBayarPokok = x.TglBayar.Value.Date, PajakId = 9 })
                                        .Select(x => new
                                        {
                                            Tanggal = x.Key.TglBayarPokok,
                                            x.Key.PajakId,
                                            TotalRealisasi = x.Sum(q => q.JumlahBayarPokok)
                                        })
                                        .ToList();

                                foreach (var item in dataTargetWilayah)
                                {
                                    // ✅ Validasi Tgl sebelum buat DateTime
                                    if (item.Tgl == null || item.Tgl <= 0 || item.Tgl > totalTanggal)
                                        continue;

                                    var tanggal = new DateTime(tahun, bulan, (int)item.Tgl);
                                    var re = new DataHarian();

                                    if (item.PajakId != null)
                                    {
                                        switch ((EnumFactory.EPajak)item.PajakId)
                                        {
                                            case EPajak.MakananMinuman:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiResto.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.TenagaListrik:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiListrik.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.JasaPerhotelan:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiHotel.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.JasaParkir:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiParkir.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.JasaKesenianHiburan:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiHiburan.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.AirTanah:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiAbt.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;

                                            case EPajak.Reklame:
                                                break;

                                            case EPajak.PBB:
                                                re.Wilayah = $"UPTB {(int)uptb}";
                                                re.EnumWilayah = (int)uptb;
                                                re.Tanggal = tanggal;
                                                re.Tahun = tahun;
                                                re.Bulan = bulan;
                                                re.JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription();
                                                re.EnumPajak = (int)(EnumFactory.EPajak)item.PajakId;
                                                re.Target = item.TotalTarget;
                                                re.Realisasi = (decimal)totalRealisasiPbb.Where(x => x.Tanggal == tanggal)
                                                    .Select(q => q.TotalRealisasi).DefaultIfEmpty(0).Sum();
                                                ret.Add(re);
                                                break;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Bulan <= bulan &&
                                            Convert.ToInt32(x.Uptb) == Convert.ToInt32(wilayah) &&
                                            x.PajakId != 7 && x.PajakId != 12 &&
                                            x.PajakId != 20 && x.PajakId != 21 &&
                                            (x.PajakId != 2 || x.SubRincian == "2"))
                                .GroupBy(x => new { x.Uptb, x.PajakId, x.Tgl, x.Bulan, x.TahunBuku })
                                .Select(g => new
                                {
                                    g.Key.Uptb,
                                    g.Key.Tgl,
                                    g.Key.Bulan,
                                    g.Key.TahunBuku,
                                    g.Key.PajakId,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .AsQueryable();

                            var nopListAbt = context.DbOpAbts
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var nopListResto = context.DbOpRestos
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var nopListHotel = context.DbOpHotels
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var nopListListrik = context.DbOpListriks
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() &&
                                           x.Sumber == 55)
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var nopListParkir = context.DbOpParkirs
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            var nopListHiburan = context.DbOpHiburans
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                            // 🔽 Helper untuk ambil total realisasi per PajakId
                            List<(DateTime Tanggal, int PajakId, decimal Total)> GetRealisasi<T>(
                                IQueryable<T> query,
                                Func<T, bool> whereCondition,
                                Func<T, DateTime> tanggalSelector,
                                int pajakId,
                                Func<T, decimal> nominalSelector)
                            {
                                return query
                                    .Where(whereCondition)
                                    .GroupBy(x => tanggalSelector(x).Date)
                                    .Select(g => (Tanggal: g.Key, PajakId: pajakId, Total: g.Sum(nominalSelector)))
                                    .ToList();
                            }

                            // 🔽 Ambil realisasi masing-masing pajak
                            var totalRealisasiAbt = GetRealisasi(context.DbMonAbts,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListAbt.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 6, x => (decimal)x.NominalPokokBayar);

                            var totalRealisasiResto = GetRealisasi(context.DbMonRestos,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListResto.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 1, x => (decimal)x.NominalPokokBayar);

                            var totalRealisasiHotel = GetRealisasi(context.DbMonHotels,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListHotel.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 3, x => (decimal)x.NominalPokokBayar);

                            var totalRealisasiListrik = GetRealisasi(context.DbMonPpjs,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListListrik.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 2, x => (decimal)x.NominalPokokBayar);

                            var totalRealisasiParkir = GetRealisasi(context.DbMonParkirs,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListParkir.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 4, x => (decimal)x.NominalPokokBayar);

                            var totalRealisasiHiburan = GetRealisasi(context.DbMonHiburans,
                                x => x.TglBayarPokok.HasValue &&
                                     x.TglBayarPokok.Value.Year == tahun &&
                                     x.TglBayarPokok.Value.Month <= bulan &&
                                     nopListHiburan.Contains(x.Nop),
                                x => x.TglBayarPokok.Value, 5, x => (decimal)x.NominalPokokBayar);


                            foreach (var item in dataTargetWilayah)
                            {
                                var tanggal = new DateTime(tahun, (int)item.Bulan, Convert.ToInt32(item.Tgl));
                                var re = new DataHarian
                                {
                                    Wilayah = $"UPTB {(int)wilayah}",
                                    EnumWilayah = (int)wilayah,
                                    Tanggal = tanggal,
                                    Tahun = tahun,
                                    Bulan = (int)item.Bulan,
                                    JenisPajak = ((EnumFactory.EPajak)item.PajakId).GetDescription(),
                                    EnumPajak = (int)(EnumFactory.EPajak)item.PajakId,
                                    Target = item.TotalTarget
                                };

                                switch ((EnumFactory.EPajak)item.PajakId)
                                {
                                    case EPajak.MakananMinuman:
                                        re.Realisasi = totalRealisasiResto.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    case EPajak.TenagaListrik:
                                        re.Realisasi = totalRealisasiListrik.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    case EPajak.JasaPerhotelan:
                                        re.Realisasi = totalRealisasiHotel.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    case EPajak.JasaParkir:
                                        re.Realisasi = totalRealisasiParkir.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    case EPajak.JasaKesenianHiburan:
                                        re.Realisasi = totalRealisasiHiburan.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    case EPajak.AirTanah:
                                        re.Realisasi = totalRealisasiAbt.Where(x => x.Tanggal == tanggal).Sum(x => x.Total);
                                        break;
                                    default:
                                        re.Realisasi = 0;
                                        break;
                                }

                                ret.Add(re);
                            }

                           /* // 🔽 Grouping akhir untuk menggabungkan data duplikat
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
                                })
                                .Select(g => new DataHarian
                                {
                                    Wilayah = g.Key.Wilayah,
                                    EnumWilayah = g.Key.EnumWilayah,
                                    Tanggal = g.Key.Tanggal,
                                    Tahun = g.Key.Tahun,
                                    Bulan = g.Key.Bulan,
                                    JenisPajak = g.Key.JenisPajak,
                                    EnumPajak = g.Key.EnumPajak,
                                    Target = g.Sum(x => x.Target),
                                    Realisasi = g.Sum(x => x.Realisasi)
                                })
                                .ToList();*/

                        }
                        break;
                }
                return ret;
            }
            public static DataModal GetDataModalRow(EnumFactory.EUPTB wilayah, DateTime tanggal, EnumFactory.EPajak jenisPajak)
            {
                var ret = new DataModal();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var nopList = wilayah == EnumFactory.EUPTB.SEMUA
                        ? context.DbOpRestos
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList()
                        : context.DbOpRestos
                            .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasi = context.DbMonRestos
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopList.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        // 3. Ambil OP unik per NOP
                        var opList = context.DbOpRestos
                            .Where(x => nopList.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetail = opList
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasi.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();



                        // 4. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetail
                        };

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        // 1. Ambil daftar NOP sesuai wilayah
                        var nopListPPJ = wilayah == EnumFactory.EUPTB.SEMUA
                            ? context.DbOpListriks
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList()
                            : context.DbOpListriks
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasiPPJ = context.DbMonPpjs
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopListPPJ.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        var opListPPJ = context.DbOpListriks
                            .Where(x => nopListPPJ.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailPPJ = opListPPJ
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasiPPJ.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailPPJ
                        };
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        // 1. Ambil daftar NOP sesuai wilayah
                        var nopListHotel = wilayah == EnumFactory.EUPTB.SEMUA
                            ? context.DbOpHotels
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList()
                            : context.DbOpHotels
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasiHotel = context.DbMonHotels
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopListHotel.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        var opListHotel = context.DbOpHotels
                            .Where(x => nopListHotel.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailHotel = opListHotel
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasiHotel.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailHotel
                        };

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        // 1. Ambil daftar NOP sesuai wilayah
                        var nopListParkir = wilayah == EnumFactory.EUPTB.SEMUA
                            ? context.DbOpParkirs
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList()
                            : context.DbOpParkirs
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasiParkir = context.DbMonParkirs
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopListParkir.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        var opListParkir = context.DbOpParkirs
                            .Where(x => nopListParkir.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailParkir = opListParkir
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasiParkir.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailParkir
                        };

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        // 1. Ambil daftar NOP sesuai wilayah
                        var nopListHiburan = wilayah == EnumFactory.EUPTB.SEMUA
                            ? context.DbOpHiburans
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList()
                            : context.DbOpHiburans
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasiHiburan = context.DbMonHiburans
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopListHiburan.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        var opListHiburan = context.DbOpHiburans
                            .Where(x => nopListHiburan.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailHiburan = opListHiburan
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasiHiburan.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailHiburan
                        };

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        // 1. Ambil daftar NOP sesuai wilayah
                        var nopListABT = wilayah == EnumFactory.EUPTB.SEMUA
                            ? context.DbOpAbts
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList()
                            : context.DbOpAbts
                                .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear))
                                .Select(x => x.Nop)
                                .Distinct()
                                .ToList();

                        // 2. Ambil realisasi per NOP di tanggal tsb
                        var dataRealisasiABT = context.DbMonAbts
                            .Where(x =>
                                x.TglBayarPokok.HasValue &&
                                x.TglBayarPokok.Value.Year == tanggal.Year &&
                                x.TglBayarPokok.Value.Month == tanggal.Month &&
                                x.TglBayarPokok.Value.Day == tanggal.Day &&
                                nopListABT.Contains(x.Nop)
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.NominalPokokBayar)
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Join ke master OP (agar tetap keluar meski realisasi 0)
                        var opListABT = context.DbOpAbts
                            .Where(x => nopListABT.Contains(x.Nop))
                            .GroupBy(x => x.Nop)
                            .Select(g => g.FirstOrDefault()) // ambil satu saja per NOP
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailABT = opListABT
                            .Select(op => new DataDetailModal
                            {
                                NOP = op.Nop,
                                NamaOP = op.NamaOp,
                                AlamatOP = op.AlamatOp,
                                KategoriNama = op.KategoriNama,
                                Realisasi = dataRealisasiABT.FirstOrDefault(r => r.Nop == op.Nop)?.TotalRealisasi ?? 0
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = jenisPajak.GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailABT
                        };

                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        // 1. Ambil data master PBB berdasarkan TahunBuku
                        var dataPbbWilayah = context.DbMonPbbs
                            .Where(x => x.TahunBuku == tanggal.Year)
                            .Select(x => new
                            {
                                x.Nop,
                                Wilayah = Regex.Match(x.Uptb.ToString() ?? "", @"\d+").Value,
                                NamaOp = x.WpNama ?? "-",            // default untuk PBB
                                AlamatOp = x.AlamatOp ?? "-",          // default untuk PBB
                                KategoriNama = x.Peruntukan ?? "-",      // default untuk PBB
                                PajakId = 9m             // fix PajakId PBB
                            })
                            .ToList();

                        // 2. Ambil data realisasi bayar PBB per NOP, tanggal, pajak
                        var dataRealisasiPBB = context.DbMonPbbs
                            .Where(x =>
                                x.TahunBuku == tanggal.Year &&
                                x.TglBayar.HasValue &&
                                x.TglBayar.Value.Year == tanggal.Year &&
                                x.TglBayar.Value.Month == tanggal.Month &&
                                x.TglBayar.Value.Day == tanggal.Day
                            )
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                TotalRealisasi = g.Sum(q => q.JumlahBayarPokok),
                                PajakId = 9m
                            })
                            .ToList(); // <-- pindah ke memory

                        // 3. Filter NOP sesuai UPTB & jenis pajak
                        var nopsUptbPbb = dataPbbWilayah
                            .Where(w => Convert.ToInt32(w.Wilayah) == (int)wilayah && w.PajakId == (int)jenisPajak)
                            .Select(w => w.Nop)
                            .ToList();

                        // 4. Mapping ke DataDetailModal
                        var dataDetailPBB = dataRealisasiPBB
                            .Where(r =>
                                r.PajakId == (int)jenisPajak &&
                                nopsUptbPbb.Contains(r.Nop)
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
                                    Realisasi = r.TotalRealisasi ?? 0,
                                };
                            })
                            .Where(d => d.Realisasi > 0)
                            .ToList();

                        // 5. Isi ke DataModal
                        ret = new DataModal
                        {
                            Wilayah = wilayah == EnumFactory.EUPTB.SEMUA ? "SEMUA WILAYAH" : $"UPTB {(int)wilayah}",
                            EnumWilayah = (int)wilayah,
                            JenisPajak = ((EnumFactory.EPajak)jenisPajak).GetDescription(),
                            Tanggal = tanggal,
                            Tahun = tanggal.Year,
                            Bulan = tanggal.Month,
                            Detail = dataDetailPBB
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
            public int JmlWP { get; set; }
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
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NamaOP { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string KategoriNama { get; set; } = null!;
            public decimal Realisasi { get; set; }

        }

    }
}
