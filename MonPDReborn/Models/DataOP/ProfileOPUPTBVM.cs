using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.DataOP.ProfileOPVM;


namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPUPTBVM
    {
        public class Index
        {
            public Index()
            {

            }

        }

        public class RekapPerWilayah
        {
            public List<RekapOP> RekapOpWilayahList { get; set; } = new();
            public RekapPerWilayah(string uptb, string kec, string kel)
            {
                RekapOpWilayahList = Method.GetDataRekapPerWilayahList(uptb, kec, kel);
            }
        }


        public class Method
        {
            public static List<RekapOP> GetDataRekapPerWilayahList(string uptb, string kec, string kel)
            {
                var ret = new List<RekapOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var dataResto = context.DbOpRestos
                     .Where(x => x.TahunBuku == currentYear && x.PajakNama != "MAMIN" &&
                                 (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                     .GroupBy(x => new { x.Nop })
                     .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                     .Count();

                var dataPpj = context.DbOpListriks
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();


                var OpPbbAkhir = context.DbMonPbbs
                    .Where(x => x.TahunBuku == currentYear && x.Uptb == Convert.ToDecimal(uptb) && x.AlamatKdCamat == kec && x.AlamatKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop })
                    .Count();



                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                    JmlOpAkhir = dataResto,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                    JmlOpAkhir = dataPpj,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                    JmlOpAkhir = dataHotel,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                    JmlOpAkhir = dataParkir,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                    JmlOpAkhir = dataHiburan,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                    JmlOpAkhir = dataAbt,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.PBB,
                    JmlOpAkhir = OpPbbAkhir,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });


                return ret;
            }

            public static List<RekapDetailOP> GetDataRekapPerWilayahDetailList(EnumFactory.EPajak jenisPajak, string uptb, string kec, string kel, bool turu)
            {
                var ret = new List<RekapDetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var kategori = context.MKategoriPajaks
                .Where(x => x.PajakId == (int)jenisPajak && x.Id == 17)
                .FirstOrDefault();

                var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == kategori.Id && x.TahunBuku == currentYear &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                            .GroupBy(x => new { x.Nop })
                            .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                            .Count();
                var re = new RekapDetailOP();
                re.JenisPajak = jenisPajak.GetDescription();
                re.EnumPajak = (int)jenisPajak;
                re.KategoriId = (int)kategori.Id;
                re.KategoriNama = kategori.Nama;
                re.JmlOpAkhir = dataHotel;
                re.Uptb = uptb;
                re.KdKecamatan = kec;
                re.KdKelurahan = kel;
                ret.Add(re);

                return ret;
            }
            public static List<RekapDetailOP> GetDataRekapPerWilayahDetailList(EnumFactory.EPajak jenisPajak, string uptb, string kec, string kel)
            {
                var ret = new List<RekapDetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak)
                    .OrderBy(x => x.Urutan)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                switch (jenisPajak)
                {
                    case EPajak.MakananMinuman:
                        foreach (var item in kategoriList)
                        {
                            var dataResto = context.DbOpRestos
                             .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear && x.PajakNama != "MAMIN" &&
                                         (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                             .GroupBy(x => new { x.Nop })
                             .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                             .Count();

                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataResto;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.TenagaListrik:
                        foreach (var item in kategoriList)
                        {
                            var dataPpj = context.DbOpListriks
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop, x.KategoriId })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataPpj;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.JasaPerhotelan:
                        foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                        {
                            var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                            .GroupBy(x => new { x.Nop })
                            .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                            .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataHotel;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);

                        }
                        break;
                    case EPajak.JasaParkir:
                        foreach (var item in kategoriList)
                        {
                            var dataParkir = context.DbOpParkirs
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataParkir;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.JasaKesenianHiburan:
                        foreach (var item in kategoriList)
                        {
                            var dataHiburan = context.DbOpHiburans
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataHiburan;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.AirTanah:
                        foreach (var item in kategoriList)
                        {
                            var dataAbt = context.DbOpAbts
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop, x.KategoriId })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataAbt;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.PBB:
                        foreach (var item in kategoriList)
                        {
                            var OpPbbAkhir = context.DbMonPbbs
                                .Where(x => x.TahunBuku == currentYear && x.Uptb == Convert.ToDecimal(uptb) && x.AlamatKdCamat == kec && x.AlamatKdLurah == kel && x.KategoriId == item.Id)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = OpPbbAkhir;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    default:
                        break;
                }
                return ret;
            }

            public static List<DetailOP> GetDataRekapPerWilayahDetailOPList(EnumFactory.EPajak jenisPajak, int kategori, string uptb, string kec, string kel)
            {
                var ret = new List<DetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                switch (jenisPajak)
                {
                    case EPajak.MakananMinuman:
                        var dataResto = context.DbOpRestos
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && x.PajakNama != "MAMIN"
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();


                        ret = dataResto;
                        break;
                    case EPajak.TenagaListrik:
                        var dataListrik = context.DbOpListriks
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataListrik;
                        break;
                    case EPajak.JasaPerhotelan:
                        var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataHotel;
                        break;
                    case EPajak.JasaParkir:
                        var dataParkir = context.DbOpParkirs
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataParkir;
                        break;
                    case EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbOpHiburans
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataHiburan;
                        break;
                    case EPajak.AirTanah:
                        var dataAbt = context.DbOpAbts
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataAbt;
                        break;
                    case EPajak.PBB:
                        var dataPbb = context.DbMonPbbs
                            .Where(x => x.TahunBuku == currentYear
                                        && x.Uptb == Convert.ToDecimal(uptb)
                                        && x.AlamatKdCamat == kec
                                        && x.AlamatKdLurah == kel
                                        && x.KategoriId == kategori)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.Uptb ?? "-",
                                Kecamatan = x.AlamatKdCamat,
                                Kelurahan = x.AlamatKdLurah
                            })
                            .ToList();

                        ret = dataPbb;
                        break;
                    default:
                        break;
                }

                return ret;
            }
        
        }

        public class RekapOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
            public List<RekapDetailOP> RekapDetail { get; set; } = new();
            public List<OkupansiHotel> OkunpasiHotel { get; set; } = new();
            public string Uptb { get; set; } = null!;
            public string KdKecamatan { get; set; }
            public string KdKelurahan { get; set; }
        }

        public class DetailOP
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = "-";
            public string Kecamatan { get; set; } = "-";
            public string Kelurahan { get; set; } = "-";
            public string Status { get; set; } = null!;

        }
        public class uptbView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class kecamatanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class kelurahanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
    }
}
