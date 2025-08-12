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
                //JenisPajakList.Add(new SelectListItem { Value = "0", Text = "Semua Jenis Pajak" });
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
                //JenisPajakList.Add(new SelectListItem { Value = "0", Text = "Semua Jenis Pajak" });
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

        public class DetailKategoriProfile
        {
            public List<DetailProfileKategori> Data { get; set; }
            public DetailKategoriProfile(EnumFactory.EPajak jenisPajak, int kategoriId)
            {
                Data = Method.GetDetailProfileKategoriList(jenisPajak, kategoriId);
            }
        }

        public class DetailKategoriPorfileOP
        {
            public List<DetailOPProfileKategori> Data { get; set; }
            public DetailKategoriPorfileOP(EnumFactory.EPajak jenisPajak, int kategoriId, string nip)
            {
                Data = Method.GetDetailOPProfileKategoriList(jenisPajak, kategoriId, nip);
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
                            var targetResto = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && x.BulanBuku <= DateTime.Now.Month && listOpResto.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
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
                        var dataListrik = context.DbOpListriks
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpListrik = dataListrik.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetListrik = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpListrik.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpListrik.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetListrik;
                            re.RealisasiSdBulanIni = realisasiListrik;
                            re.JumlahOP = listOpListrik.Count;

                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel = context.DbOpHotels
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList.OrderBy(x => x.Id).ToList())
                        {
                            var listOpHotel = dataHotel.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetHotel = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpHotel.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetHotel;
                            re.RealisasiSdBulanIni = realisasiHotel;
                            re.JumlahOP = listOpHotel.Count;

                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir = context.DbOpParkirs
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpParkir = dataParkir.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetParkir = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpParkir.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpParkir.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetParkir;
                            re.RealisasiSdBulanIni = realisasiParkir;
                            re.JumlahOP = listOpParkir.Count;

                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbOpHiburans
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpHiburan = dataHiburan.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetHiburan = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpHiburan.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetHiburan;
                            re.RealisasiSdBulanIni = realisasiHiburan;
                            re.JumlahOP = listOpHiburan.Count;

                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt = context.DbOpAbts
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpAbt = dataAbt.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetAbt = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpAbt.Contains(x.Nop)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpAbt.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetAbt;
                            re.RealisasiSdBulanIni = realisasiAbt;
                            re.JumlahOP = listOpAbt.Count;

                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataReklame = context.DbOpReklames
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year))))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();
                        foreach (var item in kategoriList)
                        {
                            var listOpReklame = dataReklame.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var targetReklame = context.DbAkunTargetObjekReklames.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpReklame.Contains(x.Nor)).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpReklame.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var re = new ProfileKategori();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.Kategori = item.Nama;
                            re.TargetSdBulanIni = targetReklame;
                            re.RealisasiSdBulanIni = realisasiReklame;
                            re.JumlahOP = listOpReklame.Count;

                            ret.Add(re);
                        }
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
                        ret =
                            (from r in context.DbOpRestos
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekRestos
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonRestos
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        ret =
                            (from r in context.DbOpListriks
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekPpjs
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonPpjs
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret =
                            (from r in context.DbOpHotels
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekHotels
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonHotels
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        ret =
                            (from r in context.DbOpParkirs
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekParkirs
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonParkirs
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        ret =
                            (from r in context.DbOpHiburans
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekHiburans
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonHiburans
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        ret =
                            (from r in context.DbOpAbts
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekAbts
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nop == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonAbts
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:
                        ret =
                            (from r in context.DbOpReklames
                             join pj in context.DbMonPjOps on r.Nop equals pj.Nop
                             where r.TahunBuku == DateTime.Now.Year
                                   && (r.TglOpTutup == null || r.TglOpTutup.Value.Year > DateTime.Now.Year)
                                   && r.KategoriId == kategoriId
                                   && pj.PajakId == (int)jenisPajak
                             select new
                             {
                                 r.Nop,
                                 pj.KategoriId,
                                 pj.Nip,
                                 pj.NipNavigation.Nama,
                                 Target = context.DbAkunTargetObjekReklames
                                     .Where(t => t.TahunBuku == DateTime.Now.Year && t.BulanBuku <= DateTime.Now.Month && t.Nor == r.Nop)
                                     .Sum(t => (decimal?)t.TargetBulan) ?? 0,
                                 Realisasi = context.DbMonReklames
                                     .Where(m => m.TglBayarPokok.HasValue &&
                                                 m.TglBayarPokok.Value.Year == DateTime.Now.Year &&
                                                 m.Nop == r.Nop)
                                     .Sum(m => (decimal?)m.NominalPokokBayar) ?? 0
                             })
                            .AsEnumerable()
                            .GroupBy(x => new { x.KategoriId, x.Nama, x.Nip })
                            .Select(g => new DetailProfileKategori
                            {
                                Nip = g.Key.Nip,
                                EnumPajak = (int)jenisPajak,
                                KategoriId = g.Key.KategoriId,
                                PenanggungJawab = g.Key.Nama,
                                TargetSdBulanIni = g.Sum(i => i.Target),
                                RealisasiSdBulanIni = g.Sum(i => i.Realisasi),
                                JumlahOP = g.Count(),
                                Keterangan = string.Empty
                            })
                            .ToList();
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
                        var targetResto = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpResto.Contains(x.Nop)).ToList();
                        var realisasiResto = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpResto = context.DbOpRestos
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpResto.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpResto.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetResto.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiResto
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var listOpListrik = data.Select(x => x.Nop).ToList();
                        var targetListrik = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpListrik.Contains(x.Nop)).ToList();
                        var realisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpListrik.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpListrik = context.DbOpListriks
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpListrik.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpListrik.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetListrik.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiListrik
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var listOpHotel = data.Select(x => x.Nop).ToList();
                        var targetHotel = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpHotel.Contains(x.Nop)).ToList();
                        var realisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpHotel = context.DbOpHotels
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpHotel.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpHotel.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetHotel.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiHotel
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var listOpParkir = data.Select(x => x.Nop).ToList();
                        var targetParkir = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpParkir.Contains(x.Nop)).ToList();
                        var realisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpParkir.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpParkir = context.DbOpParkirs
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpParkir.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpParkir.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetParkir.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiParkir
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var listOpHiburan = data.Select(x => x.Nop).ToList();
                        var targetHiburan = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpHiburan.Contains(x.Nop)).ToList();
                        var realisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpHiburan = context.DbOpHiburans
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpHiburan.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpHiburan.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetHiburan.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiHiburan
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var listOpAbt = data.Select(x => x.Nop).ToList();
                        var targetAbt = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpAbt.Contains(x.Nop)).ToList();
                        var realisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpAbt.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpAbt = context.DbOpAbts
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpAbt.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.NamaOp)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpAbt.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.AlamatOp)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetAbt.Where(p => p.Nop == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiAbt
                            })
                            .ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var listOpReklame = data.Select(x => x.Nop).ToList();
                        var targetReklame = context.DbAkunTargetObjekReklames.Where(x => x.TahunBuku == DateTime.Now.Year && x.BulanBuku <= DateTime.Now.Month && listOpReklame.Contains(x.Nor)).ToList();
                        var realisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpReklame.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var dbOpReklame = context.DbOpReklames
                            .Where(x => data.Select(y => y.Nop).Contains(x.Nop))
                            .ToList();
                        ret = data
                            .GroupBy(x => new { x.Nip, x.Nop, x.NamaPj, x.KategoriId })
                            .Select(g => new DetailOPProfileKategori
                            {
                                EnumPajak = (int)jenisPajak,
                                KategoriId = kategoriId,
                                Nop = g.Key.Nop,
                                NamaOP = dbOpReklame.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.Nama)
                                    .FirstOrDefault() ?? string.Empty,
                                AlamatOP = dbOpReklame.Where(x => x.Nop == g.Key.Nop)
                                    .Select(x => x.Alamatreklame)
                                    .FirstOrDefault() ?? string.Empty,
                                TargetSdBulanIni = targetReklame.Where(p => p.Nor == g.Key.Nop).Sum(q => q.TargetBulan) ?? 0,
                                RealisasiSdBulanIni = realisasiReklame
                            })
                            .ToList();
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
            public string Nip { get; set; } = string.Empty;
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
            public int EnumPajak { get; set; }
            public int KategoriId { get; set; }
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
