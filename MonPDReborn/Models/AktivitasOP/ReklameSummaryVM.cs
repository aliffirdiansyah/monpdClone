using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using System.Globalization;
using System.Web.Mvc;

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
            public List<TerbatasReklame> TerbatasReklameList { get; set; } = new();
            public List<IsidentilReklame> IsidentilReklameList { get; set; } = new();
            public int Tahun { get; set; }
            public Show() { }

            public Show(int tahun)
            {
                Tahun = tahun;
                ReklamePermanenList = Method.GetReklamePermanen(tahun);
                TerbatasReklameList = Method.GetTerbatasReklame(tahun);
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
            public List<SelectListItem> UpayaList { get; set; } = new();
            public GetDetailUpaya() { }
            public GetDetailUpaya(string noFormulir, int tahun, int bulan)
            {
                var context = DBClass.GetContext();
                Data.NoFormulir = noFormulir;
                Data.NewRowUpaya.TglUpaya = DateTime.Now;

                UpayaList = context.MUpayaReklames.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Upaya
                }).ToList();


                // panggil GetDetailUpaya
                //Data = Method.GetDetailUpaya(noFormulir ?? string.Empty);
            }
        }

        public class Method
        {
            //public static List<ReklamePermanen> GetReklamePermanen(int tahun)
            //{
            //    var ret = new List<ReklamePermanen>();
            //    var context = DBClass.GetContext();

            //    var dataPer = context.MvReklameSummaries.AsQueryable();
            //    for (int i = 1; i <= 12; i++)
            //    {
            //        var currentDate = new DateTime(tahun, i, 1);

            //        // 1 = JT Belumbayar
            //        ret.Add(new ReklamePermanen()
            //        {
            //            BulanNama = currentDate.ToString("MMMM", new CultureInfo("id-ID")),
            //            Bulan = i,
            //            Tahun = tahun,
            //            Jenis = 2,
            //            Kategori = 1,

            //            SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
            //            NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,
            //            SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
            //            NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0
            //        });

            //        // 2 = PJ BelumPerpanjangan
            //        ret.Add(new ReklamePermanen()
            //        {
            //            BulanNama = currentDate.ToString("MMMM", new CultureInfo("id-ID")),
            //            Bulan = i,
            //            Tahun = tahun,
            //            Jenis = 2,
            //            Kategori = 2,

            //            SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && !string.IsNullOrEmpty(x.NoFormulirA)).Count(),
            //            NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && !string.IsNullOrEmpty(x.NoFormulirA)).Sum(x => x.PajakPokok) ?? 0,
            //            SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Count(),
            //            NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0
            //        });

            //        // 3 = KB BelumBayar
            //        ret.Add(new ReklamePermanen()
            //        {
            //            BulanNama = currentDate.ToString("MMMM", new CultureInfo("id-ID")),
            //            Bulan = i,
            //            Tahun = tahun,
            //            Jenis = 2,
            //            Kategori = 3,

            //            SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Count(),
            //            NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,
            //            SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && !x.TglBayarPokokA.HasValue).Count(),
            //            NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && !x.TglBayarPokokA.HasValue).Sum(x => x.PajakPokok) ?? 0
            //        });
            //    }


            //    return ret;

            //}
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
                        Jenis = 2, // Jenis 2 untuk Permanen

                        SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
                        NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,

                        SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                        NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                        SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Count(),
                        NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.PajakPokok) ?? 0,

                        SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Count(),
                        NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Count(),
                        NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Count(),
                        NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokok) ?? 0
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

                        SKPDBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                        NilaiBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                        SKPDPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Count(),
                        NilaiPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && (!string.IsNullOrEmpty(x.NoFormulirA))).Sum(x => x.PajakPokok) ?? 0,

                        SKPDBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Count(),
                        NilaiBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && string.IsNullOrEmpty(x.NoFormulirA) && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Count(),
                        NilaiKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,

                        SKPDBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Count(),
                        NilaiBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokok) ?? 0
                    });
                }
                return ret;
            }

            public static List<IsidentilReklame> GetIsidentilReklame(int tahun)
            {
                var ret = new List<IsidentilReklame>();
                var context = DBClass.GetContext();

                var dataIns = context.MvReklameSummaries.
                    Where(x => x.IdFlagPermohonanA == 1 && x.TahunA == tahun).ToList();

                for (int i = 1; i <= 12; i++)
                {
                    ret.Add(new IsidentilReklame
                    {
                        BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Bulan = i,
                        Tahun = tahun,
                        Jenis = 1, // Jenis 1 untuk Insidentil
                        SKPD = dataIns.Where(x => x.BulanA == i && x.NoFormulir != null).Count(),
                        Nilai = dataIns.Where(x => x.BulanA == i && x.NoFormulir != null && x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,
                        SKPDBlmByr = dataIns.Where(x => x.BulanA == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                        NilaiBlmByr = dataIns.Where(x => x.BulanA == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0
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

                var upaya = context.TUpayaReklames
                    .Select(x => new { x.NoFormulir, x.Upaya })
                    .ToList();

                var upayaGrouped = upaya
                    .GroupBy(x => x.NoFormulir)
                    .ToDictionary(g => g.Key, g => g.Select(u => u.Upaya).ToList());

                Func<MvReklameSummary, bool> predicate = x => false;

                if (jenis == 1 && kategori == 3)
                {
                    predicate = x => x.TahunA == tahun && x.BulanA == bulan && x.NoFormulir != null &&
                                     !x.TglBayarPokok.HasValue && x.IdFlagPermohonanA == jenis;
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 1)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan && x.NoFormulir != null &&
                                     !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis;
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 2)
                {
                    predicate = x => x.Tahun == tahun && x.Bulan == bulan && !string.IsNullOrEmpty(x.NoFormulirA) &&
                                     x.IdFlagPermohonan == jenis;
                }
                else if ((jenis == 2 || jenis == 3) && kategori == 3)
                {
                    predicate = x => x.TahunA == tahun && x.BulanA == bulan && x.NoFormulir != null &&
                                     !x.TglBayarPokok.HasValue && x.IdFlagPermohonanA == jenis;
                }

                ret = data.Where(predicate).Select(x => new DetailSummary
                {
                    Bulan = bulan,
                    BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                    Tahun = tahun,
                    NoFormulir = string.Concat(x.NoFormulir, " (", x.FlagPermohonan, ")") ?? string.Empty,
                    Nama = string.Concat(x.Nama, " (", x.NamaPerusahaan, ")") ?? string.Empty,
                    AlamatOP = x.Alamatreklame ?? string.Empty,
                    IsiReklame = x.IsiReklame ?? string.Empty,
                    AkhirBerlaku = x.TglAkhirBerlaku.HasValue
                        ? string.Concat(x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID")), " (BELUM TERBAYAR)")
                        : string.Empty,
                    MasaTahunPajak = (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue)
                        ? $"{x.TahunA} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})"
                        : string.Empty,
                    JumlahNilai = x.PajakPokok ?? 0,
                    InformasiEmail = string.Empty,
                    JumlahUpaya = upayaGrouped
                        .Where(f => f.Key == x.NoFormulir)
                        .Select(f => $"{f.Value.Count}x: {string.Join(", ", f.Value)}")
                        .FirstOrDefault() ?? "0"
                }).ToList();

                return ret;
            }
            //public static List<DetailSummary> GetDetailSummary(int tahun, int bulan, int jenis, int kategori)
            //{
            //    var ret = new List<DetailSummary>();
            //    var context = DBClass.GetContext();

            //    var data = context.MvReklameSummaries.AsQueryable();

            //    var upaya = context.TUpayaReklames
            //        .Select(x => new { x.NoFormulir, x.Upaya })
            //        .ToList();

            //    var upayaGrouped = upaya
            //        .GroupBy(x => x.NoFormulir)
            //        .ToDictionary(g => g.Key, g => g.Select(u => u.Upaya).ToList());

            //    var bulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID"));

            //    // Filter awal
            //    var filtered = data
            //    .Where(x =>
            //        x.TahunA == tahun &&
            //        x.BulanA == bulan &&
            //        x.NoFormulir != null &&
            //        x.IdFlagPermohonanA == jenis &&
            //        (
            //            (kategori == 1 && !x.TglBayarPokok.HasValue) ||  // JT Belum Bayar
            //            (kategori == 2 && x.TglBayarPokok.HasValue && x.TglAkhirBerlaku < new DateTime(tahun, bulan, 1)) || // PJ Belum Perpanjang
            //            (kategori == 3 && !x.TglBayarPokok.HasValue)  // KB Belum Bayar
            //        )
            //    )
            //    .ToList();


            //    if (kategori == 1) // JT Belum Bayar
            //    {
            //        filtered = filtered.Where(x => !x.TglBayarPokok.HasValue).ToList();
            //    }
            //    else if (kategori == 2) // PJ Belum Perpanjang
            //    {
            //        filtered = filtered.Where(x =>
            //            x.TglBayarPokok.HasValue &&
            //            x.TglAkhirBerlaku < new DateTime(tahun, bulan, 1)).ToList();
            //    }
            //    else if (kategori == 3) // KB Belum Bayar (khusus Insidentil)
            //    {
            //        filtered = filtered.Where(x => !x.TglBayarPokok.HasValue).ToList();
            //    }

            //    ret = filtered.Select(x => new DetailSummary
            //    {
            //        Bulan = bulan,
            //        BulanNama = bulanNama,
            //        Tahun = tahun,
            //        NoFormulir = $"{x.NoFormulir} ({x.FlagPermohonan})",
            //        Nama = $"{x.Nama} ({x.NamaPerusahaan})",
            //        AlamatOP = x.Alamatreklame ?? string.Empty,
            //        IsiReklame = x.IsiReklame ?? string.Empty,
            //        AkhirBerlaku = x.TglAkhirBerlaku.HasValue
            //            ? $"{x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID"))} {(x.TglBayarPokok.HasValue ? "" : "(BELUM TERBAYAR)")}"
            //            : string.Empty,
            //        MasaTahunPajak = (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue)
            //            ? $"{x.TahunA} ({x.TglMulaiBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID"))} - {x.TglAkhirBerlaku.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID"))})"
            //            : $"{x.TahunA}",
            //        JumlahNilai = x.PajakPokok ?? 0,
            //        InformasiEmail = string.Empty,
            //        JumlahUpaya = upayaGrouped.TryGetValue(x.NoFormulir, out var listUpaya)
            //            ? $"{listUpaya.Count}x: {string.Join(", ", listUpaya)}"
            //            : "0"
            //    }).ToList();

            //    return ret;
            //}


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
            //    var upaya = context.TUpayaReklames
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
            //}
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
                public int IdUpaya { get; set; }
                public string Keterangan { get; set; } = null!;
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
