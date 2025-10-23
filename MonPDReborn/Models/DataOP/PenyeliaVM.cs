using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EFPenyelia;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class PenyeliaVM
    {
        private static PenyeliaContext _context = DBClass.GetPenyeliaContext();
        public class Index
        {
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
            public string SelectedBidang { get; set; } = null!;
            public List<SelectListItem> BulanList { get; set; } = new();
            public List<SelectListItem> TahunList { get; set; } = new();
            public Index()
            {
                SelectedBulan = DateTime.Now.Month;
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
            public List<ViewModels.Penyelia> Data { get; set; } = new();
            public Show(int tahun, int bulan, string bidang)
            {
                Data = Methods.GetDataPenyelia(tahun, bulan, bidang);
            }

        }
        public class Detail
        {
            public List<ViewModels.DetailPenyelia> Data { get; set; } = new();
            public Detail(int tahun, int bulan, string nip)
            {
                Data = Methods.GetDetailPenyelia(tahun, bulan, nip);
            }
        }
        public class ViewModels
        {
            public class Penyelia
            {
                public string Nip { get; set; } = null!;
                public string Nama { get; set; } = null!;
                public decimal JmlNOP { get; set; }
                public decimal Capaian { get; set; }
                public int Tahun { get; set; }
                public int Bulan { get; set; }
            }
            public class DetailPenyelia
            {
                public string Nip { get; set; } = null!;
                public string JenisPajak { get; set; } = null!;
                public int pajakId { get; set; }
                public decimal JmlNOP { get; set; }
                public decimal Capaian { get; set; }
            }
            public class DetailNop
            {
                public string Nip { get; set; } = null!;
                public int pajakId { get; set; }
                public string Nop { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public string NamaOP { get; set; } = null!;
                public decimal JmlUpaya { get; set; }
                public decimal Capaian { get; set; }
            }
            public class BidangView
            {
                public string Value { get; set; } = null!;
                public string Text { get; set; } = null!;
            }
        }

        public class Methods
        {
            public static List<ViewModels.Penyelia> GetDataPenyelia(int tahun, int bulan, string bidang)
            {
                var MonPdContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.Penyelia>();

                // Ambil semua NOP yang sudah bayar di bulan & tahun tertentu
                var nopSudahBayar = new HashSet<string>(
                    MonPdContext.DbMonRestos
                        .Where(x => x.NominalPokokBayar.HasValue &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month == bulan)
                        .Select(x => x.Nop)
                        .Concat(MonPdContext.DbMonHotels
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonHiburans
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonParkirs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonPpjs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonAbts
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Distinct()
                        .ToList()
                );

                // Ambil aktivitas, tapi tidak lagi dipakai untuk menentukan capaian
                var aktivitasPerNop = context.TAktifitasPegawais
                    .GroupBy(a => a.Nop)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Hitung capaian per pegawai
                ret = context.MPegawaiBarus
                    .Where(p => p.UnitKerja == bidang)
                    .AsEnumerable()
                    .Select(p =>
                    {
                        var nopsPegawai = context.MPegawaiOpDets
                            .Where(op => op.Nip == p.NipNik)
                            .Select(op => op.Nop)
                            .ToList();

                        var totalNOP = nopsPegawai.Count;
                        if (totalNOP == 0)
                            return new ViewModels.Penyelia
                            {
                                Nip = p.NipNik ?? "-",
                                Nama = p.Nama,
                                JmlNOP = 0,
                                Capaian = 0
                            };

                        // 🔹 Capaian hanya dihitung untuk NOP yang sudah bayar
                        var totalCapaian = nopsPegawai.Count(nop => nopSudahBayar.Contains(nop));

                        var capaianPersen = (decimal)totalCapaian / totalNOP * 100;

                        return new ViewModels.Penyelia
                        {
                            Nip = p.NipNik ?? "-",
                            Nama = p.Nama,
                            JmlNOP = totalNOP,
                            Tahun = tahun,
                            Bulan = bulan,
                            Capaian = Math.Round(capaianPersen, 2)
                        };
                    })
                    .ToList();

                return ret;

            }
            public static List<ViewModels.DetailPenyelia> GetDetailPenyelia(int tahun, int bulan, string nip)
            {
                var MonPdContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.DetailPenyelia>();

                var nopSudahBayar = new HashSet<string>(
                    MonPdContext.DbMonRestos
                        .Where(x => x.NominalPokokBayar.HasValue &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month == bulan)
                        .Select(x => x.Nop)
                        .Concat(MonPdContext.DbMonHotels
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonHiburans
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonParkirs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonPpjs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonAbts
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Distinct()
                        .ToList()
                );

                var jumlahOp = context.MPegawaiOpDets
                    .Where(x => x.Nip == nip)
                    .Select(x => x.Nop)
                    .ToList();

                var totalCapaian = jumlahOp.Count(nop => nopSudahBayar.Contains(nop));

                var capaianPersen = (decimal)totalCapaian / jumlahOp.Count() * 100;

                ret = context.MPegawaiOpDets
                    .Where(x => x.Nip == nip)
                    .AsEnumerable()
                    .Select(x => new ViewModels.DetailPenyelia
                    {
                        Nip = x.Nip,
                        pajakId = (int)x.NopNavigation.PajakId,
                        JenisPajak = ((EnumFactory.EPajak)x.NopNavigation.PajakId).GetDescription(),
                        JmlNOP = jumlahOp.Count(),
                        Capaian = nopSudahBayar.Contains(x.Nop) ? Math.Round(capaianPersen, 2) : 0
                    })
                    .ToList();

                return ret;
            }
        }
    }
}
