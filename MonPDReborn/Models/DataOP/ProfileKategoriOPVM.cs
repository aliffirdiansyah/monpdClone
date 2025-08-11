using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileKategoriOPVM
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
            }
        }
        public class Show
        {
            public List<ProfileKategori> ProfileKategoriList { get; set; } = new();
            public Show(EnumFactory.EPajak jenisPajak)
            {
                ProfileKategoriList = Method.GetProfileKategoriList(jenisPajak);
            }
        }
        public class Method
        {
            public static List<ProfileKategori> GetProfileKategoriList(EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<ProfileKategori>();
                var context = DBClass.GetContext();

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto = context.DbOpRestos
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpResto = dataResto.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetResto = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiResto = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetResto;
                            re.RealisasiSdBulanIni = realisasiResto;
                            re.JumlahOP = listOpResto.Count;

                            ret.Add(re);
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
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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
            public static List<DetailProfileKategori> GetDetailProfileKategoriList(EnumFactory.EPajak jenisPajak, int kategoriId)
            {
                var ret = new List<DetailProfileKategori>();
                var context = DBClass.GetContext();

                var data = context.DbMonPjOps.Where(x => x.PajakId == (int)jenisPajak && x.KategoriId == kategoriId)
                    .Include(x => x.NipNavigation)
                    .Select(x => new
                    {
                        Nip = x.Nip,
                        NamaPj = x.NipNavigation.Nama,
                        Nop = x.Nop,
                        KategoriId = x.KategoriId
                    })
                    .ToList();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var listOpResto = data.Select(x => x.Nop).ToList();
                        var targetResto = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                        var realisasiResto = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                        ret = data
                            .GroupBy(x => new { x.Nip, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailProfileKategori
                            {
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.NamaPj,
                                TargetSdBulanIni = targetResto,
                                RealisasiSdBulanIni = realisasiResto,
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
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
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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
            public static List<DetailOPProfileKategori> GetDetailOPProfileKategoriList(EnumFactory.EPajak jenisPajak, int kategoriId, string nip)
            {
                var ret = new List<DetailOPProfileKategori>();
                var context = DBClass.GetContext();

                var data = context.DbMonPjOps.Where(x => x.PajakId == (int)jenisPajak && x.KategoriId == kategoriId && x.Nip == nip)
                    .Include(x => x.NipNavigation)
                    .Select(x => new
                    {
                        Nip = x.Nip,
                        NamaPj = x.NipNavigation.Nama,
                        Nop = x.Nop,
                        KategoriId = x.KategoriId
                    })
                    .ToList();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var listOpResto = data.Select(x => x.Nop).ToList();
                        var targetResto = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                        var realisasiResto = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpResto = context.DbOpRestos
                            .Where(x => data.Select(y =>y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                Nop = g.Key.Nop,
                                NamaOP = dbOpResto.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpResto.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetResto,
                                RealisasiSdBulanIni = realisasiResto
                            })
                            .ToList();
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
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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
            }
        }
        public class ProfileKategori
        {
            public int EnumPajak { get; set; }
            public int KategoriId { get; set; }
            public string Kategori { get; set; } = string.Empty;
            public decimal TargetSdBulanIni { get; set; }
            public decimal RealisasiSdBulanIni { get; set; }
            public decimal Selisih
            {
                get
                {
                    return TargetSdBulanIni - RealisasiSdBulanIni;
                }
            }
            public decimal Persentase
            {
                get
                {
                    return TargetSdBulanIni != 0
                        ? (RealisasiSdBulanIni / TargetSdBulanIni) * 100
                        : 0;
                }
            }
            public int JumlahOP { get; set; }
        }
        public class DetailProfileKategori
        {
            public int EnumPajak { get; set; }
            public int KategoriId { get; set; }
            public string PenanggungJawab { get; set; } = string.Empty;

            public decimal TargetSdBulanIni { get; set; }

            public decimal RealisasiSdBulanIni { get; set; }

            public decimal Selisih
            {
                get
                {
                    return TargetSdBulanIni - RealisasiSdBulanIni;
                }
            }

            public decimal Persentase
            {
                get
                {
                    return TargetSdBulanIni != 0
                        ? (RealisasiSdBulanIni / TargetSdBulanIni) * 100
                        : 0;
                }
            }

            public int JumlahOP { get; set; }

            public string Keterangan { get; set; } = string.Empty;
        }
        public class DetailOPProfileKategori
        {
            //public int EnumPajak { get; set; }
            //public int KategoriId { get; set; }
            public string Nop { get; set; } = string.Empty;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public string NamaOP { get; set; } = string.Empty;
            public string AlamatOP { get; set; } = string.Empty;
            public decimal TargetSdBulanIni { get; set; }
            public decimal RealisasiSdBulanIni { get; set; }
            public decimal Selisih
            {
                get
                {
                    return TargetSdBulanIni - RealisasiSdBulanIni;
                }
            }
            public decimal Persentase
            {
                get
                {
                    return TargetSdBulanIni != 0
                        ? (RealisasiSdBulanIni / TargetSdBulanIni) * 100
                        : 0;
                }
            }
        }

    }
}
