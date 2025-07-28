using DevExpress.XtraRichEdit.Import.Html;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using System.Globalization;
using System.Web.Mvc;
using static MonPDReborn.Models.MonitoringGlobal.MonitoringTahunanVM.MonitoringTahunanViewModels;

namespace MonPDReborn.Models.AktivitasOP
{
    public class ReklameSummaryVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }

        }

        public class Show
        {
            public List<ReklamePermanen> ReklamePermanenList { get; set; } = new();
            public int Tahun { get; set; }
            public Show() { }

            public Show(int tahun)
            {
                Tahun = tahun;
                ReklamePermanenList = Method.GetReklamePermanen(tahun);
            }
        }
        public class ShowTerbatas
        {
            public List<TerbatasReklame> TerbatasReklameList { get; set; } = new();
            public int Tahun { get; set; }

            public ShowTerbatas(int tahun)
            {
                Tahun = tahun;
                TerbatasReklameList = Method.GetTerbatasReklame(tahun);

            }
        }
        public class ShowIsidentil
        {
            public List<IsidentilReklame> IsidentilReklameList { get; set; } = new();
            public int Tahun { get; set; }

            public ShowIsidentil(int tahun)
            {
                Tahun = tahun;
                IsidentilReklameList = Method.GetIsidentilReklame(tahun);
            }
        }

        public class GetDetailSummary
        {
            public List<DetailSummary> Data { get; set; } = new();
            public GetDetailSummary(int tahun, int bulan, int jenis, int kategori)
            {
                Data = Method.GetDetailSummary(tahun, bulan, jenis, kategori);
            }
        }

        // Detail Upaya

        public class GetDetailUpaya
        {
            public DetailUpaya Data { get; set; } = new();
            public int SelectedUpaya { get; set; }
            public int SelectedTindakan { get; set; }
            public IFormFile Lampiran { get; set; } = null!;
            public GetDetailUpaya() { }
            public GetDetailUpaya(string noFormulir, int tahun, int bulan)
            {
                // panggil GetDetailUpaya
                Data = Method.GetDetailUpaya(noFormulir, tahun, bulan);

                
                Data.NoFormulir = noFormulir;
                Data.NewRowUpaya.NoFormulir = noFormulir;
                Data.NewRowUpaya.TglUpaya = DateTime.Now;

            }
        }

        public class Method
        {
            public static List<ReklamePermanen> GetReklamePermanen(int tahun)
            {
                var ret = new List<ReklamePermanen>();
                var context = DBClass.GetContext();

                var dataPer = context.MvReklameSummaries.AsQueryable();
                for (int i = 1; i <= 12; i++)
                {
                    var currentDate = new DateTime(tahun, i, 1);
                    ret.Add(new ReklamePermanen()
                    {
                        BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Bulan = i,
                        Tahun = tahun,
                        Jenis = 2, // Jenis 3 untuk Terbatas

                        SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
                        NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Bantip) ?? 0,
                        JmlSilangJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Silang) ?? 0,
                        JmlBongkarJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Bongkar) ?? 0,

                        SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                        NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                        SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Count(),
                        NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Bantip) ?? 0,
                        JmlSilangPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Silang) ?? 0,
                        JmlBongkarPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Bongkar) ?? 0,

                        SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Count(),
                        NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Count(),
                        NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BantipA) ?? 0,
                        JmlSilangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.SilangA) ?? 0,
                        JmlBongkarKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BongkarA) ?? 0,

                        SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Count(),
                        NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokokA) ?? 0
                    });
                }

                return ret;

            }

            public static List<TerbatasReklame> GetTerbatasReklame(int tahun)
            {
                var ret = new List<TerbatasReklame>();
                var context = DBClass.GetContext();
                var dataTer = context.MvReklameSummaries.AsQueryable();

                for (int i = 1; i <= 12; i++)
                {
                    ret.Add(new TerbatasReklame()
                    {
                        BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Bulan = i,
                        Tahun = tahun,
                        Jenis = 3, // Jenis 3 untuk Terbatas

                        SKPDJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
                        NilaiJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Bantip) ?? 0,
                        JmlSilangJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Silang) ?? 0,
                        JmlBongkarJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.Bongkar) ?? 0,

                        SKPDBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                        NilaiBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                        SKPDPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Count(),
                        NilaiPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Bantip) ?? 0,
                        JmlSilangPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Silang) ?? 0,
                        JmlBongkarPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.Bongkar) ?? 0,

                        SKPDBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Count(),
                        NilaiBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Count(),
                        NilaiKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,

                        JmlBantipKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BantipA) ?? 0,
                        JmlSilangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.SilangA) ?? 0,
                        JmlBongkarKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BongkarA) ?? 0,

                        SKPDBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Count(),
                        NilaiBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokokA) ?? 0
                    });
                }
                return ret;
            }

            public static List<IsidentilReklame> GetIsidentilReklame(int tahun)
            {
                var ret = new List<IsidentilReklame>();
                var context = DBClass.GetContext();

                var dataIns = context.MvReklameSummaries
                    .Where(x => x.IdFlagPermohonanA == 1 && x.TahunA == tahun)
                    .ToList();

                for (int i = 1; i <= 12; i++)
                {
                    var dataRekIns = dataIns.Where(x => x.BulanA == i).AsQueryable();
                    int skpd = dataRekIns.Count();
                    decimal nilai = dataRekIns.Sum(q => q.PajakPokokA) ?? 0;

                    decimal JmlBantip = dataRekIns.Sum(q => q.BantipA) ?? 0;
                    decimal JmlSilang = dataRekIns.Sum(q => q.SilangA) ?? 0;
                    decimal JmlBongkar = dataRekIns.Sum(q => q.BongkarA) ?? 0;

                    int skpdBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Count();
                    decimal nilaiBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Sum(q => q.PajakPokokA) ?? 0;

                    ret.Add(new IsidentilReklame
                    {
                        BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Bulan = i,
                        Tahun = tahun,
                        Jenis = 1, // Jenis 1 untuk Insidentil
                        SKPD = skpd,
                        Nilai = nilai,
                        SKPDBlmByr = skpdBlmByr,
                        NilaiBlmByr = nilaiBlmByr
                    });
                }
                return ret;
            }
            // Detail Reklame Permanen
            public static List<DetailSummary> GetDetailSummary(int tahun, int bulan, int jenis, int kategori)
            {
                var ret = new List<DetailSummary>();
                var context = DBClass.GetContext();

                var data = context.MvReklameSummaries.AsQueryable();

                var upaya = context.DbMonReklameUpayas
                    .Select(x => new { x.NoFormulir, x.Upaya })
                    .ToList();

                // ✅ NORMALISASI key saat GroupBy (pakai Trim + ToLower)
                var upayaGrouped = upaya
                    .Where(x => !string.IsNullOrWhiteSpace(x.NoFormulir))
                    .GroupBy(x => x.NoFormulir.Trim().ToLower())
                    .ToDictionary(g => g.Key, g => g.Select(u => u.Upaya).ToList());


                Func<MvReklameSummary, bool> predicate = x => false;

                // (Filter: tidak diubah)
                if (jenis == 1 && kategori == 1)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan && x.NoFormulir != null &&
                                     !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis;
                }
                else if (jenis == 1 && kategori == 2)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                     !string.IsNullOrEmpty(x.NoFormulirA) && x.IdFlagPermohonan == jenis;
                }
                else if (jenis == 1 && kategori == 3)
                {
                    predicate = x => x.IdFlagPermohonanA == jenis &&
                                 x.TahunA == tahun &&
                                 x.BulanA == bulan &&
                                 !x.NominalPokokBayarA.HasValue;
                    /*predicate = x => x.TahunA == tahun && x.BulanA == bulan &&
                                     !x.NominalPokokBayarA.HasValue &&
                                     x.IdFlagPermohonanA == jenis;*/
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 1)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan && !string.IsNullOrWhiteSpace(x.NoFormulir) &&
                                     !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis;
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 2)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                     string.IsNullOrEmpty(x.NoFormulirA) && x.IdFlagPermohonan == jenis;
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 3)
                {
                    predicate = x => x.IdFlagPermohonanA == jenis &&
                                 x.TahunA == tahun &&
                                 x.BulanA == bulan &&
                                 !x.TglBayarPokokA.HasValue;
                }

                // ✅ Proyeksikan hasil dengan normalisasi kunci NoFormulir
                ret = data.Where(predicate).Select(x =>
                {
                    string noFormulirDigunakan = (kategori == 3) ? x.NoFormulirA : x.NoFormulir;

                    string flag = (kategori == 3) ? x.FlagPermohonanA : x.FlagPermohonan;

                    string tampilFormulir = !string.IsNullOrEmpty(noFormulirDigunakan)
                        ? $"{noFormulirDigunakan} ({flag})"
                        : string.Empty;

                    var key = noFormulirDigunakan?.Trim().ToLower();

                    var email = context.DbMonReklameEmails
                        .Where(e => e.NoFormulir == noFormulirDigunakan)
                        .Select(e => e.Email)
                        .FirstOrDefault();

                    string informasiEmail = !string.IsNullOrEmpty(email) ? email : string.Empty;


                    string jumlahUpaya = "0";
                    if (!string.IsNullOrEmpty(key) && upayaGrouped.TryGetValue(key, out var upayaList))
                    {
                        jumlahUpaya = $"{upayaList.Count}x: {string.Join(", ", upayaList)}";
                    }
                    
                    return new DetailSummary
                    {
                        Bulan = bulan,
                        BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Tahun = tahun,
                        NoFormulir = tampilFormulir,
                        Nama = (kategori == 3)
                            ? string.Concat(x.NamaA ?? "", " (", x.NamaPerusahaanA ?? "", ")")
                            : string.Concat(x.Nama ?? "", " (", x.NamaPerusahaan ?? "", ")"),

                        AlamatOP = (kategori == 3)
                            ? x.AlamatreklameA ?? string.Empty
                            : x.Alamatreklame ?? string.Empty,

                        IsiReklame = (kategori == 3)
                            ? x.IsiReklameA ?? string.Empty
                            : x.IsiReklame ?? string.Empty,

                        AkhirBerlaku = (kategori == 3 && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TglAkhirBerlakuA.Value:dd MMM yyyy} (BELUM TERBAYAR)"
                            : (x.TglAkhirBerlaku.HasValue ? $"{x.TglAkhirBerlaku.Value:dd MMM yyyy} (BELUM TERBAYAR)" : string.Empty),

                        MasaTahunPajak = (kategori == 3 && x.TglMulaiBerlakuA.HasValue && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TahunA} ({x.TglMulaiBerlakuA.Value:dd MMM yyyy} - {x.TglAkhirBerlakuA.Value:dd MMM yyyy})"
                            : (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue
                            ? $"{x.Tahun} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})"
                            : string.Empty),

                        JumlahNilai = (kategori == 3)
                            ? x.PajakPokokA ?? 0
                            : x.PajakPokok ?? 0,

                        InformasiEmail = informasiEmail,
                        JumlahUpaya = jumlahUpaya
                    };
                }).ToList();

                return ret;
            }

            //public static List<DetailSummary> GetDetailSummary(int tahun, int bulan, int jenis, int kategori)
            //{
            //    var ret = new List<DetailSummary>();
            //    var context = DBClass.GetContext();

            //    // Jenis Permohonan:
            //    // 1 = Insidentil
            //    // 2 = Permanen
            //    // 3 = Terbatas

            //    //NO EMAIL:
            //    //kategori //2 = PJ BelumPerpanjangan

            //    //1 = JT Belumbayar
            //    //2 = PJ BelumPerpanjangan
            //    //3 = KB BelumBayar  || INSIDENTIL PASTI 3 PASTI FLAG_PERMOHONAN_A

            //    var data = context.MvReklameSummaries.AsQueryable();
            //    //.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.IdFlagPermohonan == jenis)
            //    //.ToList();
            //    var upaya = context.DbMonReklameUpayas
            //    .Select(x => new
            //    {
            //        x.NoFormulir,
            //        x.Upaya
            //    })
            //    .ToList();

            //    var upayaGrouped = upaya
            //        .GroupBy(x => x.NoFormulir)
            //        .ToDictionary(g => g.Key, g => g.Select(u => u.Upaya).ToList());

            //    if (jenis == 1) //Insidentil
            //    {
            //        if (kategori == 3)
            //        {
            //            // KB BelumBayar atau INSIDENTIL
            //            ret = data
            //                .Where(x => x.TahunA == tahun && x.BulanA == bulan && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.IdFlagPermohonanA == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //    }
            //    else if (jenis == 2) //Permanen
            //    {
            //        if (kategori == 1)
            //        {
            //            // JT Belumbayar
            //            ret = data
            //                .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //        else if (kategori == 2)
            //        {
            //            // PJ BelumPerpanjangan
            //            ret = data
            //                .Where(x => x.Tahun == tahun && x.Bulan == bulan && (!string.IsNullOrEmpty(x.NoFormulirA)) && x.IdFlagPermohonan == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //        else if (kategori == 3)
            //        {
            //            // KB BelumBayar atau INSIDENTIL
            //            ret = data
            //                .Where(x => x.TahunA == tahun && x.BulanA == bulan && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.IdFlagPermohonanA == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //    }
            //    else if (jenis == 3) //Terbatas
            //    {
            //        if (kategori == 1)
            //        {
            //            // JT Belumbayar
            //            ret = data
            //                .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //        else if (kategori == 2)
            //        {
            //            // PJ BelumPerpanjangan
            //            ret = data
            //                .Where(x => x.Tahun == tahun && x.Bulan == bulan && (!string.IsNullOrEmpty(x.NoFormulirA)) && x.IdFlagPermohonan == jenis)
            //                .Select(x => new DetailSummary
            //                {
            //                    Bulan = bulan,
            //                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                    Tahun = tahun,
            //                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                    AlamatOP = x.Alamatreklame ?? string.Empty,
            //                    IsiReklame = x.IsiReklame ?? string.Empty,
            //                    AkhirBerlaku = string.Concat(
            //                        x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                        " (BELUM TERBAYAR)"
            //                    ),
            //                    MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                    JumlahNilai = x.PajakPokok ?? 0,
            //                    InformasiEmail = string.Empty,
            //                    JumlahUpaya = upayaGrouped
            //                        .Where(f => f.Key == x.NoFormulir)
            //                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                        .FirstOrDefault() ?? "0"
            //                })
            //                .ToList();
            //        }
            //        else if (kategori == 3)
            //        {
            //            // KB BelumBayar atau INSIDENTIL
            //            ret = data
            //                 .Where(x => x.TahunA == tahun && x.BulanA == bulan && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.IdFlagPermohonanA == jenis)
            //                 .Select(x => new DetailSummary
            //                 {
            //                     Bulan = bulan,
            //                     BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
            //                     Tahun = tahun,
            //                     NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
            //                     Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
            //                     AlamatOP = x.Alamatreklame ?? string.Empty,
            //                     IsiReklame = x.IsiReklame ?? string.Empty,
            //                     AkhirBerlaku = string.Concat(
            //                         x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")),
            //                         " (BELUM TERBAYAR)"
            //                     ),
            //                     MasaTahunPajak = $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})",
            //                     JumlahNilai = x.PajakPokok ?? 0,
            //                     InformasiEmail = string.Empty,
            //                     JumlahUpaya = upayaGrouped
            //                         .Where(f => f.Key == x.NoFormulir)
            //                         .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
            //                         .FirstOrDefault() ?? "0"
            //                 })
            //                 .ToList();
            //        }
            //    }

            //    return ret;
            //    //}
            //}
            public static void SimpanUpaya(DetailUpaya.NewRow NewRowUpaya)
            {
                var context = DBClass.GetContext();
                if (NewRowUpaya.IdUpaya == 0)
                {
                    throw new ArgumentException("Upaya tidak boleh kosong.");
                }
                if (NewRowUpaya.IdTindakan == 0)
                {
                    throw new ArgumentException("Keterangan tidak boleh kosong.");
                }
                if (NewRowUpaya.TglUpaya == null || NewRowUpaya.TglUpaya == DateTime.MinValue)
                {
                    throw new ArgumentException("Tanggal Upaya tidak boleh kosong.");
                }
                if (string.IsNullOrEmpty(NewRowUpaya.NamaPetugas))
                {
                    throw new ArgumentException("Nama Petugas tidak boleh kosong.");
                }
                if (NewRowUpaya.Lampiran == null && NewRowUpaya.Lampiran.Length <= 0)
                {
                    throw new ArgumentException("lampiran foto tidak boleh kosong.");
                }
                var tindakan = context.MTindakanReklames.Where(x => x.Id == NewRowUpaya.IdTindakan && x.IdUpaya == NewRowUpaya.IdUpaya).SingleOrDefault().Tindakan;
                var upaya = context.MUpayaReklames.Where(x => x.Id == NewRowUpaya.IdUpaya).SingleOrDefault().Upaya;

                var seq = context.DbMonReklameUpayas
                    .Where(x => x.NoFormulir == NewRowUpaya.NoFormulir)
                    .Select(x => x.Seq)
                    .Count() + 1;


                var newUpaya = new MonPDLib.EF.DbMonReklameUpaya();
                newUpaya.DbMonReklameUpayaDok = new DbMonReklameUpayaDok();

                newUpaya.NoFormulir = NewRowUpaya.NoFormulir;
                newUpaya.Seq = seq;
                newUpaya.TglUpaya = NewRowUpaya.TglUpaya;
                newUpaya.Upaya = upaya ?? "-";
                newUpaya.Keterangan = tindakan ?? "-";
                newUpaya.Petugas = NewRowUpaya.NamaPetugas;
                newUpaya.DbMonReklameUpayaDok.Gambar = NewRowUpaya.Lampiran;


                context.DbMonReklameUpayas.Add(newUpaya);
                context.SaveChanges();
            }
            public static DetailUpaya GetDetailUpaya(string noFormulir, int tahun, int bulan)
            {
                var context = DBClass.GetContext();

                // Ambil data reklame yang cocok
                var reklame = context.MvReklameSummaries
                    .FirstOrDefault(x =>
                        (x.NoFormulir == noFormulir && x.Tahun == tahun && x.Bulan == bulan) ||
                        (x.NoFormulirA == noFormulir && x.TahunA == tahun && x.BulanA == bulan)
                    );

                if (reklame == null)
                    return null;

                // Ambil data upaya (pencocokan ke dua kemungkinan NoFormulir juga)
                var upayaList = context.DbMonReklameUpayas
                    .Include(x => x.DbMonReklameUpayaDok)
                    .Where(x => x.NoFormulir == reklame.NoFormulir || x.NoFormulir == reklame.NoFormulirA)
                    .OrderByDescending(x => x.TglUpaya)
                    .ToList();

                var dataUpayaList = upayaList.Select(x => new DetailUpaya.DataUpaya
                {
                    NoFormulir = x.NoFormulir,
                    TglUpaya = x.TglUpaya.ToString("dd/MM/yyyy"),
                    NamaUpaya = x.Upaya,
                    Keterangan = x.Keterangan,
                    Petugas = x.Petugas,
                    Lampiran = x.DbMonReklameUpayaDok.Gambar != null ? Convert.ToBase64String(x.DbMonReklameUpayaDok.Gambar) : null
                }).ToList();

                var model = new DetailUpaya
                {
                    Tahun = tahun,
                    Bulan = bulan,
                    NoFormulir = noFormulir,
                    InfoReklameUpaya = new DetailUpaya.InfoReklame
                    {
                        IsiReklame = reklame.IsiReklame ?? "-",
                        AlamatReklame = reklame.Alamatreklame ?? "-",
                        JenisReklame = reklame.NmJenis ?? "-",
                        Panjang = reklame.Panjang ?? 0,
                        Lebar = reklame.Lebar ?? 0,
                        Luas = reklame.Luas ?? 0,
                        Tinggi = reklame.Ketinggian ?? 0,
                        TglMulaiBerlaku = reklame.TglMulaiBerlaku ?? DateTime.MinValue,
                        TglAkhirBerlaku = reklame.TglAkhirBerlaku ?? DateTime.MinValue,
                        TahunPajak = reklame.TahunA?.ToString() ?? reklame.Tahun?.ToString() ?? "-",
                        MasaPajak = (reklame.TglMulaiBerlaku.HasValue && reklame.TglAkhirBerlaku.HasValue)
                            ? $"{reklame.TglMulaiBerlaku.Value.ToString("MMM yyyy", new CultureInfo("id-ID"))} - {reklame.TglAkhirBerlaku.Value.ToString("MMM yyyy", new CultureInfo("id-ID"))}"
                            : "-"
                    },
                    DataUpayaList = dataUpayaList
                };

                return model;
            }

            /*public static DetailUpaya GetDetailUpaya(string noFormulir, int tahun, int bulan)
            {
                var context = DBClass.GetContext();

                // Ambil data reklame yang cocok
                var reklame = context.MvReklameSummaries
                    .FirstOrDefault(x =>
                        (x.NoFormulir == noFormulir || x.NoFormulirA == noFormulir) &&
                        (x.Tahun == tahun || x.TahunA == tahun) &&
                        (x.Bulan == bulan || x.BulanA == bulan)
                    );

                if (reklame == null)
                    return null;

                // Ambil data upaya
                var upayaList = context.DbMonReklameUpayas.Include(x => x.DbMonReklameUpayaDok)
                    .Where(x => x.NoFormulir == noFormulir)
                    .OrderByDescending(x => x.TglUpaya)
                    .ToList();

                var dataUpayaList = upayaList.Select(x => new DetailUpaya.DataUpaya
                {
                    NoFormulir = x.NoFormulir,
                    TglUpaya = x.TglUpaya.ToString("dd/MM/yyyy"),
                    NamaUpaya = x.Upaya,
                    Keterangan = x.Keterangan,
                    Petugas = x.Petugas,
                    Lampiran = x.DbMonReklameUpayaDok.Gambar != null ? Convert.ToBase64String(x.DbMonReklameUpayaDok.Gambar) : null
                }).ToList();

                var model = new DetailUpaya
                {
                    Tahun = tahun,
                    Bulan = bulan,
                    NoFormulir = noFormulir,
                    InfoReklameUpaya = new DetailUpaya.InfoReklame
                    {
                        IsiReklame = reklame.IsiReklame ?? "-",
                        AlamatReklame = reklame.Alamatreklame ?? "-",
                        JenisReklame = reklame.NmJenis ?? "-",
                        Panjang = reklame.Panjang ?? 0,
                        Lebar = reklame.Lebar ?? 0,
                        Luas = reklame.Luas ?? 0,
                        Tinggi = reklame.Ketinggian ?? 0,
                        TglMulaiBerlaku = reklame.TglMulaiBerlaku ?? DateTime.MinValue,
                        TglAkhirBerlaku = reklame.TglAkhirBerlaku ?? DateTime.MinValue,
                        TahunPajak = reklame.TahunA?.ToString() ?? reklame.Tahun.Value.ToString(),
                        MasaPajak = (reklame.TglMulaiBerlaku.HasValue && reklame.TglAkhirBerlaku.HasValue)
                            ? $"{reklame.TglMulaiBerlaku.Value.ToString("MMM yyyy", new CultureInfo("id-ID"))} - {reklame.TglAkhirBerlaku.Value.ToString("MMM yyyy", new CultureInfo("id-ID"))}"
                            : "-"
                    },
                    DataUpayaList = dataUpayaList
                };

                return model;
            }*/
        }

        public class UpayaCbView
        {
            public int Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class TindakanCbView
        {
            public int Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class ReklamePermanen
        {
            public string BulanNama { get; set; } = null!;
            public int Jenis { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public int Kategori { get; set; }
            public int SKPDJT { get; set; }
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; }
            public decimal NilaiBlmJT { get; set; }
            public int SKPDPanjang { get; set; }
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }

            public decimal JmlBantipJT { get; set; }
            public decimal JmlSilangJT { get; set; }
            public decimal JmlBongkarJT { get; set; }
            public decimal JmlBantipPanjang { get; set; }
            public decimal JmlSilangPanjang { get; set; }
            public decimal JmlBongkarPanjang { get; set; }
            public decimal JmlBantipKB { get; set; }
            public decimal JmlSilangKB { get; set; }
            public decimal JmlBongkarKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;

        }

        public class TerbatasReklame
        {
            public string BulanNama { get; set; } = null!;
            public int Jenis { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public int SKPDJT { get; set; }
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; }
            public decimal NilaiBlmJT { get; set; }
            public int SKPDPanjang { get; set; }
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }

            public decimal JmlBantipJT { get; set; }
            public decimal JmlSilangJT { get; set; }
            public decimal JmlBongkarJT { get; set; }
            public decimal JmlBantipPanjang { get; set; }
            public decimal JmlSilangPanjang { get; set; }
            public decimal JmlBongkarPanjang { get; set; }
            public decimal JmlBantipKB { get; set; }
            public decimal JmlSilangKB { get; set; }
            public decimal JmlBongkarKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;
        }

        public class IsidentilReklame
        {
            public string BulanNama { get; set; } = null!;
            public int Jenis { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public int SKPD { get; set; }
            public decimal Nilai { get; set; }
            public int SKPDBlmByr { get; set; }
            public decimal NilaiBlmByr { get; set; }

            public decimal JmlBantip { get; set; }
            public decimal JmlSilang { get; set; }
            public decimal JmlBongkar { get; set; }
            public decimal Potensi => NilaiBlmByr;
        }

        public class DetailSummary
        {
            public string BulanNama { get; set; } = null!;
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string AkhirBerlaku { get; set; } = null!;
            public string MasaTahunPajak { get; set; } = null!;
            public decimal JumlahNilai { get; set; }
            public string? InformasiEmail { get; set; }
            public string JumlahUpaya { get; set; } = null!;
        }
        public class DetailUpaya
        {
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public NewRow NewRowUpaya { get; set; } = new NewRow();
            public InfoReklame InfoReklameUpaya { get; set; } = new InfoReklame();
            public List<DataUpaya> DataUpayaList { get; set; } = new();
            public class NewRow
            {
                public string NoFormulir { get; set; } = null!;
                public int IdUpaya { get; set; }
                public int IdTindakan { get; set; }
                public string NamaPetugas { get; set; } = null!;
                public DateTime TglUpaya { get; set; }
                public byte[] Lampiran { get; set; } = null!;
            }
            public class DataUpaya
            {
                public string NoFormulir { get; set; } = null!;
                public string TglUpaya { get; set; } = null!;
                public string NamaUpaya { get; set; } = null!;
                public string Keterangan { get; set; } = null!;
                public string Petugas { get; set; } = null!;
                public string Lampiran { get; set; }
            }
            public class InfoReklame
            {
                public string IsiReklame { get; set; } = null!;
                public string AlamatReklame { get; set; } = null!;
                public string JenisReklame { get; set; } = null!;
                public decimal Panjang { get; set; }
                public decimal Lebar { get; set; }
                public decimal Luas { get; set; }
                public decimal Tinggi { get; set; }
                public DateTime TglMulaiBerlaku { get; set; }
                public DateTime TglAkhirBerlaku { get; set; }
                public string TahunPajak { get; set; } = null!;
                public string MasaPajak { get; set; } = null!;
            }
        }
    }
}
