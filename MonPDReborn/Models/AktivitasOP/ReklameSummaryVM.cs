using DevExpress.XtraRichEdit.Import.Html;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using static MonPDReborn.Models.AktivitasOP.ReklameSummaryIndoorVM;
using static MonPDReborn.Models.AktivitasOP.ReklameSummaryVM.DetailUpaya;
using static MonPDReborn.Models.DashboardUPTBVM.ViewModel;
using static MonPDReborn.Models.DashboardVM.ViewModel;
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
            public int TahunNow { get; set; }
            public int TahunMin1 { get; set; }
            public int Lokasi { get; set; }
            public Show() { }

            public Show(int tahun, int lokasi)
            {
                Tahun = tahun;
                Lokasi = lokasi;
                ReklamePermanenList = Method.GetReklamePermanen(tahun, lokasi);

            }
        }
        public class ShowTerbatas
        {
            public List<TerbatasReklame> TerbatasReklameList { get; set; } = new();
            public int Tahun { get; set; }
            public int TahunNow { get; set; }
            public int TahunMin1 { get; set; }
            public int Lokasi { get; set; }
            public ShowTerbatas(int tahun, int lokasi)
            {
                Tahun = tahun;
                Lokasi = lokasi;
                TerbatasReklameList = Method.GetTerbatasReklame(tahun, lokasi);

            }
        }
        public class ShowIsidentil
        {
            public List<IsidentilReklame> IsidentilReklameList { get; set; } = new();
            public int Tahun { get; set; }
            public int TahunNow { get; set; }
            public int TahunMin1 { get; set; }
            public int Lokasi { get; set; }

            public ShowIsidentil(int tahun, int lokasi)
            {
                Tahun = tahun;
                Lokasi = lokasi;
                IsidentilReklameList = Method.GetIsidentilReklame(tahun, lokasi);
            }
        }

        public class GetDetailSummary
        {
            public List<DetailSummary> Data { get; set; } = new();
            public GetDetailSummary(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                Data = Method.GetDetailSummary(tahun, bulan, jenis, kategori, lokasi);
            }
        }

        public class BongkarDetail
        {
            public List<DetailBongkar> Data { get; set; } = new();
            public BongkarDetail(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                Data = Method.GetDetailBongkar(tahun, bulan, jenis, kategori, lokasi);
            }
        }

        public class SilangDetail
        {
            public List<DetailSilang> Data { get; set; } = new();
            public SilangDetail(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                Data = Method.GetDetailSilang(tahun, bulan, jenis, kategori, lokasi);
            }
        }
        public class TeguranDetail
        {
            public List<DetailTeguran> Data { get; set; } = new();
            public TeguranDetail(int tahun, int bulan, int jenis, int kategori)
            {
                Data = Method.GetDetailTegur(tahun, bulan, jenis, kategori);
            }
        }
        // Detail Upaya

        public class GetDetailUpaya
        {
            public DetailUpaya Data { get; set; } = new();
            public int SelectedUpaya { get; set; }
            public int SelectedTindakan { get; set; }
            public string SelectedNOR { get; set; } = null!;
            public IFormFile Lampiran { get; set; } = null!;
            public GetDetailUpaya() { }
            public GetDetailUpaya(string noFormulir, int tahun, int bulan, int lokasi)
            {
                // panggil GetDetailUpaya
                Data = Method.GetDetailUpaya(noFormulir, tahun, bulan, lokasi);


                Data.NoFormulir = noFormulir;
                Data.NewRowUpaya.NoFormulir = noFormulir;
                Data.NewRowUpaya.TglUpaya = DateTime.Now;

            }
        }

        public class Method
        {
            public static List<ReklamePermanen> GetReklamePermanen(int tahun, int lokasi)
            {
                var ret = new List<ReklamePermanen>();
                var context = DBClass.GetContext();

                var dataPer = context.MvReklameSummaries.AsQueryable();
                var dataUpaya = context.DbMonReklameUpayas.AsQueryable();
                //tampilkan semua
                if (lokasi == 1)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        // join summary dengan upaya
                        var joinData = from s in dataPer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == i
                                       select new { s, u };

                        var currentDate = new DateTime(tahun, i, 1);
                        ret.Add(new ReklamePermanen()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Jenis = 2, // Jenis 3 untuk Terbatas
                            Lokasi = 1,

                            SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
                            NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                            NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue).Count(),
                            NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i).Count(),
                            NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.u.Upaya == "TEGURAN").Count(),

                            SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Count(),
                            NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue).Count(),
                            NilaiPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue).Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 2 && x.s.TahunA == tahun && x.s.BulanA == i && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && !x.TglBayarPokokA.HasValue).Count(),
                            NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokokA) ?? 0
                        });
                    }
                }
                else if (lokasi == 2)
                {
                    // indoor
                    for (int i = 1; i <= 12; i++)
                    {
                        var bulan = i;

                        // join summary dengan upaya
                        var joinData = from s in dataPer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == bulan
                                       select new { s, u };

                        var currentDate = new DateTime(tahun, i, 1);
                        ret.Add(new ReklamePermanen()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 2,
                            Jenis = 2, // Jenis 3 untuk Terbatas

                            SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.s.LetakReklame == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.s.LetakReklame == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            SKPDPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 2 && x.s.TahunA == tahun && x.s.BulanA == i && x.s.LetakReklameA == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0
                        });
                    }

                }
                else if (lokasi == 3)
                {
                    //outdoor
                    for (int i = 1; i <= 12; i++)
                    {
                        var bulan = i;
                        var joinData = from s in dataPer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == bulan
                                       select new { s, u };

                        var currentDate = new DateTime(tahun, i, 1);
                        ret.Add(new ReklamePermanen()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 3,
                            Jenis = 2, // Jenis 3 untuk Terbatas

                            SKPDJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.s.LetakReklame == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiBlmJT = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiBlmPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataPer.Where(x => x.IdFlagPermohonan == 2 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 2 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.s.LetakReklame == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            SKPDPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiPanjangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 2 && x.s.TahunA == tahun && x.s.BulanA == i && x.s.LetakReklameA == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiBlmKB = dataPer.Where(x => x.IdFlagPermohonanA == 2 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0
                        });
                    }
                }

                return ret;
            }

            public static List<TerbatasReklame> GetTerbatasReklame(int tahun, int lokasi)
            {
                var ret = new List<TerbatasReklame>();
                var context = DBClass.GetContext();
                var dataTer = context.MvReklameSummaries.AsQueryable();
                var dataUpaya = context.DbMonReklameUpayas.AsQueryable();

                if (lokasi == 1)
                {
                    // semua 

                    for (int i = 1; i <= 12; i++)
                    {
                        var bulan = i;
                        var currentDate = new DateTime(tahun, i, 1);
                        var joinData = from s in dataTer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == bulan
                                       select new { s, u };

                        ret.Add(new TerbatasReklame()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 1,
                            Jenis = 3, // Jenis 3 untuk Terbatas

                            SKPDJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Count(),
                            NilaiJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null).Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                            NilaiBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue).Count(),
                            NilaiPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0).Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i).Count(),
                            NilaiBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i).Sum(x => x.PajakPokok) ?? 0,

                            SKPDKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Count(),
                            NilaiKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.PajakPokokA) ?? 0,

                            SKPDPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue).Count(),
                            NilaiPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue).Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i).Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 3 && x.s.TahunA == tahun && x.s.BulanA == i && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Count(),
                            NilaiBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue)).Sum(x => x.PajakPokokA) ?? 0
                        });
                    }

                }
                else if (lokasi == 2)
                {
                    // Indoor
                    for (int i = 1; i <= 12; i++)
                    {
                        var bulan = i;
                        var currentDate = new DateTime(tahun, i, 1);
                        var joinData = from s in dataTer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == bulan
                                       select new { s, u };
                        ret.Add(new TerbatasReklame()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 2,
                            Jenis = 3, // Jenis 3 untuk Terbatas

                            SKPDJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.s.LetakReklame == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                            NilaiBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.s.LetakReklame == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            SKPDPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 3 && x.s.TahunA == tahun && x.s.BulanA == i && x.s.LetakReklameA == "DALAM RUANGAN (IN DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Count(),
                            NilaiBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)").Sum(x => x.PajakPokokA) ?? 0
                        });
                    }
                }
                else if (lokasi == 3)
                {
                    // Outdoor
                    for (int i = 1; i <= 12; i++)
                    {
                        var bulan = i;
                        var currentDate = new DateTime(tahun, i, 1);
                        var joinData = from s in dataTer
                                       join u in dataUpaya on s.NoFormulir equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // left join
                                       where s.Tahun == tahun && s.Bulan == bulan
                                       select new { s, u };
                        ret.Add(new TerbatasReklame()
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 3,
                            Jenis = 3, // Jenis 3 untuk Terbatas

                            SKPDJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurJT = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.NoFormulir != null && !x.s.TglBayarPokok.HasValue && x.s.LetakReklame == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Count(),
                            NilaiBlmJT = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.NoFormulir != null && !x.TglBayarPokok.HasValue).Sum(x => x.PajakPokok) ?? 0,

                            SKPDPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.TglBayarPokok.HasValue && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            JmlBantipPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bantip) ?? 0,
                            JmlSilangPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Silang) ?? 0,
                            JmlBongkarPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.Tahun == tahun && x.Bulan == i && x.IsPerpanjangan == 0 && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.Bongkar) ?? 0,
                            TegurPanjang = joinData.Where(x => x.s.IdFlagPermohonan == 3 && x.s.Tahun == tahun && x.s.Bulan == i && x.s.IsPerpanjangan == 0 && x.s.LetakReklame == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiBlmPanjang = dataTer.Where(x => x.IdFlagPermohonan == 3 && x.IsPerpanjangan == 0 && x.Tahun == tahun && x.Bulan == i && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokok) ?? 0,

                            SKPDKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            SKPDPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiPanjangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.TglBayarPokokA.HasValue && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0,

                            JmlBantipKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.BantipA) ?? 0,
                            JmlSilangKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.SilangA) ?? 0,
                            JmlBongkarKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.BongkarA) ?? 0,
                            TegurKB = joinData.Where(x => x.s.IdFlagPermohonanA == 3 && x.s.TahunA == tahun && x.s.BulanA == i && x.s.LetakReklameA == "LUAR RUANGAN (OUT DOOR)" && x.u.Upaya == "TEGURAN").Count(),

                            SKPDBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Count(),
                            NilaiBlmKB = dataTer.Where(x => x.IdFlagPermohonanA == 3 && x.TahunA == tahun && x.BulanA == i && (!x.TglBayarPokokA.HasValue) && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)").Sum(x => x.PajakPokokA) ?? 0
                        });
                    }
                }

                return ret;
            }

            public static List<IsidentilReklame> GetIsidentilReklame(int tahun, int lokasi)
            {
                var ret = new List<IsidentilReklame>();
                var context = DBClass.GetContext();

                if (lokasi == 1)
                {
                    var dataIns = context.MvReklameSummaries
                        .Where(x => x.IdFlagPermohonanA == 1 && x.TahunA == tahun)
                        .ToList();

                    var dataUpaya = context.DbMonReklameUpayas.AsQueryable();

                    for (int i = 1; i <= 12; i++)
                    {
                        // Join dataIns bulan tertentu dengan dataUpaya
                        var joinData = from s in dataIns.Where(x => x.BulanA == i)
                                       join u in dataUpaya
                                            on s.NoFormulir
                                            equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // Left Join
                                       select new { s, u };

                        var dataRekIns = dataIns.Where(x => x.BulanA == i).AsQueryable();
                        int skpd = dataRekIns.Count();
                        decimal nilai = dataRekIns.Sum(q => q.PajakPokokA) ?? 0;

                        decimal JmlBantip = dataRekIns.Sum(q => q.BantipA) ?? 0;
                        decimal JmlSilang = dataRekIns.Sum(q => q.SilangA) ?? 0;
                        decimal JmlBongkar = dataRekIns.Sum(q => q.BongkarA) ?? 0;
                        decimal JmlTegur = joinData.Where(q => (q.u?.Upaya ?? "").ToUpper() == "TEGURAN").Count();

                        int skpdPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Count();
                        decimal nilaiPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Sum(q => q.PajakPokokA) ?? 0;

                        int skpdBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Count();
                        decimal nilaiBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Sum(q => q.PajakPokokA) ?? 0;

                        ret.Add(new IsidentilReklame
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 1,
                            Jenis = 1, // Jenis 1 untuk Insidentil
                            SKPD = skpd,
                            Nilai = nilai,
                            SKPDPanjangKB = skpdPanjang,
                            NilaiPanjangKB = nilaiPanjang,
                            SKPDBlmByr = skpdBlmByr,
                            NilaiBlmByr = nilaiBlmByr,
                            Teguran = JmlTegur
                        }); 
                    }
                }
                else if (lokasi == 2)
                {
                    //indoor
                    var dataIns = context.MvReklameSummaries
                        .Where(x => x.IdFlagPermohonanA == 1 && x.TahunA == tahun && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)")
                        .ToList();

                    var dataUpaya = context.DbMonReklameUpayas.AsQueryable();

                    for (int i = 1; i <= 12; i++)
                    {
                        // Join dataIns bulan tertentu dengan dataUpaya
                        var joinData = from s in dataIns.Where(x => x.BulanA == i)
                                       join u in dataUpaya
                                            on s.NoFormulir
                                            equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // Left Join
                                       select new { s, u };

                        var dataRekIns = dataIns.Where(x => x.BulanA == i).AsQueryable();
                        int skpd = dataRekIns.Count();
                        decimal nilai = dataRekIns.Sum(q => q.PajakPokokA) ?? 0;

                        decimal JmlBantip = dataRekIns.Sum(q => q.BantipA) ?? 0;
                        decimal JmlSilang = dataRekIns.Sum(q => q.SilangA) ?? 0;
                        decimal JmlBongkar = dataRekIns.Sum(q => q.BongkarA) ?? 0;
                        decimal JmlTegur = joinData.Where(q => (q.u?.Upaya ?? "").ToUpper() == "TEGURAN").Count();

                        int skpdPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Count();
                        decimal nilaiPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Sum(q => q.PajakPokokA) ?? 0;

                        int skpdBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Count();
                        decimal nilaiBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Sum(q => q.PajakPokokA) ?? 0;

                        ret.Add(new IsidentilReklame
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 1,
                            Jenis = 1, // Jenis 1 untuk Insidentil
                            SKPD = skpd,
                            Nilai = nilai,
                            SKPDPanjangKB = skpdPanjang,
                            NilaiPanjangKB = nilaiPanjang,
                            SKPDBlmByr = skpdBlmByr,
                            NilaiBlmByr = nilaiBlmByr,
                            Teguran = JmlTegur
                        });
                    }

                }
                else if (lokasi == 3)
                {
                    //outdoor
                    var dataIns = context.MvReklameSummaries
                        .Where(x => x.IdFlagPermohonanA == 1 && x.TahunA == tahun && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)")
                        .ToList();

                    var dataUpaya = context.DbMonReklameUpayas.AsQueryable();

                    for (int i = 1; i <= 12; i++)
                    {
                        // Join dataIns bulan tertentu dengan dataUpaya
                        var joinData = from s in dataIns.Where(x => x.BulanA == i)
                                       join u in dataUpaya
                                            on s.NoFormulir
                                            equals u.NoFormulir into g
                                       from u in g.DefaultIfEmpty() // Left Join
                                       select new { s, u };

                        var dataRekIns = dataIns.Where(x => x.BulanA == i).AsQueryable();
                        int skpd = dataRekIns.Count();
                        decimal nilai = dataRekIns.Sum(q => q.PajakPokokA) ?? 0;

                        decimal JmlBantip = dataRekIns.Sum(q => q.BantipA) ?? 0;
                        decimal JmlSilang = dataRekIns.Sum(q => q.SilangA) ?? 0;
                        decimal JmlBongkar = dataRekIns.Sum(q => q.BongkarA) ?? 0;
                        decimal JmlTegur = joinData.Where(q => (q.u?.Upaya ?? "").ToUpper() == "TEGURAN").Count();

                        int skpdPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Count();
                        decimal nilaiPanjang = dataRekIns.Where(x => x.TglBayarPokokA.HasValue).Sum(q => q.PajakPokokA) ?? 0;

                        int skpdBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Count();
                        decimal nilaiBlmByr = dataRekIns.Where(x => x.NominalPokokBayarA == null).Sum(q => q.PajakPokokA) ?? 0;

                        ret.Add(new IsidentilReklame
                        {
                            BulanNama = new DateTime(tahun, i, 1).ToString("MMMM", new CultureInfo("id-ID")),
                            Bulan = i,
                            Tahun = tahun,
                            Lokasi = 1,
                            Jenis = 1, // Jenis 1 untuk Insidentil
                            SKPD = skpd,
                            Nilai = nilai,
                            SKPDPanjangKB = skpdPanjang,
                            NilaiPanjangKB = nilaiPanjang,
                            SKPDBlmByr = skpdBlmByr,
                            NilaiBlmByr = nilaiBlmByr,
                            Teguran = JmlTegur
                        });
                    }

                }
                return ret;
            }

            // Detail Reklame Permanen
            public static List<DetailSummary> GetDetailSummary(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                using var context = DBClass.GetContext();

                var upayaGrouped = context.DbMonReklameUpayas
                    .Where(x => !string.IsNullOrWhiteSpace(x.NoFormulir))
                    .GroupBy(x => x.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, List = g.Select(u => u.Upaya).ToList() })
                    .ToDictionary(x => x.Key, x => x.List);

                var emails = context.DbMonReklameEmails
                    .Where(e => !string.IsNullOrWhiteSpace(e.NoFormulir))
                    .GroupBy(e => e.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, Email = g.FirstOrDefault().Email })
                    .ToDictionary(x => x.Key, x => x.Email);

                Func<MvReklameSummary, bool> predicate = x => false;

                if (lokasi == 1)
                {
                    //semua
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
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
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         !x.NominalPokokBayarA.HasValue;
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
                                         !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis;
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 2)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 && x.IdFlagPermohonan == jenis;
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == jenis &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         (!x.TglBayarPokokA.HasValue);
                    }

                }
                else if (lokasi == 2)
                {
                    //indoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
                                         !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulirA) && x.IdFlagPermohonan == jenis
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == jenis &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         !x.NominalPokokBayarA.HasValue &&
                                         x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
                                         !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis &&
                                         x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 2)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 && x.IdFlagPermohonan == jenis &&
                                         x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == jenis &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         !x.TglBayarPokokA.HasValue &&
                                         x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }

                }
                else if (lokasi == 3)
                {
                    //outdoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
                                         !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulirA) && x.IdFlagPermohonan == jenis
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == jenis &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         !x.NominalPokokBayarA.HasValue &&
                                         x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 1)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         !string.IsNullOrEmpty(x.NoFormulir) &&
                                         !x.TglBayarPokok.HasValue && x.IdFlagPermohonan == jenis &&
                                         x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 2)
                    {
                        predicate = x => x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 && x.IdFlagPermohonan == jenis &&
                                         x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if ((jenis == 2 || jenis == 3) && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == jenis &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         !x.TglBayarPokokA.HasValue &&
                                         x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                }

                var rawData = context.MvReklameSummaries
                    .Where(predicate)
                    .ToList();

                var ret = rawData.Select(x =>
                {
                    string noFormulirDigunakan = (kategori == 3) ? x.NoFormulirA : x.NoFormulir;
                    string flag = (kategori == 3) ? x.FlagPermohonanA : x.FlagPermohonan;

                    string tampilFormulir = !string.IsNullOrEmpty(noFormulirDigunakan)
                        ? $"{noFormulirDigunakan} ({flag})"
                        : string.Empty;

                    var key = noFormulirDigunakan?.Trim().ToLower();

                    string informasiEmail = (key != null && emails.TryGetValue(key, out var email))
                        ? email
                        : string.Empty;

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
                        Lokasi = lokasi,
                        Jenis = (kategori == 3) ? (x.FlagPermohonanA ?? string.Empty) : (x.FlagPermohonan ?? string.Empty),
                        DetailReklame = (kategori == 3) ? (x.DetailLokasiA ?? string.Empty) : (x.DetailLokasi ?? string.Empty),

                        Nama = (kategori == 3)
                            ? string.Concat(x.NamaA ?? "", " (", x.NamaPerusahaanA ?? "", ")")
                            : string.Concat(x.Nama ?? "", " (", x.NamaPerusahaan ?? "", ")"),

                        AlamatOP = (kategori == 3) ? (x.AlamatreklameA ?? string.Empty) : (x.Alamatreklame ?? string.Empty),
                        IsiReklame = (kategori == 3) ? (x.IsiReklameA ?? string.Empty) : (x.IsiReklame ?? string.Empty),

                        AkhirBerlaku = (kategori == 3 && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TglAkhirBerlakuA.Value:dd MMM yyyy} (BELUM TERBAYAR)"
                            : (x.TglAkhirBerlaku.HasValue ? $"{x.TglAkhirBerlaku.Value:dd MMM yyyy} (BELUM TERBAYAR)" : string.Empty),

                        MasaTahunPajak = (kategori == 3 && x.TglMulaiBerlakuA.HasValue && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TahunA} ({x.TglMulaiBerlakuA.Value:dd MMM yyyy} - {x.TglAkhirBerlakuA.Value:dd MMM yyyy})"
                            : (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue
                                ? $"{x.Tahun} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})"
                                : string.Empty),

                        JumlahNilai = (kategori == 3) ? (x.PajakPokokA ?? 0) : (x.PajakPokok ?? 0),

                        InformasiEmail = informasiEmail,
                        JumlahUpaya = jumlahUpaya,

                        KategoriText = kategori switch
                        {
                            1 => "Belum Bayar",
                            2 => "Belum Perpanjangan",
                            3 => "Belum Bayar",
                            _ => string.Empty
                        }
                    };
                }).ToList();

                return ret;
            }

            public static void SimpanUpaya(DetailUpaya.NewRow NewRowUpaya)
            {
                var context = DBClass.GetContext();
                if (string.IsNullOrEmpty(NewRowUpaya.NoFormulir) && string.IsNullOrEmpty(NewRowUpaya.NOR))
                    throw new ArgumentException("Silakan isi salah satu: No Formulir atau NOR.");

                if (NewRowUpaya.IdUpaya == 0)
                    throw new ArgumentException("Upaya tidak boleh kosong.");

                if (NewRowUpaya.IdTindakan == 0)
                    throw new ArgumentException("Keterangan tidak boleh kosong.");

                if (NewRowUpaya.TglUpaya == null || NewRowUpaya.TglUpaya == DateTime.MinValue)
                    throw new ArgumentException("Tanggal Upaya tidak boleh kosong.");

                if (string.IsNullOrEmpty(NewRowUpaya.NamaPetugas))
                    throw new ArgumentException("Nama Petugas tidak boleh kosong.");

                if (string.IsNullOrEmpty(NewRowUpaya.NIKPetugas))
                    throw new ArgumentException("NIK Petugas tidak boleh kosong.");

                if (NewRowUpaya.Lampiran == null || NewRowUpaya.Lampiran.Length <= 0)
                    throw new ArgumentException("Lampiran foto tidak boleh kosong.");

                var tindakan = context.MTindakanReklames
                    .Where(x => x.Id == NewRowUpaya.IdTindakan && x.IdUpaya == NewRowUpaya.IdUpaya)
                    .Select(x => x.Tindakan)
                    .SingleOrDefault();

                var upaya = context.MUpayaReklames
                    .Where(x => x.Id == NewRowUpaya.IdUpaya)
                    .Select(x => x.Upaya)
                    .SingleOrDefault();

                var seq = 1;
                if (!string.IsNullOrEmpty(NewRowUpaya.NoFormulir))
                {
                    seq = context.DbMonReklameUpayas
                        .Where(x => x.NoFormulir == NewRowUpaya.NoFormulir)
                        .Select(x => x.Seq)
                        .Count() + 1;
                }
                else if (!string.IsNullOrEmpty(NewRowUpaya.NOR))
                {
                    seq = context.DbMonReklameUpayas
                        .Where(x => x.Nor == NewRowUpaya.NOR)
                        .Select(x => x.Seq)
                        .Count() + 1;
                }

                // Simpan data
                var newUpaya = new MonPDLib.EF.DbMonReklameUpaya();
                newUpaya.DbMonReklameUpayaDok = new DbMonReklameUpayaDok();

                newUpaya.NoFormulir = NewRowUpaya.NoFormulir.Trim().ToUpper();
                newUpaya.Nor = NewRowUpaya.NOR.Trim().ToUpper();
                newUpaya.Seq = seq;
                newUpaya.TglUpaya = NewRowUpaya.TglUpaya;
                newUpaya.Upaya = upaya ?? "-";
                newUpaya.Keterangan = tindakan ?? "-";
                newUpaya.Petugas = NewRowUpaya.NamaPetugas;
                newUpaya.NikPetugas = NewRowUpaya.NIKPetugas;
                newUpaya.KdAktifitas = NewRowUpaya.KdKatifitas;
                newUpaya.DbMonReklameUpayaDok.Gambar = NewRowUpaya.Lampiran;

                context.DbMonReklameUpayas.Add(newUpaya);
                context.SaveChanges();
            }
            public static DetailUpaya GetDetailUpaya(string noFormulir, int tahun, int bulan, int lokasi)
            {
                var context = DBClass.GetContext();

                // cari reklame sesuai lokasi
                List<MvReklameSummary> reklameList = null;

                if (lokasi == 1)
                {
                    reklameList = context.MvReklameSummaries
                        .Where(x =>
                            (x.NoFormulir == noFormulir && x.Tahun == tahun && x.Bulan == bulan) ||
                            (x.NoFormulirA == noFormulir && x.TahunA == tahun && x.BulanA == bulan))
                        .ToList();
                }
                else if (lokasi == 2)
                {
                    reklameList = context.MvReklameSummaries
                        .Where(x =>
                            (x.NoFormulir == noFormulir && x.Tahun == tahun && x.Bulan == bulan && x.LetakReklame == "DALAM RUANGAN (IN DOOR)") ||
                            (x.NoFormulirA == noFormulir && x.TahunA == tahun && x.BulanA == bulan && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)"))
                        .ToList();
                }
                else if (lokasi == 3)
                {
                    reklameList = context.MvReklameSummaries
                        .Where(x =>
                            (x.NoFormulir == noFormulir && x.Tahun == tahun && x.Bulan == bulan && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)") ||
                            (x.NoFormulirA == noFormulir && x.TahunA == tahun && x.BulanA == bulan && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)"))
                        .ToList();
                }
                else
                {
                    return null;
                }

                if (reklameList == null || !reklameList.Any())
                    return null;

                // kumpulkan semua formulir yang match
                var matchedFormulirs = reklameList
                    .SelectMany(r => new[]
                    {
                        (!string.IsNullOrEmpty(r.NoFormulir) && r.NoFormulir == noFormulir && r.Tahun == tahun && r.Bulan == bulan) ? r.NoFormulir : null,
                        (!string.IsNullOrEmpty(r.NoFormulirA) && r.NoFormulirA == noFormulir && r.TahunA == tahun && r.BulanA == bulan) ? r.NoFormulirA : null
                    })
                    .Where(f => f != null)
                    .Distinct()
                    .ToList();

                // ambil hanya upaya yang tidak memiliki KodeAktifitas
                var dataUpayaList = context.DbMonReklameUpayas
                    .Include(x => x.DbMonReklameUpayaDok)
                    .Where(x =>
                        matchedFormulirs.Contains(x.NoFormulir) &&
                        (x.KdAktifitas == null || x.KdAktifitas == "-")) // ✅ filter hanya yang tidak ada kode aktifitas
                    .OrderByDescending(x => x.TglUpaya)
                    .Select(x => new DetailUpaya.DataUpaya
                    {
                        NoFormulir = x.NoFormulir,
                        TglUpaya = x.TglUpaya.ToString("dd/MM/yyyy"),
                        NamaUpaya = x.Upaya,
                        Keterangan = x.Keterangan,
                        Petugas = x.Petugas,
                        Lampiran = x.DbMonReklameUpayaDok != null && x.DbMonReklameUpayaDok.Gambar != null
                            ? Convert.ToBase64String(x.DbMonReklameUpayaDok.Gambar)
                            : null
                    })
                    .ToList();

                // pilih salah satu reklame untuk InfoReklame
                var reklame = reklameList.First();
                bool isFormulirA = matchedFormulirs.Contains(reklame.NoFormulirA);

                var model = new DetailUpaya
                {
                    Tahun = tahun,
                    Bulan = bulan,
                    NoFormulir = noFormulir,
                    InfoReklameUpaya = new DetailUpaya.InfoReklame
                    {
                        IsiReklame = isFormulirA ? reklame.IsiReklameA ?? "-" : reklame.IsiReklame ?? "-",
                        AlamatReklame = isFormulirA ? reklame.AlamatreklameA ?? "-" : reklame.Alamatreklame ?? "-",
                        JenisReklame = isFormulirA ? reklame.NmJenisA ?? "-" : reklame.NmJenis ?? "-",
                        DetailReklame = isFormulirA ? reklame.DetailLokasiA ?? "-" : reklame.DetailLokasi ?? "-",
                        Panjang = isFormulirA ? reklame.PanjangA ?? 0 : reklame.Panjang ?? 0,
                        Lebar = isFormulirA ? reklame.LebarA ?? 0 : reklame.Lebar ?? 0,
                        Luas = isFormulirA ? reklame.LuasA ?? 0 : reklame.Luas ?? 0,
                        Tinggi = isFormulirA ? reklame.KetinggianA ?? 0 : reklame.Ketinggian ?? 0,
                        TglMulaiBerlaku = isFormulirA ? reklame.TglMulaiBerlakuA ?? DateTime.MinValue : reklame.TglMulaiBerlaku ?? DateTime.MinValue,
                        TglAkhirBerlaku = isFormulirA ? reklame.TglAkhirBerlakuA ?? DateTime.MinValue : reklame.TglAkhirBerlaku ?? DateTime.MinValue,
                        TahunPajak = isFormulirA ? reklame.TahunA?.ToString() ?? "-" : reklame.Tahun?.ToString() ?? "-",
                        MasaPajak = (isFormulirA && reklame.TglMulaiBerlakuA.HasValue && reklame.TglAkhirBerlakuA.HasValue)
                            ? $"{reklame.TglMulaiBerlakuA.Value:MMM yyyy} - {reklame.TglAkhirBerlakuA.Value:MMM yyyy}"
                            : (reklame.TglMulaiBerlaku.HasValue && reklame.TglAkhirBerlaku.HasValue
                                ? $"{reklame.TglMulaiBerlaku.Value:MMM yyyy} - {reklame.TglAkhirBerlaku.Value:MMM yyyy}"
                                : "-")
                    },
                    DataUpayaList = dataUpayaList
                };

                return model;
            }


            public static List<DetailBongkar> GetDetailBongkar(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                using var context = DBClass.GetContext();

                var upayaGrouped = context.DbMonReklameUpayas
                    .Where(x => !string.IsNullOrWhiteSpace(x.NoFormulir))
                    .GroupBy(x => x.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, List = g.Select(u => u.Upaya).ToList() })
                    .ToDictionary(x => x.Key, x => x.List);

                var emails = context.DbMonReklameEmails
                    .Where(e => !string.IsNullOrWhiteSpace(e.NoFormulir))
                    .GroupBy(e => e.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, Email = g.FirstOrDefault().Email })
                    .ToDictionary(x => x.Key, x => x.Email);

                Expression<Func<MvReklameSummary, bool>> predicate = x => false;

                if (lokasi == 1)
                {
                    //semua
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null && !x.TglBayarPokok.HasValue &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1;
                    }
                }
                else if (lokasi == 2)
                {
                    //indoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                }
                else if (lokasi == 3)
                {
                    //outdoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Bongkar.HasValue && x.Bongkar.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.BongkarA.HasValue && x.BongkarA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                }




                var rawData = context.MvReklameSummaries
                    .Where(predicate)
                    .AsNoTracking()
                    .ToList();

                var ret = rawData.Select(x =>
                {
                    string noFormulirDigunakan = (kategori == 3) ? x.NoFormulirA : x.NoFormulir;
                    string flag = (kategori == 3) ? x.FlagPermohonanA : x.FlagPermohonan;
                    string tampilFormulir = !string.IsNullOrEmpty(noFormulirDigunakan)
                        ? $"{noFormulirDigunakan} ({flag})"
                        : string.Empty;

                    var key = noFormulirDigunakan?.Trim().ToLower();

                    string informasiEmail = (key != null && emails.TryGetValue(key, out var email))
                        ? email
                        : string.Empty;

                    string jumlahUpaya = "0";
                    if (!string.IsNullOrEmpty(key) && upayaGrouped.TryGetValue(key, out var upayaList))
                    {
                        jumlahUpaya = $"{upayaList.Count}x: {string.Join(", ", upayaList)}";
                    }

                    return new DetailBongkar
                    {
                        Bulan = bulan,
                        BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Tahun = tahun,
                        NoFormulir = tampilFormulir,
                        Jenis = (kategori == 3) ? (x.FlagPermohonanA ?? string.Empty) : (x.FlagPermohonan ?? string.Empty),
                        Nama = (kategori == 3)
                            ? string.Concat(x.NamaA ?? "", " (", x.NamaPerusahaanA ?? "", ")")
                            : string.Concat(x.Nama ?? "", " (", x.NamaPerusahaan ?? "", ")"),
                        AlamatOP = (kategori == 3) ? (x.AlamatreklameA ?? string.Empty) : (x.Alamatreklame ?? string.Empty),
                        IsiReklame = (kategori == 3) ? (x.IsiReklameA ?? string.Empty) : (x.IsiReklame ?? string.Empty),
                        DetailLokasi = (kategori == 3) ? (x.DetailLokasiA ?? string.Empty) : (x.DetailLokasi ?? string.Empty),
                        AkhirBerlaku = (kategori == 3 && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TglAkhirBerlakuA.Value:dd MMM yyyy} (BONGKAR)"
                            : (x.TglAkhirBerlaku.HasValue
                                ? $"{x.TglAkhirBerlaku.Value:dd MMM yyyy} (BONGKAR)"
                                : string.Empty),
                        MasaTahunPajak = (kategori == 3 && x.TglMulaiBerlakuA.HasValue && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TahunA} ({x.TglMulaiBerlakuA.Value:dd MMM yyyy} - {x.TglAkhirBerlakuA.Value:dd MMM yyyy})"
                            : (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue
                                ? $"{x.Tahun} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})"
                                : string.Empty),
                        JumlahNilai = (kategori == 3) ? (x.PajakPokokA ?? 0) : (x.PajakPokok ?? 0),
                        InformasiEmail = informasiEmail,
                        JumlahUpaya = jumlahUpaya,
                        KategoriText = kategori switch
                        {
                            1 => "Belum Bayar",
                            2 => "Belum Perpanjangan",
                            3 => "Belum Bayar",
                            _ => string.Empty
                        }
                    };
                }).ToList();

                return ret;
            }

            public static List<DetailSilang> GetDetailSilang(int tahun, int bulan, int jenis, int kategori, int lokasi)
            {
                using var context = DBClass.GetContext();

                var upayaGrouped = context.DbMonReklameUpayas
                    .Where(x => !string.IsNullOrWhiteSpace(x.NoFormulir))
                    .GroupBy(x => x.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, List = g.Select(u => u.Upaya).ToList() })
                    .ToDictionary(x => x.Key, x => x.List);

                var emails = context.DbMonReklameEmails
                    .Where(e => !string.IsNullOrWhiteSpace(e.NoFormulir))
                    .GroupBy(e => e.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, Email = g.FirstOrDefault().Email })
                    .ToDictionary(x => x.Key, x => x.Email);

                Expression<Func<MvReklameSummary, bool>> predicate = x => false;

                if (lokasi == 1)
                {
                    //semua
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null && !x.TglBayarPokok.HasValue &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1;
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1;
                    }
                }
                else if (lokasi == 2)
                {
                    //indoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "DALAM RUANGAN (IN DOOR)";
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "DALAM RUANGAN (IN DOOR)";
                    }
                }
                else if (lokasi == 3)
                {
                    //outdoor
                    if (jenis == 1 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 1 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 1 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 1 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 2 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 2 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 2 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 1)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.NoFormulir != null &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 2)
                    {
                        predicate = x => x.IdFlagPermohonan == 3 &&
                                         x.Tahun == tahun && x.Bulan == bulan &&
                                         x.IsPerpanjangan == 0 &&
                                         x.Silang.HasValue && x.Silang.Value == 1
                                         && x.LetakReklame == "LUAR RUANGAN (OUT DOOR)";
                    }
                    else if (jenis == 3 && kategori == 3)
                    {
                        predicate = x => x.IdFlagPermohonanA == 3 &&
                                         x.TahunA == tahun && x.BulanA == bulan &&
                                         x.SilangA.HasValue && x.SilangA.Value == 1
                                         && x.LetakReklameA == "LUAR RUANGAN (OUT DOOR)";
                    }
                }




                var rawData = context.MvReklameSummaries
                    .Where(predicate)
                    .AsNoTracking()
                    .ToList();

                var ret = rawData.Select(x =>
                {
                    string noFormulirDigunakan = (kategori == 3) ? x.NoFormulirA : x.NoFormulir;
                    string flag = (kategori == 3) ? x.FlagPermohonanA : x.FlagPermohonan;
                    string tampilFormulir = !string.IsNullOrEmpty(noFormulirDigunakan)
                        ? $"{noFormulirDigunakan} ({flag})"
                        : string.Empty;

                    var key = noFormulirDigunakan?.Trim().ToLower();

                    string informasiEmail = (key != null && emails.TryGetValue(key, out var email))
                        ? email
                        : string.Empty;

                    string jumlahUpaya = "0";
                    if (!string.IsNullOrEmpty(key) && upayaGrouped.TryGetValue(key, out var upayaList))
                    {
                        jumlahUpaya = $"{upayaList.Count}x: {string.Join(", ", upayaList)}";
                    }

                    return new DetailSilang
                    {
                        Bulan = bulan,
                        BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Tahun = tahun,
                        NoFormulir = tampilFormulir,
                        Jenis = (kategori == 3) ? (x.FlagPermohonanA ?? string.Empty) : (x.FlagPermohonan ?? string.Empty),
                        Nama = (kategori == 3)
                            ? string.Concat(x.NamaA ?? "", " (", x.NamaPerusahaanA ?? "", ")")
                            : string.Concat(x.Nama ?? "", " (", x.NamaPerusahaan ?? "", ")"),
                        AlamatOP = (kategori == 3) ? (x.AlamatreklameA ?? string.Empty) : (x.Alamatreklame ?? string.Empty),
                        IsiReklame = (kategori == 3) ? (x.IsiReklameA ?? string.Empty) : (x.IsiReklame ?? string.Empty),
                        DetailLokasi = (kategori == 3) ? (x.DetailLokasiA ?? string.Empty) : (x.DetailLokasi ?? string.Empty),
                        AkhirBerlaku = (kategori == 3 && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TglAkhirBerlakuA.Value:dd MMM yyyy} (SILANG)"
                            : (x.TglAkhirBerlaku.HasValue
                                ? $"{x.TglAkhirBerlaku.Value:dd MMM yyyy} (SILANG)"
                                : string.Empty),
                        MasaTahunPajak = (kategori == 3 && x.TglMulaiBerlakuA.HasValue && x.TglAkhirBerlakuA.HasValue)
                            ? $"{x.TahunA} ({x.TglMulaiBerlakuA.Value:dd MMM yyyy} - {x.TglAkhirBerlakuA.Value:dd MMM yyyy})"
                            : (x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.HasValue
                                ? $"{x.Tahun} ({x.TglMulaiBerlaku.Value:dd MMM yyyy} - {x.TglAkhirBerlaku.Value:dd MMM yyyy})"
                                : string.Empty),
                        JumlahNilai = (kategori == 3) ? (x.PajakPokokA ?? 0) : (x.PajakPokok ?? 0),
                        InformasiEmail = informasiEmail,
                        JumlahUpaya = jumlahUpaya,
                        KategoriText = kategori switch
                        {
                            1 => "Belum Bayar",
                            2 => "Belum Perpanjangan",
                            3 => "Belum Bayar",
                            _ => string.Empty
                        }
                    };
                }).ToList();

                return ret;
            }
            public static List<DetailTeguran> GetDetailTegur(int tahun, int bulan, int jenis, int kategori)
            {
                using var context = DBClass.GetContext();

                // ambil hanya NoFormulir yang ada upaya TEGURAN
                var formulirTeguran = context.DbMonReklameUpayas
                    .Where(u => !string.IsNullOrEmpty(u.NoFormulir) && u.Upaya == "TEGURAN")
                    .Select(u => u.NoFormulir.Trim().ToLower())
                    .Distinct()
                    .ToList();

                // filter summary sesuai jenis + kategori + hanya yang punya teguran
                var summaries = context.MvReklameSummaries
                    .Where(s =>
                        formulirTeguran.Contains(s.NoFormulir.Trim().ToLower()) &&
                        (
                            (jenis == 1 && kategori == 1 && s.IdFlagPermohonan == 1 && s.Tahun == tahun && s.Bulan == bulan && !string.IsNullOrEmpty(s.NoFormulir)) ||
                            (jenis == 1 && kategori == 2 && s.IdFlagPermohonan == 1 && s.Tahun == tahun && s.Bulan == bulan && s.IsPerpanjangan == 0) ||
                            (jenis == 1 && kategori == 3 && s.IdFlagPermohonanA == 1 && s.TahunA == tahun && s.BulanA == bulan && s.BongkarA == 1) ||
                            (jenis == 2 && kategori == 1 && s.IdFlagPermohonan == 2 && s.Tahun == tahun && s.Bulan == bulan && !s.TglBayarPokok.HasValue) ||
                            (jenis == 2 && kategori == 2 && s.IdFlagPermohonan == 2 && s.Tahun == tahun && s.Bulan == bulan && s.IsPerpanjangan == 0) ||
                            (jenis == 2 && kategori == 3 && s.IdFlagPermohonanA == 2 && s.TahunA == tahun && s.BulanA == bulan) ||
                            (jenis == 3 && kategori == 1 && s.IdFlagPermohonan == 3 && s.Tahun == tahun && s.Bulan == bulan && !s.TglBayarPokok.HasValue) ||
                            (jenis == 3 && kategori == 2 && s.IdFlagPermohonan == 3 && s.Tahun == tahun && s.Bulan == bulan && s.IsPerpanjangan == 0) ||
                            (jenis == 3 && kategori == 3 && s.IdFlagPermohonanA == 3 && s.TahunA == tahun && s.BulanA == bulan)
                        )
                    )
                    .AsNoTracking()
                    .ToList();

                // ambil email
                var emails = context.DbMonReklameEmails
                    .Where(e => !string.IsNullOrEmpty(e.NoFormulir))
                    .GroupBy(e => e.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, Email = g.FirstOrDefault().Email })
                    .ToDictionary(x => x.Key, x => x.Email);

                // ambil jumlah upaya teguran per NoFormulir
                var upayaGrouped = context.DbMonReklameUpayas
                    .Where(u => !string.IsNullOrEmpty(u.NoFormulir) && u.Upaya == "TEGURAN")
                    .GroupBy(u => u.NoFormulir.Trim().ToLower())
                    .Select(g => new { Key = g.Key, List = g.Select(x => x.Upaya).ToList() })
                    .ToDictionary(x => x.Key, x => x.List);

                // mapping ke DetailTeguran
                var ret = summaries.Select(s =>
                {
                    var key = s.NoFormulir?.Trim().ToLower();
                    string email = (key != null && emails.TryGetValue(key, out var e)) ? e : "";
                    string jumlahUpaya = (key != null && upayaGrouped.TryGetValue(key, out var list))
                        ? $"{list.Count}x: {string.Join(", ", list)}"
                        : "0";

                    return new DetailTeguran
                    {
                        Bulan = bulan,
                        BulanNama = new DateTime(tahun, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Tahun = tahun,
                        NoFormulir = s.NoFormulir ?? "",
                        Nama = (kategori == 3)
                            ? string.Concat(s.NamaA ?? "", " (", s.NamaPerusahaanA ?? "", ")")
                            : string.Concat(s.Nama ?? "", " (", s.NamaPerusahaan ?? "", ")"),
                        AlamatOP = (kategori == 3) ? (s.AlamatreklameA ?? string.Empty) : (s.Alamatreklame ?? string.Empty),
                        IsiReklame = (kategori == 3) ? (s.IsiReklameA ?? string.Empty) : (s.IsiReklame ?? string.Empty),
                        DetailLokasi = (kategori == 3) ? (s.DetailLokasiA ?? string.Empty) : (s.DetailLokasi ?? string.Empty),
                        AkhirBerlaku = (kategori == 3 && s.TglAkhirBerlakuA.HasValue)
                            ? $"{s.TglAkhirBerlakuA.Value:dd MMM yyyy} (TEGURAN)"
                            : (s.TglAkhirBerlaku.HasValue
                                ? $"{s.TglAkhirBerlaku.Value:dd MMM yyyy} (TEGURAN)"
                                : string.Empty),
                        MasaTahunPajak = (kategori == 3 && s.TglMulaiBerlakuA.HasValue && s.TglAkhirBerlakuA.HasValue)
                            ? $"{s.TahunA} ({s.TglMulaiBerlakuA.Value:dd MMM yyyy} - {s.TglAkhirBerlakuA.Value:dd MMM yyyy})"
                            : (s.TglMulaiBerlaku.HasValue && s.TglAkhirBerlaku.HasValue
                                ? $"{s.Tahun} ({s.TglMulaiBerlaku.Value:dd MMM yyyy} - {s.TglAkhirBerlaku.Value:dd MMM yyyy})"
                                : string.Empty),
                        JumlahNilai = (kategori == 3) ? (s.PajakPokokA ?? 0) : (s.PajakPokok ?? 0),
                        InformasiEmail = email,
                        JumlahUpaya = jumlahUpaya,
                        KategoriText = kategori switch
                        {
                            1 => "Belum Bayar",
                            2 => "Belum Perpanjangan",
                            3 => "Belum Bayar",
                            _ => string.Empty
                        }
                    };
                }).ToList();

                return ret;
            }

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
            public int Lokasi { get; set; }
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
            public int SKPDPanjangKB { get; set; }
            public decimal NilaiPanjangKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal TegurJT { get; set; }
            public decimal TegurPanjang { get; set; }
            public decimal TegurKB { get; set; }

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
            public int Lokasi { get; set; }
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
            public int SKPDPanjangKB { get; set; }
            public decimal NilaiPanjangKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal TegurJT { get; set; }
            public decimal TegurPanjang { get; set; }
            public decimal TegurKB { get; set; }

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
            public int Lokasi { get; set; }
            public int Jenis { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public int SKPD { get; set; }
            public decimal Nilai { get; set; }
            public int SKPDPanjangKB { get; set; }
            public decimal NilaiPanjangKB { get; set; }
            public int SKPDBlmByr { get; set; }
            public decimal NilaiBlmByr { get; set; }
            public decimal Teguran { get; set; }

            public decimal JmlBantip { get; set; }
            public decimal JmlSilang { get; set; }
            public decimal JmlBongkar { get; set; }
            public decimal Potensi => NilaiBlmByr;
        }

        public class DetailSummary
        {
            public int Lokasi { get; set; }
            public string Jenis { get; set; }
            public string KategoriText { get; set; }
            public string BulanNama { get; set; } = null!;
            public string DetailReklame { get; set; } = null!;
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
            public int Lokasi { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public NewRow NewRowUpaya { get; set; } = new NewRow();
            public InfoReklame InfoReklameUpaya { get; set; } = new InfoReklame();
            public List<DataUpaya> DataUpayaList { get; set; } = new();
            public class NewRow
            {
                public string NoFormulir { get; set; } = null!;
                public string NOR { get; set; } = null!;
                public int IdUpaya { get; set; }
                public int IdTindakan { get; set; }
                public string NamaPetugas { get; set; } = null!;
                public string KdKatifitas { get; set; } = null!;
                public string NIKPetugas { get; set; } = null!;
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
                public string DetailReklame { get; set; } = null!;
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
        public class DetailBongkar
        {
            public string Lokasi { get; set; } = null!;
            public string Jenis { get; set; }
            public string KategoriText { get; set; }
            public string DetailReklame { get; set; } = null!;
            public string BulanNama { get; set; } = null!;
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string AkhirBerlaku { get; set; } = null!;
            public string MasaTahunPajak { get; set; } = null!;
            public string DetailLokasi { get; set; } = null!;
            public decimal JumlahNilai { get; set; }
            public string? InformasiEmail { get; set; }
            public string JumlahUpaya { get; set; } = null!;
        }
        public class DetailSilang
        {
            public string Lokasi { get; set; } = null!;
            public string Jenis { get; set; }
            public string KategoriText { get; set; }
            public string DetailReklame { get; set; } = null!;
            public string BulanNama { get; set; } = null!;
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string AkhirBerlaku { get; set; } = null!;
            public string MasaTahunPajak { get; set; } = null!;
            public string DetailLokasi { get; set; } = null!;
            public decimal JumlahNilai { get; set; }
            public string? InformasiEmail { get; set; }
            public string JumlahUpaya { get; set; } = null!;
        }
        public class DetailTeguran
        {
            public string DetailLokasi { get; set; } = null!;
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
    }
}
